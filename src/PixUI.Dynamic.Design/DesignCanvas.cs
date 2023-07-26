namespace PixUI.Dynamic.Design;

public sealed class DesignCanvas : View
{
    public DesignCanvas()
    {
        Child = new Container()
        {
            BgColor = new Color(0xFFA2A2A2),
            Padding = EdgeInsets.All(10),
            Child = new Card()
            {
                Elevation = 10,
                Child = new Transform(Matrix4.CreateIdentity())
                {
                    Child = new DesignPlaceholder()
                }
            }
        };
    }
}