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