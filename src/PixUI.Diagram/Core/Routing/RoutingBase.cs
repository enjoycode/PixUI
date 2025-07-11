namespace PixUI.Diagram;

internal abstract class RoutingBase : IRouter
{
    private readonly DiagramSurface _diagram;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoutingBase"/> class.
    /// </summary>
    /// <param name="graph">The graph.</param>
    protected RoutingBase(DiagramSurface graph)
    {
        _diagram = graph;
    }

    /// <summary>
    /// Gets the route points.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <param name="showLastLine">If set to <c>true</c> [show last line].</param>
    /// <returns></returns>
    public abstract IList<Point> GetRoutePoints(IConnection connection, bool showLastLine = true);

    /// <summary>
    /// Given a point on the edge of a shape, this returns the middle point of the edge adjacent to it in the clockwise direction.
    /// </summary>
    /// <param name="point">The point on the edge of the shape.</param>
    /// <param name="shape">The shape holding the point.</param>
    /// <remarks>This method is called when the routing algorithm is unable to find a route from the given starting point and an alternative starting point 
    /// at the edge of the shape is necessary.
    /// </remarks>
    protected static Point ClockwiseMoveAroundEdgeOfShape(Point point, IDiagramItem shape)
    {
        var bounds = shape.Bounds;
        if (Math.Abs(point.Y - bounds.Top) < Utils.Epsilon)
            return new Point(bounds.Right, bounds.Top + (bounds.Height / 2));
        if (Math.Abs(point.X - bounds.Right) < Utils.Epsilon)
            return new Point(bounds.Left + (bounds.Width / 2), bounds.Bottom);
        if (Math.Abs(point.Y - bounds.Bottom) < Utils.Epsilon)
            return new Point(bounds.Left, bounds.Top + (bounds.Height / 2));
        if (Math.Abs(point.X - bounds.Left) < Utils.Epsilon)
            return new Point(bounds.Left + (bounds.Width / 2), bounds.Top);
        throw new ArgumentException("NotOnShapeEdge point");
    }

    /// <summary>
    /// Finds the crossings.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <param name="source">The source.</param>
    /// <param name="target">The target.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    protected static void FindCrossings(IConnection connection, ref Point source, ref Point target)
    {
        connection.Source.Bounds.IntersectsLineSegment(connection.Source.Bounds.Center(),
            connection.Target.Bounds.Center(), ref source);
        connection.Target.Bounds.IntersectsLineSegment(connection.Target.Bounds.Center(),
            connection.Source.Bounds.Center(), ref target);
    }

    /// <summary>
    /// Given the connection, the most probable start and end points are returned
    /// together with some additional points if necessary.
    /// </summary>
    /// <param name="connection">The connection which is being routed.</param>
    /// <returns>
    /// A tuple containing 
    /// <list type="bullet">
    /// <item>
    /// <description>the source point; this is the start point given by the
    /// connection</description></item>
    /// <item>
    /// <description>the intermediate start point: the point which is not necessarily on
    /// the grid but which forms the corner between the endpoint of the connection and
    /// the nearest grid point. It's defined by the place where the endpoint is located
    /// on the shape. If the endpoint is free (because the connection is free at that
    /// point) the intermediate endpoint is <c>NaN</c>.</description></item>
    /// <item>
    /// <description>the actual start point for the routing
    /// procedure</description></item>
    /// <item>
    /// <description>the end point of the routing procedure</description></item>
    /// <item>
    /// <description>the intermediate end point: analog as the intermediate start
    /// point.</description></item>
    /// <item>
    /// <description>the target point; this is the end or target point given by the
    /// connection.</description></item></list>
    /// </returns>
    protected static Tuple<Point, Point, Point, Point, Point, Point> FindMostProbableTuple(IConnection? connection)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection));

        var intermediateEndPoint = new Point(float.NaN, float.NaN);
        var intermediateStartPoint = new Point(float.NaN, float.NaN);

        var sourcePoint = connection.StartPoint;
        var targetPoint = connection.EndPoint;

        Point startPoint;
        if (connection.Source == null) startPoint = GetNearestGridPoint(sourcePoint);
        else
            startPoint = GetNearestGridPoint(
                sourcePoint, connection.SourceConnectorResult.Name, connection.Source.Bounds.Center(),
                ref intermediateStartPoint);
        Point endPoint;
        if (connection.Target == null) endPoint = GetNearestGridPoint(targetPoint);
        else
            endPoint = GetNearestGridPoint(targetPoint, connection.TargetConnectorResult.Name,
                connection.Target.Bounds.Center(), ref intermediateEndPoint);
        return Tuple.Create(sourcePoint, intermediateStartPoint, startPoint, endPoint, intermediateEndPoint,
            targetPoint);
    }

    /// <summary>
    /// Gets the sector.
    /// </summary>
    protected static Sector GetSector(Point point, Rect bounds)
    {
        if (bounds.Contains(point)) return Sector.Center;
        if (point.X <= bounds.Left)
        {
            if (point.Y <= bounds.Top) return Sector.TopLeft;
            return point.Y <= bounds.Bottom ? Sector.Left : Sector.BottomLeft;
        }

        if (point.X <= bounds.Right)
        {
            if (point.Y <= bounds.Top) return Sector.Top;
            return point.Y <= bounds.Bottom ? Sector.Center : Sector.Bottom;
        }
        else
        {
            if (point.Y <= bounds.Top) return Sector.TopRight;
            return point.Y <= bounds.Bottom ? Sector.Right : Sector.BottomRight;
        }
    }

    /// <summary>
    /// Returns whether the given point is in the neighborhood of a shape.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns></returns>
    protected bool PointIsInNeighborhoodOfShape(Point point) => _diagram.GetShapes().Any(shape =>
    {
        var bounds = shape.Bounds;
        bounds.Inflate(5f, 5f);
        return bounds.Contains(point);
    });

    /// <summary>
    /// Returns whether the given point resides in one of the shapes in the diagram.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns></returns>
    protected bool PointIsInShape(Point point)
    {
        return _diagram.GetShapes().Any(shape => shape.Bounds.Contains(point));
    }

    /// <summary>
    /// Gets the nearest point on the routing grid for the given point.
    /// </summary>
    /// <remarks>The point falls either inside a square of the grid or sits on a grid point. In both case, calculating the four distances to the four corners of a grid wherein the point sits and taking the minimum
    /// distance will result in the nearest grid point.</remarks>
    /// <param name="p">The point for which the nearest grid point has to be returned.</param>
    /// <returns>The nearest point on the grid.</returns>
    private static Point GetNearestGridPoint(Point p)
    {
        var list = new[]
        {
            new Point(
                (float)Math.Floor(p.X / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize,
                (float)Math.Floor(p.Y / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize),
            new Point(
                (float)Math.Ceiling(p.X / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize,
                (float)Math.Floor(p.Y / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize),
            new Point(
                (float)Math.Floor(p.X / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize,
                (float)Math.Ceiling(p.Y / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize),
            new Point(
                (float)Math.Ceiling(p.X / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize,
                (float)Math.Ceiling(p.Y / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize)
        };
        return list.OrderByDescending(q => p.Distance(q)).First();
    }

    /// <summary>
    /// Gets the nearest grid point on the routing grid.
    /// </summary>
    /// <param name="p">The point for which the nearest grid point has to be found.</param>
    /// <param name="connectorName">The name of the connector.</param>
    /// <param name="center">The center of the shape.</param>
    /// <param name="intermediatePoint">The intermediate point.</param>
    /// <returns></returns>
    private static Point GetNearestGridPoint(Point p, string connectorName, Point center,
        ref Point intermediatePoint)
    {
        float x, y;
        const float Margin = 10f;
        switch (connectorName)
        {
            case ConnectorPosition.Auto:
                x = (float)(Math.Floor(p.X / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize);
                y = (float)(Math.Floor(p.Y / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize);
                break;
            case ConnectorPosition.Left:
                x = (float)(Math.Floor(p.X / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize);
                if (Math.Abs(x - p.X) <= Margin) x -= DiagramConstants.RoutingGridSize;
                y = (float)(Math.Floor(p.Y / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize);
                intermediatePoint = new Point(x, p.Y);
                break;
            case ConnectorPosition.Top:
                x = (float)(Math.Floor(p.X / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize);
                y = (float)(Math.Floor(p.Y / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize);
                if (Math.Abs(y - p.Y) <= Margin) y -= DiagramConstants.RoutingGridSize;
                intermediatePoint = new Point(p.X, y);
                break;
            case ConnectorPosition.Right:
                x = (float)(Math.Ceiling(p.X / DiagramConstants.RoutingGridSize) *
                            DiagramConstants.RoutingGridSize);
                if (Math.Abs(x - p.X) <= Margin) x += DiagramConstants.RoutingGridSize;
                y = (float)(Math.Floor(p.Y / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize);
                intermediatePoint = new Point(x, p.Y);
                break;
            case ConnectorPosition.Bottom:
                x = (float)(Math.Floor(p.X / DiagramConstants.RoutingGridSize) * DiagramConstants.RoutingGridSize);
                y = (float)(Math.Ceiling(p.Y / DiagramConstants.RoutingGridSize) *
                            DiagramConstants.RoutingGridSize);
                if (Math.Abs(y - p.Y) <= Margin) y += DiagramConstants.RoutingGridSize;
                intermediatePoint = new Point(p.X, y);
                break;

            // note that this will handle the custom and gliding connectors as well
            default:
                var vec = new Point(p.X - center.X, p.Y - center.Y);

                // reuse the above definitions
                return GetNearestGridPoint(p, GetSector(vec), center, ref intermediatePoint);
        }

        return new Point(x, y);
    }

    /// <summary>
    /// Gets the sector in which the given point/vector sits.
    /// </summary>
    /// <remarks>The splitting is not in the usual quadrants but rather the sector defined by the diagonals x=y and x=-y.</remarks>
    /// <param name="p">Any given point or vector.</param>
    /// <returns></returns>
    private static string? GetSector(Point p)
    {
        if (Math.Abs(p.Y) <= p.X) return ConnectorPosition.Right;
        if (Math.Abs(p.Y) <= -p.X) return ConnectorPosition.Left;
        if (Math.Abs(p.X) <= p.Y) return ConnectorPosition.Bottom;
        if (Math.Abs(p.X) <= -p.Y) return ConnectorPosition.Top;
        return null;
    }
}