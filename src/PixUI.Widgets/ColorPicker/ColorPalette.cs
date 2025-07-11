namespace PixUI;

public sealed class ColorPalette : View
{
    public ColorPalette()
    {
        var rawColor = _hueValue.ToComputed(hue => Color.FromHsl(hue));

        Child = new Column()
        {
            Spacing = 5,
            Children =
            [
                new Expanded(new ColorHslPanel(rawColor, _colorFromHsl)),
                new ColorSlider(_hueValue),
                new OpacitySlider(_alphaValue, rawColor),
            ]
        };
    }

    private readonly State<double> _hueValue = 0;
    private readonly State<double> _alphaValue = 255;
    private readonly State<Color> _colorFromHsl = Colors.Red;
}