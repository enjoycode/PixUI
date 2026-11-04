namespace PixUI.AIChatBox;

public sealed class ChatBox : Widget, IScrollable
{
    public ChatBox(ChatBoxController controller)
    {
        Controller = controller;
        Controller.RepaintAction = () => Repaint();
    }

    public readonly ChatBoxController Controller;

    #region ====IScrollable====

    float IScrollable.ScrollOffsetX => Controller.ScrollController.OffsetX;
    float IScrollable.ScrollOffsetY => Controller.ScrollController.OffsetY;
    ScrollDirection IScrollable.ScrollDirection => Controller.ScrollController.Direction;
    ScrollBarVisibility IScrollable.ShowScrollBar => ScrollBarVisibility.Hover;

    Offset IScrollable.OnScroll(float dx, float dy)
    {
        if (Controller.Messages.Count == 0) return Offset.Empty;

        var lastChild = Controller.Messages[^1];

        if (lastChild.Y + lastChild.H <= H) return Offset.Empty;

        var maxOffsetY = lastChild.Y + lastChild.H - H;

        var offset = Controller.ScrollController.OnScroll(dx, dy, 0, maxOffsetY);
        if (!offset.IsEmpty)
            Repaint();
        return offset;
    }

    #endregion

    protected override void OnLayout(Size maxSize)
    {
        SetLayoutSize(maxSize);

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (Controller.Width != maxSize.Width)
        {
            Controller.Width = maxSize.Width;
            var offsetY = 0f;
            foreach (var child in Controller.Messages)
            {
                child.Y = offsetY;
                child.OnLayout(Controller.Width);
                offsetY += child.H;
            }
        }
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        canvas.Save();
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H));
        canvas.Translate(0, -Controller.ScrollController.OffsetY);

        var scrollOffsetY = Controller.ScrollController.OffsetY;
        foreach (var child in Controller.Messages)
        {
            if (child.Y + child.H <= scrollOffsetY) continue; //小于上边界
            if (child.Y >= scrollOffsetY + H) break; //大于下边界

            canvas.Translate(0, child.Y);
            child.OnPaint(canvas, Controller);
            canvas.Translate(0, -child.Y);
        }

        canvas.Restore();
    }
}