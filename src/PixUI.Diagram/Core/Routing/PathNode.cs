namespace PixUI.Diagram;

/// <summary>
/// Runtime data bucket for the A* algorithm.
/// </summary>
/// <remarks> The <see cref="F"/>, <see cref="H"/> and <see cref="G"/> property names have been kept in accordance with the literature even though more appropriate names would help.</remarks>
internal sealed class PathNode : IComparer<PathNode>, IPriorityObject
{
    public static readonly PathNode Empty = new PathNode(new Point(0, 0));

    /// <summary>
    /// Initializes a new instance of the <see cref="PathNode"/> class.
    /// </summary>
    public PathNode()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PathNode"/> class.
    /// </summary>
    public PathNode(Point point)
    {
        Position = point;
    }

    /// <summary>
    /// Gets the actual lattice distance from the start to the current point in the search.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "G")]
    public float G { get; set; }

    /// <summary>
    /// Gets the so-called heuristic distance which is usually the Euclidean distance to the endpoint or goal. The smaller the value the closer to the goal.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "H")]
    public float H { get; set; }

    /// <summary>
    /// Gets the current path length which is an estimate since the remains of the path  to the target is a estimated to be a straight line.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "F")]
    public float F { get; set; }

    /// <summary>
    /// Gets or sets whether this node is part of the open set of not estimated nodes in the search.
    /// </summary>
    /// <value>
    ///   <c>True</c> if this instance is open; otherwise, <c>false</c>.
    /// </value>
    public bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets whether this node is accessible as part of the path searching.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is wall; otherwise, <c>false</c>.
    /// </value>
    public bool IsWall { get; set; }

    /// <summary>
    /// Gets or sets the priority.
    /// </summary>
    /// <value>
    /// The priority.
    /// </value>
    public int Priority { get; set; }

    /// <summary>
    /// Gets or sets the position in the lattice.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public Point Position { get; set; }

    /// <summary>
    /// Compares the given nodes by comparing their estimated path length, i.e. the <see cref="F"/> values.
    /// </summary>
    /// <param name="x">A node on the lattice.</param>
    /// <param name="y">Another node on the lattice.</param>
    /// <returns></returns>
    public int Compare(PathNode x, PathNode y)
    {
        return x.F < y.F ? -1 : (x.F > y.F ? 1 : 0);
    }
}