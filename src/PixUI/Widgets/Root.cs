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
        // set IsMounted flag before set child
        IsMounted = true;
        Child = child;
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