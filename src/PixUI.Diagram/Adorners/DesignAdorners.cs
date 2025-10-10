namespace PixUI.Diagram;

public sealed class DesignAdorners : FlowDecorator<DiagramSurface>
{
    public DesignAdorners(DiagramSurface canvas) : base(canvas, true) { }

    private readonly List<DesignAdorner> _adorners = [];

    internal DesignAdorner? HitTestItem { get; private set; }

    #region ====Methods====

    internal void ClearSelected()
    {
        for (var i = _adorners.Count - 1; i >= 0; i--)
        {
            if (_adorners[i] is ISelectionAdorner)
                _adorners.RemoveAt(i);
        }
    }

    internal void Add(DesignAdorner adorner)
    {
        _adorners.Add(adorner);
    }

    private void MarkDirty() => Repaint();

    #endregion

    #region ====Overrides Methods====

    protected override bool HitTest(float x, float y, HitTestResult result)
    {
        return HitTest(x, y);
    }

    internal bool HitTest(float winX, float winY)
    {
        //TODO: use total transform to map window point
        var winPt = Target.LocalToWindow(0, 0);
        var canvasPt = new Point(winX - winPt.X, winY - winPt.Y);
        if (!Target.LayoutBounds.Contains(canvasPt))
            return false;

        //HitTest各子级，注意：point已转换为DiagramItem的本地坐标系
        DesignAdorner? hitItem = null;
        var hitTestCursor = Cursors.Arrow;
        for (var i = 0; i < _adorners.Count; i++)
        {
            var item = _adorners[i];
            var ptItemClient = Point.Empty;
            ptItemClient.X = (int)item.Target.Bounds.X;
            ptItemClient.Y = (int)item.Target.Bounds.Y;
            if (item.Target.Parent != null)
                ptItemClient = item.Target.Parent.PointToSurface(ptItemClient);
            ptItemClient.X = canvasPt.X - ptItemClient.X;
            ptItemClient.Y = canvasPt.Y - ptItemClient.Y;

            if (item.HitTest(ptItemClient, ref hitTestCursor))
                hitItem = item;
        }

        HitTestItem = hitItem;
        //改变当前Cursor
        Cursor.Current = HitTestItem == null || hitTestCursor == null ? Cursors.Arrow : hitTestCursor;
        return true;
    }

    protected override void PaintCore(Canvas canvas)
    {
        if (_creationModel == CreationMode.None && _adorners.Count == 0)
            return;

        //Clip to canvas bounds
        canvas.Save();
        canvas.ClipRect(Target.LayoutBounds);

        for (var i = 0; i < _adorners.Count; i++)
        {
            var item = _adorners[i];
            var ptCanvas = item.Target.PointToSurface(Point.Empty);

            canvas.Translate(ptCanvas.X, ptCanvas.Y);
            _adorners[i].OnRender(canvas);
            canvas.Translate(-ptCanvas.X, -ptCanvas.Y);
        }

        //画新建框或新建连接线
        if (_creationModel == CreationMode.Item)
        {
            var x1 = Math.Min(_x1, _x2);
            var y1 = Math.Min(_y1, _y2);
            var x2 = Math.Max(_x1, _x2);
            var y2 = Math.Max(_y1, _y2);
            canvas.DrawRectangle(Colors.Black, 1, Rect.FromLTWH(x1, y1, x2 - x1, y2 - y1));
        }
        else if (_creationModel == CreationMode.Connection)
        {
            canvas.DrawLine(Colors.DarkGray, 2f, _x1, _y1, _x2, _y2);
        }

        DrawShapeConnectors(canvas); //突出画需要连接至的附件的IShape

        canvas.Restore();
    }

    private void DrawShapeConnectors(Canvas canvas)
    {
        if (_nearestShapes == null) return;

        for (var i = 0; i < _nearestShapes.Count; i++)
        {
            var shape = _nearestShapes[i];
            //var shapeBounds = shape.Bounds; //todo: 需要转换至画布坐标系
            //graphics.DrawRectangle(Color.DarkGreen, 2, shapeBounds);

            var connectorBounds = RectI.FromLTWH(0, 0, 8, 8);
            for (int j = 0; j < shape.Connectors.Count; j++)
            {
                var connector = shape.Connectors[j];
                connectorBounds.X = (int)connector.AbsolutePosition.X - 4;
                connectorBounds.Y = (int)connector.AbsolutePosition.Y - 4;

                //判断IsActive，不同的绘制方法
                var activeColor = connector == ActiveConnector ? Colors.Red : Colors.White;
                canvas.FillEllipse(activeColor, connectorBounds);
                canvas.DrawEllipse(Colors.Red, 1f, connectorBounds);
            }
        }
    }

    #endregion

    #region ====新建相关====

    private CreationMode _creationModel = CreationMode.None;
    private int _x1;
    private int _y1;
    private int _x2;
    private int _y2;

    internal Point CreationStartPoint
    {
        get { return new Point(_x1, _y1); }
    }

    internal Point CreationEndPoint
    {
        get { return new Point(_x2, _y2); }
    }

    internal RectI CreationRectangle
    {
        get
        {
            var x1 = Math.Min(_x1, _x2);
            var y1 = Math.Min(_y1, _y2);
            var x2 = Math.Max(_x1, _x2);
            var y2 = Math.Max(_y1, _y2);
            return RectI.FromLTWH(x1, y1, x2 - x1, y2 - y1);
        }
    }

    internal void BeginCreation(int x, int y, bool isConnection)
    {
        _creationModel = isConnection ? CreationMode.Connection : CreationMode.Item;
        _x1 = _x2 = x;
        _y1 = _y2 = y;
    }

    internal void UpdateCreationEndPoint(int x, int y)
    {
        _x2 = x;
        _y2 = y;
        MarkDirty();
    }

    internal void EndCreation()
    {
        _creationModel = CreationMode.None;
    }

    private enum CreationMode : byte
    {
        None,
        Connection,
        Item
    }

    #endregion

    #region ====Connector相关====

    private IList<IShape>? _nearestShapes; //需要显示ConnectorsAdorner的IShape列表
    internal IConnector? ActiveConnector;

    internal void OnMouseUp()
    {
        //清空显示
        _nearestShapes = null;
        MarkDirty();

        if (ActiveConnector != null)
        {
            if (HitTestItem is ConnectionSelectionAdorner connectionAdorner)
            {
                var connection = (DiagramConnection)connectionAdorner.Target;
                var editPoint = connectionAdorner.ActiveEditPoint;
                if (editPoint.HasValue)
                {
                    if (editPoint.Value.Type == ManipulationPointType.First)
                        connection.Attach(source: ActiveConnector);
                    else if (editPoint.Value.Type == ManipulationPointType.Last)
                        connection.Attach(target: ActiveConnector);
                }
            }

            ActiveConnector = null;
        }
    }

    internal void UpdateNearestShapes(IList<IShape>? nearest)
    {
        var oldCount = _nearestShapes?.Count ?? 0;
        var newCount = nearest?.Count ?? 0;

        _nearestShapes = nearest;
        if (newCount == 0)
            ActiveConnector = null;

        if (oldCount != newCount)
            MarkDirty();
    }

    #endregion
}