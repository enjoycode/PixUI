namespace PixUI;

/// <summary>
/// 用于动态视图切换动画
/// </summary>
internal sealed class TransitionStack : Widget
{
    private readonly Widget _from;
    private readonly Widget _to;

    internal TransitionStack(Widget from, Widget to)
    {
        _from = from;
        _from.Parent = this;
        _to = to;
        _to.Parent = this;
    }

    public override void VisitChildren<TVisitor>(ref TVisitor visitor)
    {
        // if (!IsMounted) return; //Do not do this, Unmount children when animation done need this.
        if (visitor.Visit(_from)) return;
        visitor.Visit(_to);
    }

    protected override void OnLayout(Size maxSize)
    {
        maxSize = AvailableSize;
        SetLayoutSize(maxSize);

        _from.PerformLayout(maxSize);
        _from.SetLayoutLocation(0, 0);
        _to.PerformLayout(maxSize);
        _to.SetLayoutLocation(0, 0);
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        //do nothing
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        if (!IsMounted) return; //maybe has remove from widget tree when animation done.

        _from.BeforePaint(canvas);
        _from.OnPaint(canvas, null /*Paint all, area?.ToChild(_from)*/);
        _from.AfterPaint(canvas);
        _to.BeforePaint(canvas);
        _to.OnPaint(canvas, null /*Paint all, area?.ToChild(_to)*/);
        _to.AfterPaint(canvas);
    }
}