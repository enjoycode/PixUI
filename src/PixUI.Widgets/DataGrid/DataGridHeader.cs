using System.Collections.Generic;
using System.Linq;

namespace PixUI;

internal sealed class DataGridHeader<T> : Widget
{
    public DataGridHeader(DataGridController<T> controller)
    {
        _controller = controller;
    }

    private readonly DataGridController<T> _controller;

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        if (!_controller.HitTestInHeader(x, y))
            return false;

        result.Add(this);
        return true;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        SetSize(availableWidth, _controller.TotalHeaderHeight);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var size = new Size(_controller.DataGrid.W, _controller.DataGrid.H);
        PaintHeader(canvas, size, _controller.TotalColumnsWidth, _controller.CachedVisibleColumns);
    }

    private void PaintHeader(Canvas canvas, Size size, float totalColumnsWidth, IList<DataGridColumn<T>> visibleColumns)
    {
        var paintedGroupColumns = new List<DataGridGroupColumn<T>>();

        if (size.Width < totalColumnsWidth && _controller.HasFrozen)
        {
            //先画冻结列
            var frozenColumns = visibleColumns.Where(c => c.Frozen);
            foreach (var col in frozenColumns)
            {
                PaintHeaderCell(canvas, col, paintedGroupColumns);
            }

            //clip scroll region
            var clipRect = _controller.GetScrollClipRect(0, size.Height);
            canvas.Save();
            canvas.ClipRect(clipRect, ClipOp.Intersect, false);

            //再画其他列
            var noneFrozenColumns = visibleColumns.Where(c => !c.Frozen);
            foreach (var col in noneFrozenColumns)
            {
                PaintHeaderCell(canvas, col, paintedGroupColumns);
            }

            canvas.Restore();
        }
        else
        {
            foreach (var col in visibleColumns)
            {
                PaintHeaderCell(canvas, col, paintedGroupColumns);
            }
        }
    }

    private void PaintHeaderCell(Canvas canvas, DataGridColumn<T> column,
        IList<DataGridGroupColumn<T>> paintedGroupColumns)
    {
        var cellRect = GetHeaderCellRect(column);
        column.PaintHeader(canvas, cellRect, _controller.Theme);
        _controller.PaintCellBorder(canvas, cellRect);

        if (column.Parent != null && !paintedGroupColumns.Contains(column.Parent))
        {
            //因为布局时没有计算parent的位置
            var parent = column.Parent!;
            var index = parent.Children.IndexOf(column);
            var offsetLeft = 0.0f;
            for (var i = 0; i < index; i++)
            {
                offsetLeft += parent.Children[i].LayoutWidth;
            }

            parent.CachedLeft = column.CachedLeft - offsetLeft;

            PaintHeaderCell(canvas, parent, paintedGroupColumns);
            paintedGroupColumns.Add(parent);
        }
    }

    private Rect GetHeaderCellRect(DataGridColumn<T> column)
    {
        var rowIndex = column.HeaderRowIndex;
        var cellTop = rowIndex * _controller.HeaderRowHeight;
        var cellHeight = column is DataGridGroupColumn<T>
            ? _controller.HeaderRowHeight
            : (_controller.HeaderRows - rowIndex) * _controller.HeaderRowHeight;
        return Rect.FromLTWH(column.CachedLeft, cellTop, column.LayoutWidth, cellHeight);
    }
}