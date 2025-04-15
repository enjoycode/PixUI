using System;

namespace PixUI;

public sealed class Expanded : SingleChildWidget
{
    public Expanded() { }

    public Expanded(Widget? child = null, int flex = 1)
    {
        Child = child;
        Flex = flex;
    }

    private int _flex = 1;

    public int Flex
    {
        get => _flex;
        set
        {
            if (value <= 0)
                value = 1;
            _flex = value;
            if (IsMounted)
                Parent?.Relayout();
        }
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        CachedAvailableWidth = availableWidth;
        CachedAvailableHeight = availableHeight;

        if (Child != null)
        {
            Child.Layout(availableWidth, availableHeight);
            Child.SetPosition(0, 0);
        }

        var w = Parent is Column ? Child?.W ?? 0 : availableWidth;
        var h = Parent is Row ? Child?.H ?? 0 : availableHeight;
        SetSize(w, h);
    }

    protected internal override void OnChildSizeChanged(Widget child,
        float dx, float dy, AffectsByRelayout affects)
    {
        var oldWidth = W;
        var oldHeight = H;
        var w = Parent is Column ? child.W : CachedAvailableWidth;
        var h = Parent is Row ? child.H : CachedAvailableHeight;
        SetSize(w, h);

        TryNotifyParentIfSizeChanged(oldWidth, oldHeight, affects);
    }
}