using System;
using System.Collections.ObjectModel;

namespace PixUI;

[TSType("PixUI.WidgetList")]
public sealed class WidgetList<T> : Collection<T> where T : Widget
{
    private readonly Widget _parent;

    public WidgetList(Widget parent)
    {
        _parent = parent;
    }

    protected override void ClearItems()
    {
        foreach (var child in this)
        {
            // child.ClearBindings();
            child.Parent = null;
        }

        base.ClearItems();
    }

    protected override void InsertItem(int index, T item)
    {
        if (item.Parent != null)
            throw new InvalidOperationException("insert item already has parent");

        item.Parent = _parent;
        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        // item.ClearBindings();
        item.Parent = null;
        base.RemoveItem(index);
    }

    protected override void SetItem(int index, T item)
    {
        if (item.Parent != null)
            throw new InvalidOperationException("insert item already has parent");

        var old = this[index];
        old.Parent = null;
        base.SetItem(index, item);
    }
}