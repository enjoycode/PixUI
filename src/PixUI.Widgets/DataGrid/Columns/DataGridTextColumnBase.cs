using System;
using System.Collections.Generic;

namespace PixUI;

public abstract class DataGridTextColumnBase<T> : DataGridColumn<T>
{
    protected DataGridTextColumnBase(string label, Func<T, int, string> cellValueGetter) : base(label)
    {
        _cellValueGetter = cellValueGetter;
    }

    protected readonly Func<T, int, string> _cellValueGetter;

    private readonly List<CellCache<Paragraph>> _cellParagraphs = new();

    protected internal override void PaintCell(Canvas canvas, DataGridController<T> controller, 
        int rowIndex, Rect cellRect)
    {
        //先画背景
        var cellStyle = PaintCellBackground(canvas, controller, rowIndex, cellRect);
        //再画文本
        var row = controller.DataView![rowIndex];
        var cellValue = _cellValueGetter(row, rowIndex);
        if (string.IsNullOrEmpty(cellValue)) return;

        var ph = GetCellParagraph(rowIndex, cellRect, cellValue, cellStyle, controller.Theme.DefaultRowCellStyle);
        DataGridPainter.PaintCellParagraph(canvas, cellRect, cellStyle, ph);
    }

    /// <summary>
    /// 从缓存中获取承载的Widget,没有则新建并加入缓存
    /// </summary>
    private Paragraph GetCellParagraph(int rowIndex, in Rect cellRect, string cellValue,
        CellStyle style, CellStyle defaultStyle)
    {
        var pattern = new CellCache<Paragraph>(rowIndex, null);
        var index = _cellParagraphs.BinarySearch(pattern, CellCacheComparerForParagraph.Instance);
        if (index >= 0)
            return _cellParagraphs[index].CachedItem!;

        index = ~index;
        //没找到开始新建
        var ph = DataGridPainter.BuildCellParagraph(cellRect, style, defaultStyle, cellValue, 1);
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