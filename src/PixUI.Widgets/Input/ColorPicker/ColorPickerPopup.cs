using System;

namespace PixUI;

public sealed class ColorPickerPopup : Popup
{
    public ColorPickerPopup(Overlay overlay, State<Color> value, Action hideAction) : base(overlay)
    {
        _hideAction = hideAction;
        AutoFitInWindowWidth = true;
        _child = new Card
        {
            Elevation = 8,
            Padding = EdgeInsets.All(5),
            Child = new ColorPalette(value)
        };
        _child.Parent = this;
    }

    private readonly Card _child;
    private readonly Action _hideAction;

    public override void VisitChildren(Func<Widget, bool> action) => action(_child);

    public override void Layout(float availableWidth, float availableHeight)
    {
        SetSize(250, 220);
        _child.Layout(W, H);
    }

    public override EventPreviewResult PreviewEvent(EventType type, object? e)
    {
        if (type == EventType.PointerDown)
        {
            //TODO:应转换为本地坐标点再判断是否包含
            var pointerEvent = (PointerEvent)e!;
            var localPos = LocalToWindow(0, 0);
            var winBounds = Rect.FromLTWH(localPos.X, localPos.Y, W, H);
            if (!winBounds.ContainsPoint(pointerEvent.X, pointerEvent.Y)) //TODO:排除下拉按钮
            {
                _hideAction();
                // return EventPreviewResult.Processed;
            }
        }
        else if (type == EventType.MoveOutWindow)
        {
            _hideAction();
            // return EventPreviewResult.NotProcessed;
        }

        return base.PreviewEvent(type, e);
    }
}