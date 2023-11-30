using System;
using System.Linq;

namespace PixUI;

public sealed class DataGridButtonColumn<T> : DataGridHostColumn<T>
{
    public DataGridButtonColumn(string label, Func<T, int, Button> cellBuilder, ColumnWidth? width = null)
        : base(label, cellBuilder)
    {
        if (width != null)
            Width = width;
    }
    
    public DataGridButtonColumn(string label, Func<T, int, ButtonGroup> cellBuilder, ColumnWidth? width = null)
        : base(label, cellBuilder)
    {
        if (width != null)
            Width = width;
    }

    public DataGridButtonColumn(string lable, Func<T, int, Button[]> cellBuilder, ColumnWidth? width = null)
        : base(lable, (v, i) =>
        {
            var buttons = cellBuilder(v, i).Cast<Widget>().ToList();
            return new Row { Children = buttons };
        })
    {
        if (width != null)
            Width = width;
    }
}