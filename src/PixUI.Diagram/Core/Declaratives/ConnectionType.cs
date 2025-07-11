namespace PixUI.Diagram;

/// <summary>
/// The type of the connection.
/// </summary>
public enum ConnectionType
{
    /// <summary>
    /// Polyline connection type.
    /// </summary>
    Polyline,

    /// <summary>
    /// Bezier connection type.
    /// </summary>
    Bezier,

    /// <summary>
    /// The canonical spline interpolates smoothly between the intermediate connection points without the usage of handles like the <see cref="Bezier"/> type.
    /// </summary>
    Spline,
}