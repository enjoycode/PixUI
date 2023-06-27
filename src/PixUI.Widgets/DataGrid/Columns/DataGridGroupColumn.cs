using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PixUI;

public sealed class DataGridGroupColumn<T> : DataGridColumn<T>, IEnumerable<DataGridColumn<T>>
{
    public DataGridGroupColumn(string label) : base(label)
    {
        Children = new List<DataGridColumn<T>>();
    }

    public readonly IList<DataGridColumn<T>> Children; //TODO: change children relayout

    internal override float LayoutWidth => Children.Sum(c => c.LayoutWidth);

    public void Add(DataGridColumn<T> column) => Children.Add(column);

    public IEnumerator<DataGridColumn<T>> GetEnumerator() => Children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();

}