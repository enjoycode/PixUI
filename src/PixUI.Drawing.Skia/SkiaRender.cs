namespace PixUI.Drawing.Skia;

public sealed class SkiaRender : IRender
{
    public IFontCollection FontCollection { get; } = new SKFontCollection();

    public ISurface MakeSurface(ImageInfo imageInfo) => SKSurface.Create(imageInfo);

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

    public ITextStyle MakeTextStyle() => new SKTextStyle();

    public IParagraphStyle MakeParagraphStyle() => new SKParagraphStyle();

    public IParagraphBuilder MakeParagraphBuilder(IParagraphStyle paragraphStyle) =>
        new SKParagraphBuilder((SKParagraphStyle)paragraphStyle);

    public IPath MakePath() => new SKPath();

    public IPath MakePathFromSvgData(string svgPath) => SKPath.ParseSvgPathData(svgPath);

    public IPathEffect? MakePathEffectDash(float[] intervals, float phase) =>
        SKPathEffect.CreateDash(intervals, phase);
}