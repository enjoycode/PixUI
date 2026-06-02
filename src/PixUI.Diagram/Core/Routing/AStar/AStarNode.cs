using System.Diagnostics;

namespace PixUI.Diagram;

/// <summary>
/// Represents an instance of the Node class that is used by the AStarRouter.
/// </summary>
[DebuggerDisplay("P={Position} :: G={G} :: H={H} :: F={F}")]
public sealed class AStarNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AStarNode" /> class.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <param name="isStartOrEndWall">The is start wall.</param>
    /// <param name="isWall">The is wall.</param>
    public AStarNode(Point point, bool isStartOrEndWall, bool isWall)
    {
        Position = point;
        IsWall = isWall;
        IsStartOrEndWall = isStartOrEndWall;
    }

    /// <summary>
    /// Gets or sets the actual distance from the start to the current point in the search.
    /// </summary>
    public float G { get; set; }

    /// <summary>
    /// Gets or sets the heuristic distance to the endpoint or goal. The smaller the value the closer to the goal.
    /// </summary>
    public float H { get; set; }

    /// <summary>
    /// Gets or sets the whole path length through this point. This is the sum of G and H.
    /// </summary>
    public float F { get; set; }

    /// <summary>
    /// Gets or sets the is open.
    /// </summary>
    /// <value>The is open.</value>
    public bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    /// <value>The position.</value>
    public Point Position { get; private set; }

    /// <summary>
    /// Gets or sets whether this node is wall.
    /// </summary>
    /// <value>The is wall.</value>
    public bool IsWall { get; private set; }

    /// <summary>
    /// Gets or sets whether this node is a start or end wall.
    /// </summary>
    /// <value>The is start wall.</value>
    public bool IsStartOrEndWall { get; private set; }
}