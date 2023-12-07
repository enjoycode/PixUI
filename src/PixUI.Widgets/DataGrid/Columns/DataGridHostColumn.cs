using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PixUI;

internal interface IDataGridHostColumn
{
    /// <summary>
    /// 已减横向滚动偏移量的X坐标
    /// </summary>
    float LeftToDataGrid { get; }
}

/// <summary>
/// 用于承载Widget的列
/// </summary>
public class DataGridHostColumn<T> : DataGridColumn<T>, IDataGridHostColumn
{
    public DataGridHostColumn(string label, Func<T, int, Widget> cellBuilder) : base(label)
    {
        _cellBuilder = cellBuilder;
    }

    private readonly Func<T, int, Widget> _cellBuilder;
    private readonly List<CellCache<Widget>> _cellWidgets = new();

    public float LeftToDataGrid => CachedLeft;

    protected internal override void PaintCell(Canvas canvas, DataGridController<T> controller, int rowIndex,
        Rect cellRect)
    {
        var cellWidget = GetOrMakeCellWidget(rowIndex, controller, cellRect);
        canvas.Translate(cellRect.Left, cellRect.Top);
        // cellWidget.BeforePaint(canvas);
        cellWidget.Paint(canvas, null);
        // cellWidget.AfterPaint(canvas);
        canvas.Translate(-cellRect.Left, -cellRect.Top);
    }

    /// <summary>
    /// 从缓存中获取承载的Widget,没有则新建并加入缓存
    /// </summary>
    private Widget GetOrMakeCellWidget(int rowIndex, DataGridController<T> controller, in Rect cellRect)
    {
        var pattern = new CellCache<Widget>(rowIndex, null);
        var index = _cellWidgets.BinarySearch(pattern, CellCacheComparerForWidget.Instance);
        if (index >= 0)
            return _cellWidgets[index].CachedItem!;

        index = ~index;
        //没找到开始新建
        var row = controller.DataView![rowIndex];
        var cellWidget = _cellBuilder(row, rowIndex);
        var hostedWidget = new HostedCellWidget(controller.DataGrid.Body, this, cellWidget,
            rowIndex * controller.Theme.RowHeight);
        hostedWidget.Parent = controller.DataGrid.Body;
        hostedWidget.Layout(cellRect.Width, cellRect.Height);
        //不需要设置hostedWidget的位置(动态计算)
        var cellCachedWidget = new CellCache<Widget>(rowIndex, hostedWidget);
        _cellWidgets.Insert(index, cellCachedWidget);
        return hostedWidget;
    }

    internal Widget? GetCellWidget(int rowIndex)
    {
        var pattern = new CellCache<Widget>(rowIndex, null);
        var index = _cellWidgets.BinarySearch(pattern, CellCacheComparerForWidget.Instance);
        //TODO:临时修复由于快速滚动后开始新的HitTest,但当前rowIndex承载的Widget还未创建
        if (index < 0) return null;
        //Debug.Assert(index >= 0);
        return _cellWidgets[index].CachedItem!;
    }

    protected internal override void ClearAllCache() => _cellWidgets.Clear();

    protected internal override void OnWidthChanged(float width, float height)
    {
        //尽量重用缓存的Widget，所以不用ClearAllCache
        foreach (var cellWidget in _cellWidgets)
        {
            cellWidget.CachedItem?.Layout(width, height);
        }
    }

    protected internal override void ClearCacheOnScroll(bool isScrollDown, int rowIndex)
    {
        if (isScrollDown)
            _cellWidgets.RemoveAll(t => t.RowIndex < rowIndex);
        else
            _cellWidgets.RemoveAll(t => t.RowIndex >= rowIndex);
    }
}

internal static class CellCacheComparerForWidget
{
    internal static readonly CellCacheComparer<Widget> Instance = new();
}