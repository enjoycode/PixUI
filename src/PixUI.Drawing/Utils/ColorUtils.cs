using System;

namespace PixUI;

internal static class ColorUtils
{
    private static Color ScaleAlpha(Color a, double factor)
    {
        var alpha = Math.Clamp((byte)Math.Round(a.Alpha * factor), byte.MinValue,
            byte.MaxValue);
        return a.WithAlpha(alpha);
    }
        
    private static double LerpInt(int a, int b, double t) => a + (b - a) * t;

    internal static Color? Lerp(Color? a, Color? b, double t)
    {
        if (b == null)
            return a == null ? null : ScaleAlpha(a.Value, 1.0 - t);

        if (a == null) return ScaleAlpha(b.Value, t);

        var red = Math.Clamp((byte)LerpInt(a.Value.Red, b.Value.Red, t), byte.MinValue,
            byte.MaxValue);
        var green = Math.Clamp((byte)LerpInt(a.Value.Green, b.Value.Green, t), byte.MinValue,
            byte.MaxValue);
        var blue = Math.Clamp((byte)LerpInt(a.Value.Blue, b.Value.Blue, t), byte.MinValue,
            byte.MaxValue);
        var alpha = Math.Clamp((byte)LerpInt(a.Value.Alpha, b.Value.Alpha, t), byte.MinValue,
            byte.MaxValue);
        return new Color(red, green, blue, alpha);
    }
}