using System;

namespace PixUI;

public static class PaintDebugger
{
    public static event Action? EnableChanged;

    private static bool _enable;

    public static void Switch()
    {
        _enable = !_enable;
        EnableChanged?.Invoke();
    }

    internal static void PaintWidgetBorder(Widget widget, Canvas canvas)
    {
        if (!_enable) return;

        var color = Colors.Random(125);
        var paint = PaintUtils.Shared(color, PaintStyle.Stroke, 2);
        canvas.DrawRect(Rect.FromLTWH(widget.X + 1, widget.Y + 1, widget.W - 2, widget.H - 2), paint);
        
        // using var ph = TextPainter.BuildParagraph(widget.ToString(), float.MaxValue, 12, color);
        // canvas.DrawParagraph(ph, widget.X + 2, widget.Y + 2);
    }
}