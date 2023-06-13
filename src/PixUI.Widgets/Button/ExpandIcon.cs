using System;

namespace PixUI;

public sealed class ExpandIcon : SingleChildWidget, IMouseRegion
{
    public MouseRegion MouseRegion { get; }

    public Action<PointerEvent> OnPointerDown
    {
        set => MouseRegion.PointerDown += value;
    }

    public ExpandIcon(Animation<float> turns, State<float>? size = null,
        State<Color>? color = null)
    {
        MouseRegion = new MouseRegion();

        Child = new RotationTransition(turns)
        {
            Child = new Icon(MaterialIcons.ExpandMore) { Size = size, Color = color }
        };
    }
}