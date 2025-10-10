namespace PixUI.Diagram;

internal sealed class ConnectionSelectionAdorner : DesignAdorner, ISelectionAdorner
{
    private const int POINT_SIZE = 6;
    private IList<EditPoint>? _pointsCache; //TODO:需要监测目标更新，取消当前缓存
    private EditPoint? _hitPoint; //mouse移动时击中的编辑点

    internal EditPoint? ActiveEditPoint => _hitPoint;

    public ConnectionSelectionAdorner(DesignAdorners owner, DiagramConnection target) : base(owner, target) { }

    internal void ResetCache()
    {
        _pointsCache = null;
    }

    private IList<EditPoint> GetEditPoints()
    {
        if (_pointsCache != null)
            return _pointsCache;

        //注意：画布已偏移坐标系
        var connection = (DiagramConnection)Target;
        var offsetX = connection.Position.X;
        var offsetY = connection.Position.Y;

        var list = new List<EditPoint>();
        var startPt = connection.StartPoint;
        var rect = Rect.FromLTWH(startPt.X - offsetX - POINT_SIZE / 2f, startPt.Y - offsetY - POINT_SIZE / 2f,
            POINT_SIZE, POINT_SIZE);
        list.Add(new EditPoint() { Type = ManipulationPointType.First, Rect = rect, Index = 0 });

        //添加中间点
        for (var i = 0; i < connection.ConnectionPoints.Count; i++)
        {
            rect = Rect.FromLTWH(connection.ConnectionPoints[i].X - offsetX - POINT_SIZE / 2f
                , connection.ConnectionPoints[i].Y - offsetY - POINT_SIZE / 2f
                , POINT_SIZE, POINT_SIZE);
            list.Add(new EditPoint() { Type = ManipulationPointType.Intermediate, Rect = rect, Index = list.Count });
        }

        var endPt = connection.EndPoint;
        rect = Rect.FromLTWH(endPt.X - offsetX - POINT_SIZE / 2f, endPt.Y - offsetY - POINT_SIZE / 2f, POINT_SIZE,
            POINT_SIZE);
        list.Add(new EditPoint() { Type = ManipulationPointType.Last, Rect = rect, Index = list.Count });
        _pointsCache = list;
        return _pointsCache;
    }

    protected internal override bool HitTest(Point pt, ref Cursor? cursor)
    {
        var list = GetEditPoints();
        for (var i = 0; i < list.Count; i++)
        {
            if (list[i].Rect.Contains(pt))
            {
                _hitPoint = list[i];
                Log.Debug($"Hit EditPoint {_hitPoint.Value.Index}");
                return true;
            }
        }

        _hitPoint = null;
        return false;
    }

    protected internal override void OnMouseMove(PointerEvent e)
    {
        if (!_hitPoint.HasValue)
            return;

        var connection = (DiagramConnection)Target;
        //如果已连接则暂时取消连接点
        if (_hitPoint.Value.Type == ManipulationPointType.First && connection.Source != null)
            connection.Source = null;
        else if (_hitPoint.Value.Type == ManipulationPointType.Last && connection.Target != null)
            connection.Target = null;

        //通知Connection更新Point
        var pointIndex = _hitPoint.Value.Index;
        connection.UpdateGeometryPoint(pointIndex, e.DeltaX, e.DeltaY); 
        
        if (_hitPoint.Value.Type is ManipulationPointType.First or ManipulationPointType.Last)
        {
            //var activeConnector = hitPoint.Value.Type == ManipulationPointType.First ? 
            //                      connection.TargetConnectorResult : connection.SourceConnectorResult;
            ConnectionUtilities.ActivateNearestConnector(connection.Surface!, connection
                , _hitPoint.Value.Type == ManipulationPointType.First
                , new Point(e.X, e.Y) /*e.TransformedPoint*/);
        }
    }

    protected internal override void OnRender(Canvas canvas)
    {
        var connection = (DiagramConnection)Target;
        if (connection.ConnectionType == ConnectionType.Bezier)
        {
            DrawBezierConnectionHandles(canvas, connection);
        }

        var points = GetEditPoints();
        for (var i = 0; i < points.Count; i++)
        {
            DrawEditPoint(canvas, points[i].Rect);
        }
    }

    private static void DrawEditPoint(Canvas g, Rect rect)
    {
        var paint = Paint.Shared(Colors.White);
        g.DrawRect(rect, paint);
        paint.Color = Colors.Black;
        paint.Style = PaintStyle.Stroke;
        g.DrawRect(rect, paint);
    }

    private static void DrawBezierConnectionHandles(Canvas canvas, DiagramConnection connection)
    {
        var startBezierPoint = connection.StartPoint.Substract(connection.Position);
        var endBezierPoint = connection.EndPoint.Substract(connection.Position);

        var startHandleBezierPoint = connection.ConnectionPoints[0].Substract(connection.Position);
        var endHandleBezierPoint = connection.ConnectionPoints[1].Substract(connection.Position);

        var dotEffect = PathEffect.CreateDash([1, 1], 1);
        var paint = Paint.Shared(Colors.Gray, PaintStyle.Stroke, connection.StrokeThickness);
        paint.AntiAlias = true;
        paint.PathEffect = dotEffect;

        canvas.DrawLine(startHandleBezierPoint, startBezierPoint, paint);

        paint.Color = connection.BackColor;
        canvas.DrawLine(endHandleBezierPoint, endBezierPoint, paint);

        paint.Reset();
    }
}

internal readonly struct EditPoint
{
    public ManipulationPointType Type { get; init; }
    public Rect Rect { get; init; }
    public int Index { get; init; }
}