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

    IImage? ImageFromEncodedData(byte[] data);

    IImage? ImageFromEncodedData(Stream data);

    IImage ImageFromPicture(IPicture picture, SizeI size);

    IPictureRecorder MakePictureRecorder();

    #endregion

    #region ====Shader====

    IShader? ShaderCreateRadialGradient(Point center, float radius, Color[] colors, float[]? colorPos, TileMode mode);

    IShader? ShaderCreateLinearGradient(Point start, Point end, Color[] colors, float[]? colorPos, TileMode mode);

    #endregion

    #region ====Paragraph====

    ITextStyle MakeTextStyle();
    IParagraphStyle MakeParagraphStyle();
    IParagraphBuilder MakeParagraphBuilder(IParagraphStyle paragraphStyle);

    #endregion

    IMaskFilter? MaskFilterCreateBlur(BlurStyle blurStyle, float sigma);

    IImageFilter? ImageFilterCreateDropShadow(float dx, float dy, float sigmaX, float sigmaY, 
        Color color, IImageFilter? input);

    IPath MakePath();

    IPath PathFromSvgData(string svgPath);

    IPathEffect? PathEffectCreateDash(float[] intervals, float phase);
}

public static class Render
{
    public static IRender Provider { get; private set; } = null!;

    public static void Init(IRender provider)
    {
        Provider = provider;
    }
}