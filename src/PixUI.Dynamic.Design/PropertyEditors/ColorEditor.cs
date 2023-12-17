using System.Globalization;

namespace PixUI.Dynamic.Design;

public sealed class ColorEditor : ValueEditorBase
{
    //TODO: 暂简单实现输入ARGB值

    public ColorEditor(State<Color?> color, DesignElement element) : base(element)
    {
        _color = color;
        if (color.Value.HasValue)
            _colorHex.Value = ((uint)color.Value.Value).ToString("X2");
        _colorHex.AddListener(OnColorHexChanged);

        var colorState = color.ToNoneNullable(Colors.Transparent);
        //var iconState = color.ToComputed(v => v.HasValue ? MaterialIcons.Square : MaterialIcons.ColorLens);
        //Child = new Button(icon: iconState) { TextColor = colorState, Style = ButtonStyle.Outline, OnTap = OnTap };

        Child = new TextInput(_colorHex)
        {
            Prefix = new Icon(MaterialIcons.Square) { Color = colorState }
        };
    }

    private readonly State<Color?> _color;
    private readonly State<string> _colorHex = string.Empty;

    private void OnColorHexChanged(State state)
    {
        if (!uint.TryParse(_colorHex.Value, NumberStyles.HexNumber, null, out var value))
            return;
        _color.Value = value;
    }
}