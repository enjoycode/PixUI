using System.Runtime.InteropServices;

namespace PixUI;

public interface IParagraph : IDisposable
{
    float Width { get; }
    float LongestLine { get; }
    float MaxIntrinsicWidth { get; }
    float Height { get; }
    int Lines { get; }

    LineMetrics GetLineMetricsAt(int lineNumber);

    PositionWithAffinity GetGlyphPositionAtCoordinate(float x, float y);

    TextBox GetRectForPosition(int pos, /*TODO: maybe need endPos for multi code unit*/
        BoxHeightStyle heightStyle, BoxWidthStyle widthStyle);

    void Layout(float width);
}

public interface ITextStyle : IDisposable
{
    Color Color { get; set; }
    float FontSize { get; set; }
    FontStyle FontStyle { get; set; }
    TextBaseline TextBaseline { get; set; }
    float Height { get; set; }
    void AddShadow(Shadow shadow);
    void ResetShadows();
    void SetFontFamilies(string[] familyNames);
}

public interface IParagraphStyle : IDisposable
{
    uint MaxLines { get; set; }
    TextAlign TextAlign { get; set; }
    float Height { get; set; }
    ITextStyle? TextStyle { get; set; }
}

public interface IParagraphBuilder : IDisposable
{
    void PushStyle(ITextStyle textStyle);
    void Pop();
    void AddText(string text);
    IParagraph Build();
}

public readonly struct PositionWithAffinity
{
    public readonly int Position;
    public readonly Affinity Affinity;

    public PositionWithAffinity(int pos, int affinity)
    {
        Position = pos;
        Affinity = (Affinity)affinity;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct TextBox
{
    public Rect Rect;
    public TextDirection Direction;
}

public static class TextStyle
{
    public static ITextStyle Create() => Render.Backend.MakeTextStyle();

    public static ITextStyle Create(Color color, float height)
    {
        var ts = Render.Backend.MakeTextStyle();
        ts.Color = color;
        ts.Height = height;
        return ts;
    }
}

public static class ParagraphStyle
{
    public static IParagraphStyle Create() => Render.Backend.MakeParagraphStyle();
}

public static class ParagraphBuilder
{
    public static IParagraphBuilder Create(IParagraphStyle paragraphStyle) =>
        Render.Backend.MakeParagraphBuilder(paragraphStyle);
}