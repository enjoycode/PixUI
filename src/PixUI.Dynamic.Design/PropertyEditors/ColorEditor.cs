using System;

namespace PixUI.Dynamic.Design;

public sealed class ColorEditor : SingleChildWidget
{
    public ColorEditor(State<Color?> color)
    {
        _color = color;

        var colorState = color.ToNoneNullable(Colors.Black);
        var iconState = color.ToComputed(v => v.HasValue ? MaterialIcons.Square : MaterialIcons.ColorLens);

        Child = new Button(icon: iconState) { TextColor = colorState, Style = ButtonStyle.Outline, OnTap = OnTap };
    }

    private readonly State<Color?> _color;

    private void OnTap(PointerEvent e)
    {
        //TODO: test only
        _color.Value = Colors.Random();
    }
}