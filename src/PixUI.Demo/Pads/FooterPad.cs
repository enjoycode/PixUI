namespace PixUI.Demo;

internal sealed class FooterPad : View
{
    public FooterPad()
    {
        Child = new Container
        {
            Height = 25,
            FillColor = new Color(0xFFCC653A),
            Child = new Center
            {
                Child = new Text("enjoycode@icloud.com") { TextColor = Colors.White }
            }
        };
    }
}