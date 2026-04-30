namespace PixUI;

public sealed class Stack : MultiChildWidget<Positioned>
{
    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        SetSize(maxSize.Width, maxSize.Height);

        foreach (var child in _children)
        {
            child.Layout(maxSize.Width, maxSize.Height); //TODO: use float.MaxValue?
        }
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        affects.Widget = this;
        affects.OldX = 0;
        affects.OldY = 0;
        affects.OldW = W;
        affects.OldH = H;
        //不需要再通知上级了
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (area is RepaintChild)
            base.Paint(canvas, null); //TODO:***暂强制重画全部,应该重绘所有相交的子组件
        else
            base.Paint(canvas, area);
    }
}