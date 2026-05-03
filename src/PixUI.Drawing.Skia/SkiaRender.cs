namespace PixUI;

public sealed class SkiaRender : IRender
{
    public IFontCollection FontCollection { get; } = new SKFontCollection();

    public IPaint MakePaint() => new SKPaint();

    public IPaint PaintShared(in Color? color = null, PaintStyle style = PaintStyle.Fill, float strokeWidth = 1)
        => SKPaint.Shared(color, style, strokeWidth);

    public IImage? ImageFromEncodedData(byte[] data) => SKImage.FromEncodedData(data);

    public IImage? ImageFromEncodedData(Stream data) => SKImage.FromEncodedData(data);

    public IShader? ShaderCreateRadialGradient(Point center, float radius, Color[] colors, float[]? colorPos,
        TileMode mode)
        => SKShader.CreateRadialGradient(center, radius, colors, colorPos, mode);

    public IShader? ShaderCreateLinearGradient(Point start, Point end, Color[] colors, float[]? colorPos, TileMode mode)
        => SKShader.CreateLinearGradient(start, end, colors, colorPos, mode);

    public IPath MakePath() => new SKPath();

    public IPath PathFromSvgData(string svgPath) => SKPath.ParseSvgPathData(svgPath);

    public IPathEffect? PathEffectCreateDash(float[] intervals, float phase) =>
        SKPathEffect.CreateDash(intervals, phase);
}