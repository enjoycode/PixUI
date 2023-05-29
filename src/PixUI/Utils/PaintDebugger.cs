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

        var paint = PaintUtils.Shared(Colors.Random(125), PaintStyle.Stroke, 2);
        canvas.DrawRect(Rect.FromLTWH(widget.X + 1, widget.Y + 1, widget.W - 2, widget.H - 2),
            paint);
    }
}