using System;

namespace PixUI;

public static class PaintUtils
{
    [Obsolete("Use Paint.Shared()")]
    public static Paint Shared(in Color? color = null, PaintStyle style = PaintStyle.Fill,
        float strokeWidth = 1) => Paint.Shared(color, style, strokeWidth);
}