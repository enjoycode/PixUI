using System;
using System.Collections.Generic;
using System.Linq;

namespace PixUI;

internal sealed class DataGridBody<T> : Widget, IScrollable
{
    public DataGridBody(DataGridController<T> controller)
    {
        _controller = controller;
    }

    private readonly DataGridController<T> _controller;
    private DataGridTheme Theme => _controller.Theme;

    #region ====IScrollable====

    public float ScrollOffsetX => _controller.ScrollController.OffsetX;
    public float ScrollOffsetY => _controller.ScrollController.OffsetY;

    public Offset OnScroll(float dx, float dy)
    {
        if (_controller.DataView == null || _controller.DataView.Count == 0)
            return Offset.Empty;

        var totalRowsHeight = _controller.TotalRowsHeight;
        var maxOffsetX = Math.Max(0, _controller.TotalColumnsWidth - W);
        var maxOffsetY = Math.Max(0, totalRowsHeight - H);

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

            Repaint();
            if (offset.Dx != 0)
            {
                //TODO: 考虑在这里重新计算布局可视列，而不是在DataGrid绘制每次计算
                _controller.DataGrid.Header?.Repaint();
                _controller.DataGrid.Footer?.Repaint();
            }
        }

        return offset;
    }

    #endregion

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        if (y < 0 || y > H) return false;

        result.Add(this);

        var hitRow = _controller.HitTestInRows(x, y);
        //继续判断是否命中HostedCellWidget
        if (hitRow != null && !hitRow.Value.IsColumnResizer && hitRow.Value.RowIndex >= 0 &&
            hitRow.Value.Column is DataGridHostColumn<T> hostColumn)
        {
            var cellWidget = hostColumn.GetCellWidget(hitRow.Value.RowIndex);
            if (cellWidget != null) //TODO:临时解决快速滚动后尚未创建承载的Widget实例
                HitTestChild(cellWidget, x, y, result); //不用考虑列是否冻结，HostedCellWidget.X已抵消
        }

        return true;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        SetSize(availableWidth, availableHeight);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_controller.DataView == null || _controller.DataView.Count == 0)
        {
            //TODO: draw no data
            return;
        }

        var totalColumnsWidth = _controller.TotalColumnsWidth;

        if (_controller.ScrollController.OffsetY > 0)
        {
            var clipRect = Rect.FromLTWH(0, 0, W, H);
            canvas.Save();
            canvas.ClipRect(clipRect, ClipOp.Intersect, false);
        }

        PaintRows(canvas, totalColumnsWidth, _controller.CachedVisibleColumns);

        //draw shadow for scroll vertical
        if (_controller.ScrollController.OffsetY > 0)
        {
            using var shadowPath = new Path();
            var headerHeight = _controller.TotalHeaderHeight;
            shadowPath.AddRect(Rect.FromLTWH(0, -headerHeight, Math.Min(W, totalColumnsWidth), headerHeight));
            canvas.DrawShadow(shadowPath, Colors.Black, 5.0f, false, Root!.Window.ScaleFactor);
            canvas.Restore();
        }

        //draw highlights //TODO:考虑使用Overlay绘制，暂直接绘制
        PaintHighlight(canvas);
    }

    private void PaintRows(Canvas canvas, float totalColumnsWidth, IList<DataGridColumn<T>> visibleColumns)
    {
        // var headerHeight = _controller.TotalHeaderHeight;
        var deltaY = _controller.ScrollDeltaY;
        var bodyWidth = W;
        var bodyHeight = H;
        var startRowIndex = _controller.VisibleStartRowIndex;

        if (bodyWidth < totalColumnsWidth && _controller.HasFrozen)
        {
            //先画冻结列
            var frozenColumns = visibleColumns.Where(c => c.Frozen);
            foreach (var col in frozenColumns)
            {
                PaintColumnCells(canvas, col, startRowIndex, deltaY, bodyHeight);
            }

            //clip scroll region
            var clipRect = _controller.GetScrollClipRect(0, bodyHeight);
            canvas.Save();
            canvas.ClipRect(clipRect, ClipOp.Intersect, false);

            //再画其他列
            var noneFrozenColumns = visibleColumns.Where(c => c.Frozen == false);
            foreach (var col in noneFrozenColumns)
            {
                PaintColumnCells(canvas, col, startRowIndex, deltaY, bodyHeight);
            }

            canvas.Restore();
        }
        else
        {
            foreach (var col in visibleColumns)
            {
                PaintColumnCells(canvas, col, startRowIndex, deltaY, bodyHeight);
            }
        }
    }

    private void PaintColumnCells(Canvas canvas, DataGridColumn<T> col, int startRow, float deltaY, float maxHeight)
    {
        var rowHeight = Theme.RowHeight;
        var offsetY = 0f;
        var lastMergeBeginRow = -1;
        var lastMergeEndRow = -1;

        for (var j = startRow; j < _controller.DataView!.Count; j++)
        {
            if (offsetY - deltaY >= maxHeight) break;

            var cellRect = Rect.FromLTWH(col.CachedLeft, offsetY - deltaY, col.LayoutWidth, rowHeight);

            //尝试合并单元格
            if (col.AutoMergeCells)
            {
                //先判断是否已经合并
                if (j >= lastMergeBeginRow && j <= lastMergeEndRow)
                {
                    offsetY += rowHeight;
                    continue; //继续下一个循环
                }

                lastMergeBeginRow = lastMergeEndRow = j;
                if (j == startRow)
                {
                    var mergeUpCount = col.TryMergeUp(_controller, j);
                    lastMergeBeginRow = j - mergeUpCount;
                    cellRect.Top -= mergeUpCount * rowHeight;
                }

                var mergeDownCount = col.TryMergeDown(_controller, j);
                lastMergeEndRow = j + mergeDownCount;
                cellRect.Bottom += mergeDownCount * rowHeight;
            }

            //画单元格(背景由各自具体实现处理)
            col.PaintCell(canvas, _controller, j, cellRect);

            //画单元格边框
            var borderRect = new Rect(col.CachedVisibleLeft, cellRect.Top, col.CachedVisibleRight, cellRect.Bottom);
            DataGridPainter.PaintCellBorder(canvas, borderRect, _controller.Theme.BorderColor);

            offsetY += rowHeight;
        }
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
                    var paint = PaintUtils.Shared(Theme.HighlightRowFillColor);
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
                var paint = PaintUtils.Shared(PixUI.Theme.FocusedColor, PaintStyle.Stroke,
                    PixUI.Theme.FocusedBorderWidth);
                canvas.DrawRect(cellRect.Value, paint);
            }
        }
    }
}