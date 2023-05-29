namespace PixUI;

public sealed class Expanded : SingleChildWidget
{
    /// <summary>
    /// Must > 0
    /// </summary>
    public int Flex { get; private set; } = 1;

    public Expanded(Widget? child = null, int flex = 1)
    {
        Child = child;
        Flex = flex;
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