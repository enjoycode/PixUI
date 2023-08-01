using System;

namespace PixUI.Dynamic.Design;

/// <summary>
/// 设计时包装目标Widget
/// </summary>
public sealed class DesignElement : Widget, IMouseRegion
{
    /// <summary>
    /// Ctor for Root DesignElement
    /// </summary>
    public DesignElement(DesignController controller)
    {
        _controller = controller;
        MouseRegion = new MouseRegion(opaque: false);
        MouseRegion.PointerDown += OnPointerDown;
    }

    /// <summary>
    /// Ctor for design time make default
    /// </summary>
    public DesignElement(DesignController controller, DynamicWidgetMeta meta) : this(controller)
    {
        Meta = meta;
    }

    private static readonly Lazy<Paragraph> _hint = new(() =>
        TextPainter.BuildParagraph("Drag Widget Here", float.PositiveInfinity, 16, Colors.Gray));

    private readonly DesignController _controller;
    private DynamicWidgetMeta? _meta;
    private Widget? _wrapTarget;
    private bool _isSelected;

    public MouseRegion MouseRegion { get; }

    public bool IsSelected
    {
        get => _isSelected;
        internal set
        {
            if (_isSelected == value) return;

            _isSelected = value;
            Invalidate(InvalidAction.Repaint);
        }
    }

    public bool IsContainer => _meta == null /*Root*/ ||
                               _meta.ContainerType != ContainerType.None;

    public DynamicWidgetMeta? Meta
    {
        get => _meta;
        set
        {
            _meta = value;
            MouseRegion.Opaque = !IsContainer;
            Target = _meta?.MakeDefaultInstance();
        }
    }

    /// <summary>
    /// 包装的目标组件
    /// </summary>
    public Widget? Target
    {
        get => _wrapTarget;
        private set
        {
            ChangeTarget(_wrapTarget, value);
            Invalidate(InvalidAction.Relayout);
        }
    }

    private void ChangeTarget(Widget? oldTarget, Widget? newTarget)
    {
        if (oldTarget != null) oldTarget.Parent = null;

        _wrapTarget = newTarget;

        if (_wrapTarget != null)
        {
            _wrapTarget.Parent = this;
#if DEBUG
            DebugLabel = _wrapTarget.GetType().Name;
#endif
        }
    }

    #region ====Event Handler====

    private void OnPointerDown(PointerEvent e)
    {
        e.IsHandled = true;
        _controller.Select(this);
    }

    #endregion

    #region ====Widget Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (_wrapTarget != null)
            action(_wrapTarget);
    }

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        if (!ContainsPoint(x, y)) return false;

        result.Add(this);

        if (IsContainer) //如果非容器组件，不检测包装目标
            VisitChildren(child => HitTestChild(child, x, y, result));

        return true;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        if (_wrapTarget == null)
        {
            SetSize(width, height);
            return;
        }

        _wrapTarget.Layout(availableWidth, availableHeight);
        SetSize(_wrapTarget.W, _wrapTarget.H);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_wrapTarget != null)
        {
            if (IsSelected) canvas.Save();
            base.Paint(canvas, area);
            if (IsSelected) canvas.Restore();
        }
        else
            DrawPlaceholder(canvas);

        DrawSelection(canvas);
    }

    private void DrawPlaceholder(Canvas canvas)
    {
        var paint = PaintUtils.Shared(Colors.Gray, PaintStyle.Stroke, 2f);
        paint.AntiAlias = true;
        canvas.DrawLine(0, 0, W, H, paint);
        canvas.DrawLine(W, 0, 0, H, paint);

        var paragraph = _hint.Value;
        canvas.DrawParagraph(paragraph, (W - paragraph.MaxIntrinsicWidth) / 2f, (H - paragraph.Height) / 2f);
    }

    private void DrawSelection(Canvas canvas)
    {
        if (!IsSelected) return;

        var scaleRatio = _controller.Zoom.Value / 100f;
        var anchorSize = 10f * scaleRatio;
        var borderSize = 3f;

        var paint = PaintUtils.Shared(Theme.FocusedColor, PaintStyle.Stroke, borderSize * scaleRatio);
        canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), paint);

        paint = PaintUtils.Shared(Theme.FocusedColor, PaintStyle.Fill, 1f * scaleRatio);

        //TopLeft
        DrawAnchor(canvas, paint, Rect.FromLTWH(0 - anchorSize / 2, 0 - anchorSize / 2, anchorSize, anchorSize));
        //TopMiddle
        DrawAnchor(canvas, paint, Rect.FromLTWH(W / 2 - anchorSize / 2, 0 - anchorSize / 2, anchorSize, anchorSize));
        //TopRight
        DrawAnchor(canvas, paint, Rect.FromLTWH(W - anchorSize / 2, 0 - anchorSize / 2, anchorSize, anchorSize));

        //MiddleLeft
        DrawAnchor(canvas, paint, Rect.FromLTWH(0 - anchorSize / 2, H / 2 - anchorSize / 2, anchorSize, anchorSize));
        //MiddleRight
        DrawAnchor(canvas, paint, Rect.FromLTWH(W - anchorSize / 2, H / 2 - anchorSize / 2, anchorSize, anchorSize));

        //BottomLeft
        DrawAnchor(canvas, paint, Rect.FromLTWH(0 - anchorSize / 2, H - anchorSize / 2, anchorSize, anchorSize));
        //BottomMiddle
        DrawAnchor(canvas, paint, Rect.FromLTWH(W / 2 - anchorSize / 2, H - anchorSize / 2, anchorSize, anchorSize));
        //BottomRight
        DrawAnchor(canvas, paint, Rect.FromLTWH(W - anchorSize / 2, H - anchorSize / 2, anchorSize, anchorSize));
    }

    private static void DrawAnchor(Canvas canvas, Paint paint, Rect rect)
    {
        canvas.DrawRect(rect, paint);
    }

    #endregion
}