namespace PixUI.Demo;

public sealed class DemoColorPicker : View
{
    public DemoColorPicker()
    {
        Child = new Container()
        {
            Padding = EdgeInsets.All(20),
            Width = 300,
            Height = 200,
            Child = new ColorPalette()
        };
    }
}