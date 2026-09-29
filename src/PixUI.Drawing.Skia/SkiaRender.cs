namespace PixUI.Drawing.Skia;

public sealed class SkiaRender : IRender
{
    public IColorSpace ColorSpaceSRGB => SKColorSpace.SRGB;

    public IFontCollection FontCollection { get; } = new SKFontCollection();

    public IPaint MakePaint() => new SKPaint();

    public IPaint PaintShared(in Color? color = null, PaintStyle style = PaintStyle.Fill, float strokeWidth = 1)
        => SKPaint.Shared(color, style, strokeWidth);

    public IImage? MakeImageFromEncodedData(byte[] data) => SKImage.FromEncodedData(data);

    public IImage? MakeImageFromEncodedData(Stream data) => SKImage.FromEncodedData(data);

    public IImage MakeImageFromPicture(IPicture picture, SizeI size) => SKImage.FromPicture((SKPicture)picture, size);

    public IPictureRecorder MakePictureRecorder() => new SKPictureRecorder();

    public IShader? MakeShaderRadialGradient(Point center, float radius, Color[] colors, float[]? colorPos,
        TileMode mode)
        => SKShader.CreateRadialGradient(center, radius, colors, colorPos, mode);

    public IShader? MakeShaderLinearGradient(Point start, Point end, Color[] colors, float[]? colorPos, TileMode mode)
        => SKShader.CreateLinearGradient(start, end, colors, colorPos, mode);

    public IMaskFilter? MakeMaskFilterBlur(BlurStyle blurStyle, float sigma)
        => SKMaskFilter.CreateBlur(blurStyle, sigma);

    public IImageFilter? MakeImageFilterDropShadow(float dx, float dy, float sigmaX, float sigmaY,
        Color color, IImageFilter? input)
        => SKImageFilter.CreateDropShadow(dx, dy, sigmaX, sigmaY, color, input as SKImageFilter);

    public IImageFilter? MakeImageFilterBlur(float sigmaX, float sigmaY, TileMode tileMode, IImageFilter? input)
        => SKImageFilter.CreateBlur(sigmaX, sigmaY, tileMode, input as SKImageFilter);

    public IColorFilter? MakeColorFilterBlendMode(Color color, BlendMode blendMode)
        => SKColorFilter.CreateBlendMode(color, blendMode);

    public ITextStyle MakeTextStyle() => new SKTextStyle();

    public IParagraphStyle MakeParagraphStyle() => new SKParagraphStyle();

    public IParagraphBuilder MakeParagraphBuilder(IParagraphStyle paragraphStyle) =>
        new SKParagraphBuilder((SKParagraphStyle)paragraphStyle);

    public ITextBlob? MakeTextBlob(ReadOnlySpan<char> text, IFont font, out float width, Point origin = default)
        => SKTextBlob.Create(text, font, out width, origin);

    public IPath MakePath() => new SKPath();

    public IPathBuilder MakePathBuilder() => new SKPathBuilder();

    public IPath MakePathFromSvgData(string svgPath) => SKPath.ParseSvgPathData(svgPath);

    public IPathEffect? MakePathEffectDash(float[] intervals, float phase) =>
        SKPathEffect.CreateDash(intervals, phase);

    public IPathEffect? MakePathEffectCorner(float radius) => SKPathEffect.CreateCorner(radius);

    public IGpuContext MakeWebGpuContext(IntPtr instanceId, IntPtr deviceId, IntPtr queueId)
    {
        var backendContext = new SKGraphiteDawnBackendContext
        {
            WgpuInstance = instanceId,
            WgpuDevice = deviceId,
            WgpuQueue = queueId,
        };
        return SKGraphiteContext.CreateDawn(backendContext)
               ?? throw new InvalidOperationException("SKGraphiteContext.CreateDawn returned null.");
    }

    public IGpuBackendTexture MakeWebGpuBackendTexture(IntPtr textureId)
    {
        return SKGraphiteBackendTexture.CreateDawn(textureId) ??
               throw new InvalidOperationException("SKGraphiteBackendTexture.CreateDawn returned null.");
    }

    public void FlushSurface(IGpuContext gpuContext, IGpuRecorder gpuRecorder)
    {
        var context = (SKGraphiteContext)gpuContext;
        var recorder = (SKGraphiteRecorder)gpuRecorder;
        using (var recording = recorder.Snap() ??
                               throw new InvalidOperationException("Recorder.Snap() returned null."))
        {
            if (context.InsertRecording(recording) != SKGraphiteInsertStatus.Success)
                throw new InvalidOperationException("InsertRecording did not report Success.");
        }

        var done = context.Submit(new SKGraphiteSubmitInfo { Sync = false });
#if DEBUG
        if (!done) throw new Exception("Failed to submit.");
#endif
    }

    public ISurface MakeSurface(IGpuRecorder recorder, IGpuBackendTexture backendTexture, ColorType colorType)
    {
#if SK_GRAPHITE
        return SKSurface.Create((SKGraphiteRecorder)recorder, (SKGraphiteBackendTexture)backendTexture, colorType)
               ?? throw new InvalidOperationException("SKSurface.Create returned null on Graphite.");
#else
        throw new NotSupportedException("MakeSurface for Graphite is not supported.");
#endif
    }

    public IGRContext? MakeGRContextWebGL()
    {
        var glInterface = GRGlInterface.Create();
        if (glInterface == null) throw new Exception("Can't create WebGL Interface");

        return GRContext.CreateGl(glInterface);
    }

    public IGRContext? MakeGRContextMetal(IntPtr device, IntPtr queue) => GRContext.CreateMetal(device, queue);

    public IGRContext? MakeGRContextDirect3D(out IDirect3DBackendContext direct3DBackendContext)
    {
        var backend = new Direct3DBackendContext(SkiaApi.gr_d3d_new_backend_context());
        direct3DBackendContext = backend;
        return GRContext.CreateDirect3D(backend.Handle);
    }

    public ISurface MakeSurface(ImageInfo imageInfo) => SKSurface.Create(imageInfo);

    public ISurface MakeSurface(ImageInfo imageInfo, IntPtr pixels, int rowBytes) =>
        SKSurface.Create(imageInfo, pixels, rowBytes);

    public ISurface? MakeSurface(IGRContext context, bool budgeted, ImageInfo info, int sampleCount,
        SurfaceOrigin origin, ISurfaceProperties? props, bool shouldCreateWithMips) =>
#if !SK_GRAPHITE
        SKSurface.Create((GRRecordingContext)context, budgeted, info, sampleCount, origin, props, shouldCreateWithMips);
#else
        throw new NotSupportedException("MakeSurface for Graphite is not supported.");
#endif

    public ISurface? MakeSurfaceForWebGL(IGRContext context, int width, int height) =>
#if !SK_GRAPHITE
        SKSurface.CreateGLOnScreen((GRContext)context, width, height);
#else
        throw new NotSupportedException("MakeSurface for Graphite is not supported.");
#endif

    public unsafe ISurface? MakeSurfaceForMetalWindow(IGRContext context, IntPtr textureHandle,
        int width, int height, int sampleCount,
        IColorSpace? colorSpace, ISurfaceProperties? surfaceProperties)
    {
#if !SK_GRAPHITE
        var fbInfo = new GRMtlTextureInfoNative();
        fbInfo.fTexture = (void*)textureHandle;

        using var backendRt = GRBackendRenderTarget.CreateMetal(width, height, sampleCount, fbInfo);
        return SKSurface.Create((GRContext)context, backendRt, SurfaceOrigin.TopLeft,
            ColorType.Bgra8888, colorSpace as SKColorSpace, surfaceProperties as SKSurfaceProperties);
#else
        throw new NotSupportedException("MakeSurface for Graphite is not supported.");
#endif
    }

    public ISurface? MakeSurfaceForDirect3DWindow(IGRContext context, IDirect3DSwapChain swapChain, int bufferIndex,
        int width, int height,
        IColorSpace? colorSpace, ISurfaceProperties? surfaceProperties)
    {
#if !SK_GRAPHITE
        var backBuffer = SkiaApi.gr_d3d_swapchain_get_buffer(((Direct3DSwapChain)swapChain).Handle, bufferIndex);
        using var backendRt = GRBackendRenderTarget.CreateDirect3D(width, height, backBuffer);
        return SKSurface.Create((GRContext)context, backendRt, SurfaceOrigin.TopLeft,
            ColorType.Rgba8888, colorSpace as SKColorSpace, surfaceProperties as SKSurfaceProperties);
#else
        throw new NotSupportedException("MakeSurface for Graphite is not supported.");
#endif
    }

    public IDirect3DSwapChain MakeDirect3DSwapChain(IntPtr windowHandle, IDirect3DBackendContext direct3DBackendContext,
        uint width, uint height) =>
#if !SK_GRAPHITE
        new Direct3DSwapChain(SkiaApi.gr_d3d_new_swapchain(windowHandle,
            ((Direct3DBackendContext)direct3DBackendContext).Handle, width, height));
#else
        throw new NotSupportedException("MakeSurface for Graphite is not supported.");
#endif

    public IDocument MakeDocumentPdf(Stream stream, float dpi) => SKDocument.CreatePdf(stream, dpi);
}