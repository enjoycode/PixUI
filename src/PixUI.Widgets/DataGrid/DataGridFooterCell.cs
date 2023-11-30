using System;

namespace PixUI;

public sealed class DataGridFooterCell
{
    public DataGridFooterCell(int columnIndex, ContentType contentType = ContentType.Text)
    {
        if (columnIndex < 0)
            throw new IndexOutOfRangeException();
        BeginColumnIndex = EndColumnIndex = columnIndex;
        Content = contentType;
    }

    public DataGridFooterCell(int columnIndex, Func<string> contentBuilder) : this(columnIndex, ContentType.Custom)
    {
        CustomBuilder = contentBuilder;
    }

    public DataGridFooterCell(int beginColumnIndex, int endColumnIndex, ContentType contentType = ContentType.Text)
    {
        if (beginColumnIndex < 0 || endColumnIndex < beginColumnIndex)
            throw new IndexOutOfRangeException();

        if (endColumnIndex > beginColumnIndex && contentType != ContentType.Text)
            throw new NotSupportedException("只能聚合单列的值");

        BeginColumnIndex = beginColumnIndex;
        EndColumnIndex = endColumnIndex;
        Content = contentType;
    }

    public readonly int BeginColumnIndex;
    public readonly int EndColumnIndex;
    public readonly ContentType Content;

    public bool IsSingleColumn => BeginColumnIndex == EndColumnIndex;

    /// <summary>
    /// 文本内容，另外内容为聚合值时作为前缀：eg: "合计:" + 聚合值
    /// </summary>
    public string? Text { get; set; }

    public CellStyle? CellStyle { get; set; }

    public Func<string>? CustomBuilder { get; set; }
    
    //TODO:考虑缓存计算的值，DataView变更后清除缓存

    public bool Contains(int colIndex) => colIndex >= BeginColumnIndex && colIndex <= EndColumnIndex;

    public enum ContentType
    {
        Text,
        Sum,
        Avg,
        Custom,
    }
}