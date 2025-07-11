namespace PixUI.Diagram;

/// <summary>
/// Dictionary of Points needed in GridRouter.
/// </summary>
internal class LatticeDictionary : Dictionary<Point, PathNode>
{
    private readonly bool _autoCreate;
    private readonly Func<Point, bool> _isValid;

    /// <summary>
    /// Initializes a new instance of the <see cref="LatticeDictionary"/> class.
    /// </summary>
    /// <param name="locationDelegate">The location delegate.</param>
    /// <param name="autoCreate">If set to <c>true</c> [auto create].</param>
    public LatticeDictionary(Func<Point, bool> locationDelegate, bool autoCreate = false)
    {
        this._autoCreate = autoCreate;
        _isValid = locationDelegate;
    }

    /// <summary>
    /// Gets or sets the bounds.
    /// </summary>
    /// <value>
    /// The bounds.
    /// </value>
    public Rect Bounds { get; set; }

    /// <summary>
    /// Gets or sets the start point.
    /// </summary>
    /// <value>
    /// The start point.
    /// </value>
    public Point StartPoint { get; set; }

    /// <summary>
    /// Gets or sets the end point.
    /// </summary>
    /// <value>
    /// The end point.
    /// </value>
    public Point EndPoint { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="PathNode"/> with the specified point.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1043:UseIntegralOrStringArgumentsForIndexers")]
    public new PathNode this[Point point]
    {
        get
        {
            if (!_autoCreate)
                return ContainsKey(point) ? base[point] : null;
            else
            {
                if (ContainsKey(point)) return base[point];
                var node = new PathNode(point)
                {
                    IsOpen = true,
                    IsWall = IsWall(point)
                };

                Add(point, node);
                return base[point];
            }
        }
        set
        {
            value.Position = point;
            if (!ContainsKey(point))
                Add(point, value);
            else base[point] = value;
        }
    }

    private bool IsWall(Point p)
    {
        // make sure the start and end are always visible
        var r = (!_isValid(p)
                 || p.X <= Bounds.Left
                 || p.X >= (Bounds.Right - DiagramConstants.RoutingGridSize)
                 || p.Y <= Bounds.Top
                 || p.Y >= (Bounds.Bottom - DiagramConstants.RoutingGridSize))
                && p != StartPoint
                && p != EndPoint;

        // if (r) Debug.WriteLine("[{0},{1}] is hidden.", p.X, p.Y);
        return r;
    }
}