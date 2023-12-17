using System;

namespace PixUI;

public sealed class CellStyle
{
    public const float CellPadding = 5.0f;

    /// <summary>
    /// 单元格文本颜色
    /// </summary>
    public Color? TextColor;

    /// <summary>
    /// 单元格背景颜色
    /// </summary>
    public Color? FillColor;

    [Obsolete("Use TextColor")]
    public Color? Color
    {
        get => TextColor;
        set => TextColor = value;
    }

    [Obsolete("Use FillColor")]
    public Color? BackgroundColor
    {
        get => FillColor;
        set => FillColor = value;
    }

    public float FontSize = 15;
    public FontWeight FontWeight = FontWeight.Normal;
    public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Left;
    public VerticalAlignment VerticalAlignment = VerticalAlignment.Middle;

    public CellStyle WithFillColor(Color color)
    {
        FillColor = color;
        return this;
    }

    public static CellStyle AlignCenter() => new() { HorizontalAlignment = HorizontalAlignment.Center };

    public static CellStyle AlignMiddleRight() => new() { HorizontalAlignment = HorizontalAlignment.Right };
}