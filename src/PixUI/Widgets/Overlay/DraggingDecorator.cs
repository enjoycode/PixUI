namespace PixUI;

internal sealed class DraggingDecorator : Widget
{
    protected internal override bool HitTest(float x, float y, HitTestResult result) => false;

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (DragDropManager.DragEvent == null) return;

        var e = DragDropManager.DragEvent;
        var winX = Root!.Window.LastMouseX;
        var winY = Root!.Window.LastMouseY;
        var scale = Root!.Window.ScaleFactor;
        var dest = Rect.FromLTWH(winX, winY, e.DragHintImage.Width / scale, e.DragHintImage.Height / scale);
        canvas.DrawImage(e.DragHintImage, dest);
    }
}