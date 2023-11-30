namespace PixUI.Dynamic.Design;

public sealed class TextEditor : ValueEditorBase
{
    public TextEditor(State<string?> state, DesignElement element) : base(element)
    {
        Child = new TextInput(state.ToNoneNullable());
    }
}