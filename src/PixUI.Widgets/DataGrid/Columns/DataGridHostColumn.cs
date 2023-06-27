using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PixUI;

/// <summary>
/// 用于承载Widget的列
/// </summary>
public class DataGridHostColumn<T> : DataGridColumn<T>
{
    public DataGridHostColumn(string label, Func<T, int, Widget> cellBuilder) : base(label)
    {
        _cellBuilder = cellBuilder;
    }

    private readonly Func<T, int, Widget> _cellBuilder;
    private readonly List<CellCache<Widget>> _cellWidgets = new();

    private static readonly CellCacheComparer<Widget> _cellCacheComparer = new();

    internal override void PaintCell(Canvas canvas, DataGridController<T> controller, int rowIndex, Rect cellRect)
    {
        var cellWidget = GetOrMakeCellWidget(rowIndex, controller, cellRect);
        //TODO:对齐cellWidget
        canvas.Translate(cellRect.Left, cellRect.Top);
        cellWidget.Paint(canvas, null);
        canvas.Translate(-cellRect.Left, -cellRect.Top);
    }

    /// <summary>
    /// 从缓存中获取承载的Widget,没有则新建并加入缓存
    /// </summary>
    private Widget GetOrMakeCellWidget(int rowIndex, DataGridController<T> controller, in Rect cellRect)
    {
        var pattern = new CellCache<Widget>(rowIndex, null);
        var index = _cellWidgets.BinarySearch(pattern, _cellCacheComparer);
        if (index >= 0)
            return _cellWidgets[index].CachedItem!;

        index = ~index;
        //没找到开始新建
        var row = controller.DataView![rowIndex];
        var cellWidget = _cellBuilder(row, rowIndex);
        cellWidget.Parent = controller.DataGrid;
        cellWidget.Layout(cellRect.Width, cellRect.Height);
        //需要设置相对于DataGrid的位置(不考虑滚动变量)
        cellWidget.SetPosition(cellRect.Left, cellRect.Top + controller.ScrollController.OffsetY);
        var cellCachedWidget = new CellCache<Widget>(rowIndex, cellWidget);
        _cellWidgets.Insert(index, cellCachedWidget);
        return cellWidget;
    }

    internal Widget GetCellWidget(int rowIndex)
    {
        var pattern = new CellCache<Widget>(rowIndex, null);
        var index = _cellWidgets.BinarySearch(pattern, _cellCacheComparer);
        Debug.Assert(index >= 0);
        return _cellWidgets[index].CachedItem!;
    }

    internal override void ClearCacheOnScroll(bool isScrollDown, int rowIndex)
    {
        if (isScrollDown)
            _cellWidgets.RemoveAll(t => t.RowIndex < rowIndex);
        else
            _cellWidgets.RemoveAll(t => t.RowIndex >= rowIndex);
    }
}