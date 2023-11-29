using System;
using System.Collections.Generic;

namespace PixUI;

public abstract class DataGridTextColumnBase<T> : DataGridColumn<T>
{
    protected DataGridTextColumnBase(string label, Func<T, int, string> cellValueGetter) : base(label)
    {
        _cellValueGetter = cellValueGetter;
    }

    private readonly Func<T, int, string> _cellValueGetter;

    private readonly List<CellCache<Paragraph>> _cellParagraphs = new();

    protected internal override void PaintCell(Canvas canvas, DataGridController<T> controller, int rowIndex,
        Rect cellRect)
    {
        var row = controller.DataView![rowIndex];
        var cellValue = _cellValueGetter(row, rowIndex);
        if (string.IsNullOrEmpty(cellValue)) return;

        var style = CellStyleGetter != null
            ? CellStyleGetter(row, rowIndex)
            : CellStyle ?? controller.Theme.DefaultRowCellStyle;

        var ph = GetCellParagraph(rowIndex, cellRect, cellValue, style);
        PaintCellParagraph(canvas, cellRect, style, ph);
    }

    /// <summary>
    /// 从缓存中获取承载的Widget,没有则新建并加入缓存
    /// </summary>
    private Paragraph GetCellParagraph(int rowIndex, in Rect cellRect, string cellValue, CellStyle style)
    {
        var pattern = new CellCache<Paragraph>(rowIndex, null);
        var index = _cellParagraphs.BinarySearch(pattern, CellCacheComparerForParagraph.Instance);
        if (index >= 0)
            return _cellParagraphs[index].CachedItem!;

        index = ~index;
        //没找到开始新建
        var ph = BuildCellParagraph(cellRect, style, cellValue, 1);
        var cellCachedWidget = new CellCache<Paragraph>(rowIndex, ph);
        _cellParagraphs.Insert(index, cellCachedWidget);
        return ph;
    }

    protected internal override void ClearAllCache()
    {
        _cellParagraphs.Clear();
    }

    protected internal override void ClearCacheAt(int rowIndex)
    {
        var pattern = new CellCache<Paragraph>(rowIndex, null);
        var index = _cellParagraphs.BinarySearch(pattern, CellCacheComparerForParagraph.Instance);
        if (index >= 0)
            _cellParagraphs.RemoveAt(index);
    }

    protected internal override void ClearCacheOnScroll(bool isScrollDown, int rowIndex)
    {
        if (isScrollDown)
            _cellParagraphs.RemoveAll(t => t.RowIndex < rowIndex);
        else
            _cellParagraphs.RemoveAll(t => t.RowIndex >= rowIndex);
    }
}

internal static class CellCacheComparerForParagraph
{
    internal static readonly CellCacheComparer<Paragraph> Instance = new();
}