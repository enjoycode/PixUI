namespace PixUI;

public sealed class Text : TextBase
{
    public Text() { }

    public Text(BindableProperty<string> text) : this() => Text = text;
}