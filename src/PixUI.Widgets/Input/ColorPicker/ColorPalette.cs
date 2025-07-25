namespace PixUI;

public sealed class ColorPalette : View
{
    public ColorPalette(State<Color> color)
    {
        State<double> hueState = color.Value.GetHue();
        State<double> opacityState = color.Value.Alpha;
        State<Color> hslColor = color.Value.WithAlpha(255);

        hslColor.AddListener(_ => color.Value = hslColor.Value.WithAlpha((byte)opacityState.Value));
        opacityState.AddListener(_ => color.Value = color.Value.WithAlpha((byte)opacityState.Value));

        Child = new Column()
        {
            Spacing = 5,
            Children =
            [
                new Expanded(new HslColorPanel(hueState, hslColor)),
                new HueSlider(hueState),
                new OpacitySlider(opacityState, hslColor),
            ]
        };
    }
}