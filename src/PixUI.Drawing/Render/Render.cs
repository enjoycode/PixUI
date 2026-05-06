namespace PixUI;

public interface IRender
{
    IColorSpace ColorSpaceSRGB { get; }

    IFontCollection FontCollection { get; }

    #region ====Paint====

    IPaint MakePaint();

    IPaint PaintShared(in Color? color = null, PaintStyle style = PaintStyle.Fill, float strokeWidth = 1);

    #endregion

    #region ====Image====

    IImage? MakeImageFromEncodedData(byte[] data);

    IImage? MakeImageFromEncodedData(Stream data);

    IImage MakeImageFromPicture(IPicture picture, SizeI size);

    IPictureRecorder MakePictureRecorder();

    #endregion

    #region ====Shader====

    IShader? MakeShaderRadialGradient(Point center, float radius, Color[] colors, float[]? colorPos, TileMode mode);

    IShader? MakeShaderLinearGradient(Point start, Point end, Color[] colors, float[]? colorPos, TileMode mode);

    #endregion

    #region ====Paragraph====

    ITextStyle MakeTextStyle();
    IParagraphStyle MakeParagraphStyle();
    IParagraphBuilder MakeParagraphBuilder(IParagraphStyle paragraphStyle);

    #endregion

    IMaskFilter? MakeMaskFilterBlur(BlurStyle blurStyle, float sigma);

    IImageFilter? MakeImageFilterDropShadow(float dx, float dy, float sigmaX, float sigmaY,
        Color color, IImageFilter? input);

    IImageFilter? MakeImageFilterBlur(float sigmaX, float sigmaY, TileMode tileMode, IImageFilter? input);

    IPath MakePath();

    IPath MakePathFromSvgData(string svgPath);

    IPathEffect? MakePathEffectDash(float[] intervals, float phase);

    #region ====Surface Factory====

    ISurface MakeSurface(ImageInfo imageInfo);
    ISurface MakeSurface(ImageInfo imageInfo, IntPtr pixels, int rowBytes);

    ISurface? MakeSurface(IGRContext context, bool budgeted, ImageInfo info,
        int sampleCount, SurfaceOrigin origin, ISurfaceProperties? props, bool shouldCreateWithMips);

    ISurface? MakeSurfaceForWebGL(IGRContext context, int width, int height);

    ISurface? MakeSurfaceForMetalWindow(IGRContext context, IntPtr textureHandle,
        int width, int height, int sampleCount,
        IColorSpace? colorSpace, ISurfaceProperties? surfaceProperties);

    ISurface? MakeSurfaceForDirect3DWindow(IGRContext context, IDirect3DSwapChain swapChain, int bufferIndex, 
        int width, int height,
        IColorSpace? colorSpace, ISurfaceProperties? surfaceProperties);

    #endregion

    #region ====GRContext Factory====

    IGRContext? MakeGRContextWebGL(int webglHandle);
    IGRContext? MakeGRContextMetal(IntPtr device, IntPtr queue);
    IGRContext? MakeGRContextDirect3D(out IDirect3DBackendContext direct3DBackendContext);

    #endregion

    #region ====Direct3D SwapChain====

    IDirect3DSwapChain MakeDirect3DSwapChain(IntPtr windowHandle, IDirect3DBackendContext direct3DBackendContext,
        uint width, uint height);

    #endregion
}

public static class Render
{
    public static IRender Backend { get; private set; } = null!;

    public static void Init(IRender backend)
    {
        Backend = backend;
    }
}