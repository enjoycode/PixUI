namespace PixUI.Dynamic.Design;

public sealed class ColorEditor : ValueEditorBase
{
    // ReSharper disable once UnusedParameter.Local
    public ColorEditor(State<Color?> color, DesignController controller) : base(controller)
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