namespace PixUI.Diagram;

/// <summary>
/// Base class for the Connection Routers.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
internal abstract class TreeRouterBase
{
    /// <summary>
    /// Gets or sets the connection spacing.
    /// </summary>
    protected float ConnectionSpacing { get; set; }

    /// <summary>
    /// Gets or sets the connection.
    /// </summary>
    protected IConnection Connection { get; set; }

    /// <summary>
    /// Gets the source inflated rectangle.
    /// </summary>
    protected Rect SourceInflatedRect { get; private set; }

    /// <summary>
    /// Gets the target inflated rectangle.
    /// </summary>
    protected Rect TargetInflatedRect { get; private set; }

    /// <summary>
    /// Gets the route.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <param name="connectionSpacing">The connection spacing.</param>
    /// <returns></returns>
    public IList<Point> GetRoute(IConnection connection, float connectionSpacing)
    {
        Connection = connection;
        ConnectionSpacing = connectionSpacing;
        SourceInflatedRect = Rect.Inflate(Connection.Source.Bounds, connectionSpacing, connectionSpacing);
        TargetInflatedRect = Rect.Inflate(Connection.Target.Bounds, connectionSpacing, connectionSpacing);

        bool areRectsIntersected = SourceInflatedRect.IntersectsWith(TargetInflatedRect);
        if (areRectsIntersected)
        {
            SetConnectorsWhenShapesOverlap();
            return GetUnionRectangleRoutePoints();
        }
        else
        {
            SetSourceAndTargetConnectors();
        }

        return GetRoutePoints();
    }

    /// <summary>
    /// Sets the source and target connectors.
    /// </summary>
    protected abstract void SetSourceAndTargetConnectors();

    /// <summary>
    /// Gets the route points.
    /// </summary>
    /// <returns></returns>
    protected abstract IList<Point> GetRoutePoints();

    /// <summary>
    /// Gets the router specific rectangle horizontal points.
    /// </summary>
    /// <param name="clockwise">If set to <c>false</c> returns the reversed list.</param>
    /// <param name="rectangle">The rectangle.</param>
    /// <returns></returns>
    protected abstract IList<Point> GetRouterSpecificRectHorizontalPoints(bool clockwise, Rect rectangle);

    /// <summary>
    /// Gets the router specific rectangle vertical points.
    /// </summary>
    /// <param name="clockwise">If set to <c>false</c> returns the reversed list.</param>
    /// <param name="rectangle">The rectangle.</param>
    /// <returns></returns>
    protected abstract IList<Point> GetRouterSpecificRectVerticalPoints(bool clockwise, Rect rectangle);

    /// <summary>
    /// Sets the connectors when shapes overlap.
    /// </summary>
    protected void SetConnectorsWhenShapesOverlap()
    {
        IShape source = Connection.Source;
        IShape target = Connection.Target;
        Rect sourceRect = source.Bounds;
        Rect targetRect = target.Bounds;
        Rect unionRect = Rect.Union(sourceRect, targetRect); // Utils.Union(sourceRect, targetRect);
        Rect unionInflated = Rect.Inflate(unionRect, -1, -1); //unionRect.InflateRect(-1);

        List<IConnector> sourceRectPoints = source.Connectors
            .Where(x => x.Name != ConnectorPosition.Auto &&
                        unionRect.Contains(x.AbsolutePosition) && !unionInflated.Contains(x.AbsolutePosition))
            .ToList();
        List<IConnector> targetRectPoints = target.Connectors
            .Where(x => x.Name != ConnectorPosition.Auto && unionRect.Contains(x.AbsolutePosition) &&
                        !unionInflated.Contains(x.AbsolutePosition)).ToList();
        var shortestTuple = GetShortestDistancedPoints(sourceRectPoints, targetRectPoints);
        if (shortestTuple != null)
        {
            Connection.SourceConnectorPosition = shortestTuple.Item1.Name;
            Connection.TargetConnectorPosition = shortestTuple.Item2.Name;
        }
    }

    /// <summary>
    /// Gets the points between source and target point lying on a given rectangle.
    /// </summary>
    protected IList<Point> GetUnionRectangleRoutePoints()
    {
        Point sourceNear = GetFirstNearPoint(SourceInflatedRect, Connection.SourceConnectorResult);
        Point targetNear = GetFirstNearPoint(TargetInflatedRect, Connection.TargetConnectorResult);
        Rect inflated =
            Rect.Union(Connection.Source.Bounds,
                Connection.Target
                    .Bounds); //Utils.Union(this.Connection.Source.Bounds, this.Connection.Target.Bounds);
        Rect outerInflectedrect =
            Rect.Inflate(inflated, ConnectionSpacing,
                ConnectionSpacing); //inflated.InflateRect(this.ConnectionSpacing);
        return GetUnionRectangleRoutePoints(outerInflectedrect, sourceNear, targetNear);
    }

    /// <summary>
    /// Gets the route points when shapes overlap.
    /// </summary>
    /// <param name="rectangle">The rectangle.</param>
    /// <param name="startPoint">The start point.</param>
    /// <param name="endPoint">The end point.</param>
    /// <returns></returns>
    protected IList<Point> GetUnionRectangleRoutePoints(Rect rectangle, Point startPoint, Point endPoint)
    {
        List<Point> list = new List<Point>();
        Point topLeft = rectangle.TopLeft();
        Point topRight = rectangle.TopRight();
        Point bottomLeft = rectangle.BottomLeft();
        Point bottomRight = rectangle.BottomRight();

        list.Add(startPoint);
        if (rectangle.Left.IsEqual(startPoint.X))
        {
            if (rectangle.Top.IsEqual(endPoint.Y))
                list.Add(topLeft);
            else if (rectangle.Bottom.IsEqual(endPoint.Y))
                list.Add(bottomLeft);
            else if (rectangle.Right.IsEqual(endPoint.X))
                list.AddRange(GetRouterSpecificRectHorizontalPoints(true, rectangle));
        }
        else if (rectangle.Right.IsEqual(startPoint.X))
        {
            if (rectangle.Top.IsEqual(endPoint.Y))
                list.Add(topRight);
            else if (rectangle.Bottom.IsEqual(endPoint.Y))
                list.Add(bottomRight);
            else if (rectangle.Left.IsEqual(endPoint.X))
                list.AddRange(GetRouterSpecificRectHorizontalPoints(false, rectangle));
        }
        else if (rectangle.Top.IsEqual(startPoint.Y))
        {
            if (rectangle.Right.IsEqual(endPoint.X))
                list.Add(topRight);
            else if (rectangle.Left.IsEqual(endPoint.X))
                list.Add(topLeft);
            else if (rectangle.Bottom.IsEqual(endPoint.Y))
                list.AddRange(GetRouterSpecificRectVerticalPoints(true, rectangle));
        }
        else if (rectangle.Bottom.IsEqual(startPoint.Y))
        {
            if (rectangle.Right.IsEqual(endPoint.X))
                list.Add(bottomRight);
            else if (rectangle.Left.IsEqual(endPoint.X))
                list.Add(bottomLeft);
            else if (rectangle.Top.IsEqual(endPoint.Y))
                list.AddRange(GetRouterSpecificRectVerticalPoints(false, rectangle));
        }

        if (AreBothPointsOnASameRectangleSide(rectangle, startPoint, endPoint))
        {
            list.Clear();
            list.Add(startPoint);
        }

        list.Add(endPoint);
        if (startPoint.X.IsEqual(endPoint.X) && startPoint.Y.IsEqual(endPoint.Y))
        {
            list.Clear();
        }

        return list;
    }

    /// <summary>
    /// Gets the first near point from the inflated rectangle (closest ortogonal projection).
    /// </summary>
    /// <param name="inflatedRect">The inflated rectangle.</param>
    /// <param name="connector">The connector.</param>
    /// <returns></returns>
    protected Point GetFirstNearPoint(Rect inflatedRect, IConnector? connector)
    {
        if (connector == null)
            return inflatedRect.TopLeft();

        return connector.Name switch
        {
            ConnectorPosition.Left => inflatedRect.CenterLeft(),
            ConnectorPosition.Right => inflatedRect.CenterRight(),
            ConnectorPosition.Bottom => inflatedRect.CenterBottom(),
            ConnectorPosition.Top => inflatedRect.CenterTop(),
            _ => new Point
                { X = connector.AbsolutePosition.X, Y = connector.AbsolutePosition.Y + ConnectionSpacing }
        };
    }

    /// <summary>
    /// Returns true if the both points are on same rectangle side.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <param name="a">A.</param>
    /// <param name="b">The b.</param>
    /// <returns></returns>
    private static bool AreBothPointsOnASameRectangleSide(Rect rect, Point a, Point b)
    {
        return rect.Left.IsEqual(a.X) && rect.Left.IsEqual(b.X) ||
               rect.Right.IsEqual(a.X) && rect.Right.IsEqual(b.X) ||
               rect.Top.IsEqual(a.Y) && rect.Top.IsEqual(b.Y) ||
               rect.Bottom.IsEqual(a.Y) && rect.Bottom.IsEqual(b.Y);
    }

    /// <summary>
    /// Gets the shortest distanced points, first from source point list, second from target point list.
    /// </summary>
    /// <param name="sourceList">The source list.</param>
    /// <param name="targetList">The target list.</param>
    /// <returns></returns>
    private static Tuple<IConnector, IConnector> GetShortestDistancedPoints(IEnumerable<IConnector> sourceList,
        List<IConnector> targetList)
    {
        // connector.AbsolutePosition is very costly operation.  
        IConnector resultSourceConnector = null;
        IConnector resultTargetConnector = null;
        var minDistance = double.MaxValue;
        foreach (var xItem in sourceList)
        {
            foreach (var yItem in targetList.Where(yItem =>
                         xItem.AbsolutePosition.Distance(yItem.AbsolutePosition) <= minDistance))
            {
                resultSourceConnector = xItem;
                resultTargetConnector = yItem;
                minDistance = xItem.AbsolutePosition.Distance(yItem.AbsolutePosition);
            }
        }

        return resultTargetConnector != null
            ? new Tuple<IConnector, IConnector>(resultSourceConnector, resultTargetConnector)
            : null;
    }
}