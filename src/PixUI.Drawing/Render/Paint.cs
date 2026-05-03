namespace PixUI;

public static class Paint
{
    public static IPaint Shared(in Color? color = null, PaintStyle style = PaintStyle.Fill, float strokeWidth = 1)
        => Render.Provider.PaintShared(color, style, strokeWidth);
}