namespace PixUI.Dynamic.Design;

public sealed class TextEditor : SingleChildWidget
{
    public TextEditor(State<string?> state)
    {
        Child = new TextInput(state.ToNoneNullable());
    }
}