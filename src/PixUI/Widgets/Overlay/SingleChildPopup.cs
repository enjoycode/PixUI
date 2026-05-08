namespace PixUI;

public sealed class SingleChildPopup : Popup
{
    public SingleChildPopup(Widget child, Overlay? overlay = null)
        : base(overlay ?? UIWindow.Current.Overlay)
    {
        _child = child;
        _child.Parent = this;
    }

    private readonly Widget _child;

    public override void VisitChildren<TVisitor>(ref TVisitor visitor) => visitor.Visit(_child);

    protected override void OnLayout(Size maxSize)
    {
        _child.PerformLayout(AvailableSize);
        SetLayoutSize(_child.W, _child.H);
    }
}