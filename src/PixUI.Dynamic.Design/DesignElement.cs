using System;
using System.Diagnostics;
using System.Linq;

namespace PixUI.Dynamic.Design;

/// <summary>
/// 设计时包装目标Widget
/// </summary>
public sealed class DesignElement : Widget, IMouseRegion, IDesignElement
{
    /// <summary>
    /// Ctor for Root DesignElement or Deserialize
    /// </summary>
    public DesignElement(DesignController controller, string slotName)
    {
        Controller = controller;
        SlotName = slotName;
        MouseRegion = new MouseRegion(opaque: false);
        MouseRegion.PointerDown += OnPointerDown;
        MouseRegion.PointerMove += OnPointerMove;
        MouseRegion.HoverChanged += OnHoverChanged;

        if (SlotName == string.Empty)
        {
            Controller.RootElement = this;
            DebugLabel = "Root";
        }
    }

    /// <summary>
    /// Ctor for designtime
    /// </summary>
    internal DesignElement(DesignController controller, DynamicWidgetMeta meta, string slotName)
        : this(controller, slotName)
    {
        if (slotName == string.Empty)
            throw new ArgumentNullException(nameof(slotName));

        ChangeMeta(meta, !meta.IsReversedWrapElement);
    }

    // private static readonly Lazy<Paragraph> _hint = new(() =>
    //     TextPainter.BuildParagraph("Drop Here", float.PositiveInfinity, 16, Colors.Gray));

    public readonly DesignController Controller;
    private bool _isSelected;
    private Widget? _child;
    private SelectedDecorator? _selectedDecorator;
    private AnchorPosition? _hoverAnchor;

    /// <summary>
    /// 当前针对上级的Slot属性名
    /// </summary>
    internal string SlotName { get; }

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

            if (_isSelected)
                ShowSelectedDecorator();
            else
                HideSelectedDecorator();
        }
    }

    internal bool IsRoot => ReferenceEquals(this, Controller.RootElement);

    public bool IsContainer => Meta == null /*Root*/ || Meta.IsContainer;

    #region ====Meta & Property Value====

    /// <summary>
    /// 用于删除时清除
    /// </summary>
    internal void ClearMeta()
    {
        Meta = null;
        Child = null;
        Data.Properties?.Clear();
        MouseRegion.Opaque = !IsContainer;
    }

    internal void ChangeMeta(DynamicWidgetMeta meta, bool makeDefaultTarget)
    {
        Meta = meta;
        Data.Properties?.Clear();
        MouseRegion.Opaque = !IsContainer;
        if (makeDefaultTarget)
        {
            Child = Meta.CreateInstance();
            if (Meta.Properties != null) //设置设计时初始化属性值
            {
                var initProps = Meta.Properties.Where(p => p.InitValue != null);
                foreach (var prop in initProps)
                {
                    var propValue = Data.SetPropertyValue(prop.Name, prop.InitValue!.Value);
                    SetPropertyValue(propValue);
                }
            }

            //暂在这里特殊处理Row, Column等IsLayoutTight的容器实例，默认添加占位的子组件
            if (Child is Row || Child is Column) //TODO: 应判断childMeta是否需要创建占位子组件
            {
                var defaultSlot = Meta!.Slots![0];
                for (var i = 0; i < 3; i++)
                {
                    defaultSlot.TryAddChild(Child, new DesignElement(Controller, "Children")
                    {
                        Width = 100, Height = 50,
                    });
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

        var propMeta = Meta.GetPropertyMeta(propertyValue.Name);
        propMeta.SetRuntimeValue(Meta, Target, propertyValue.Value);
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

    internal void ChangeLayoutPropertyValue(string propName, float value)
    {
        SetPropertyValue(Data.SetPropertyValue(propName, value));
        Controller.NotifyLayoutPropertyChanged?.Invoke(propName);
    }

    #endregion

    #region ====Event Handler====

    private void ShowSelectedDecorator()
    {
        _selectedDecorator = new SelectedDecorator(this);
        Overlay?.Show(_selectedDecorator);
    }

    private void HideSelectedDecorator()
    {
        if (_selectedDecorator == null) return;

        (_selectedDecorator.Parent as Overlay)?.Remove(_selectedDecorator);
        _selectedDecorator.Dispose();
        _selectedDecorator = null;
    }

    private void OnHoverChanged(bool isHover)
    {
        if (!isHover && IsSelected)
            Cursor.Current = Cursors.Arrow; //reset cursor for anchor
    }

    private void OnPointerDown(PointerEvent e)
    {
        e.IsHandled = true;
        if (e.Buttons == PointerButtons.Left)
        {
            Controller.Select(this);
        }
        else if (e.Buttons == PointerButtons.Right)
        {
            Controller.ShowContextMenu();
        }
    }

    private void OnPointerMove(PointerEvent e)
    {
        if (e.Buttons == PointerButtons.None)
        {
            if (IsSelected)
            {
                Cursor.Current = Cursors.Arrow;
                _hoverAnchor = null;
                foreach (var pos in Enum.GetValues<AnchorPosition>())
                {
                    if (GetAnchorRect(pos).ContainsPoint(e.X, e.Y))
                    {
                        _hoverAnchor = pos;
                        if (pos == AnchorPosition.MiddleLeft || pos == AnchorPosition.MiddleRight)
                            Cursor.Current = Cursors.ResizeLR;
                        else
                            Cursor.Current = Cursors.ResizeUD;
                        break;
                    }
                }
            }
        }
        else if (e.Buttons == PointerButtons.Left)
        {
            //先判断是否移动Anchor
            if (_hoverAnchor.HasValue)
            {
                Resize(_hoverAnchor.Value, e.DeltaX, e.DeltaY);
            }
            else
            {
                //再移动元素
                var cmd = new MoveElementsCommand(e.DeltaX, e.DeltaY);
                cmd.Run(Controller);
            }

            e.IsHandled = true;
        }
    }

    private const string LEFT = "Left";
    private const string TOP = "Top";
    private const string RIGHT = "Right";
    private const string BOTTOM = "Bottom";
    private const string WIDTH = "Width";
    private const string HEIGHT = "Height";

    private void Resize(AnchorPosition pos, float dx, float dy)
    {
        if (Meta is { IsReversedWrapElement: true }) return; //暂不允许
        if (Target == null) return; //不允许Resize占位用的组件

        DesignElement? parentPositioned = null;
        if (Parent is DesignElement { Target: Positioned } parent)
            parentPositioned = parent;

        switch (pos)
        {
            case AnchorPosition.MiddleLeft:
                ResizeLeft(parentPositioned, dx);
                break;
            case AnchorPosition.MiddleRight:
                ResizeRight(parentPositioned, dx);
                break;
            case AnchorPosition.TopMiddle:
                ResizeTop(parentPositioned, dy);
                break;
            case AnchorPosition.BottomMiddle:
                ResizeBottom(parentPositioned, dy);
                break;
        }
    }

    private void ResizeLeft(DesignElement? parentPositioned, float dx)
    {
        if (dx == 0) return;

        var oldWidth = Data.TryGetPropertyValue(WIDTH, out var width)
            ? (float)width!.Value.Value!
            : Target!.W;
        var newWidth = oldWidth - dx;
        if (parentPositioned == null)
        {
            ChangeLayoutPropertyValue(WIDTH, newWidth); //设置自身的宽度
        }
        else
        {
            var hasLeft = parentPositioned.Data.TryGetPropertyValue(LEFT, out var left);
            var hasRight = parentPositioned.Data.TryGetPropertyValue(RIGHT, out _);
            if (hasLeft && hasRight)
            {
                //Left与Right都有值仅调整Left
                parentPositioned.ChangeLayoutPropertyValue(LEFT, ((float)left!.Value.Value!) + dx);
            }
            else if (hasLeft)
            {
                //仅Left有值调整Left并修改宽度
                ChangeLayoutPropertyValue(WIDTH, newWidth);
                parentPositioned.ChangeLayoutPropertyValue(LEFT, ((float)left!.Value.Value!) + dx);
            }
            else
            {
                //仅Right有值修改宽度
                ChangeLayoutPropertyValue(WIDTH, newWidth);
            }
        }
    }

    private void ResizeRight(DesignElement? parentPositioned, float dx)
    {
        if (dx == 0) return;

        var oldWidth = Data.TryGetPropertyValue(WIDTH, out var width)
            ? (float)width!.Value.Value!
            : Target!.W;
        var newWidth = oldWidth + dx;
        if (parentPositioned == null)
        {
            ChangeLayoutPropertyValue(WIDTH, newWidth); //设置自身的宽度
        }
        else
        {
            var hasLeft = parentPositioned.Data.TryGetPropertyValue(LEFT, out _);
            var hasRight = parentPositioned.Data.TryGetPropertyValue(RIGHT, out var right);
            if (hasLeft && hasRight)
            {
                //Left与Right都有值仅调整Right
                parentPositioned.ChangeLayoutPropertyValue(RIGHT, ((float)right!.Value.Value!) - dx);
            }
            else if (hasRight)
            {
                //仅Right有值调整Right并修改宽度
                ChangeLayoutPropertyValue(WIDTH, newWidth);
                parentPositioned.ChangeLayoutPropertyValue(RIGHT, ((float)right!.Value.Value!) - dx);
            }
            else
            {
                //仅Left有值修改宽度
                ChangeLayoutPropertyValue(WIDTH, newWidth);
            }
        }
    }

    private void ResizeTop(DesignElement? parentPositioned, float dy)
    {
        if (dy == 0) return;

        var oldHeight = Data.TryGetPropertyValue(HEIGHT, out var height)
            ? (float)height!.Value.Value!
            : Target!.H;
        var newHeight = oldHeight - dy;
        if (parentPositioned == null)
        {
            ChangeLayoutPropertyValue(HEIGHT, newHeight); //设置自身的高度
        }
        else
        {
            var hasTop = parentPositioned.Data.TryGetPropertyValue(TOP, out var top);
            var hasBottom = parentPositioned.Data.TryGetPropertyValue(BOTTOM, out _);
            if (hasTop && hasBottom)
            {
                parentPositioned.ChangeLayoutPropertyValue(TOP, ((float)top!.Value.Value!) + dy);
            }
            else if (hasTop)
            {
                ChangeLayoutPropertyValue(HEIGHT, newHeight);
                parentPositioned.ChangeLayoutPropertyValue(TOP, ((float)top!.Value.Value!) + dy);
            }
            else
            {
                ChangeLayoutPropertyValue(HEIGHT, newHeight);
            }
        }
    }

    private void ResizeBottom(DesignElement? parentPositioned, float dy)
    {
        if (dy == 0) return;

        var oldHeight = Data.TryGetPropertyValue(HEIGHT, out var height)
            ? (float)height!.Value.Value!
            : Target!.H;
        var newHeight = oldHeight + dy;
        if (parentPositioned == null)
        {
            ChangeLayoutPropertyValue(HEIGHT, newHeight);
        }
        else
        {
            var hasTop = parentPositioned.Data.TryGetPropertyValue(TOP, out _);
            var hasBottom = parentPositioned.Data.TryGetPropertyValue(BOTTOM, out var bottom);
            if (hasTop && hasBottom)
            {
                parentPositioned.ChangeLayoutPropertyValue(BOTTOM, ((float)bottom!.Value.Value!) - dy);
            }
            else if (hasBottom)
            {
                ChangeLayoutPropertyValue(HEIGHT, newHeight);
                parentPositioned.ChangeLayoutPropertyValue(BOTTOM, ((float)bottom!.Value.Value!) - dy);
            }
            else
            {
                ChangeLayoutPropertyValue(HEIGHT, newHeight);
            }
        }
    }

    public void OnDrop(DynamicWidgetMeta meta /*TODO: args for x, y*/)
    {
        //是否根节点或用于占位的Slot
        if (Meta == null)
        {
            DropToPlaceholder(meta);
            return;
        }

        //非容器退出
        if (!Meta.IsContainer) return;

        //获取默认的Slot
        var defaultSlot = Meta.Slots![0];
        switch (defaultSlot.ContainerType)
        {
            case ContainerType.MultiChild:
                DropToMultiSlot(defaultSlot, meta);
                break;
            case ContainerType.SingleChild:
                DropToSingleSlot(defaultSlot, meta);
                break;
            default:
                DropToReversedSlot(defaultSlot, meta);
                break;
        }
    }

    private void DropToPlaceholder(DynamicWidgetMeta meta)
    {
        if (meta.IsReversedWrapElement) //eg: drop Expanded to Row's placeholder
        {
            var parentElement = Parent?.Parent as DesignElement;
            if (parentElement == null || !parentElement.Meta!.IsContainer) return;
            var parentDefaultSlot = parentElement.Meta.Slots![0];
            if (parentDefaultSlot.ContainerType != ContainerType.MultiChild) return;

            var childWidget = meta.CreateInstance();
            var childElement = new DesignElement(Controller, meta, parentDefaultSlot.PropertyName);
            var childDefaultSlot = meta.Slots![0];
            childDefaultSlot.TrySetChild(childWidget, childElement);

            parentDefaultSlot.TryReplaceChild(parentElement.Target!, this, childWidget);
            parentElement.Invalidate(InvalidAction.Relayout);
            Controller.Select(childElement);
        }
        else
        {
            //需要清除占位大小
            Width = null;
            Height = null;
            ChangeMeta(meta, !meta.IsReversedWrapElement);
            Controller.OnSelectionChanged(); //强制刷新属性面板
        }

        Controller.RaiseOutlineChanged();
    }

    private void DropToMultiSlot(ContainerSlot defaultSlot, DynamicWidgetMeta meta)
    {
        Widget childToBeAdded;
        DesignElement childElement;

        //TODO:暂简单特殊处理添加非Positioned至Stack内
        if (Meta!.WidgetType == typeof(Stack) && meta.WidgetType != typeof(Positioned))
        {
            childElement = new DesignElement(Controller, meta, nameof(Positioned.Child));
            var positionedMeta = DynamicWidgetManager.GetByName(nameof(Positioned));
            var positionedElement = new DesignElement(Controller, positionedMeta, defaultSlot.PropertyName)
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
            childToBeAdded = childElement = new DesignElement(Controller, meta, defaultSlot.PropertyName);
        }

        if (defaultSlot.TryAddChild(Target!, childToBeAdded))
        {
            Invalidate(InvalidAction.Relayout);
            Controller.Select(childElement);
            Controller.RaiseOutlineChanged();
        }
    }

    private void DropToSingleSlot(ContainerSlot defaultSlot, DynamicWidgetMeta meta)
    {
        var newChild = new DesignElement(Controller, meta, defaultSlot.PropertyName);
        if (defaultSlot.TrySetChild(Target!, newChild))
        {
            Invalidate(InvalidAction.Relayout);
            Controller.Select(newChild);
            Controller.RaiseOutlineChanged();
        }
    }

    private void DropToReversedSlot(ContainerSlot defaultSlot, DynamicWidgetMeta meta)
    {
        //eg: drop widget to Expanded
        var newChild = new DesignElement(Controller, meta, defaultSlot.PropertyName);
        Child = newChild;
        Controller.Select(newChild);
        Controller.OnSelectionChanged();
    }

    #endregion

    #region ====Widget Overrides====

    protected override void OnMounted()
    {
        base.OnMounted();
        if (_isSelected)
            ShowSelectedDecorator();
    }

    protected override void OnUnmounted()
    {
        HideSelectedDecorator();
        base.OnUnmounted();
    }

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
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        if (Child == null)
        {
            SetSize(width, height); //仅设置占位宽高
            return;
        }

        Child.Layout(CachedAvailableWidth, CachedAvailableHeight); //始终为允许的宽高
        SetSize(Child.W, Child.H);
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        Debug.Assert(AutoSize);

        if (IsRoot) return;

        var oldWidth = W;
        var oldHeight = H;
        SetSize(child.W, child.H); //直接更新自己的大小

        TryNotifyParentIfSizeChanged(oldWidth, oldHeight, affects);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        DrawBackground(canvas);

        if (Child != null)
            base.Paint(canvas, area);
        else
            DrawPlaceholder(canvas);
    }

    private void DrawBackground(Canvas canvas)
    {
        if (!IsRoot) return;

        if (Controller.BackgroundImage != null)
        {
            canvas.DrawImage(Controller.BackgroundImage, Rect.FromLTWH(0, 0, W, H)); //暂填满
        }
    }

    private void DrawPlaceholder(Canvas canvas)
    {
        using var dash = PathEffect.CreateDash(new[] { 5f, 5f }, 10);
        var paint = PaintUtils.Shared(Colors.Gray, PaintStyle.Stroke, 1f);
        paint.AntiAlias = true;
        paint.PathEffect = dash;
        canvas.DrawRect(Rect.FromLTWH(0.5f, 0.5f, W - 1, H - 1), paint);
        canvas.DrawLine(0, 0, W, H, paint);
        canvas.DrawLine(W, 0, 0, H, paint);

        // var paragraph = _hint.Value;
        // canvas.DrawParagraph(paragraph, (W - paragraph.MaxIntrinsicWidth) / 2f, (H - paragraph.Height) / 2f);
    }

    internal Rect GetAnchorRect(AnchorPosition position)
    {
        //TopLeft
        //Rect.FromLTWH(0 - anchorSize / 2, 0 - anchorSize / 2, anchorSize, anchorSize)
        //TopRight
        //Rect.FromLTWH(W - anchorSize / 2, 0 - anchorSize / 2, anchorSize, anchorSize)
        //BottomLeft
        //Rect.FromLTWH(0 - anchorSize / 2, H - anchorSize / 2, anchorSize, anchorSize)
        //BottomRight
        //Rect.FromLTWH(W - anchorSize / 2, H - anchorSize / 2, anchorSize, anchorSize)

        var scaleRatio = Controller.Zoom.Value / 100f;
        var anchorSize = 10f * scaleRatio;

        return position switch
        {
            AnchorPosition.TopMiddle => Rect.FromLTWH(W / 2 - anchorSize / 2, 0 - anchorSize / 2, anchorSize,
                anchorSize),
            AnchorPosition.MiddleLeft => Rect.FromLTWH(0 - anchorSize / 2, H / 2 - anchorSize / 2, anchorSize,
                anchorSize),
            AnchorPosition.MiddleRight => Rect.FromLTWH(W - anchorSize / 2, H / 2 - anchorSize / 2, anchorSize,
                anchorSize),
            AnchorPosition.BottomMiddle => Rect.FromLTWH(W / 2 - anchorSize / 2, H - anchorSize / 2, anchorSize,
                anchorSize),
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
        };
    }

    #endregion
}