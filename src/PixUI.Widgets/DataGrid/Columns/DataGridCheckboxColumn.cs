using System;

namespace PixUI;

public sealed class DataGridCheckboxColumn<T> : DataGridHostColumn<T>
{
    public DataGridCheckboxColumn(string label,
        Func<T, bool> cellValueGetter,
        Action<T, bool>? cellValueSetter = null)
        : base(label, (data, _) =>
        {
            var state = new RxProxy<bool>(() => cellValueGetter(data),
                cellValueSetter == null ? null : v => cellValueSetter(data, v));
            //TODO: no cellValueSetter set to readonly
            return new Checkbox(state);
        }) { }
}