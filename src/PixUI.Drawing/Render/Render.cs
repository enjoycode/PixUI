namespace PixUI;

public interface IRender
{
    #region ====Font====

    IFontCollection FontCollection { get; }

    #endregion

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

    ISurface MakeSurface(ImageInfo imageInfo);
}

public static class Render
{
    public static IRender Backend { get; private set; } = null!;

    public static void Init(IRender backend)
    {
        Backend = backend;
    }
}