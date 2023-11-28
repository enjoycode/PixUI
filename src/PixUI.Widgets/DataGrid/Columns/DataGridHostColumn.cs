using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PixUI;

internal interface IDataGridHostColumn
{
    float LeftToDataGrid { get; }
}

/// <summary>
/// 用于DataGridHostColumn承载单元格组件
/// </summary>
internal sealed class HostedCellWidget : Widget
{
    public HostedCellWidget(IScrollable dataGridBody, IDataGridHostColumn column, Widget child, float offsetYToDataGrid)
    {
        IsLayoutTight = false; //充满单元格
        child.Parent = this;
        _hostedWidget = child;
        _dataGridBody = dataGridBody;
        _column = column;
        _offsetYToDataGrid = offsetYToDataGrid;
    }

    private readonly Widget _hostedWidget;

    /// <summary>
    /// 不包含滚动偏移量的相对于DataGrid的Y位置
    /// </summary>
    private readonly float _offsetYToDataGrid;

    private readonly IScrollable _dataGridBody;
    private readonly IDataGridHostColumn _column;

    protected internal override float X => _column.LeftToDataGrid;

    protected internal override float Y => _offsetYToDataGrid - _dataGridBody.ScrollOffsetY;

    public override void VisitChildren(Func<Widget, bool> action) => action(_hostedWidget);

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        SetSize(width, height);
        _hostedWidget.Layout(width, height);
        //TODO:根据对齐方式设置位置，暂简单居中
        _hostedWidget.SetPosition((width - _hostedWidget.W) / 2, 0);
    }
}

/// <summary>
/// 用于承载Widget的列
/// </summary>
public class DataGridHostColumn<T> : DataGridColumn<T>, IDataGridHostColumn
{
    protected DataGridHostColumn(string label, Func<T, int, Widget> cellBuilder) : base(label)
    {
        _cellBuilder = cellBuilder;
    }

    private readonly Func<T, int, Widget> _cellBuilder;
    private readonly List<CellCache<Widget>> _cellWidgets = new();

    private static readonly CellCacheComparer<Widget> _cellCacheComparer = new();

    public float LeftToDataGrid => CachedLeft;

    protected internal override void PaintCell(Canvas canvas, DataGridController<T> controller, int rowIndex,
        Rect cellRect)
    {
        var cellWidget = GetOrMakeCellWidget(rowIndex, controller, cellRect);
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
        var hostedWidget = new HostedCellWidget(controller.DataGrid.Body, this, cellWidget,
            cellRect.Top + controller.ScrollController.OffsetY);
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
        var index = _cellWidgets.BinarySearch(pattern, _cellCacheComparer);
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