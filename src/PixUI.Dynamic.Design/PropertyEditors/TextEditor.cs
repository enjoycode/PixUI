namespace PixUI.Dynamic.Design;

public sealed class TextEditor : ValueEditorBase
{
    public TextEditor(State<string?> state, DesignController controller) : base(controller)
    {
        Child = new TextInput(state.ToNoneNullable());
    }
}