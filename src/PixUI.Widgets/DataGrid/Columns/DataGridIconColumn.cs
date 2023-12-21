using System;

namespace PixUI;

public sealed class DataGridIconColumn<T> : DataGridColumn<T>
{
    public DataGridIconColumn(string label, Func<T, IconData?> cellValueGetter, ColumnWidth? width = null) : base(label)
    {
        if (width != null)
            Width = width;
        _cellValueGetter = cellValueGetter;
    }

    private readonly Func<T, IconData?> _cellValueGetter;

    protected internal override void PaintCell(Canvas canvas, DataGridController<T> controller,
        int rowIndex, Rect cellRect)
    {
        //先画背景
        var cellStyle = PaintCellBackground(canvas, controller, rowIndex, cellRect);
        //再画图标
        var row = controller.DataView![rowIndex];
        var icon = _cellValueGetter(row);
        if (icon == null) return;

        //TODO: cache icon painter
        using var iconPainter = new IconPainter(() => controller.DataGrid.Body.Repaint() /*TODO: repaint column only*/);
        var offsetX = cellRect.Left + CellStyle.CellPadding;
        var offsetY = cellRect.Top;
        if (cellStyle.VerticalAlignment == VerticalAlignment.Middle)
            offsetY += (cellRect.Height - cellStyle.FontSize) / 2f;
        else if (cellStyle.VerticalAlignment == VerticalAlignment.Bottom)
            offsetY = offsetY - cellRect.Bottom - cellStyle.FontSize;

        iconPainter.Paint(canvas, cellStyle.FontSize, cellStyle.TextColor ?? Colors.Black, icon.Value,
            offsetX, offsetY);
    }
}