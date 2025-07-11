namespace PixUI.Diagram;

public static class DiagramConstants
{
    /// <summary>
    /// Gets or sets the connection corner radius.
    /// </summary>
    /// <value>
    /// The connection corner radius.
    /// </value>
    public static float ConnectionCornerRadius { get; set; } = 10f;

    /// <summary>
    /// The distance, within which the shape's connectors are visible.
    /// </summary>
    public static float ConnectorActivationRadius { get; set; } = 15f;

    /// <summary>
    /// The radius around connector where connection can attach.
    /// </summary>
    public static float ConnectorHitTestRadius { get; set; } = 5f;

    /// <summary>
    /// The routing grid size.
    /// </summary>
    public static float RoutingGridSize { get; set; } = 40f;

    /// <summary>
    /// Gets or sets the default offset when the Bezier handles are automatically calculated according to the connector's position.
    /// </summary>
    public static float BezierAutoOffset { get; set; } = 30f;

    /// <summary>
    /// Gets or sets the radius of the connection bridge or gap.
    /// </summary>
    public static float CrossingRadius { get; set; } = 5f;
}