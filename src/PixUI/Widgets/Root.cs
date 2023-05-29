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

    public override void Layout(float availableWidth, float availableHeight)
    {
        SetPosition(0, 0);
        SetSize(Window.Width, Window.Height);
        Child!.Layout(W, H);
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy,
        AffectsByRelayout affects)
    {
        //do nothing
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        canvas.Clear(Window.BackgroundColor);
        base.Paint(canvas, area);
    }
}