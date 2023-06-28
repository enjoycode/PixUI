using System;
using System.Collections.Generic;

namespace PixUI;

public sealed class DataGridTextColumn<T> : DataGridColumn<T>
{
    public DataGridTextColumn(string label, Func<T, string> cellValueGetter) : base(label)
    {
        _cellValueGetter = cellValueGetter;
    }

    private readonly Func<T, string> _cellValueGetter;

    private readonly List<CellCache<Paragraph>> _cellParagraphs = new();

    private static readonly CellCacheComparer<Paragraph> _cellCacheComparer = new();

    internal override void PaintCell(Canvas canvas, DataGridController<T> controller,
        int rowIndex, Rect cellRect)
    {
        var row = controller.DataView![rowIndex];
        var cellValue = _cellValueGetter(row);
        if (string.IsNullOrEmpty(cellValue)) return;

        var style = CellStyleGetter != null
            ? CellStyleGetter(row, rowIndex)
            : CellStyle ?? controller.Theme.DefaultRowCellStyle;

        var ph = GetCellParagraph(rowIndex, controller, cellRect, cellValue, style);
        PaintCellParagraph(canvas, cellRect, style, ph);
    }

    /// <summary>
    /// 从缓存中获取承载的Widget,没有则新建并加入缓存
    /// </summary>
    private Paragraph GetCellParagraph(int rowIndex, DataGridController<T> controller,
        in Rect cellRect, string cellValue, CellStyle style)
    {
        var pattern = new CellCache<Paragraph>(rowIndex, null);
        var index = _cellParagraphs.BinarySearch(pattern, _cellCacheComparer);
        if (index >= 0)
            return _cellParagraphs[index].CachedItem!;

        index = ~index;
        //没找到开始新建
        // var row = controller.DataView![rowIndex];
        var ph = BuildCellParagraph(cellRect, style, cellValue, 1);
        var cellCachedWidget = new CellCache<Paragraph>(rowIndex, ph);
        _cellParagraphs.Insert(index, cellCachedWidget);
        return ph;
    }

    protected internal override void ClearAllCache()
    {
        _cellParagraphs.Clear();
    }

    protected internal override void ClearCacheOnScroll(bool isScrollDown, int rowIndex)
    {
        if (isScrollDown)
            _cellParagraphs.RemoveAll(t => t.RowIndex < rowIndex);
        else
            _cellParagraphs.RemoveAll(t => t.RowIndex >= rowIndex);
    }
}