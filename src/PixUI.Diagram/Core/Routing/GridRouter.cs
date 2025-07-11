namespace PixUI.Diagram;

/// <summary>
/// Routing based on the A* algorithm.
/// </summary>
internal sealed class GridRouter : RoutingBase
{

    /// <summary>
    /// The size of the routing grid.
    /// </summary>
    private const float RoutingGridBounds = 5000f;

    /// <summary>
    /// This dictionary acts as a series of linked vectors. The value for a given position is the previous position, thus allowing one to reconstruct the chosen path (see <see cref="ReconstructPath"/>).
    /// </summary>
    private readonly Dictionary<PathNode, PathNode> _cameFrom;

    private readonly LatticeDictionary _lattice;

    /// <summary>
    /// The set of available positions in the lattice ordered by increasing distance to the final goal. Meaning that the node with the shortest (Euclidean) distance to the goal is presented first.
    /// </summary>
    private readonly PriorityQueue<PathNode, double> _orderedOpenSet;

    /// <summary>
    /// Initializes static members of the <see cref="GridRouter" /> class.
    /// </summary>
    static GridRouter()
    {
        Cutoff = 100000;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GridRouter"/> class.
    /// </summary>
    /// <param name="diagram">The diagram.</param>
    public GridRouter(DiagramSurface diagram)
        : base(diagram)
    {
        _lattice = new LatticeDictionary(point => !PointIsInNeighborhoodOfShape(point), true) { Bounds = Rect.FromLTWH(-RoutingGridBounds, -RoutingGridBounds, 2 * RoutingGridBounds, 2 * RoutingGridBounds) };
        _cameFrom = new Dictionary<PathNode, PathNode>();
        _orderedOpenSet = new PriorityQueue<PathNode, double>(OrderType.Ascending);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GridRouter"/> class.
    /// </summary>
    /// <param name="diagram">The diagram.</param>
    /// <param name="locationDelegate">The location delegate.</param>
    public GridRouter(DiagramSurface diagram, Func<Point, bool> locationDelegate)
        : this(diagram)
    {
        _lattice = new LatticeDictionary(locationDelegate, true);
    }

    /// <summary>
    /// A method which tells the algorithm whether the discovered point is a valid result.
    /// </summary>
    /// <param name="p">The point which has to be validated by the delegate.</param>
    /// <returns></returns>
    internal delegate bool IsValidLocationDelegate(Point p);

    /// <summary>
    /// This is a cutoff for the A*-search and represents the buffer of points.
    /// </summary>
    public static int Cutoff { get; set; }

    /// <summary>
    /// Finds the duplicate in the list of points which leads to loops in the paths.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    public static Point? FindDuplicate(IEnumerable<Point> source)
    {
        var seenKeys = new HashSet<Point>();
        var duplicates = source.Where(element => !seenKeys.Add(element)).ToArray();
        return duplicates.Any() ? (Point?)duplicates.First() : null;
    }

    /// <summary>
    /// Removes the loops in the point list.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns></returns>
    public static IList<Point> RemoveLoops(IList<Point> points)
    {
        var duplicate = FindDuplicate(points);
        var pointsList = points.ToList();
        while (duplicate.HasValue)
        {
            var s = pointsList.IndexOf(duplicate.Value);
            var n = pointsList.IndexOf(duplicate.Value, s + 1) - s;
            for (var i = 0; i < n; i++) points.RemoveAt(s);
            duplicate = FindDuplicate(points);
        }
        return points;
    }

    /// <summary>
    /// Gets the route points.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <param name="showLastLine">If set to <c>true</c> the intermediate points will be added to the resulting path.</param>
    /// <returns></returns>
    public override IList<Point> GetRoutePoints(IConnection connection, bool showLastLine = true)
    {
        var startPoint = connection.StartPoint;
        var endPoint = connection.EndPoint;

        // special case: if the endpoints are within a square of size of the routing grid
        if (startPoint.Distance(endPoint) * Math.Sqrt(2) <= DiagramConstants.RoutingGridSize) return new List<Point>();

        var tuple = FindMostProbableTuple(connection);

        var result = Route(tuple.Item3, tuple.Item4) ?? new List<Point>();
        if (showLastLine)
        {
            result.Insert(0, tuple.Item2);
            result.Add(tuple.Item5);
        }

        result = RemoveLoops(result.ToList());
        result = RemoveEndPointAnomalies(result.ToList());
        return result;
    }

    /// <summary>
    /// Returns null, if no path is found. Start- and End-Node are included in returned path. 
    /// </summary>
    public IList<Point> Route(Point startPoint, Point endPoint)
    {
        // if start and end are within a grid's square there is no route 
        if (startPoint == endPoint || startPoint.Distance(endPoint) <= DiagramConstants.RoutingGridSize * Math.Sqrt(2)) return new List<Point>();
        _lattice.Clear();

        // ensures that the start and finish are never considered as walls
        _lattice.StartPoint = startPoint;
        _lattice.EndPoint = endPoint;

        var startNode = _lattice[startPoint];
        var endNode = _lattice[endPoint];

        if (startNode == endNode) return new List<Point> { startNode.Position };

        _orderedOpenSet.Clear();

        // clear the linked list of chosen nodes
        _cameFrom.Clear();

        startNode.G = 0;
        startNode.H = Heuristic(startNode, endNode);
        startNode.F = startNode.H;
        startNode.IsOpen = true;
        ////this.openSet.Add(startPoint, startNode);
        _orderedOpenSet.Push(startNode, startNode.H);

        while (_orderedOpenSet.Count > 0 && _orderedOpenSet.Count < Cutoff)
        {
            var node = _orderedOpenSet.Pop();
            node.IsOpen = false;
            if (node == endNode)
            {
                var result = ReconstructPath(_cameFrom, endNode);
                return result.Select(n => n.Position).ToList();
            }

            var neighborNodes = GetNeighbors(node);

            // we got stuck in a corner or surrounded by walls
            if (!neighborNodes.Any()) return null;
            foreach (var neighborNode in neighborNodes.Where(n => !n.IsWall))
            {
                if (neighborNode.IsOpen && !_orderedOpenSet.Contains(neighborNode)) _orderedOpenSet.Push(neighborNode, neighborNode.H);

                bool tentativeIsBetter;
                var tentativeGScore = node.G + Heuristic(node, neighborNode);

                if (neighborNode.IsOpen) tentativeIsBetter = true;
                else if (tentativeGScore < neighborNode.G) tentativeIsBetter = true;
                else tentativeIsBetter = false;

                if (tentativeIsBetter)
                {
                    if (_cameFrom.ContainsKey(neighborNode)) _cameFrom[neighborNode] = node;
                    else _cameFrom.Add(neighborNode, node);

                    neighborNode.G = tentativeGScore;
                    neighborNode.H = Heuristic(neighborNode, endNode);
                    neighborNode.F = neighborNode.G + neighborNode.H;
                    if (neighborNode.IsOpen)
                    {
                        ////update the queue since the H has changed
                        _orderedOpenSet.Remove(neighborNode);
                        _orderedOpenSet.Push(neighborNode, neighborNode.H);
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// This returns the metric or value through which the algorithm decides whether it gets closer to the searched endpoint.
    /// </summary>
    /// <param name="node">A path node.</param>
    /// <param name="targetNode">The node to reach.</param>
    /// <returns></returns>
    private static float Heuristic(PathNode node, PathNode targetNode)
    {
        // a simple Euclidean distance will do
        return node.Position.Distance(targetNode.Position);
    }

    private static IEnumerable<PathNode> ReconstructPath(IDictionary<PathNode, PathNode> cameFrom, PathNode currentNode)
    {
        var result = new List<PathNode>();

        ReconstructPathRecursive(cameFrom, currentNode, ref result);
        result.Reverse();
        return result;
    }

    private static void ReconstructPathRecursive(IDictionary<PathNode, PathNode> cameFrom, PathNode currentNode, ref List<PathNode> result)
    {
        result.Add(currentNode);
        if (cameFrom.ContainsKey(currentNode)) ReconstructPathRecursive(cameFrom, cameFrom[currentNode], ref result);
    }

    /// <summary>
    /// Removes the end point anomalies created by the first and last segment containing the first respectively last intermediate point.
    /// </summary>
    /// <param name="points">The original points.</param>
    /// <returns></returns>
    private static List<Point> RemoveEndPointAnomalies(List<Point> points)
    {
        var n = points.Count;

        // only meaningful if we have a 'normal' polyline
        if (n < 6) return points;

        // the actual start point of the routing
        var s2 = points[1];

        // the first calculated point of the route
        var s3 = points[2];

        // the intermediate start point
        var s1 = points[0];

        // the actual end point of the routing
        var e2 = points[n - 2];

        // the last calculated point of the route
        var e3 = points[n - 3];

        // the intermediate end point
        var e1 = points[n - 1];

        if (SitsInSegment(s1, s2, s3) || SitsInSegment(s1, s3, s2)) points.Remove(s2);

        if (SitsInSegment(e1, e2, e3) || SitsInSegment(e1, e3, e2)) points.Remove(e2);
        return points;
    }

    /// <summary>
    /// Returns whether the first given point is in the line segment defined by the other two given points.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="a">A.</param>
    /// <param name="b">The b.</param>
    /// <returns></returns>
    private static bool SitsInSegment(Point x, Point a, Point b)
    {
        // horizontal
        if (Math.Abs(x.Y - b.Y) < Utils.Epsilon && Math.Abs(x.Y - a.Y) < Utils.Epsilon) return x.X >= a.X && x.X <= b.X;

        // vertical
        if (Math.Abs(x.X - b.X) < Utils.Epsilon && Math.Abs(x.X - a.X) < Utils.Epsilon) return x.Y >= a.Y && x.Y <= b.Y;

        return false;
    }

    private PathNode[] GetNeighbors(PathNode node)
    {
        var routingGridSize = DiagramConstants.RoutingGridSize;
        var x = node.Position.X;
        var y = node.Position.Y;

        /*
            0
         1  *  2
            3
        */

        var neighbors = new PathNode[4];

        neighbors[0] = _lattice[new Point(x, y - routingGridSize)];
        neighbors[1] = _lattice[new Point(x - routingGridSize, y)];
        neighbors[2] = _lattice[new Point(x + routingGridSize, y)];
        neighbors[3] = _lattice[new Point(x, y + routingGridSize)];

        return neighbors;

        /*

       0     1
          *
       2     3

        var neighbors = new PathNode[4];

        neighbors[0] = this.lattice[new Point(x - RoutingGridSize, y - RoutingGridSize)];
        neighbors[1] = this.lattice[new Point(x + RoutingGridSize, y - RoutingGridSize)];
        neighbors[2] = this.lattice[new Point(x - RoutingGridSize, y + RoutingGridSize)];
        neighbors[3] = this.lattice[new Point(x + RoutingGridSize, y + RoutingGridSize)];

        return neighbors;
        */
        /*

         0  1  2
         3  *  4
         5  6  7


        var neighbors = new PathNode[8];

        neighbors[0] = this.lattice[new Point(x - RoutingGridSize, y - RoutingGridSize)];

        neighbors[1] = this.lattice[new Point(x, y - RoutingGridSize)];

        neighbors[2] = this.lattice[new Point(x + RoutingGridSize, y - RoutingGridSize)];

        neighbors[3] = this.lattice[new Point(x - RoutingGridSize, y)];

        neighbors[4] = this.lattice[new Point(x + RoutingGridSize, y)];

        neighbors[5] = this.lattice[new Point(x - RoutingGridSize, y + RoutingGridSize)];

        neighbors[6] = this.lattice[new Point(x, y + RoutingGridSize)];

        neighbors[7] = this.lattice[new Point(x + RoutingGridSize, y + RoutingGridSize)];

        return neighbors;
        */
    }

}