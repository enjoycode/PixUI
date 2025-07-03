using PixUI.PdfViewer;

namespace PixUI.Demo;

public sealed class DemoPdfViewer : View
{
    public DemoPdfViewer()
    {
        Child = new PdfView(_controller);
    }

    private readonly PdfViewController _controller = new();
    private bool _hasLoaded;
    
    protected override void OnMounted()
    {
        base.OnMounted();
        if (!_hasLoaded)
        {
            _hasLoaded = true;
            _ = _controller.OpenAsync(ResourceLoad.LoadStream("Resources.Demo.pdf"));
        }
    }
}