namespace PixUI.Diagram;

/// <summary>
/// A helper class that handles the creation of AStarNodes.
/// </summary>
internal sealed class NodeGenerator
{
    private readonly List<Rect> _walls;
    private readonly Rect _startWall;
    private readonly Rect _endWall;
    private readonly Dictionary<Point, AStarNode> _generatedNodes;

    internal NodeGenerator(List<Rect> walls, Rect startWall, Rect endWall)
    {
        _walls = walls;
        _startWall = startWall;
        _endWall = endWall;
        _generatedNodes = new Dictionary<Point, AStarNode>();
    }

    internal AStarNode GenerateNode(Point point)
    {
        if (_generatedNodes.TryGetValue(point, out var node))
            return node;

        var isStarOrEndWall = AStarRouter.Contains(_startWall, point) || AStarRouter.Contains(_endWall, point);
        var isWall = isStarOrEndWall || _walls.Any(wall => !wall.IsEmpty && AStarRouter.Contains(wall, point));

        node = new AStarNode(point, isStarOrEndWall, isWall);
        node.IsOpen = true;
        _generatedNodes.Add(point, node);

        return node;
    }
}