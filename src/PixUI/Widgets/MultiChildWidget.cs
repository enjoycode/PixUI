using System;
using System.Collections.Generic;

namespace PixUI;

/// <summary>
/// 具有多个子级的Widget
/// </summary>
public abstract class MultiChildWidget<T> : Widget where T : Widget
{
    protected MultiChildWidget()
    {
        _children = new WidgetList<T>(this);
    }

    // ReSharper disable once InconsistentNaming
    protected readonly WidgetList<T> _children;

    public IEnumerable<T> Children
    {
        set
        {
            _children.Clear();
            foreach (var child in value)
            {
                _children.Add(child);
            }
        }
    }

    internal Widget GetChildAt(int index) => _children[index];

    public override void VisitChildren(Func<Widget, bool> action)
    {
        foreach (var child in _children)
        {
            if (action(child))
                break; //stop visit
        }
    }

    protected internal sealed override int IndexOfChild(Widget child) =>
        _children.IndexOf((T)child);
}