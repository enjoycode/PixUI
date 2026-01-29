using System;
using System.Collections.ObjectModel;

namespace PixUI;

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
        item.Parent = null;
        base.RemoveItem(index);
    }

    protected override void SetItem(int index, T item)
    {
        if (item.Parent != null)
            throw new InvalidOperationException("insert item already has parent");

        var old = this[index];
        old.Parent = null;
        item.Parent = _parent;
        base.SetItem(index, item);
    }

    internal bool MoveItem(T item, MoveChildAction action)
    {
        if (Count <= 1) return false;
        var index = IndexOf(item);
        if (index < 0)
        {
            Log.Warn("Move item is not a child");
            return false;
        }

        switch (action)
        {
            case MoveChildAction.First:
                if (index == 0) return false;
                base.RemoveItem(index);
                base.InsertItem(0, item);
                break;
            case MoveChildAction.Last:
                if (index == Count - 1) return false;
                base.RemoveItem(index);
                base.InsertItem(Count, item);
                break;
            case MoveChildAction.Forward:
                if (index == 0) return false;
                base.SetItem(index, base[index - 1]);
                base.SetItem(index - 1, item);
                break;
            case MoveChildAction.Backward:
                if (index == Count - 1) return false;
                base.SetItem(index, base[index + 1]);
                base.SetItem(index + 1, item);
                break;
        }

        return true;
    }
}

public enum MoveChildAction
{
    First,
    Forward,
    Backward,
    Last
}