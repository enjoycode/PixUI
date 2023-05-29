namespace PixUI;

public sealed class Input : InputBase<EditableText>
{
    public Input(State<string> text) : base(new EditableText(text))
    {
        Readonly = text.Readonly;
    }

    public State<float>? FontSize
    {
        get => _editor.FontSize;
        set => _editor.FontSize = value;
    }

    public Widget? Prefix
    {
        set => PrefixWidget = value;
    }

    public Widget? Suffix
    {
        set => SuffixWidget = value;
    }

    public override State<bool>? Readonly
    {
        get => _editor.Readonly;
        set => _editor.Readonly = value;
    }

    public bool IsObscure
    {
        set => _editor.IsObscure = value;
    }

    public string HintText
    {
        set => _editor.HintText = value;
    }
}