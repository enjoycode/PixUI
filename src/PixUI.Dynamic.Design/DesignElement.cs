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
        MouseRegion.PointerMove += OnPointerMove;

        if (isRoot)
        {
            controller.RootElement = this;
            DebugLabel = "Root";
        }
    }

    /// <summary>
    /// Ctor for designtime
    /// </summary>
    private DesignElement(DesignController controller, DynamicWidgetMeta meta) : this(controller)
    {
        ChangeMeta(meta, meta.ContainerType != ContainerType.SingleChildReversed);
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
    /// <remarks>如果是反向包装，返回的是上级</remarks>
    public Widget? Target => Meta?.ContainerType == ContainerType.SingleChildReversed ? Parent : Child;

    public Widget? Child { get; private set; }

    public MouseRegion MouseRegion { get; }

    public bool IsSelected
    {
        get => _isSelected;
        internal set
        {
            if (_isSelected == value) return;

            _isSelected = value;
            //TODO: 使用选择装饰器
        }
    }

    public bool IsContainer => Meta == null /*Root*/ || Meta.ContainerType != ContainerType.None;

    internal void ChangeMeta(DynamicWidgetMeta? meta, bool makeDefaultTarget)
    {
        Meta = meta;
        Data.Type = Meta == null ? string.Empty : Meta.Name;
        MouseRegion.Opaque = !IsContainer;
        if (makeDefaultTarget || (Meta == null && Child != null))
            ChangeChild(Child, Meta?.MakeDefaultInstance());
    }

    internal void ChangeChild(Widget? oldChild, Widget? newChild)
    {
        if (oldChild != null) oldChild.Parent = null;

        Child = newChild;

        if (Child != null)
        {
            Child.Parent = this;
#if DEBUG
            DebugLabel = Target?.GetType().Name;
#endif
        }

        if (IsMounted) Invalidate(InvalidAction.Relayout);
    }

    /// <summary>
    /// 用于包装的目标添加子组件
    /// </summary>
    public void AddChild(Widget child)
    {
        if (!IsContainer) throw new InvalidOperationException();
        if (Meta == null || Child == null || Meta.ContainerType == ContainerType.SingleChildReversed)
            throw new InvalidOperationException();

        Meta.AddChild(Child, child);
    }

    /// <summary>
    /// 构造参数改变后重新创建实例
    /// </summary>
    public void OnCtorArgValueChanged()
    {
        if (Meta == null) throw new Exception();

        var newTarget = Data.CtorArgs == null ? Meta.MakeDefaultInstance() : Meta.MakeInstance(Data.CtorArgs);
        ChangeChild(Target, newTarget);

        //重设属性值
        if (Data.Properties != null)
        {
            foreach (var propertyValue in Data.Properties)
            {
                SetPropertyValue(propertyValue);
            }
        }
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
        // var propMeta = Meta.GetPropertyMeta(name);
        var propInfo = Meta.WidgetType.GetProperty(name);
        propInfo!.SetValue(Target, null);
    }

    #region ====Event Handler====

    private void OnPointerDown(PointerEvent e)
    {
        e.IsHandled = true;
        _controller.Select(this);
    }

    private void OnPointerMove(PointerEvent e)
    {
        if (e.Buttons != PointerButtons.Left) return;

        //判断是否可以移动，目前仅针对Stack下的Positioned组件,
        //另需要注意如果位置属性绑定了状态不可手工移动
        DesignElement? moveable = null;
        if (Target?.GetType() == typeof(Positioned))
            moveable = this;
        else if (Parent is DesignElement parentElement && parentElement.Target?.GetType() == typeof(Positioned))
            moveable = parentElement;
        if (moveable == null) return;

        var positioned = (Positioned)moveable.Target!;
        var oldX = positioned.Left?.Value ?? 0f;
        var oldY = positioned.Top?.Value ?? 0f;

        // Log.Debug($"old={oldX}, {oldY} delta={e.DeltaX}, {e.DeltaY}");
        moveable.SetPropertyValue(moveable.Data.SetPropertyValue("Left", oldX + e.DeltaX));
        _controller.NotifyLayoutPropertyChanged?.Invoke("Left");
        moveable.SetPropertyValue(moveable.Data.SetPropertyValue("Top", oldY + e.DeltaY));
        _controller.NotifyLayoutPropertyChanged?.Invoke("Top");
        //TODO: maybe clear Right & Bottom value
        e.IsHandled = true;
    }

    public void OnDrop(DynamicWidgetMeta meta /*TODO: args for x, y*/)
    {
        //先判断是否容器类型(暂特殊处理多子级的容器)
        if (Meta?.ContainerType == ContainerType.MultiChild)
        {
            Widget childToBeAdded;
            DesignElement childElement;

            //TODO:暂简单特殊处理添加非Positioned至Stack内
            if (Meta.WidgetType == typeof(Stack) && meta.WidgetType != typeof(Positioned))
            {
                var positionedMeta = DynamicWidgetManager.GetByName(nameof(Positioned));
                childElement = new DesignElement(_controller, meta);
                var positionedElement = new DesignElement(_controller, positionedMeta);
                positionedElement.ChangeChild(null, childElement);
                childToBeAdded = new Positioned { Child = positionedElement };
            }
            else if (meta.ContainerType == ContainerType.SingleChildReversed)
            {
                childToBeAdded = meta.MakeDefaultInstance();
                childElement = new DesignElement(_controller, meta);
                meta.AddChild(childToBeAdded, childElement);
            }
            else
            {
                childToBeAdded = childElement = new DesignElement(_controller, meta);
            }

            AddChild(childToBeAdded);
            Invalidate(InvalidAction.Relayout);
            _controller.Select(childElement);
            return;
        }

        //判断是否反向包装
        if (Meta?.ContainerType == ContainerType.SingleChildReversed)
        {
            var newChild = new DesignElement(_controller, meta);
            ChangeChild(Child, newChild);
            _controller.Select(newChild);
            return;
        }

        ChangeMeta(meta, true);
        _controller.OnSelectionChanged(); //强制刷新属性面板
    }

    #endregion

    #region ====Widget Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (Child != null) action(Child);
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
        var width = CachedAvailableWidth = availableWidth;
        var height = CachedAvailableHeight = availableHeight;

        if (Child == null)
        {
            SetSize(width, height);
            return;
        }

        Child.Layout(width, height);
        SetSize(Child.W, Child.H);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (Child != null)
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