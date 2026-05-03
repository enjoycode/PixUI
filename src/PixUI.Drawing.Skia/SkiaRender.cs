namespace PixUI;

public sealed class SkiaRender : IRender
{
    public IFontCollection FontCollection { get; } = new SKFontCollection();
    
    public IPaint PaintShared(in Color? color = null, PaintStyle style = PaintStyle.Fill, float strokeWidth = 1)
        => SKPaint.Shared(color, style, strokeWidth);

    public IImage? ImageFromEncodedData(byte[] data) => SKImage.FromEncodedData(data);

    public IImage? ImageFromEncodedData(Stream data) => SKImage.FromEncodedData(data);
}