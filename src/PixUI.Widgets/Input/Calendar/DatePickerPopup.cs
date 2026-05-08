using System;

namespace PixUI;

public sealed class DatePickerPopup : Popup
{
    public DatePickerPopup(Overlay overlay, State<DateTime?> value, Action hideAction) : base(overlay)
    {
        AutoFitInWindowWidth = true;
        _child = new Card
        {
            Elevation = 8,
            Child = new Calendar(value) { OnSelectedChangedByUser = hideAction }
        };
        _child.Parent = this;
    }

    private readonly Card _child;

    public override void VisitChildren<TVisitor>(ref TVisitor visitor) => visitor.Visit(_child);

    protected override void OnLayout(Size maxSize)
    {
        SetLayoutSize(250, 220);
        _child.PerformLayout(LayoutSize);
    }

    // public override EventPreviewResult PreviewEvent(EventType type, object? e)
    // {
    //     return base.PreviewEvent(type, e);
    // }
}