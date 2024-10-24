using System;
using CoreAnimation;
using Metal;

namespace PixUI.Platform.Mac;

public abstract class MetalWindowContext : NativeWindowContext
{
    private bool Valid;
    protected IMTLDevice? Device;
    private IMTLCommandQueue? Queue;
    protected CAMetalLayer? MetalLayer;
    private ICAMetalDrawable? DrawableHandler;
    private Canvas? _onscreenCanvas;
    protected Canvas? OffscreenCanvas;

    //TODO: Metal sdk >= 230, MTLBinaryArchive

    protected MetalWindowContext(NativeWindow nativeWindow, DisplayParams displayParams)
        : base(nativeWindow, displayParams) { }

    /// <summary>
    /// This should be called by subclass constructor. It is also called when window/display
    /// parameters change. This will in turn call onInitializeContext().
    /// </summary>
    protected void InitializeContext()
    {
        Device = MTLDevice.SystemDefault;
        Queue = Device!.CreateCommandQueue();
        if (DisplayParams.MSAASampleCount > 1)
        {
            //TODO:
        }

        SampleCount = DisplayParams.MSAASampleCount;
        StencilBits = 8;
        Valid = OnInitializeContext();

        GrContext = GRContext.CreateMetal(Device.Handle, Queue!.Handle);
    }

    protected abstract bool OnInitializeContext();

    /// <summary>
    /// This should be called by subclass destructor. It is also called when window/display
    /// parameters change prior to initializing a new Metal context. This will in turn call
    /// onDestroyContext().
    /// </summary>
    protected internal abstract void OnDestroyContext();

    public override unsafe Canvas GetOnScreenCanvas()
    {
        // IntPtr drawable; //测试下来无值
        // var surface = SKSurface.CreateFromMetalLayer(Context!, MetalLayer!.Handle,
        //     GRSurfaceOrigin.TopLeft, SampleCount, SKColorType.Bgra8888, 
        //     DisplayParams.ColorSpace, DisplayParams.SurfaceProps, out drawable);
        // DrawableHandler = ObjCRuntime.Runtime.GetINativeObject<CoreAnimation.ICAMetalDrawable> (drawable, true);

        var currentDrawable = MetalLayer!.NextDrawable();
        var fbInfo = new GRMtlTextureInfoNative();
        fbInfo.fTexture = (void*)currentDrawable!.Texture.Handle;

        var backendRt = GRBackendRenderTarget.CreateMetal(Width, Height, SampleCount, fbInfo);
        var surface = SKSurface.Create(GrContext!, backendRt, GRSurfaceOrigin.TopLeft,
            ColorType.Bgra8888, DisplayParams.ColorSpace, DisplayParams.SurfaceProps);
        backendRt.Dispose();

        DrawableHandler = currentDrawable;
        _onscreenCanvas = surface!.Canvas;
        return _onscreenCanvas;
    }

    public override Canvas GetOffScreenCanvas()
    {
        if (OffscreenCanvas != null) return OffscreenCanvas;

        var imageInfo = new ImageInfo { Width = Width, Height = Height, ColorType = ColorType.Bgra8888 };
        var surface = SKSurface.Create(GrContext!, true, imageInfo);
        OffscreenCanvas = surface!.Canvas;
        //直接缩放一次OffscreenCanvas，后续就不用处理了
        OffscreenCanvas.Scale(NativeWindow.ScaleFactor, NativeWindow.ScaleFactor);
        return OffscreenCanvas;
    }

    public override void SwapBuffers()
    {
        //flush and submit
        GrContext!.Flush();

        var commandBuffer = Queue!.CommandBufferWithUnretainedReferences(); //Queue!.CommandBuffer();
        commandBuffer!.AddCompletedHandler(buf => buf.Dispose());
        commandBuffer.Label = "Present";
        commandBuffer.PresentDrawable(DrawableHandler!);
        commandBuffer.Commit();
        // DrawableHandler!.Present();

        //clear back buffer resources
        _onscreenCanvas?.Dispose();
        _onscreenCanvas?.Surface?.Dispose();
        _onscreenCanvas = null;
        DrawableHandler!.Dispose();
        DrawableHandler = null;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            OffscreenCanvas?.Dispose();
            OffscreenCanvas?.Surface?.Dispose();
            OffscreenCanvas = null;

            GrContext?.AbandonContext(true);
            GrContext?.Dispose();
            GrContext = null;
            OnDestroyContext();

            MetalLayer?.Dispose();
            Queue?.Dispose();
            Device?.Dispose();
        }
    }
}