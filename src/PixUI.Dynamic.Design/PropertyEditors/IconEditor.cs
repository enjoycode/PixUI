namespace PixUI.Dynamic.Design;

public sealed class IconEditor : ValueEditorBase
{
    public IconEditor(State<IconData?> state, DesignElement element) : base(element)
    {
        _state = state;
        Child = new Button("...") { Width = float.MaxValue, OnTap = OnTap };
    }

    private readonly State<IconData?> _state;

    private void OnTap(PointerEvent _)
    {
        var dlg = new IconDialog(_state);
        dlg.Show();
    }
}