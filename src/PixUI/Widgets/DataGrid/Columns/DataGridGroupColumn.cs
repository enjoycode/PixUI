using System.Collections.Generic;
using System.Linq;

namespace PixUI;

public sealed class DataGridGroupColumn<T> : DataGridColumn<T>
{
    public DataGridGroupColumn(string label, DataGridColumn<T>[] children) : base(label)
    {
        Children = children;
    }

    public readonly DataGridColumn<T>[] Children;

    internal override float LayoutWidth => Children.Sum(c => c.LayoutWidth);
}