using System;

namespace PixUI;

public sealed class ExpandIcon : SingleChildWidget, IMouseRegion
{
    public MouseRegion MouseRegion { get; }

    public Action<PointerEvent> OnPointerDown
    {
        set => MouseRegion.PointerDown += value;
    }

    public ExpandIcon(Animation<float> turns, BindableProperty<float>? size = null,
        BindableProperty<Color>? color = null)
    {
        MouseRegion = new MouseRegion();

        var iconStyle = Theme.GetDefaultStyle<IconStyle>();
        if (size != null || color != null)
        {
            iconStyle = new IconStyle();
            if (size != null) iconStyle.Size = size.Value;
            if (color != null) iconStyle.Color = color.Value;
        }

        Child = new RotationTransition(turns)
        {
            Child = new Icon(MaterialIcons.ExpandMore) { Style = iconStyle }
        };
    }
}