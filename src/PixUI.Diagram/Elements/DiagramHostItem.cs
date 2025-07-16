namespace PixUI.Diagram;

public class DiagramHostItem : DiagramItem
{
    public DiagramHostItem(Widget target)
    {
        HostWidget = target;
    }

    public Widget HostWidget { get; }

    public override Rect Bounds
    {
        get { return HostWidget.LayoutBounds; }
        set { SetBounds(value.X, value.Y, value.Width, value.Height, BoundsSpecified.All); }
    }

    public override bool Visible
    {
        get { return true; }
    }

    protected internal override bool IsContainer
    {
        get { return false; /*TODO:*/ }
    }

    protected override void SetBounds(float x, float y, float width, float height, BoundsSpecified specified)
    {
        var oldBounds = Bounds;
        HostWidget.Relayout();
        //通知Canvas刷新相关区域
        InvalidateOnBoundsChanged(oldBounds);
    }

    protected internal override void OnAddToSurface()
    {
        throw new NotImplementedException();
        // if (this.Parent == null)
        //     this.Surface.Controls.Add(hostControl);
        // else
        //     ((HostItemDesigner)Parent).hostControl.Controls.Add(hostControl);
    }

    protected internal override void OnRemoveFromSurface()
    {
        throw new NotImplementedException();
        // if (this.Parent == null)
        //     this.Surface.Controls.Remove(hostControl);
        // else
        //     ((HostItemDesigner)Parent).hostControl.Controls.Remove(hostControl);
    }

    public override void Paint(Canvas g) { }
}