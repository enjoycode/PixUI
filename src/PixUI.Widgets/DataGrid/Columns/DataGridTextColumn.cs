using System;

namespace PixUI;

public sealed class DataGridTextColumn<T> : DataGridTextColumnBase<T>
{
    public DataGridTextColumn(string label, Func<T, string> cellValueGetter) :
        base(label, (v, _) => cellValueGetter(v)) { }
}