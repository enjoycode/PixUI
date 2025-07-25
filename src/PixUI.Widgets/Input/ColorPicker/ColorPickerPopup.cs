using System;

namespace PixUI;

public sealed class ColorPickerPopup : Popup
{
    public ColorPickerPopup(Overlay overlay, State<Color> value, Action hideAction) : base(overlay)
    {
        AutoFitInWindowWidth = true;
        _child = new Card
        {
            Elevation = 8,
            Padding = EdgeInsets.All(5),
            Child = new ColorPalette(value)
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
}