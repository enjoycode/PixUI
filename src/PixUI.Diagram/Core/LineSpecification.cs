namespace PixUI.Diagram;

/// <summary>
/// Parametrization of a polyline geometry which allows you to use multiple <see cref="Points"/> and
/// bridges (using the <see cref="Crossings"/> data and the <see cref="BridgeType"/>).
/// The  method uses such specifications to create a concrete geometry.
/// </summary>
/// <remarks>
/// There are a few things to consider in relation to crossings:
/// <list type="bullet">
/// <item>
/// <term>None: </term>
/// <description>the <see cref="StartPoint"/> and <see cref="EndPoint"/> will be used to define
/// the start and end of the polyline.
/// If some caps are used, these points will be shifted if necessary so that the polyline makes space for the caps.
/// This depends on the type of the cap.</description>
/// </item> 
/// <item><term>Some bridge type:</term>
/// <description>the <see cref="Crossings"/> data will define the start and end points,
/// while the <see cref="StartPoint"/> and <see cref="EndPoint"/> will be irrelevant in this case. </description> 
/// </item>
/// </list>
/// </remarks>
internal class LineSpecification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LineSpecification" /> class.
    /// </summary>
    public LineSpecification()
    {
        StartPoint = new Point();
        EndPoint = new Point();
        RoundedCorners = false;
        BridgeType = BridgeType.None;
        Crossings = null;
        Points = null;
        BezierTension = 1f;
        Bounds = new Rect();
    }

    /// <summary>
    /// Gets or sets the bounds of the connection.
    /// </summary>
    public Rect Bounds { get; init; }

    /// <summary>
    /// Gets or sets the type of the connection.
    /// </summary>
    /// <value>
    /// The type of the connection.
    /// </value>
    public ConnectionType ConnectionType { get; init; }

    /// <summary>
    /// Gets or sets the Bezier tension.
    /// </summary>
    /// <value>
    /// A value or zero turns the Bezier connection into a straight line, a value of one and above increase the sharpness of the Bezier curve.
    /// </value>
    public float BezierTension { get; init; }

    /// <summary>
    /// Gets or sets the start point.
    /// </summary>
    /// <remarks>
    /// This position is with respect to the local coordinate system (of the connection).
    /// </remarks>
    public Point StartPoint { get; init; }

    /// <summary>
    /// Gets or sets the end point.
    /// </summary>
    /// <remarks>
    /// This position is with respect to the local coordinate system (of the connection).
    /// </remarks>
    public Point EndPoint { get; init; }

    /// <summary>
    /// Gets or sets the corners should be rounded.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [rounded corners]; otherwise, <c>false</c>.
    /// </value>
    public bool RoundedCorners { get; init; }

    /// <summary>
    /// Gets or sets the type of the bridge or crossing.
    /// </summary>
    public BridgeType BridgeType { get; init; }

    /// <summary>
    /// Gets or sets the crossing data.
    /// </summary>
    public CrossingsData? Crossings { get; init; }

    /// <summary>
    /// Gets or sets the intermediate connection points.
    /// </summary>
    /// <remarks>Do NOT include the start/end points.</remarks>
    public IList<Point>? Points { get; init; }
}