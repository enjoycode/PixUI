namespace PixUI.Diagram;

public abstract class DiagramItem
{
    #region ====Fields & Properties====

    private DiagramSurface? _surface;
    private bool _isSelected;

    public DiagramSurface? Surface
    {
        get => Parent == null ? _surface : Parent.Surface;
        set => _surface = value;
    }

    public abstract Rect Bounds { get; set; }

    public virtual bool Visible => true;

    public bool IsSelected
    {
        get => _isSelected;
        internal set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnIsSelectedChanged(!_isSelected, _isSelected);
            }
        }
    }

    public Point Location
    {
        get => new(Bounds.X, Bounds.Y);
        set => SetBounds(value.X, value.Y, Bounds.Width, Bounds.Height, BoundsSpecified.Location);
    }

    /// <summary>
    /// 获取当前设计对象的设计时行为
    /// </summary>
    public virtual DesignBehavior DesignBehavior => DesignBehavior.CanMove | DesignBehavior.CanResize;

    public float StrokeThickness { get; set; } = 1f;

    public float[]? StrokeDashArray { get; set; }

    #endregion

    #region ====Event Handlers====

    /// <summary>
    /// Called when the IsSelected property has changed.
    /// </summary>
    /// <param name="oldValue">The old value of the IsSelected property.</param>
    /// <param name="newValue">The new value of the IsSelected property.</param>
    protected virtual void OnIsSelectedChanged(bool oldValue, bool newValue)
    {
        //TODO:
        // if (!newValue)
        //     this.IsInEditMode = false;
        //
        // this.NotifyChangedSelection(newValue);
        //
        // this.UpdateVisualStates();
    }

    /// <summary>
    /// 用于设计时新建的时候初始化
    /// </summary>
    protected internal virtual void OnCreated() { }

    #endregion

    #region ====Paint Methods====

    public abstract void Paint(Canvas canvas);

    protected internal virtual void Invalidate()
    {
        if (Parent != null)
        {
            Parent.Invalidate(); //todo:
        }
        else if (Surface != null)
        {
            //var ptCanvas = PointToSurface(Point.Empty);
            //var rect = Rect.FromLTWH(ptCanvas.X, ptCanvas.Y, Bounds.Width, Bounds.Height);
            //rect.Inflate(1,1);
            Surface.Repaint( /*TODO: Rect.Ceiling(rect)*/);
        }
    }

    #endregion

    #region ====Layout Methods====

    /// <summary>
    /// 将本地坐标转换为画布坐标
    /// </summary>
    protected internal Point PointToSurface(Point clientPt)
    {
        var x = clientPt.X;
        var y = clientPt.Y;

        DiagramItem? temp = this;
        while (temp != null)
        {
            x += (int)temp.Bounds.X;
            y += (int)temp.Bounds.Y;
            temp = temp.Parent;
        }

        return new Point(x, y);
    }

    /// <summary>
    /// 将画布坐标转换为本地坐标
    /// </summary>
    public Point PointToClient(Point surfacePt)
    {
        var zero = PointToSurface(Point.Empty);
        return new Point(surfacePt.X - zero.X, surfacePt.Y - zero.Y);
    }

    protected abstract void SetBounds(float x, float y, float width, float height, BoundsSpecified specified);

    /// <summary>
    /// 元素Bounds改变后通知Canvas重新绘制合并区域
    /// </summary>
    protected void InvalidateOnBoundsChanged(Rect oldBounds)
    {
        if (Surface == null)
            return;

        // var ptCanvas = Parent?.PointToSurface(Point.Empty) ?? Point.Empty;
        // var invalidRect = Rect.Inflate(Rect.Union(oldBounds, Bounds), 2, 2);
        // invalidRect.X += ptCanvas.X;
        // invalidRect.Y += ptCanvas.Y;

        Surface.Repaint( /*TODO: Rect.Ceiling(invalidRect)*/); //画旧区域与新区域的Union
    }

    public void Move(Offset delta)
    {
        if ((DesignBehavior & DesignBehavior.CanMove) != DesignBehavior.CanMove) return;
        if (delta is { Dx: 0, Dy: 0 }) return;

        if (Parent != null)
        {
            //预留10像素点 否则移到极限位置时候不容易在选中
            const float threshold = 10f;
            var newBounds = Rect.FromLTWH(Bounds.X + delta.Dx, Bounds.Y + delta.Dy, Bounds.Width, Bounds.Height);
            if (newBounds.Y + threshold >= Parent.Bounds.Height) return;
            if (newBounds.X + threshold >= Parent.Bounds.Width) return;
            if (newBounds.X + newBounds.Width <= threshold) return;
            if (newBounds.Y + newBounds.Height <= threshold) return;
        }

        SetBounds(Bounds.X + delta.Dx, Bounds.Y + delta.Dy, Bounds.Width, Bounds.Height, BoundsSpecified.Location);
    }

    internal void Resize(ResizeAnchorLocation location, int deltaX, int deltaY)
    {
        if ((DesignBehavior & DesignBehavior.CanResize) != DesignBehavior.CanResize) return;
        if (deltaX == 0 && deltaY == 0) return;

        //TODO: check minSize
        var newBounds = Bounds;

        switch (location)
        {
            case ResizeAnchorLocation.LeftTop:
                newBounds.X += deltaX;
                newBounds.Y += deltaY;
                newBounds.Width -= deltaX;
                newBounds.Height -= deltaY;
                break;
            case ResizeAnchorLocation.LeftCenter:
                newBounds.X += deltaX;
                newBounds.Width -= deltaX;
                break;
            case ResizeAnchorLocation.LeftBottom:
                newBounds.X += deltaX;
                newBounds.Width -= deltaX;
                newBounds.Height += deltaY;
                break;
            case ResizeAnchorLocation.RightTop:
                newBounds.Y += deltaY;
                newBounds.Width += deltaX;
                newBounds.Height -= deltaY;
                break;
            case ResizeAnchorLocation.RightCenter:
                newBounds.Width += deltaX;
                break;
            case ResizeAnchorLocation.RightBottom:
                newBounds.Width += deltaX;
                newBounds.Height += deltaY;
                break;
            case ResizeAnchorLocation.TopCenter:
                newBounds.Y += deltaY;
                newBounds.Height -= deltaY;
                break;
            case ResizeAnchorLocation.BottomCenter:
                newBounds.Height += deltaY;
                break;
        }

        SetBounds(newBounds.X, newBounds.Y, newBounds.Width, newBounds.Height,
            BoundsSpecified.All); //TODO: fix BoundsSpecified
    }

    /// <summary>
    /// 从画布或上级移除自身
    /// </summary>
    public void Remove()
    {
        //必须先发出重绘指令
        Invalidate();

        if (Parent == null)
            Surface!.RemoveItem(this);
        else
            Parent.RemoveItem(this);
    }

    protected internal virtual void OnAddToSurface() { }

    protected internal virtual void OnRemoveFromSurface() { }

    #endregion

    #region ====Container Implements====

    private List<DiagramItem>? _items;

    public IEnumerable<DiagramItem> Items => _items ?? Enumerable.Empty<DiagramItem>();

    public DiagramItem? Parent { get; private set; }

    protected internal virtual bool IsContainer => false;

    public void AddItem(DiagramItem item)
    {
        if (!IsContainer)
            throw new InvalidOperationException();

        item.Parent = this;
        _items ??= [];
        _items.Add(item);
        item.OnAddToSurface();
        //非DiagramHostItem需要刷新Canvas
        Invalidate();
    }

    public void RemoveItem(DiagramItem item)
    {
        if (!IsContainer)
            throw new InvalidOperationException();

        item.OnRemoveFromSurface(); //注意：必须先于下面代码
        item.Parent = null;
        _items!.Remove(item);
        //todo: 非DiagramHostItem需要刷新Canvas
        Invalidate();
    }

    /// <summary>
    /// 上层调用时本身已Hover，用于确认ContainerItem下的子项是否Hover
    /// </summary>
    /// <param name="p">已转换为当前元素的坐标系</param>
    /// <returns>如果找不到子项，必须返回自己</returns>
    protected internal DiagramItem FindHoverItem(Point p)
    {
        DiagramItem? found = null;
        if (_items != null)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i].Visible && _items[i].Bounds.Contains(p))
                {
                    found = _items[i];
                    if (found.IsContainer)
                    {
                        found = found.FindHoverItem(new Point(p.X - (int)found.Bounds.X, p.Y - (int)found.Bounds.Y));
                    }
                }
            }
        }

        return found ?? this;
    }

    #endregion

    #region ====Mouse Methods====

    protected ISelectionAdorner? SelectionAdorner;

    /// <summary>
    /// 用于选择时获取相应的装饰器
    /// </summary>
    protected internal virtual ISelectionAdorner GetSelectionAdorner(DesignAdorners adorners)
    {
        return SelectionAdorner ??= new SelectionAdorner(adorners, this);
    }

    /// <summary>
    /// Previews the mouse down. 画布坐标系
    /// </summary>
    protected internal virtual bool PreviewMouseDown(PointerEvent e) => false;

    #endregion
}

/// <summary>
/// 用于DesignSurface控制设计时的行为
/// </summary>
[Flags]
public enum DesignBehavior
{
    None = 0,

    /// <summary>
    /// 允许重新设置设计对象的位置
    /// </summary>
    CanMove = 1,

    /// <summary>
    /// 允许改变设计对象的大小
    /// </summary>
    CanResize = 2
}

[Flags]
public enum BoundsSpecified
{
    None = 0x00000000,
    X = 0x00000001,
    Y = 0x00000002,
    Location = 0x00000003,
    Width = 0x00000004,
    Height = 0x00000008,
    Size = 0x0000000c,
    All = 0x0000000f
}