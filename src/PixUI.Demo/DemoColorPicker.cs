namespace PixUI.Demo;

public sealed class DemoColorPicker : View
{
    public DemoColorPicker()
    {
        Child = new Container()
        {
            Padding = EdgeInsets.All(20),
            Width = 300,
            Height = 300,
            Child = new Column()
            {
                Spacing = 20,
                Children =
                [
                    new ColorPicker(Colors.Red.WithAlpha(128)),
                    new ColorPalette(Colors.Blue),
                ]
            }
        };
    }
}