namespace PixUI;

public interface IPaint : IDisposable
{
    PaintStyle Style { get; set; }
    Color Color { get; set; }
    float StrokeWidth { get; set; }
    BlendMode BlendMode { get; set; }
    bool AntiAlias { get; set; }
    StrokeCap StrokeCap { get; set; }
    IPathEffect? PathEffect { get; set; }
    IShader? Shader { get; set; }
    void Reset();
}

public static class Paint
{
    public static IPaint Shared(in Color? color = null, PaintStyle style = PaintStyle.Fill, float strokeWidth = 1)
        => Render.Provider.PaintShared(color, style, strokeWidth);

    public static IPaint Create() => Render.Provider.MakePaint();
}