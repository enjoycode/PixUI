namespace PixUI;

public sealed class CellStyle
{
    public const float CellPadding = 5.0f;

    public CellStyle() { }

    public Color? Color;
    public Color? BackgroundColor;
    public float FontSize = 15;
    public FontWeight FontWeight = FontWeight.Normal;
    public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Left;
    public VerticalAlignment VerticalAlignment = VerticalAlignment.Middle;

    public CellStyle WithBackgroud(Color color)
    {
        BackgroundColor = color;
        return this;
    }

    public static CellStyle AlignCenter() => new() { HorizontalAlignment = HorizontalAlignment.Center };

    public static CellStyle AlignMiddleRight() => new() { HorizontalAlignment = HorizontalAlignment.Right };
}