using System;

namespace PixUI;

public enum ColumnWidthType
{
    /// <summary>
    /// 均分剩余宽度
    /// </summary>
    Auto,

    /// <summary>
    /// 按百分比分配剩余宽度
    /// </summary>
    Percent,

    /// <summary>
    /// 固定宽度
    /// </summary>
    Fixed
}

public sealed class ColumnWidth
{
    private ColumnWidth(ColumnWidthType type, float value, float minValue)
    {
        Type = type;
        Value = value;
        MinValue = minValue;
    }

    public ColumnWidthType Type { get; }

    public float Value { get; private set; } //定义的值，可能为百分比或固定值

    public float MinValue { get; } //非固定值时的最小宽度

    public static ColumnWidth Percent(float percent, float min = 20)
    {
        percent = Math.Clamp(percent, 0, 100);
        return new ColumnWidth(ColumnWidthType.Percent, percent, min);
    }

    public static ColumnWidth Auto(float min = 20)
    {
        return new ColumnWidth(ColumnWidthType.Auto, 0, min);
    }

    public static ColumnWidth Fixed(float width)
    {
        width = Math.Max(0, width);
        return new ColumnWidth(ColumnWidthType.Fixed, width, 0);
    }

    public static ColumnWidth Parse(string value)
    {
        if (string.IsNullOrEmpty(value)) return Auto();
        if (value.EndsWith('%'))
            return float.TryParse(value.AsSpan(0, value.Length - 1), out var p) ? Percent(p) : Percent(0);
        return float.TryParse(value, out var v) ? Fixed(v) : Fixed(20);
    }

    internal void ChangeValue(float newValue)
    {
        Value = newValue;
    }
}