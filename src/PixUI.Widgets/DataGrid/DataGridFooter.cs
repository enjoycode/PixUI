namespace PixUI;

internal sealed class DataGridFooter<T> : Widget
{
    public DataGridFooter(DataGridController<T> controller)
    {
        _controller = controller;
    }
    
    private readonly DataGridController<T> _controller;

    public override void Layout(float availableWidth, float availableHeight)
    {
        SetSize(availableWidth, _controller.Theme.RowHeight);
    }
}