using System;

namespace PixUI;

public sealed class DataGridTextColumn<T> : DataGridTextColumnBase<T>
{
    public DataGridTextColumn(string label, Func<T, string> cellValueGetter) :
        base(label, (v, _) => cellValueGetter(v)) { }

    protected internal override int TryMergeUp(DataGridController<T> controller, int currentRow)
    {
        if (currentRow == 0) return 0;

        var currentValue = _cellValueGetter(controller.DataView![currentRow], currentRow);
        var mergeUpCount = 0;
        for (var i = currentRow - 1; i >= 0; i--)
        {
            var value = _cellValueGetter(controller.DataView![i], i);
            if (value == currentValue)
                mergeUpCount++;
            else
                break;
        }

        return mergeUpCount;
    }

    protected internal override int TryMergeDown(DataGridController<T> controller, int currentRow)
    {
        var totalRows = controller.DataView!.Count;
        if (currentRow == totalRows - 1) return 0;

        var currentValue = _cellValueGetter(controller.DataView![currentRow], currentRow);
        var mergeDownCount = 0;
        for (var i = currentRow + 1; i < totalRows; i++)
        {
            var value = _cellValueGetter(controller.DataView![i], i);
            if (value == currentValue)
                mergeDownCount++;
            else
                break;
        }

        return mergeDownCount;
    }
}