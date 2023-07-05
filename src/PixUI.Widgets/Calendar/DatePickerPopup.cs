using System;

namespace PixUI;

public sealed class DatePickerPopup : Popup
{
    public DatePickerPopup(Overlay overlay, State<DateTime?> value, Action hideAction) : base(overlay)
    {
        _child = new Card
        {
            Elevation = 8,
            Child = new Calendar(value) { OnSelectedChangedByUser = hideAction }
        };
        _child.Parent = this;
    }

    private readonly Card _child;

    public override void VisitChildren(Func<Widget, bool> action) => action(_child);

    public override void Layout(float availableWidth, float availableHeight)
    {
        SetSize(250, 220);
        _child.Layout(W, H);
    }

    // public override EventPreviewResult PreviewEvent(EventType type, object? e)
    // {
    //     return base.PreviewEvent(type, e);
    // }
}