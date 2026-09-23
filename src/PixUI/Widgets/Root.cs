namespace PixUI;

/// <summary>
/// 每个窗体的根节点
/// </summary>
public sealed class Root : SingleChildWidget, IRootWidget
{
    public UIWindow Window { get; }

    internal Root(UIWindow window, Widget child)
    {
        Window = window;
        IsMounted = false; //set to true on FirstShow
        Child = child;
    }

    /// <summary>
    /// 在首秀前设置
    /// </summary>
    public void Mount()
    {
        if (IsMounted) return;

        IsMounted = true;
        var visitor = new MountChildrenVisitor();
        VisitChildren(ref visitor);
    }

    protected override void OnLayout(Size maxSize)
    {
        SetLayoutLocation(0, 0);
        SetLayoutSize(Window.Width, Window.Height);
        Child!.PerformLayout(new(Window.Width, Window.Height));
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        //do nothing
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        canvas.Clear(Window.BackgroundColor);
        base.OnPaint(canvas, area);
    }
}