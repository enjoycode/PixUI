using System;
using System.Collections.Generic;
using System.Linq;

namespace PixUI;

public sealed class DataGrid<T> : Widget, IScrollable, IMouseRegion
{
    public DataGrid(DataGridController<T> controller, DataGridTheme? theme = null)
    {
        _controller = controller;
        _controller.Attach(this);

        Theme = theme ?? DataGridTheme.Default;

        MouseRegion = new MouseRegion(null, false);
        MouseRegion.PointerMove += _controller.OnPointerMove;
        MouseRegion.PointerDown += _controller.OnPointerDown;
    }

    private readonly DataGridController<T> _controller;
    internal readonly DataGridTheme Theme;

    public DataGridColumns<T> Columns => _controller.Columns;

    public MouseRegion MouseRegion { get; }

    #region ====IScrollable====

    public float ScrollOffsetX => _controller.ScrollController.OffsetX;
    public float ScrollOffsetY => _controller.ScrollController.OffsetY;

    public Offset OnScroll(float dx, float dy)
    {
        if (_controller.DataView == null || _controller.DataView.Count == 0)
            return Offset.Empty;

        var totalRowsHeight = _controller.TotalRowsHeight;
        var totalHeaderHeight = _controller.TotalHeaderHeight;
        var maxOffsetX = Math.Max(0, _controller.TotalColumnsWidth - W);
        var maxOffsetY = Math.Max(0, totalRowsHeight - (H - totalHeaderHeight));

        var oldVisibleRowStart = _controller.VisibleStartRowIndex;
        var visibleRows = _controller.VisibleRows;

        var offset = _controller.ScrollController.OnScroll(dx, dy, maxOffsetX, maxOffsetY);
        if (!offset.IsEmpty)
        {
            //根据向上或向下滚动计算需要清除缓存的边界, TODO:考虑多一部分范围，现暂超出范围即清除
            if (dy > 0)
            {
                var newVisibleRowStart = _controller.VisibleStartRowIndex;
                if (newVisibleRowStart != oldVisibleRowStart)
                    _controller.ClearCacheOnScroll(true, newVisibleRowStart);
            }
            else
            {
                var oldVisibleRowEnd = oldVisibleRowStart + visibleRows;
                var newVisibleRowEnd = _controller.VisibleStartRowIndex + visibleRows;
                if (oldVisibleRowEnd != newVisibleRowEnd)
                    _controller.ClearCacheOnScroll(false, newVisibleRowEnd);
            }

            Invalidate(InvalidAction.Repaint);
        }

        return offset;
    }

    #endregion

    #region ====Overrides====

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        if (!ContainsPoint(x, y)) return false;

        result.Add(this);

        //先判断是否命中表头
        if (_controller.HitTestInHeader(x, y)) return true;
        //再判断是否命中行
        var hitRow = _controller.HitTestInRows(x, y);
        if (hitRow != null && !hitRow.Value.IsColumnResizer && hitRow.Value.RowIndex >= 0 &&
            hitRow.Value.Column is DataGridHostColumn<T> hostColumn)
        {
            var cellWidget = hostColumn.GetCellWidget(hitRow.Value.RowIndex);
            HitTestChild(cellWidget, x, y, result);
        }

        return true;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        SetSize(width, height);
        _controller.CalcColumnsWidth(new Size(width, height));
    }

    // protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false, Rect? dirtyRect = null)
    // {
    //     base.BeforePaint(canvas, onlyTransform, dirtyRect);
    //     
    //     if (onlyTransform) //仅处理转换坐标，因HostColumn内的CellWidget需要处理滚动偏移量
    //     {
    //         Log.Debug($"偏移量: {ScrollOffsetY}");
    //         canvas.Translate(-ScrollOffsetX, -ScrollOffsetY);
    //     }
    //     //TODO:考虑始终clip，这样Hosted CellWidget的某些装饰器不会超出范围
    // }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var size = new Size(W, H);
        //clip first
        canvas.Save();
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);

        //TODO: 暂每次计算可见列
        var visibleColumns = _controller.LayoutVisibleColumns(size);
        var totalColumnsWidth = _controller.TotalColumnsWidth;

        //draw header
        PaintHeader(canvas, size, totalColumnsWidth, visibleColumns);

        //draw rows
        if (_controller.DataView == null || _controller.DataView.Count == 0)
        {
            //TODO: draw no data
            canvas.Restore();
            return;
        }

        if (_controller.ScrollController.OffsetY > 0)
        {
            var clipRect = Rect.FromLTWH(
                0, _controller.TotalHeaderHeight, size.Width,
                size.Height - _controller.TotalHeaderHeight);
            canvas.ClipRect(clipRect, ClipOp.Intersect, false);
        }

        PaintRows(canvas, size, totalColumnsWidth, visibleColumns);

        //draw shadow for scroll vertical
        if (_controller.ScrollController.OffsetY > 0)
        {
            using var shadowPath = new Path();
            shadowPath.AddRect(Rect.FromLTWH(0, 0, Math.Min(size.Width, totalColumnsWidth),
                _controller.TotalHeaderHeight));
            canvas.DrawShadow(shadowPath, Colors.Black, 5.0f, false, Root!.Window.ScaleFactor);
        }

        //draw highlights //TODO:考虑使用Overlay绘制，暂在DataGrid内直接绘制
        PaintHighlight(canvas);

        canvas.Restore();
    }

    private void PaintHeader(Canvas canvas, Size size, float totalColumnsWidth,
        IList<DataGridColumn<T>> visibleColumns)
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
        column.PaintHeader(canvas, cellRect, Theme);
        PaintCellBorder(canvas, cellRect);

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

    private void PaintRows(Canvas canvas, Size size, float totalColumnsWidth, IList<DataGridColumn<T>> visibleColumns)
    {
        var headerHeight = _controller.TotalHeaderHeight;
        var deltaY = _controller.ScrollDeltaY;
        var startRowIndex = _controller.VisibleStartRowIndex;

        if (size.Width < totalColumnsWidth && _controller.HasFrozen)
        {
            //先画冻结列
            var frozenColumns = visibleColumns.Where(c => c.Frozen);
            foreach (var col in frozenColumns)
            {
                PaintColumnCells(canvas, col, startRowIndex, headerHeight, deltaY, size.Height);
            }

            //clip scroll region
            var clipRect = _controller.GetScrollClipRect(headerHeight, size.Height - headerHeight);
            canvas.Save();
            canvas.ClipRect(clipRect, ClipOp.Intersect, false);

            //再画其他列
            var noneFrozenColumns = visibleColumns.Where(c => c.Frozen == false);
            foreach (var col in noneFrozenColumns)
            {
                PaintColumnCells(canvas, col, startRowIndex, headerHeight, deltaY, size.Height);
            }

            canvas.Restore();
        }
        else
        {
            foreach (var col in visibleColumns)
            {
                PaintColumnCells(canvas, col, startRowIndex, headerHeight, deltaY, size.Height);
            }
        }
    }

    private void PaintColumnCells(Canvas canvas, DataGridColumn<T> col, int startRow,
        float offsetY, float deltaY, float maxHeight)
    {
        var rowHeight = Theme.RowHeight;
        for (var j = startRow; j < _controller.DataView!.Count; j++)
        {
            var cellRect = Rect.FromLTWH(col.CachedLeft, offsetY - deltaY, col.LayoutWidth, rowHeight);

            //TODO:暂在这里画stripe背景
            if (Theme.StripeRows && j % 2 != 0)
            {
                var paint = PaintUtils.Shared(Theme.StripeBgColor);
                canvas.DrawRect(cellRect, paint);
            }

            col.PaintCell(canvas, _controller, j, cellRect);

            var borderRect = new Rect(col.CachedVisibleLeft, cellRect.Top,
                col.CachedVisibleRight, cellRect.Top + rowHeight);
            PaintCellBorder(canvas, borderRect);

            offsetY += rowHeight;
            if (offsetY >= maxHeight) break;
        }
    }

    private void PaintCellBorder(Canvas canvas, in Rect cellRect)
    {
        var paint = PaintUtils.Shared(Theme.BorderColor, PaintStyle.Stroke, 1);
        canvas.DrawRect(cellRect, paint);
    }

    private void PaintHighlight(Canvas canvas)
    {
        if (Theme.HighlightingCurrentRow)
        {
            var rowRect = _controller.GetCurrentRowRect();
            if (rowRect != null)
            {
                if (Theme.HighlightingCurrentCell)
                {
                    var paint = PaintUtils.Shared(Theme.HighlightRowBgColor);
                    canvas.DrawRect(rowRect.Value, paint);
                }
                else
                {
                    var paint = PaintUtils.Shared(PixUI.Theme.FocusedColor,
                        PaintStyle.Stroke, PixUI.Theme.FocusedBorderWidth);
                    canvas.DrawRect(rowRect.Value, paint);
                }
            }
        }

        if (Theme.HighlightingCurrentCell)
        {
            var cellRect = _controller.GetCurrentCellRect();
            if (cellRect != null)
            {
                var paint = PaintUtils.Shared(PixUI.Theme.FocusedColor,
                    PaintStyle.Stroke, PixUI.Theme.FocusedBorderWidth);
                canvas.DrawRect(cellRect.Value, paint);
            }
        }
    }

    #endregion
}