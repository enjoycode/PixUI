namespace PixUI;

public sealed class Stack : MultiChildWidget<Positioned>
{
    protected override void OnLayout(Size maxSize)
    {
        SetLayoutSize(maxSize.Width, maxSize.Height);

        foreach (var child in _children)
        {
            child.PerformLayout(new(maxSize.Width, maxSize.Height)); //TODO: use float.MaxValue?
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

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        if (area is RepaintChild)
            base.OnPaint(canvas, null); //TODO:***暂强制重画全部,应该重绘所有相交的子组件
        else
            base.OnPaint(canvas, area);
    }
}