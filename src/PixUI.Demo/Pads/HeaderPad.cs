namespace PixUI.Demo;

internal sealed class HeaderPad : View
{
    public HeaderPad()
    {
        Child = new Container
        {
            Height = 25,
            FillColor = new Color(43, 49, 56),
            Child = new Center
            {
                Child = new Text("PixUI Playground") { TextColor = Colors.White }
            }
        };
    }
}