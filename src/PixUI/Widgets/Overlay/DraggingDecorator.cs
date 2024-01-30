namespace PixUI;

internal sealed class DraggingDecorator : Widget
{
    protected internal override bool HitTest(float x, float y, HitTestResult result) => false;

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (DragDropManager.DragEvent == null) return;

        var e = DragDropManager.DragEvent;
        // draw drag hint image
        var winX = Root!.Window.LastMouseX;
        var winY = Root!.Window.LastMouseY;
        var scale = Root!.Window.ScaleFactor;
        var dest = Rect.FromLTWH(winX, winY, e.DragHintImage.Width / scale, e.DragHintImage.Height / scale);
        canvas.DrawImage(e.DragHintImage, dest);

        if (e.DropEffect == DropEffect.None || DragDropManager.Dropping == null) return;

        // draw drop hint image
        canvas.Save();
        var m = Matrix4.TryInvert(DragDropManager.HitTransform);
        canvas.Concat(m!.Value);
        if (e.DropHintImage == null)
        {
            //TODO:
            var target = (Widget)DragDropManager.Dropping;
            var rect = Rect.FromLTWH(0, 0, target.W, target.H);
            var paint = PixUI.Paint.Shared(Colors.Red, PaintStyle.Stroke);
            canvas.DrawRect(rect, paint);
        }
        else
        {
            dest = Rect.FromLTWH(0, 0, e.DropHintImage.Width / scale, e.DropHintImage.Height / scale);
            canvas.DrawImage(e.DropHintImage, dest);
        }

        canvas.Restore();
    }
}