using System;
using System.Diagnostics;

namespace PixUI;

public abstract class Widget : IDisposable
{
    /// <summary>
    /// 是否不透明的
    /// </summary>
    public virtual bool IsOpaque => false;

    /// <summary>
    /// 调试标签
    /// </summary>
    public string? DebugLabel { get; set; }

    //TODO: IsOverflow判断是否超出可视范围

    /// <summary>
    /// 绑定当前Widget实例至WidgetRef
    /// </summary>
    public IWidgetRef Ref
    {
        set => value.SetWidget(this);
    }

    #region ----Flags----

    private int _flag;
    private const int MountedMask = 1;
    private const int HasLayoutMask = 2; //TODO:待实现自动判断是否需要重新布局后移除
    private const int LayoutTightMask = 1 << 3;
    private const int SuspendingMountMask = 1 << 20;

    private void SetFlagValue(bool value, int mask)
    {
        if (value)
            _flag |= mask;
        else
            _flag &= ~(mask);
    }

    /// <summary>
    /// 用于移动组件上下级关系时，临时禁止激发OnMount/OnUnmount
    /// </summary>
    public bool SuspendingMount
    {
        get => (_flag & SuspendingMountMask) == SuspendingMountMask;
        set => SetFlagValue(value, SuspendingMountMask);
    }

    /// <summary>
    /// 用于一些只需要布局一次的组件判断是否已经布局过，以减少布局计算
    /// </summary>
    protected bool HasLayout
    {
        get => (_flag & HasLayoutMask) == HasLayoutMask;
        set => SetFlagValue(value, HasLayoutMask);
    }

    /// <summary>
    /// 容器类布局时计算本身大小，如true则尽量收缩为子级的大小
    /// </summary>
    public bool IsLayoutTight
    {
        get => (_flag & LayoutTightMask) == LayoutTightMask;
        set
        {
            if (value == IsLayoutTight) return;
            SetFlagValue(value, LayoutTightMask);
            if (IsMounted)
                Invalidate(InvalidAction.Relayout);
        }
    }

    /// <summary>
    /// 是否挂载至WidgetTree
    /// </summary>
    public bool IsMounted
    {
        get => (_flag & MountedMask) == MountedMask;
        protected set
        {
            if (IsMounted == value) return;

            if (value)
            {
                _flag |= MountedMask;
                OnMounted();
            }
            else
            {
                _flag &= ~(MountedMask);
                OnUnmounted();
            }
        }
    }

    protected virtual void OnMounted() { }

    protected virtual void OnUnmounted() { }

    #endregion

    #region ----Layout Bounds----

    /// <summary>
    /// 布局计算后相对于上级的位置，允许覆写以支持动态计算
    /// </summary>
    protected internal virtual float X { get; private set; }

    /// <summary>
    /// 布局计算后相对于上级的位置，允许覆写以支持动态计算
    /// </summary>
    protected internal virtual float Y { get; private set; }

    /// <summary>
    /// 布局计算后的可视宽度
    /// </summary>
    protected internal float W { get; private set; }

    /// <summary>
    /// 布局计算后的可视高度
    /// </summary>
    protected internal float H { get; private set; }

    protected internal float CachedAvailableWidth = float.NaN; //TODO:考虑移到有子级的内
    protected internal float CachedAvailableHeight = float.NaN;

    private State<float>? _width;
    private State<float>? _height;

    /// <summary>
    /// 指定的宽度
    /// </summary>
    public State<float>? Width
    {
        get => _width;
        set => _width = Bind(_width, value, RelayoutOnStateChanged);
    }

    /// <summary>
    /// 指定的高度
    /// </summary>
    public State<float>? Height
    {
        get => _height;
        set => _height = Bind(_height, value, RelayoutOnStateChanged);
    }

    /// <summary>
    /// 是否布局计算出来的大小，即非同时指定宽高
    /// </summary>
    protected bool AutoSize => _width == null || _height == null;

    protected internal void SetPosition(float x, float y)
    {
        X = x;
        Y = y;
    }

    protected void SetSize(float w, float h)
    {
        W = w;
        H = h;
    }

    #endregion

    #region ----Tree----

    private Widget? _parent;

    public Widget? Parent
    {
        get => _parent;
        internal set
        {
            if (value == null && _parent == null) return;
            if (this is IRootWidget && value != null)
                throw new InvalidOperationException("Can't set parent for IRootWidget");
            if (_parent != null && value != null && !SuspendingMount)
                throw new InvalidOperationException("Widget already has parent");
            if (SuspendingMount && value == null) return; //忽略移动过程中设上级为空

            _parent = value;

            // 自动挂载或取消挂载至WidgetTree
            if (_parent == null)
            {
                Unmount();
            }
            else
            {
                if (_parent.IsMounted) Mount();
            }
        }
    }

    /// <summary>
    /// 根节点，null意味尚未挂载至WidgetTree
    /// </summary>
    public IRootWidget? Root
    {
        get
        {
            if (_parent != null)
                return _parent.Root;
            if (this is IRootWidget root)
                return root;
            return null;
        }
    }

    /// <summary>
    /// 获取当前的路由导航
    /// </summary>
    public Navigator? CurrentNavigator
    {
        get
        {
            var routeView = FindParent(w => w is RouteView);
            if (routeView == null) return null;
            return ((RouteView)routeView).Navigator;
        }
    }

    /// <summary>
    /// 遍历处理每个子级, 遍历子项时返回true停止
    /// </summary>
    public virtual void VisitChildren(Func<Widget, bool> action) { }

    protected internal virtual int IndexOfChild(Widget child)
    {
        var index = -1;
        var found = -1;
        VisitChildren(item =>
        {
            index++;
            if (!ReferenceEquals(item, child)) return false;
            found = index;
            return true;
        });
        return found;
    }

    /// <summary>
    /// 根据条件向上(包括自己)查找
    /// </summary>
    public Widget? FindParent(Predicate<Widget> predicate)
        => predicate(this) ? this : _parent?.FindParent(predicate);

    /// <summary>
    /// 当前Widget是否指定Widget的任意上级
    /// </summary>
    internal bool IsAnyParentOf(Widget? child)
    {
        if (child?.Parent == null) return false;
        return ReferenceEquals(child.Parent, this) || IsAnyParentOf(child.Parent);
    }

    #endregion

    #region ====HitTest====

    public virtual bool ContainsPoint(float x, float y) => x >= 0 && x < W && y >= 0 && y < H;

    /// <summary>
    /// 检测命中的MouseRegion
    /// </summary>
    protected internal virtual bool HitTest(float x, float y, HitTestResult result)
    {
        if (!ContainsPoint(x, y)) return false;

        //Console.WriteLine($"{this} HitTest: {x} {y}");

        if (result.Add(this))
            return true; //不再检测嵌套的子级

        VisitChildren(child => HitTestChild(child, x, y, result));

        return true;
    }

    protected bool HitTestChild(Widget child, float x, float y, HitTestResult result)
    {
        var scrollOffsetX = 0f;
        var scrollOffsetY = 0f;
        if (this is IScrollable scrollable && !scrollable.IgnoreScrollOffsetForHitTest)
        {
            scrollOffsetX = scrollable.ScrollOffsetX;
            scrollOffsetY = scrollable.ScrollOffsetY;
        }

        return child.HitTest(x - child.X + scrollOffsetX, y - child.Y + scrollOffsetY, result);
    }

    #endregion

    #region ====Bind====

    /// <summary>
    /// 绑定状态至Widget,用于首次绑定
    /// </summary>
    protected T Bind<T>(T newState, Action<State> action)
        where T : State
    {
        newState.AddListener(action);
        return newState;
    }

    /// <summary>
    /// 绑定状态至Widget,用于重新绑定
    /// </summary>
    protected T? Bind<T>(T? oldState, T? newState, Action<State> action)
        where T : State
    {
        if (ReferenceEquals(oldState, newState)) return newState;

        oldState?.RemoveListener(action);
        newState?.AddListener(action);

        //暂强制调用一次变更
        action(newState ?? State.Empty);

        return newState;
    }

    #endregion

    #region ====Mount & Layout====

    private void Mount()
    {
        if (SuspendingMount) return;

        IsMounted = true;
        VisitChildren(child =>
        {
            child.Mount();
            return false;
        });
    }

    private void Unmount()
    {
        if (SuspendingMount) return;

        IsMounted = false;
        VisitChildren(child =>
        {
            child.Unmount();
            return false;
        });
    }

    public virtual void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        var hasChildren = false;
        SetSize(0, 0);
        VisitChildren(child =>
        {
            hasChildren = true;
            child.Layout(width, height);
            SetSize(Math.Max(W, child.W), Math.Max(H, child.H));
            return false;
        });

        if (!hasChildren)
            SetSize(width, height);
    }

    /// <summary>
    /// 缓存可用宽度，并根据是否指定宽度取指定值与可用值的小值
    /// </summary>
    protected float CacheAndCheckAssignWidth(float availableWidth)
    {
        CachedAvailableWidth = Math.Max(0, availableWidth);
        return Width == null
            ? CachedAvailableWidth
            : Math.Min(Math.Max(0, Width.Value), CachedAvailableWidth);
    }

    /// <summary>
    /// 缓存可用高度，并根据是否指定高度取指定值与可用值的小值
    /// </summary>
    protected float CacheAndCheckAssignHeight(float availableHeight)
    {
        CachedAvailableHeight = Math.Max(0, availableHeight);
        return Height == null
            ? CachedAvailableHeight
            : Math.Min(Math.Max(0, Height.Value), CachedAvailableHeight);
    }

    /// <summary>
    /// 子组件尺寸变更后通知父项更新布局, 子类重写更新布局以减少布局计算
    /// </summary>
    protected internal virtual void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        Debug.Assert(AutoSize);

        var oldWidth = W;
        var oldHeight = H;

        //TODO:考虑避免重复layout，在这里传入参数表明由子级触发，无需再layout子级?
        Layout(CachedAvailableWidth, CachedAvailableHeight);
        TryNotifyParentIfSizeChanged(oldWidth, oldHeight, affects);
    }

    /// <summary>
    /// 如果自己的尺寸发生变更且上级是AutoSize的则通知上级
    /// </summary>
    protected internal void TryNotifyParentIfSizeChanged(float oldWidth, float oldHeight, AffectsByRelayout affects)
    {
        var dx = W - oldWidth;
        var dy = H - oldHeight;
        if (dx != 0 || dy != 0)
        {
            affects.Widget = this;
            affects.OldX = X;
            affects.OldY = Y;
            affects.OldW = oldWidth;
            affects.OldH = oldHeight;
            if (Parent != null && Parent.AutoSize)
                Parent.OnChildSizeChanged(this, dx, dy, affects);
        }
    }

    /// <summary>
    /// 映射组件的本地坐标至窗体坐标
    /// </summary>
    public Point LocalToWindow(float x, float y)
    {
        Widget? temp = this;
        while (temp != null)
        {
            x += temp.X;
            y += temp.Y;
            //判断上级是否IScrollable,是则处理偏移量
            if (temp.Parent is IScrollable scrollable)
            {
                x -= scrollable.ScrollOffsetX;
                y -= scrollable.ScrollOffsetY;
            }
            //判断上级是否Transform,是则变换坐标
            else if (temp.Parent is Transform transform)
            {
                var transformed =
                    MatrixUtils.TransformPoint(transform.EffectiveTransform, x, y);
                x = transformed.Dx;
                y = transformed.Dy;
            }

            temp = temp.Parent;
        }

        // Debug.Assert(this is IRootWidget);
        return new Point(x, y);
    }

    #endregion

    #region ====Paint====

    /// <summary>
    /// 开始绘制前转换画布坐标为本地坐标，并且根所需要转换矩阵或裁剪绘制区域
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="onlyTransform">用于脏区域绘制时仅转换坐标矩阵，不需要重复裁剪</param>
    /// <param name="dirtyRect">用于脏区域绘制时裁剪区域</param>
    protected internal virtual void BeforePaint(Canvas canvas, bool onlyTransform = false,
        Rect? dirtyRect = null /*TODO: use IDirtyArea*/)
    {
        canvas.Translate(X, Y);
        if (dirtyRect.HasValue)
        {
            Debug.Assert(onlyTransform == false);
            canvas.ClipRect(dirtyRect.Value, ClipOp.Intersect, false); //不需要保存画布状态,InvalidQueue会恢复
        }
    }

    /// <summary>
    /// 绘制完成后恢复坐标，并且根据需要恢复矩阵转换或裁剪区域
    /// </summary>
    protected internal virtual void AfterPaint(Canvas canvas)
    {
        canvas.Translate(-X, -Y);
    }

    public virtual void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (W == 0 || H == 0 || canvas.IsClipEmpty)
            return;

        PaintChildren(canvas, area);
    }

    protected void PaintChildren(Canvas canvas, IDirtyArea? area = null)
    {
        if (area is RepaintChild repaintChild)
        {
            var child = repaintChild.Child;
            child.BeforePaint(canvas, true);
            child.Paint(canvas, repaintChild.ToChild(child));
        }
        else
        {
            VisitChildren(child =>
            {
                if (child.W <= 0 || child.H <= 0)
                    return false;
                if (area != null && !area.IntersectsWith(child))
                    return false; //脏区域与子组件没有相交部分，不用绘制

                child.BeforePaint(canvas);
                child.Paint(canvas, area?.ToChild(child));
                child.AfterPaint(canvas);

                PaintDebugger.PaintWidgetBorder(child, canvas);
                return false;
            });
        }
    }

    #endregion

    #region ====StateChange & Invalidate====

    public void Invalidate(InvalidAction action, IDirtyArea? area = null)
    {
        InvalidQueue.Add(this, action, area);
    }

    protected virtual void RepaintOnStateChanged(State state)
    {
        InvalidQueue.Add(this, InvalidAction.Repaint, null);
    }

    protected virtual void RelayoutOnStateChanged(State state)
    {
        InvalidQueue.Add(this, InvalidAction.Relayout, null);
    }

    #endregion

    #region ====Overlay====

    public Overlay? Overlay => Root?.Window.Overlay;

    #endregion

    #region ====IDisposable====

    public virtual void Dispose()
    {
        Parent = null;
    }

    #endregion

    [TSRawScript(@"public toString(): string {
    return `${this.constructor.name}[${this.DebugLabel ?? ''}]`;
}")]
    public override string ToString() => string.IsNullOrEmpty(DebugLabel)
        ? $"{GetType().Name}"
        : $"{GetType().Name}[\"{DebugLabel}\"]";
}