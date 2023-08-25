using System;
using System.Linq;

namespace PixUI.Dynamic.Design;

/// <summary>
/// 设计时包装目标Widget
/// </summary>
public sealed class DesignElement : Widget, IMouseRegion
{
    /// <summary>
    /// Ctor for Root DesignElement or Deserialize
    /// </summary>
    public DesignElement(DesignController controller, string slotName)
    {
        _controller = controller;
        _slotName = slotName;
        MouseRegion = new MouseRegion(opaque: false);
        MouseRegion.PointerDown += OnPointerDown;
        MouseRegion.PointerMove += OnPointerMove;

        if (_slotName == string.Empty)
        {
            _controller.RootElement = this;
            DebugLabel = "Root";
        }
    }

    /// <summary>
    /// Ctor for designtime
    /// </summary>
    private DesignElement(DesignController controller, DynamicWidgetMeta meta, string slotName) : this(controller,
        slotName)
    {
        if (slotName == string.Empty)
            throw new ArgumentNullException(nameof(slotName));

        ChangeMeta(meta, !meta.IsReversedWrapElement);
    }

    private static readonly Lazy<Paragraph> _hint = new(() =>
        TextPainter.BuildParagraph("Drop Here", float.PositiveInfinity, 16, Colors.Gray));

    private readonly DesignController _controller;
    private readonly string _slotName;
    private bool _isSelected;
    private Widget? _child;

    public DynamicWidgetMeta? Meta { get; private set; }

    public DynamicWidgetData Data { get; } = new();

    /// <summary>
    /// 包装的目标组件
    /// </summary>
    /// <remarks>如果是反向包装，返回的是上级</remarks>
    public Widget? Target => Meta is { IsReversedWrapElement: true } ? Parent : Child;

    public Widget? Child
    {
        get => _child;
        internal set
        {
            if (_child != null)
                _child.Parent = null;

            _child = value;

            if (_child != null)
            {
                _child.Parent = this;
#if DEBUG
                DebugLabel = Target?.GetType().Name;
#endif
            }

            if (IsMounted) Invalidate(InvalidAction.Relayout);
        }
    }

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

    private bool IsRoot => ReferenceEquals(this, _controller.RootElement);

    public bool IsContainer => Meta == null /*Root*/ || Meta.IsContainer;

    private void ChangeMeta(DynamicWidgetMeta? meta, bool makeDefaultTarget)
    {
        Meta = meta;
        Data.Type = Meta == null ? string.Empty : Meta.Name;
        Data.Properties?.Clear();
        MouseRegion.Opaque = !IsContainer;
        if (makeDefaultTarget || (Meta == null && Child != null))
        {
            Child = Meta?.CreateInstance();
            if (Child != null && Meta!.Properties != null) //设置设计时初始化属性值
            {
                var initProps = Meta.Properties.Where(p => p.InitValue != null);
                foreach (var prop in initProps)
                {
                    var propValue = Data.SetPropertyValue(prop.Name, prop.InitValue!.Value);
                    SetPropertyValue(propValue);
                }
            }
        }
    }

    /// <summary>
    /// 初始化属性的值改变后重新创建实例
    /// </summary>
    internal void OnInitPropertyValueChanged()
    {
        if (Meta == null) throw new Exception();

        var newTarget = Meta.CreateInstance();
        Child = newTarget; //set Child before reset properties

        //重设属性值
        if (Data.Properties != null)
        {
            foreach (var propertyValue in Data.Properties)
            {
                SetPropertyValue(propertyValue);
            }
        }
    }

    /// <summary>
    /// 设置目标组件的运行时属性值
    /// </summary>
    public void SetPropertyValue(PropertyValue propertyValue)
    {
        if (Meta == null || Target == null) throw new Exception();

        //TODO: emit 优化，暂用反射
        var propMeta = Meta.GetPropertyMeta(propertyValue.Name);
        var propValue = propMeta.GetRuntimeValue(propertyValue.Value);
        var propInfo = Meta.WidgetType.GetProperty(propertyValue.Name);
        propInfo!.SetValue(Target, propValue);
    }

    /// <summary>
    /// 移除目标组件的运行时属性值（如果可以为空）
    /// </summary>
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
        _controller.MoveElements(e.DeltaX, e.DeltaY);
        e.IsHandled = true;
    }

    public void OnDrop(DynamicWidgetMeta meta /*TODO: args for x, y*/)
    {
        //是否根节点或占位的Slot, eg: HSplitter的Left slot or Right slot
        if (Meta == null)
        {
            ChangeMeta(meta, true);
            _controller.OnSelectionChanged(); //强制刷新属性面板
            return;
        }

        //非容器退出
        if (!Meta.IsContainer) return;

        //获取默认的Slot
        var defaultSlot = Meta.Slots![0];
        if (defaultSlot.ContainerType == ContainerType.MultiChild)
        {
            Widget childToBeAdded;
            DesignElement childElement;

            //TODO:暂简单特殊处理添加非Positioned至Stack内
            if (Meta.WidgetType == typeof(Stack) && meta.WidgetType != typeof(Positioned))
            {
                childElement = new DesignElement(_controller, meta, nameof(Positioned.Child));
                var positionedMeta = DynamicWidgetManager.GetByName(nameof(Positioned));
                var positionedElement = new DesignElement(_controller, positionedMeta, defaultSlot.PropertyName)
                    { Child = childElement };
                childToBeAdded = new Positioned { Child = positionedElement };
            }
            else if (meta.IsReversedWrapElement)
            {
                throw new NotImplementedException();
                // childToBeAdded = meta.CreateInstance();
                // childElement = new DesignElement(_controller, meta, defaultSlot.PropertyName);
                // meta.AddChild(childToBeAdded, childElement);
            }
            else
            {
                childToBeAdded = childElement = new DesignElement(_controller, meta, defaultSlot.PropertyName);
            }

            if (defaultSlot.TryAddChild(Target!, childToBeAdded))
            {
                Invalidate(InvalidAction.Relayout);
                _controller.Select(childElement);
            }
        }
        else if (defaultSlot.ContainerType == ContainerType.SingleChild)
        {
            var newChild = new DesignElement(_controller, meta, defaultSlot.PropertyName);
            if (defaultSlot.TrySetChild(Target!, newChild))
            {
                Invalidate(InvalidAction.Relayout);
                _controller.Select(newChild);
            }
        }
        else
        {
            //eg: drop widget to Positioned, check it, should never be here!
            throw new NotImplementedException();
            // var newChild = new DesignElement(_controller, meta, parentChildrenPropName);
            // Child = newChild;
            // _controller.Select(newChild);
        }
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