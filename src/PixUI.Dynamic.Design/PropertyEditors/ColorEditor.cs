using System;
using System.Globalization;

namespace PixUI.Dynamic.Design;

public sealed class ColorEditor : ValueEditorBase
{
    //TODO: 暂简单实现输入ARGB值

    public ColorEditor(State<Color?> color, DesignElement element) : base(element)
    {
        var colorHex = color.ToComputed(
            s => s.HasValue ? ((uint)s.Value).ToString("X2") : string.Empty,
            v =>
            {
                if (uint.TryParse(v, NumberStyles.HexNumber, null, out var value))
                    color.Value = value;
            });

        var colorState = color.ToNoneNullable(Colors.Transparent);
        //var iconState = color.ToComputed(v => v.HasValue ? MaterialIcons.Square : MaterialIcons.ColorLens);
        //Child = new Button(icon: iconState) { TextColor = colorState, Style = ButtonStyle.Outline, OnTap = OnTap };

        Child = new TextInput(colorHex)
        {
            Suffix = new Icon(MaterialIcons.Square) { Color = colorState }
        };
    }
}