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

    protected internal override void PaintCell(Canvas canvas, DataGridController<T> controller, int rowIndex,
        Rect cellRect)
    {
        var row = controller.DataView![rowIndex];
        var icon = _cellValueGetter(row);
        if (icon == null) return;

        var style = CellStyleGetter != null
            ? CellStyleGetter(row, rowIndex)
            : CellStyle ?? controller.Theme.DefaultRowCellStyle;

        //TODO: cache icon painter
        using var iconPainter = new IconPainter(controller.Invalidate);
        var offsetX = cellRect.Left + CellStyle.CellPadding;
        var offsetY = cellRect.Top;
        if (style.VerticalAlignment == VerticalAlignment.Middle)
        {
            offsetY += (cellRect.Height - style.FontSize) / 2f;
        }
        else if (style.VerticalAlignment == VerticalAlignment.Bottom)
        {
            offsetY = offsetY - cellRect.Bottom - style.FontSize;
        }

        iconPainter.Paint(canvas, style.FontSize, style.Color ?? Colors.Black, icon.Value,
            offsetX, offsetY);
    }
}