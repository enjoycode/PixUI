namespace PixUI;

public interface IPaint : IDisposable
{
    PaintStyle Style { get; set; }
    Color Color { get; set; }
    float StrokeWidth { get; set; }
    BlendMode BlendMode { get; set; }
    bool AntiAlias { get; set; }
    float StrokeMiter { get; set; }
    StrokeCap StrokeCap { get; set; }
    StrokeJoin StrokeJoin { get; set; }
    IPathEffect? PathEffect { get; set; }
    IShader? Shader { get; set; }
    IMaskFilter? MaskFilter { get; set; }
    IImageFilter? ImageFilter { get; set; }
    void Reset();
}

public static class Paint
{
    public static IPaint Shared(in Color? color = null, PaintStyle style = PaintStyle.Fill, float strokeWidth = 1)
        => Render.Provider.PaintShared(color, style, strokeWidth);

    public static IPaint Create() => Render.Provider.MakePaint();

    public static IPaint Create(Color color)
    {
        var paint = Render.Provider.MakePaint();
        paint.Color = color;
        return paint;
    }

    public static IPaint Create(Color color, PaintStyle style)
    {
        var paint = Render.Provider.MakePaint();
        paint.Color = color;
        paint.Style = style;
        return paint;
    }
}