using System;
using System.Linq;

namespace PixUI;

public sealed class Overlay : Widget, IRootWidget
{
    internal Overlay(UIWindow window)
    {
        Window = window;
        IsMounted = true;
        _children = new WidgetList<Widget>(this);
    }

    private readonly WidgetList<Widget> _children;

    public UIWindow Window { get; }
    internal bool HasEntry => _children.Count > 0;

    public Widget? FindEntry(Predicate<Widget> predicate) =>
        _children.FirstOrDefault(entry => predicate(entry));

    #region ====Show & Hide====

    public void Show(Widget entry)
    {
        if (_children.Contains(entry)) return;

        _children.Add(entry);
        entry.Layout(Window.Width, Window.Height);

        Repaint();
    }

    public void Remove(Widget entry)
    {
        if (!_children.Remove(entry)) return;

        Repaint();
    }

    #endregion

    #region ====Overrides====

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        for (var i = _children.Count - 1; i >= 0; i--) //倒序
        {
            if (HitTestChild(_children[i], x, y, result))
                break;
        }

        return result.IsHitAnyMouseRegion;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        //do nothing, children will layout on Show()
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        foreach (var entry in _children)
        {
            // if (entry.Widget.W <= 0 || entry.Widget.H <= 0)
            //     continue;

            var needTranslate = entry.X != 0 || entry.Y != 0;
            if (needTranslate)
                canvas.Translate(entry.X, entry.Y);
            entry.Paint(canvas, area);
            if (needTranslate)
                canvas.Translate(-entry.X, -entry.Y);
        }
    }

    #endregion
}