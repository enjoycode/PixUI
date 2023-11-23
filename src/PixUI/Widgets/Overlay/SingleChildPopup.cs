using System;

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

    public override void VisitChildren(Func<Widget, bool> action) => action(_child);

    public override void Layout(float availableWidth, float availableHeight)
    {
        _child.Layout(availableWidth, availableHeight);
        SetSize(_child.W, _child.H);
    }
}