namespace PixUI.AIChatBox;

internal sealed class ChatMessagesView : Widget, IScrollable
{
    public ChatMessagesView(ChatBoxController controller)
    {
        _controller = controller;
        _controller.RepaintAction = () => Repaint();
    }

    private readonly ChatBoxController _controller;

    #region ====IScrollable====

    float IScrollable.ScrollOffsetX => _controller.ScrollController.OffsetX;
    float IScrollable.ScrollOffsetY => _controller.ScrollController.OffsetY;
    ScrollDirection IScrollable.ScrollDirection => _controller.ScrollController.Direction;
    ScrollBarVisibility IScrollable.ShowScrollBar => ScrollBarVisibility.Hover;

    Offset IScrollable.OnScroll(float dx, float dy)
    {
        if (_controller.Messages.Count == 0) return Offset.Empty;

        var lastChild = _controller.Messages[^1];

        if (lastChild.Y + lastChild.H <= H) return Offset.Empty;

        var maxOffsetY = lastChild.Y + lastChild.H - H;

        var offset = _controller.ScrollController.OnScroll(dx, dy, 0, maxOffsetY);
        if (!offset.IsEmpty)
            Repaint();
        return offset;
    }

    #endregion

    protected override void OnLayout(Size maxSize)
    {
        SetLayoutSize(maxSize);

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (_controller.Width != maxSize.Width)
        {
            _controller.Width = maxSize.Width;
            var offsetY = 0f;
            foreach (var child in _controller.Messages)
            {
                child.Y = offsetY;
                child.OnLayout(_controller.Width);
                offsetY += child.H;
            }
        }
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        canvas.Save();
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H));
        canvas.Translate(0, -_controller.ScrollController.OffsetY);

        var scrollOffsetY = _controller.ScrollController.OffsetY;
        foreach (var child in _controller.Messages)
        {
            if (child.Y + child.H <= scrollOffsetY) continue; //小于上边界
            if (child.Y >= scrollOffsetY + H) break; //大于下边界

            canvas.Translate(0, child.Y);
            child.OnPaint(canvas, _controller);
            canvas.Translate(0, -child.Y);
        }

        canvas.Restore();
    }
}