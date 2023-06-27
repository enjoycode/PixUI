using System;

namespace PixUI;

public sealed class DataGridButtonColumn<T> : DataGridHostColumn<T>
{
    public DataGridButtonColumn(string label, Func<T, int, Button> cellBuilder, ColumnWidth? width = null)
        : base(label, cellBuilder)
    {
        if (width != null)
            Width = width;
    }
}