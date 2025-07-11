namespace PixUI.Diagram;

/// <summary>
/// Parametrization of a polyline geometry which allows you to use multiple Points, bridges (using the Crossings data and the BridgeType) and end caps. 
/// </summary>    
internal sealed class PolylineSpecification : LineSpecification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PolylineSpecification" /> class.
    /// </summary>
    public PolylineSpecification()
    {
        StartPoint = new Point();
        EndPoint = new Point();
        RoundedCorners = false;
        BridgeType = BridgeType.None;
        Crossings = null;
        Points = null;
        StartCapType = CapType.None;
        EndCapType = CapType.None;
        StartCapWidth = 0f;
        StartCapHeight = 0f;
        EndCapWidth = 0f;
        EndCapHeight = 0f;
        BezierTension = 1f;
        Bounds = new Rect();
    }

    /// <summary>
    /// Gets or sets the type of the start-cap.
    /// </summary>
    public CapType StartCapType { get; set; }

    /// <summary>
    /// Gets or sets the type of the end-cap.
    /// </summary>
    public CapType EndCapType { get; set; }

    /// <summary>
    /// Gets or sets the width of the start-cap.
    /// </summary>
    public float StartCapWidth { get; set; }

    /// <summary>
    /// Gets or sets the height of the start-cap.
    /// </summary>
    /// <value>
    /// The start height of the cap.
    /// </value>
    public float StartCapHeight { get; set; }

    /// <summary>
    /// Gets or sets the width of the end-cap.
    /// </summary>
    public float EndCapWidth { get; set; }

    /// <summary>
    /// Gets or sets the height of the end-cap.
    /// </summary>
    public float EndCapHeight { get; set; }
}