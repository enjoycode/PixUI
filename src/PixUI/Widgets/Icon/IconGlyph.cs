namespace PixUI;

public struct IconGlyph : IDisposable
{
    public IconGlyph(in IconData data, float fontSize)
    {
        Font = data.Asset.Typeface.MakeFont(fontSize);
        GlyphId = Font.GetGlyph(data.CodePoint);
    }

    public IFont? Font { get; private set; }
    public ushort GlyphId { get; internal set; }

    public readonly void Paint(ICanvas canvas, Color textColor, float offsetX = 0, float offsetY = 0)
    {
        if (Font == null) return;
        
        var paint = PixUI.Paint.Shared(textColor);
        canvas.DrawGlyph(GlyphId, offsetX, Font.Size + offsetY, 0, 0, Font, paint);
    }

    public readonly void Dispose()
    {
        if (Font != null!) Font.Dispose();
    }
}