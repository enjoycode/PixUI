using System;

namespace PixUI;

/// <summary>
/// 行号列
/// </summary>
public sealed class DataGridRowNumColumn<T> : DataGridTextColumnBase<T>
{
    public DataGridRowNumColumn(string label) : base(label, (_, i) => $"{i + 1}")
    {
        CellStyle = DefaultCellStyleForRowNum.Instance;
    }
}

internal static class DefaultCellStyleForRowNum
{
    internal static readonly CellStyle Instance = new() { HorizontalAlignment = HorizontalAlignment.Center };
}