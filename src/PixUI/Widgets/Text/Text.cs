namespace PixUI;

public sealed class Text : TextBase
{
    public Text() { }

    public Text(State<string> text) : this() => Text = text;
}