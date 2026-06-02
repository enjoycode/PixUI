using System.Diagnostics.CodeAnalysis;

namespace PixUI.Diagram;

/// <summary>
/// Implements A* algorithm for finding the cheapest path between two points.
/// </summary>
public class AStarRouter : IExtendedRouter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AStarRouter"/> class.
    /// </summary>
    public AStarRouter(DiagramSurface surface)
    {
        AvoidShapes = true;
        _surface = surface;
        _cameFrom = new Dictionary<AStarNode, AStarNode>();
        _orderedOpenSet = new PriorityQueue<AStarNode, double>(OrderType.Ascending);
    }

    private readonly DiagramSurface _surface;

    //// The set  of linked vectors. The value for a given node (key) is the previous node from path.
    private readonly Dictionary<AStarNode, AStarNode> _cameFrom;

    //// The set of available nodes ordered by increasing path total cost.
    private readonly PriorityQueue<AStarNode, double> _orderedOpenSet;

    //// Used to generate nodes when searching a path between to points.
    private NodeGenerator _stepGenerator = null!;


    /// <summary>
    /// Gets or sets the wall optimization.
    /// </summary>
    /// <value>The wall optimization.</value>
    public bool WallOptimization { get; set; }

    /// <summary>
    /// Gets or sets the avoid shapes property. This property determines if the routing will go around shapes or go through them.
    /// </summary>
    /// <value>The avoid shapes.</value>
    public bool AvoidShapes { get; set; }

    /// <inheritdoc />
    public ConnectionRoute GetRoutePoints(IConnection connection)
    {
        if (connection == null!) return new ConnectionRoute();

        ShapeUtilities.GetNearestConnectors(connection, out var startConnector, out var endConnector);
        var startPoint = startConnector?.AbsolutePosition ?? connection.StartPoint;
        var endPoint = endConnector?.AbsolutePosition ?? connection.EndPoint;

        return new ConnectionRoute(CreatePath(connection, startPoint, endPoint), startConnector, endConnector);
    }

    public IList<Point> GetRoutePoints(IConnection connection, bool showLastLine)
    {
        if (connection == null!) return new List<Point>();

        return CreatePath(connection, connection.StartPoint, connection.EndPoint);
    }


    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    internal static bool Contains(Rect rect, Point point)
    {
        // We use this Contains not the Rect's extension, because for us the border points are not inside the bounds.
        // This way the connection can "step" on them without a wall penalty.
        return point.X != rect.Left && point.X != rect.Right &&
               point.Y != rect.Top && point.Y != rect.Bottom &&
               rect.Contains(point);
    }

    /// <summary>
    /// Calculates the sibling nodes.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="endWall">The end wall.</param>
    /// <param name="endPosition">The end position.</param>
    /// <param name="preferredOrientation">The preferred direction.</param>
    /// <param name="gridSize">Size of the grid.</param>
    /// <returns></returns>
    protected virtual IEnumerable<AStarNode> GetSiblingNodes(AStarNode node, Rect endWall, Point endPosition,
        Orientation preferredOrientation, double gridSize)
    {
        var x = node.Position.X;
        var y = node.Position.Y;

        var neighbors = new List<AStarNode>();
        if (preferredOrientation == Orientation.Horizontal)
        {
            if (WallOptimization)
            {
                var horizontalMargin = Math.Abs(x - endPosition.X);
                if (horizontalMargin < gridSize && horizontalMargin > Utils.Epsilon)
                {
                    // The end point's X is our first horizontal choice.
                    AddNonWall(neighbors, new Point(endPosition.X, y));
                }

                if (x > endWall.Right && x - gridSize <= endWall.Right)
                {
                    // Check if we could reach the right wall with right to left movement(←).
                    // Note: the movement direction is important because we don't want to reach the right wall if we're coming from left(→)/ from inside the shape.
                    AddNonWall(neighbors, new Point(endWall.Right, y));
                }

                if (x < endWall.Left && x + gridSize >= endWall.Left)
                {
                    // Check if we could reach the Left wall with left to right movement(→).
                    AddNonWall(neighbors, new Point(endWall.Left, y));
                }

                if (neighbors.Count > 0)
                    return neighbors;
            }

            // If wall optimization is not turner on, or there's no suitable wall we add four new nodes - one for each direction.
            neighbors.Add(_stepGenerator.GenerateNode(new Point((float)(x - gridSize), y)));
            neighbors.Add(_stepGenerator.GenerateNode(new Point((float)(x + gridSize), y)));

            neighbors.Add(_stepGenerator.GenerateNode(new Point(x, (float)(y - gridSize))));
            neighbors.Add(_stepGenerator.GenerateNode(new Point(x, (float)(y + gridSize))));
        }
        else
        {
            if (WallOptimization)
            {
                var verticalMargin = Math.Abs(y - endPosition.Y);
                if (verticalMargin < gridSize && verticalMargin > Utils.Epsilon)
                {
                    // The endPoint's Y is our first vertical choice.
                    AddNonWall(neighbors, new Point(x, endPosition.Y));
                }

                if (y > endWall.Bottom && y - gridSize <= endWall.Bottom)
                {
                    // Check if we could reach the bottom wall with bottom to top movement(↑).
                    AddNonWall(neighbors, new Point(x, endWall.Bottom));
                }

                if (y < endWall.Top && y + gridSize >= endWall.Top)
                {
                    // Check if we could reach the top wall with top to bottom movement(↓).
                    AddNonWall(neighbors, new Point(x, endWall.Top));
                }

                if (neighbors.Count > 0)
                    return neighbors;
            }

            // If wall optimization is not turner on, or there's no suitable wall we add four new nodes - one for each direction.
            neighbors.Add(_stepGenerator.GenerateNode(new Point(x, (float)(y - gridSize))));
            neighbors.Add(_stepGenerator.GenerateNode(new Point(x, (float)(y + gridSize))));

            neighbors.Add(_stepGenerator.GenerateNode(new Point((float)(x - gridSize), y)));
            neighbors.Add(_stepGenerator.GenerateNode(new Point((float)(x + gridSize), y)));
        }

        return neighbors;
    }

    /// <summary>
    /// Calculates the bend alteration.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="previousNode">The previous node.</param>
    /// <param name="endWall">The end wall.</param>
    /// <param name="endPoint">The end point.</param>
    /// <param name="currentOrientation">The current direction.</param>
    /// <param name="penaltyBaseValue">The penalty base value.</param>
    /// <returns></returns>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    protected virtual float CalculateBendAlteration(AStarNode node, AStarNode previousNode, Rect endWall,
        Point endPoint, Orientation currentOrientation, float penaltyBaseValue)
    {
        // A small bonus for keeping the current direction.
        var bendPenalty = -penaltyBaseValue / 10;
        Point tmoPoint = new Point();
        if (currentOrientation == Orientation.Vertical)
        {
            if (node.Position.X != previousNode.Position.X)
            {
                if (WallOptimization && node.Position.Y == endPoint.Y &&
                    !endWall.IntersectsLineSegment(node.Position, endPoint, ref tmoPoint))
                {
                    // A bonus for reaching the endPoint's Y (this a desired bend, that's why there's a bonus).
                    bendPenalty = -penaltyBaseValue;
                }
                else
                {
                    // A bend penalty.
                    bendPenalty = penaltyBaseValue;
                }
            }
        }
        else
        {
            if (node.Position.Y != previousNode.Position.Y)
            {
                if (WallOptimization && node.Position.X == endPoint.X &&
                    !endWall.IntersectsLineSegment(node.Position, endPoint, ref tmoPoint))
                {
                    // A bonus for reaching the endPoint's X (this a desired bend, that's why there's a bonus).
                    bendPenalty = -penaltyBaseValue;
                }
                else
                {
                    // A bend penalty.
                    bendPenalty = penaltyBaseValue;
                }
            }
        }

        return bendPenalty;
    }

    /// <summary>
    /// Calculates the wall penalty.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="penaltyBaseValue">The penalty base value.</param>
    /// <returns></returns>
    protected virtual float CalculateWallPenalty(AStarNode node, float penaltyBaseValue)
    {
        if (node.IsStartOrEndWall) return penaltyBaseValue * 3;
        return AvoidShapes ? (node.IsWall ? penaltyBaseValue : 0f) : 0f;
    }

    private static Point ProjectPoint(Point point, Rect bounds)
    {
        var leftProjection = new Tuple<float, Direction>(point.X - bounds.Left, Direction.Left);
        var rightProjections = new Tuple<float, Direction>(bounds.Right - point.X, Direction.Right);
        var topProjection = new Tuple<float, Direction>(point.Y - bounds.Top, Direction.Up);
        var bottomProjection = new Tuple<float, Direction>(bounds.Bottom - point.Y, Direction.Down);

        var possibleProjections = new List<Tuple<float, Direction>>()
            { leftProjection, rightProjections, topProjection, bottomProjection };
        var shortestProjection = possibleProjections.OrderBy(t => Math.Abs(t.Item1)).FirstOrDefault();

        Point projectedPoint = new Point();
        if (shortestProjection != null)
        {
            projectedPoint = shortestProjection.Item2 switch
            {
                Direction.Left => new Point(bounds.Left, point.Y),
                Direction.Right => new Point(bounds.Right, point.Y),
                Direction.Up => new Point(point.X, bounds.Top),
                Direction.Down => new Point(point.X, bounds.Bottom),
                _ => projectedPoint
            };
        }

        return projectedPoint.IsNanOrInfinity() ? point : projectedPoint;
    }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    private static Orientation CalculatePathSegmentDirection(AStarNode node, Orientation startDirection,
        Dictionary<AStarNode, AStarNode> cameFrom)
    {
        // If we can't calculate new direction we use the start one.
        var result = startDirection;
        if (cameFrom.TryGetValue(node, out var previousNode))
        {
            result = node.Position.X == previousNode.Position.X ? Orientation.Vertical : Orientation.Horizontal;
        }

        return result;
    }

    /// <summary>
    /// Calculates whether to cut the algorithm if no path is found in reasonable time or with reasonable distance.
    /// Using the heuristic function to determine if the searched path is getting AWAY from the target (happens when algorithm keeps missing the target).
    /// Not using total path cost, because there is no obvious relation between total path cost and start-end distance (getting through walls and bends).
    /// Adding a base value for short paths (start-end distance is small).
    /// </summary>
    private static bool IsPathDivergent(AStarNode node, AStarNode startNode, float gridSize)
    {
        return node.H > (startNode.H * 10) + (gridSize * 10);
    }

    private static float Heuristic(AStarNode source, AStarNode target)
    {
        //// The Manhattan distance give a closer approximation to the real path cost comapred to Euclidean distance.
        return Math.Abs(source.Position.X - target.Position.X) + Math.Abs(source.Position.Y - target.Position.Y);
    }

    private static bool IsInSquare(Point checkPoint, Point center, float halfSide)
    {
        return checkPoint.X.IsInClosedInterval(center.X - halfSide, center.X + halfSide) &&
               checkPoint.Y.IsInClosedInterval(center.Y - halfSide, center.Y + halfSide);
    }

    private static IList<AStarNode> ReconstructPath(IDictionary<AStarNode, AStarNode> cameFrom, AStarNode currentNode)
    {
        var result = new List<AStarNode>();
        ReconstructPathRecursive(cameFrom, currentNode, result);
        return result;
    }

    private static void ReconstructPathRecursive(IDictionary<AStarNode, AStarNode> cameFrom, AStarNode currentNode,
        List<AStarNode> result)
    {
        if (cameFrom.TryGetValue(currentNode, out var previousNode))
        {
            ReconstructPathRecursive(cameFrom, previousNode, result);
        }

        result.Add(currentNode);
    }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    private static void RemoveUnnecessaryPoints(IList<Point> path, Point startPoint, Point endPoint)
    {
        // By default the start and end points are not included in the path, but we need them
        // because there might be inner points that are on the same line as them.
        path.Insert(0, startPoint);
        path.Add(endPoint);
        for (int i = 0; i < path.Count - 2; i++)
        {
            // Search for three points in a line(row or column).
            if ((path[i].X == path[i + 1].X && path[i].X == path[i + 2].X) ||
                (path[i].Y == path[i + 1].Y && path[i].Y == path[i + 2].Y))
            {
                // We remove the middle point because we don't need it.
                path.RemoveAt(i + 1);
                i--;
            }
        }

        // We remove them because the RadDiagram needs only the intermediate points not the Start/End.
        path.Remove(startPoint);
        path.Remove(endPoint);
    }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    private IList<Point> CreatePath(IConnection connection, Point startPoint, Point endPoint)
    {
        var gridSize = DiagramConstants.RoutingGridSize;

        var startBounds = connection.Source?.ActualBounds ?? Rect.Empty;
        var endBounds = connection.Target?.ActualBounds ?? Rect.Empty;
        var inflatedStartBounds = Rect.Inflate(startBounds, DiagramConstants.RouterInflationValue);
        var inflatedEndBounds = Rect.Inflate(endBounds, DiagramConstants.RouterInflationValue);

        var inflatedStartPoint = ProjectPoint(startPoint, inflatedStartBounds);
        var inflatedEndPoint = ProjectPoint(endPoint, inflatedEndBounds);

        var walls = new List<Rect>();
        if (_surface != null!)
        {
            walls = _surface.GetShapes().Select((shape) =>
            {
                var rect = Rect.Inflate(shape.ActualBounds, DiagramConstants.RouterInflationValue);
                return !Contains(rect, inflatedEndPoint) ? rect : Rect.Empty;
            }).ToList();
        }

        _stepGenerator = new NodeGenerator(walls, inflatedStartBounds, inflatedEndBounds);

        var startOrientation = startPoint.X == inflatedStartPoint.X ? Orientation.Vertical : Orientation.Horizontal;
        IList<Point> path = Route(startOrientation, inflatedStartPoint, inflatedEndPoint, inflatedEndBounds,
            gridSize);

        if (path.Count == 0)
        {
            path.Add(startPoint);
            path.Add(endPoint);
        }
        else
        {
            //// Connects the last point found by the routing and the true end point.
            var lastPoint = path.Last();
            if (lastPoint != inflatedEndPoint)
            {
                var diagonalSibling1 = new Point(lastPoint.X, inflatedEndPoint.Y);
                var diagonalSibling2 = new Point(inflatedEndPoint.X, lastPoint.Y);

                if (endPoint.X != inflatedEndPoint.X)
                {
                    if (!inflatedEndBounds.Contains(diagonalSibling1))
                    {
                        path.Add(diagonalSibling1);
                    }
                    else
                    {
                        path.Add(diagonalSibling2);
                    }
                }
                else
                {
                    if (!inflatedEndBounds.Contains(diagonalSibling2))
                    {
                        path.Add(diagonalSibling2);
                    }
                    else
                    {
                        path.Add(diagonalSibling1);
                    }
                }

                path.Add(inflatedEndPoint);
            }
        }

        RemoveUnnecessaryPoints(path, startPoint, endPoint);

        return path;
    }

    /// <summary>
    /// Calculate a path between start point and end point. The returned path is from start point to a proximity of the end point (i.e. the found path may not reach the end point).
    /// The algorithm tries to find the cheapest path (sometimes shortest path) using a cost function.
    /// Cost function (F) has two components and is based on several factors:
    /// 1)distance between nodes; 2) whether the node is obstacle (wall); 3)whether the path is bending (turning).
    /// If no path is found a path of two points (start-end) is returned.
    /// </summary>
    private IList<Point> Route(Orientation startOrientation, Point startPoint, Point endPoint,
        Rect endWall, float gridSize)
    {
        var failurePath = new List<Point>() { startPoint, endPoint };

        _cameFrom.Clear();
        _orderedOpenSet.Clear();

        var startNode = _stepGenerator.GenerateNode(startPoint);
        var endNode = _stepGenerator.GenerateNode(endPoint);

        startNode.G = 0;
        startNode.H = Heuristic(startNode, endNode);
        startNode.F = startNode.G + startNode.H;
        _orderedOpenSet.Push(startNode, startNode.F);

        //// If start and end are close to each other.
        if (IsInSquare(startPoint, endPoint, gridSize))
        {
            return failurePath;
        }

        while (true)
        {
            var node = _orderedOpenSet.Pop();
            node.IsOpen = false;

            if (IsPathDivergent(node, startNode, gridSize))
            {
                return failurePath;
            }

            if (IsInSquare(node.Position, endNode.Position, gridSize))
            {
                var resultPath = ReconstructPath(_cameFrom, node);
                return resultPath.Select(n => n.Position).ToList();
            }

            var pathDirection = CalculatePathSegmentDirection(node, startOrientation, _cameFrom);
            var nextNodes = GetSiblingNodes(node, endWall, endPoint, pathDirection, gridSize);

            foreach (var nextNode in nextNodes)
            {
                if (nextNode == null!) continue;

                var wallPenalty = CalculateWallPenalty(nextNode, startNode.H);
                var bendAlteration =
                    CalculateBendAlteration(nextNode, node, endWall, endPoint, pathDirection, gridSize);

                var newG = node.G + Heuristic(node, nextNode) + wallPenalty + bendAlteration;
                var newH = Heuristic(nextNode, endNode);

                if (nextNode.IsOpen && !_orderedOpenSet.Contains(nextNode))
                {
                    nextNode.G = newG;
                    nextNode.H = newH;
                    nextNode.F = nextNode.G + nextNode.H;

                    _cameFrom[nextNode] = node;
                    _orderedOpenSet.Push(nextNode, nextNode.F);
                }

                if (newG < nextNode.G)
                {
                    nextNode.G = newG;
                    nextNode.H = newH;
                    nextNode.F = nextNode.G + nextNode.H;

                    _cameFrom[nextNode] = node;
                    if (nextNode.IsOpen)
                    {
                        _orderedOpenSet.Remove(nextNode);
                        _orderedOpenSet.Push(nextNode, nextNode.F);
                    }
                }
            }
        }
    }

    private void AddNonWall(List<AStarNode> collection, Point position)
    {
        // We add this optimized point if it's not a wall and we haven't already added it.
        var newNode = _stepGenerator.GenerateNode(position);
        if (newNode.IsOpen && (!AvoidShapes || !newNode.IsWall) && !collection.Contains(newNode))
            collection.Add(newNode);
    }
}