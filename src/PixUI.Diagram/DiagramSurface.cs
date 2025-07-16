namespace PixUI.Diagram;

public sealed class DiagramSurface : Widget, IMouseRegion, IFocusable
{
    public DiagramSurface()
    {
        Adorners = new DesignAdorners(this);
        ToolboxService = new ToolboxService(this);
        SelectionService = new SelectionService(this);
        RoutingService = new RoutingService(this);

        MouseRegion = new MouseRegion();
        MouseRegion.PointerMove += OnMouseMove;
        MouseRegion.PointerDown += OnMouseDown;
        MouseRegion.PointerUp += OnMouseUp;

        FocusNode = new FocusNode();
        FocusNode.KeyDown += OnKeyDown;
    }

    #region ====Fields & Properties====

    internal DesignAdorners Adorners { get; }

    private Overlay? _cachedOverlay;
    private DiagramItem? _hoverItem; //当前mouse下的元素
    private readonly List<DiagramItem> _items = new();

    public DiagramItem[] Items => _items.ToArray();

    public MouseRegion MouseRegion { get; }
    public FocusNode FocusNode { get; }

    #endregion

    #region ====Service about properties====

    public ToolboxService ToolboxService { get; }

    public SelectionService SelectionService { get; }

    public RoutingService? RoutingService { get; }

    #endregion

    #region ====Connection about properties====

    /// <summary>
    /// Gets or sets whether the corners of the (polyline) connection are rounded.
    /// </summary>
    /// <remarks>This property has only an effect when the <see cref="IConnection.ConnectionType"/> is set to <see cref="ConnectionType.Polyline"/>.</remarks>
    public bool ConnectionRoundedCorners { get; set; }

    /// <summary>
    /// Gets the connection bridge type.
    /// </summary>
    public BridgeType ConnectionBridge { get; set; }

    #endregion

    #region ====Add & Remove Item Methods====

    public void AddItem(DiagramItem item)
    {
        item.Surface = this;
        _items.Add(item);
        item.OnAddToSurface();
    }

    public void RemoveItem(DiagramItem item)
    {
        item.OnRemoveFromSurface(); //注意：必须先于下面代码
        item.Surface = null;
        _items.Remove(item);
    }

    #endregion

    #region ====Mouse Event Handlers====

    private void OnMouseMove(PointerEvent e)
    {
        //1.处理Mouse拖动
        if (e.Buttons == PointerButtons.Left)
        {
            if (Adorners.HitTestItem != null) //先处理装饰层的拖动
            {
                Adorners.HitTestItem.OnMouseMove(e);
            }
            else if (ToolboxService.SelectedItem != null) //已从工具箱选择了新建的对象，并在拖动过程中
            {
                ToolboxService.OnMouseMove((int)e.X, (int)e.Y);
            }
            else //处理已选择的对象的移动
            {
                SelectionService.MoveSelection((int)e.DeltaX, (int)e.DeltaY);
            }
        }

        //2.处理Mouse下的HoverItem
        FindHoverItemOnMouseMove(e);
    }

    private void OnMouseDown(PointerEvent e)
    {
        if (_hoverItem != null && _hoverItem.PreviewMouseDown(e))
        {
            return;
        }

        //先判断有没有击中已选择项的锚点
        if (Adorners.HitTestItem != null)
        {
            //TODO: HitTestItem.OnMouseDown
            return;
        }

        //判断有没有选择工具箱项，有则表示在新建模式
        if (e.Buttons == PointerButtons.Left && ToolboxService.SelectedItem != null)
        {
            ToolboxService.BeginCreation((int)e.X, (int)e.Y);
            return;
        }

        //设置选择的Item
        SelectionService.SelectItem(_hoverItem);

        //TODO:如果选择项为Container，则开始设置选择框的起始位置
    }

    private void OnMouseUp(PointerEvent e)
    {
        if (e.Buttons == PointerButtons.Left)
        {
            if (ToolboxService.SelectedItem != null) //新建模式下结束
                ToolboxService.EndCreation((int)e.X, (int)e.Y);

            //通知adorners
            Adorners.OnMouseUp();

            //TODO:清空选择框
        }
    }

    /// <summary>
    /// MouseMove时查找其下的设计对象
    /// </summary>
    private void FindHoverItemOnMouseMove(PointerEvent e)
    {
        if (_hoverItem != null)
        {
            var ptCanvas = _hoverItem.PointToSurface(Point.Empty);
            var rectCanvas = Rect.FromLTWH(ptCanvas.X, ptCanvas.Y, (int)_hoverItem.Bounds.Width,
                (int)_hoverItem.Bounds.Height);
            if (!rectCanvas.Contains(new Point(e.X, e.Y))) //已离开该区域 //todo:改判断为HitTest()，因为Connection不能判断边框
            {
                //todo: 
            }
            else
            {
                //如果是容器类的元素，尝试在hoverItem内部查找
                if (_hoverItem.IsContainer)
                {
                    var clientPt = new Point(e.X - ptCanvas.X, e.Y - ptCanvas.Y);
                    _hoverItem = _hoverItem.FindHoverItem(clientPt);
                }

                return;
            }
        }

        //重新开始查找hoverItem
        _hoverItem = GetItemUnderMouse((int)e.X, (int)e.Y);
        //if (hoverItem != null)
        //    Cursor.Current = Cursors.SizeAll;
        //else
        //    Cursor.Current = Cursors.Default;
    }

    private DiagramItem? GetItemUnderMouse(int x, int y)
    {
        DiagramItem? found = null;
        for (var i = 0; i < _items.Count; i++)
        {
            if (_items[i].Visible && _items[i].Bounds.Contains(new Point(x, y))) //todo:改用HitTest判断
            {
                found = _items[i];
                if (found.IsContainer)
                {
                    found = found.FindHoverItem(new Point(x - (int)found.Bounds.X, y - (int)found.Bounds.Y));
                }
            }
        }

        return found;
    }

    internal DiagramItem? GetContainerUnderMouse(int x, int y)
    {
        var temp = GetItemUnderMouse(x, y);
        while (temp != null)
        {
            if (temp.IsContainer)
                return temp;
            temp = temp.Parent;
        }

        return null;
    }

    #endregion

    #region ====Keyboard Event Handlers====

    internal void ResetHoverItem()
    {
        _hoverItem = null;
        //TODO:最好重新查找HoverItem
    }

    private void OnKeyDown(KeyEvent e)
    {
        switch (e.KeyCode)
        {
            case Keys.Delete:
                SelectionService.DeleteSelection();
                break;
            case Keys.Up:
                SelectionService.MoveSelection(0, -1);
                break;
            case Keys.Down:
                SelectionService.MoveSelection(0, 1);
                break;
            case Keys.Left:
                SelectionService.MoveSelection(-1, 0);
                break;
            case Keys.Right:
                SelectionService.MoveSelection(1, 0);
                break;
        }
    }

    #endregion

    #region ====Scroll====

    private Point _scrollPosition;

    internal Point ScrollPosition => _scrollPosition;

    internal void ScrollSurface(int offsetX, int offsetY)
    {
        if (offsetX == 0 && offsetY == 0)
            return;

        //this.SuspendLayout();
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].Location = new Point((int)_items[i].Location.X - offsetX, (int)_items[i].Location.Y - offsetY);
        }

        _scrollPosition.X += offsetX;
        _scrollPosition.Y += offsetY;

        //this.ResumeLayout(false);
    }

    #endregion

    #region ====Shape & Connection Methods====

    internal IList<IShape> GetShapes()
    {
        var list = new List<IShape>();
        for (int i = 0; i < _items.Count; i++)
        {
            LoopGetType(list, _items[i]);
        }

        return list;
    }

    internal IList<IConnection> GetConnections()
    {
        var list = new List<IConnection>();
        for (var i = 0; i < _items.Count; i++)
        {
            LoopGetType(list, _items[i]);
        }

        return list;
    }

    private static void LoopGetType<T>(IList<T> list, DiagramItem item) where T : class
    {
        if (item is T target)
            list.Add(target);

        if (item is { IsContainer: true, Items: not null })
        {
            foreach (var it in item.Items)
                LoopGetType(list, it);
        }
    }

    /// <summary>
    /// Gets the incoming connections for shape.
    /// </summary>
    public IEnumerable<IConnection> GetIncomingConnectionsForShape(IShape shape) =>
        GetConnections().Where(x => x.Target == shape);

    /// <summary>
    /// Gets the outgoing connections for shape.
    /// </summary>
    public IEnumerable<IConnection> GetOutgoingConnectionsForShape(IShape shape) =>
        GetConnections().Where(x => x.Source == shape);

    #endregion

    #region ====Overrides Methods====

    protected override void OnMounted()
    {
        base.OnMounted();
        _cachedOverlay = Overlay;
        _cachedOverlay?.Show(Adorners);
    }

    protected override void OnUnmounted()
    {
        _cachedOverlay?.Remove(Adorners);
        _cachedOverlay = null;
        base.OnUnmounted();
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //TODO:缩放坐标系

        //画各个子元素
        var clipRect = canvas.ClipBounds;
        for (int i = 0; i < _items.Count; i++)
        {
            if (!clipRect.IntersectsWith(Rect.Ceiling(_items[i].Bounds))) continue;

            var offsetX = _items[i].Bounds.X;
            var offsetY = _items[i].Bounds.Y;

            canvas.Translate(offsetX, offsetY);
            _items[i].Paint(canvas);
            canvas.Translate(-offsetX, -offsetY);
        }
    }

    #endregion
}