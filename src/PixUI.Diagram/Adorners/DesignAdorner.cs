namespace PixUI.Diagram;

internal class DesignAdorner
{
    public DiagramItem Target { get; }

    protected DesignAdorners Owner { get; }

    public DesignAdorner(DesignAdorners owner, DiagramItem target)
    {
        Owner = owner;
        Target = target;
    }

    /// <summary>
    /// Point已转为控件坐标系
    /// </summary>
    protected internal virtual bool HitTest(Point pt, ref Cursor cursor)
    {
        return false;
    }

    /// <summary>
    /// 注意：坐标系已转为目标DiagramItem的坐标系
    /// </summary>
    protected internal virtual void OnRender(Canvas canvas) { }

    /// <summary>
    /// 注意：此时MouseButton 肯定== Left
    /// </summary>
    protected internal virtual void OnMouseMove(PointerEvent e) { } //TODO: rename to OnDragging
}