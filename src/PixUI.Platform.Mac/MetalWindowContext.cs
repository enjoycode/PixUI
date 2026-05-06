using CoreAnimation;
using Metal;

namespace PixUI.Platform.Mac;

public abstract class MetalWindowContext : NativeWindowContext
{
    private bool _valid;
    protected IMTLDevice? Device;
    private IMTLCommandQueue? _queue;
    protected CAMetalLayer? MetalLayer;
    private ICAMetalDrawable? _drawableHandler;
    private ICanvas? _onscreenCanvas;
    protected ICanvas? OffscreenCanvas;

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
        _queue = Device!.CreateCommandQueue();
        if (DisplayParams.MSAASampleCount > 1)
        {
            //TODO:
        }

        SampleCount = DisplayParams.MSAASampleCount;
        StencilBits = 8;
        _valid = OnInitializeContext();

        GrContext = Render.Backend.MakeGRContextMetal(Device.Handle, _queue!.Handle);
    }

    protected abstract bool OnInitializeContext();

    /// <summary>
    /// This should be called by subclass destructor. It is also called when window/display
    /// parameters change prior to initializing a new Metal context. This will in turn call
    /// onDestroyContext().
    /// </summary>
    protected abstract void OnDestroyContext();

    public override ICanvas GetOnScreenCanvas()
    {
        var currentDrawable = MetalLayer!.NextDrawable();
        var surface = Render.Backend.MakeSurfaceForMetalWindow(GrContext!, currentDrawable!.Texture.Handle,
            Width, Height, SampleCount,
            DisplayParams.ColorSpace, DisplayParams.SurfaceProps);

        _drawableHandler = currentDrawable;
        _onscreenCanvas = surface!.Canvas;
        return _onscreenCanvas;
    }

    public override ICanvas GetOffScreenCanvas()
    {
        if (OffscreenCanvas != null) return OffscreenCanvas;

        var imageInfo = new ImageInfo { Width = Width, Height = Height, ColorType = ColorType.Bgra8888 };
        var surface = Surface.Create(GrContext!, true, imageInfo);
        OffscreenCanvas = surface!.Canvas;
        //直接缩放一次OffscreenCanvas，后续就不用处理了
        OffscreenCanvas.Scale(NativeWindow.ScaleFactor, NativeWindow.ScaleFactor);
        return OffscreenCanvas;
    }

    public override void SwapBuffers()
    {
        //flush and submit
        GrContext!.Flush(true);

        var commandBuffer = _queue!.CommandBufferWithUnretainedReferences(); //Queue!.CommandBuffer();
        commandBuffer!.AddCompletedHandler(buf => buf.Dispose());
        commandBuffer.Label = "Present";
        commandBuffer.PresentDrawable(_drawableHandler!);
        commandBuffer.Commit();
        // DrawableHandler!.Present();

        //clear back buffer resources
        _onscreenCanvas?.Dispose();
        _onscreenCanvas?.Surface?.Dispose();
        _onscreenCanvas = null;
        _drawableHandler!.Dispose();
        _drawableHandler = null;
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
            _queue?.Dispose();
            Device?.Dispose();
        }
    }
}