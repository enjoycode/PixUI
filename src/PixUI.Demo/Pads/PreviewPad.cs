namespace PixUI.Demo;

internal sealed class PreviewPad : View
{
    public PreviewPad(PlaygroundContext context)
    {
        Child = new Container()
        {
            FillColor = new Color(0xFFA2A2A2),
            Padding = EdgeInsets.All(10),
            Child = new Card()
            {
                Elevation = 10,
                Child = new Transform(_scale)
                {
                    Child = new EmbedPreviewer(context)
                }
            }
        };
    }

    private Matrix4 _scale = Matrix4.CreateIdentity();
}