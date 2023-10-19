namespace PixUI.Dynamic.Design;

/// <summary>
/// 选中的元素的装饰器
/// </summary>
internal sealed class SelectedDecorator : FlowDecorator<DesignElement>
{
    public SelectedDecorator(DesignElement target) : base(target, true) { }

    protected override void PaintCore(Canvas canvas)
    {
        var scaleRatio = Target.Controller.Zoom.Value / 100f;
        var borderSize = 3f;

        var paint = PaintUtils.Shared(Theme.FocusedColor, PaintStyle.Stroke, borderSize * scaleRatio);
        canvas.DrawRect(Rect.FromLTWH(0, 0, Target.W, Target.H), paint);

        paint = PaintUtils.Shared(Theme.FocusedColor, PaintStyle.Fill, 1f * scaleRatio);

        DrawAnchor(canvas, paint, Target.GetAnchorRect(AnchorPosition.TopMiddle));
        DrawAnchor(canvas, paint, Target.GetAnchorRect(AnchorPosition.MiddleLeft));
        DrawAnchor(canvas, paint, Target.GetAnchorRect(AnchorPosition.MiddleRight));
        DrawAnchor(canvas, paint, Target.GetAnchorRect(AnchorPosition.BottomMiddle));
    }

    private static void DrawAnchor(Canvas canvas, Paint paint, Rect rect)
    {
        canvas.DrawRect(rect, paint);
    }
}