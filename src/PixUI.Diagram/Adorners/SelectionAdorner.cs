namespace PixUI.Diagram;

internal sealed class SelectionAdorner : DesignAdorner, ISelectionAdorner
{
    private const int ANCHOR_SIZE = 6;
    private ResizeAnchorLocation _anchorLocation = ResizeAnchorLocation.None;

    #region ====Anchor Properties====

    private Rect LeftTop => Rect.FromLTWH(-ANCHOR_SIZE, -ANCHOR_SIZE, ANCHOR_SIZE, ANCHOR_SIZE);

    private Rect LeftCenter =>
        Rect.FromLTWH(-ANCHOR_SIZE, (Target.Bounds.Height - ANCHOR_SIZE) / 2, ANCHOR_SIZE, ANCHOR_SIZE);

    private Rect LeftBottom => Rect.FromLTWH(-ANCHOR_SIZE, Target.Bounds.Height, ANCHOR_SIZE, ANCHOR_SIZE);

    private Rect RightTop => Rect.FromLTWH(Target.Bounds.Width, -ANCHOR_SIZE, ANCHOR_SIZE, ANCHOR_SIZE);

    private Rect RightCenter => Rect.FromLTWH(Target.Bounds.Width,
        (Target.Bounds.Height - ANCHOR_SIZE) / 2, ANCHOR_SIZE, ANCHOR_SIZE);

    private Rect RightBottom =>
        Rect.FromLTWH(Target.Bounds.Width, Target.Bounds.Height, ANCHOR_SIZE, ANCHOR_SIZE);

    private Rect TopCenter => Rect.FromLTWH((Target.Bounds.Width - ANCHOR_SIZE) / 2, -ANCHOR_SIZE, ANCHOR_SIZE,
        ANCHOR_SIZE);

    private Rect BottomCenter => Rect.FromLTWH((Target.Bounds.Width - ANCHOR_SIZE) / 2,
        Target.Bounds.Height, ANCHOR_SIZE, ANCHOR_SIZE);

    #endregion

    public SelectionAdorner(DesignAdorners owner, DiagramItem target) : base(owner, target) { }

    protected internal override void OnRender(Canvas canvas)
    {
        //画外边框
        var outBorder = Rect.FromLTWH(-3, -3, Target.Bounds.Width + 6, Target.Bounds.Height + 6);
        var paint = Paint.Shared(Colors.DarkGray, PaintStyle.Stroke, 2);
        canvas.DrawRect(outBorder, paint);

        //画锚点
        DrawAnchor(canvas, LeftTop);
        DrawAnchor(canvas, LeftCenter);
        DrawAnchor(canvas, LeftBottom);

        DrawAnchor(canvas, RightTop);
        DrawAnchor(canvas, RightCenter);
        DrawAnchor(canvas, RightBottom);

        DrawAnchor(canvas, TopCenter);
        DrawAnchor(canvas, BottomCenter);
    }

    private static void DrawAnchor(Canvas canvas, Rect rect)
    {
        var paint = Paint.Shared(Colors.White);
        canvas.DrawRect(rect, paint);
        paint.Color = Colors.Black;
        paint.Style = PaintStyle.Stroke;
        canvas.DrawRect(rect, paint);
    }

    protected internal override bool HitTest(Point pt, ref Cursor cursor)
    {
        //TODO: 优化判断
        if (LeftTop.Contains(pt))
            _anchorLocation = ResizeAnchorLocation.LeftTop;
        else if (LeftCenter.Contains(pt))
        {
            _anchorLocation = ResizeAnchorLocation.LeftCenter;
            cursor = Cursors.ResizeLR;
        }
        else if (LeftBottom.Contains(pt))
            _anchorLocation = ResizeAnchorLocation.LeftBottom;
        else if (RightTop.Contains(pt))
            _anchorLocation = ResizeAnchorLocation.RightTop;
        else if (RightCenter.Contains(pt))
        {
            _anchorLocation = ResizeAnchorLocation.RightCenter;
            cursor = Cursors.ResizeLR;
        }
        else if (RightBottom.Contains(pt))
            _anchorLocation = ResizeAnchorLocation.RightBottom;
        else if (TopCenter.Contains(pt))
        {
            _anchorLocation = ResizeAnchorLocation.TopCenter;
            cursor = Cursors.ResizeUD;
        }
        else if (BottomCenter.Contains(pt))
        {
            _anchorLocation = ResizeAnchorLocation.BottomCenter;
            cursor = Cursors.ResizeUD;
        }
        else
            _anchorLocation = ResizeAnchorLocation.None;

        if (_anchorLocation != ResizeAnchorLocation.None)
            return true;
        else
            return false;
    }

    protected internal override void OnMouseMove(PointerEvent e)
    {
        Target.Resize(_anchorLocation, (int)e.DeltaX, (int)e.DeltaY);
    }
}

internal enum ResizeAnchorLocation
{
    None,
    LeftTop,
    LeftCenter,
    LeftBottom,
    TopCenter,
    BottomCenter,
    RightTop,
    RightCenter,
    RightBottom
}