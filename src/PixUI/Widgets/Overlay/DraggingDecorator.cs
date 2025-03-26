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
            DrawDefaultDropHint(canvas, e);
        }
        else
        {
            dest = Rect.FromLTWH(0, 0, e.DropHintImage.Width / scale, e.DropHintImage.Height / scale);
            canvas.DrawImage(e.DropHintImage, dest);
        }

        canvas.Restore();
    }

    private static void DrawDefaultDropHint(Canvas canvas, DragEvent e)
    {
        //TODO: 暂简单实现
        const float ident = 2f;
        var hintColor = Theme.AccentColor;
        var target = (Widget)DragDropManager.Dropping!;
        if (e.DropPosition == DropPosition.In)
        {
            var rect = Rect.FromLTWH(0, 0, target.W, target.H);
            var paint = PixUI.Paint.Shared(hintColor, PaintStyle.Stroke, 2f);
            canvas.DrawRect(rect, paint);
        }
        else if (e.DropPosition == DropPosition.Left)
        {
            var rect = Rect.FromLTWH(0 - ident, 0, ident * 2, target.H);
            var paint = PixUI.Paint.Shared(hintColor);
            canvas.DrawRect(rect, paint);
        }
        else if (e.DropPosition == DropPosition.Right)
        {
            var rect = Rect.FromLTWH(target.W - ident, 0, ident * 2, target.H);
            var paint = PixUI.Paint.Shared(hintColor);
            canvas.DrawRect(rect, paint);
        }
        else if (e.DropPosition == DropPosition.Upper)
        {
            var rect = Rect.FromLTWH(0, 0 - ident, target.W, ident * 2);
            var paint = PixUI.Paint.Shared(hintColor);
            canvas.DrawRect(rect, paint);
        }
        else if (e.DropPosition == DropPosition.Under)
        {
            var rect = Rect.FromLTWH(0, target.H - ident, target.W, ident * 2);
            var paint = PixUI.Paint.Shared(hintColor);
            canvas.DrawRect(rect, paint);
        }
    }
}