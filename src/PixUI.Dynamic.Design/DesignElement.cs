using System;
using System.Reflection;

namespace PixUI.Dynamic.Design;

/// <summary>
/// 设计时包装目标Widget
/// </summary>
public sealed class DesignElement : Widget, IMouseRegion
{
    /// <summary>
    /// Ctor for Root DesignElement or Deserialize
    /// </summary>
    public DesignElement(DesignController controller, bool isRoot = false)
    {
        _controller = controller;
        MouseRegion = new MouseRegion(opaque: false);
        MouseRegion.PointerDown += OnPointerDown;

        if (isRoot)
        {
            controller.RootElement = this;
            DebugLabel = "Root";
        }
    }

    /// <summary>
    /// Ctor for designtime with default instance
    /// </summary>
    public DesignElement(DesignController controller, DynamicWidgetMeta meta) : this(controller)
    {
        ChangeMeta(meta, true);
    }

    private static readonly Lazy<Paragraph> _hint = new(() =>
        TextPainter.BuildParagraph("Drag Widget Here", float.PositiveInfinity, 16, Colors.Gray));

    private readonly DesignController _controller;
    private bool _isSelected;

    public DynamicWidgetMeta? Meta { get; private set; }

    public DynamicWidgetData Data { get; } = new();

    /// <summary>
    /// 包装的目标组件
    /// </summary>
    public Widget? Target { get; private set; }

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

    public bool IsContainer => Meta == null /*Root*/ ||
                               Meta.ContainerType != ContainerType.None;

    internal void ChangeMeta(DynamicWidgetMeta? meta, bool makeDefaultTarget)
    {
        Meta = meta;
        Data.Type = Meta == null ? string.Empty : Meta.Name;
        MouseRegion.Opaque = !IsContainer;
        if (makeDefaultTarget || (Meta == null && Target != null))
            ChangeTarget(Target, Meta?.MakeDefaultInstance());
    }

    internal void ChangeTarget(Widget? oldTarget, Widget? newTarget)
    {
        if (oldTarget != null) oldTarget.Parent = null;

        Target = newTarget;

        if (Target != null)
        {
            Target.Parent = this;
#if DEBUG
            DebugLabel = Target.GetType().Name;
#endif
        }

        if (IsMounted) Invalidate(InvalidAction.Relayout);
    }

    public void AddChild(Widget child)
    {
        if (!IsContainer) throw new InvalidOperationException();
        if (Meta == null || Target == null) throw new Exception();

        Meta.AddChild!(Target, child);
    }

    public void SetPropertyValue(PropertyValue propertyValue)
    {
        if (Meta == null || Target == null) throw new Exception();

        //TODO: emit 优化，暂用反射
        var propMeta = Meta.GetPropertyMeta(propertyValue.Name);
        var propValue = propMeta.Value.GetRuntimeValue(propertyValue.Value);
        var propInfo = Meta.WidgetType.GetProperty(propertyValue.Name);
        propInfo!.SetValue(Target, propValue);
    }

    public void RemovePropertyValue(string name)
    {
        if (Meta == null || Target == null) throw new Exception();

        //TODO: emit 优化，暂用反射
        var propMeta = Meta.GetPropertyMeta(name);
        var propInfo = Meta.WidgetType.GetProperty(name);
        propInfo!.SetValue(Target, null);
    }

    #region ====Event Handler====

    private void OnPointerDown(PointerEvent e)
    {
        e.IsHandled = true;
        _controller.Select(this);
    }

    public void OnDrop(DynamicWidgetMeta meta)
    {
        ChangeMeta(meta, true);
    }

    #endregion

    #region ====Widget Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (Target != null)
            action(Target);
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

        if (Target == null)
        {
            SetSize(width, height);
            return;
        }

        Target.Layout(availableWidth, availableHeight);
        SetSize(Target.W, Target.H);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (Target != null)
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