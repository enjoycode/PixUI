namespace PixUI;

public interface IPaint : IDisposable
{
    PaintStyle Style { get; set; }
    Color Color { get; set; }
    float StrokeWidth { get; set; }
    BlendMode BlendMode { get; set; }
    bool AntiAlias { get; set; }
}