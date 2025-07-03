namespace PixUI.PdfViewer;

public sealed class PdfView : Widget
{
    public PdfView(PdfViewController controller)
    {
        _controller = controller;
        _controller.InitView(this);
    }

    private readonly PdfViewController _controller;

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        SetSize(maxSize.Width, maxSize.Height);
    }

    protected override void BeforePaint(Canvas canvas, bool onlyTransform = false, IDirtyArea? dirtyArea = null)
    {
        canvas.Translate(X, Y);
        if (!onlyTransform)
        {
            canvas.Save();
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
        }
    }

    protected override void AfterPaint(Canvas canvas)
    {
        canvas.Restore();
        canvas.Translate(-X, -Y);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //TODO: 待完成，目前仅测试
        canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(Colors.Red, PaintStyle.Stroke, 1));

        if (!_controller.TryGetRenderedPage(0, out var page))
            return;

        canvas.DrawImage(page, 10, 10);
    }
}