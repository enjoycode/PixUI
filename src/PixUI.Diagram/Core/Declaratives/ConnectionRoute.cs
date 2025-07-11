namespace PixUI.Diagram;

/// <summary>
/// Represents a connection route of intermediate points, start and end connectors.
/// </summary>
public class ConnectionRoute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionRoute" /> class.
    /// </summary>
    public ConnectionRoute() : this(new List<Point>()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionRoute" /> class.
    /// </summary>
    /// <param name="points">The intermediate points.</param>
    /// <param name="startConnector">The start connector.</param>
    /// <param name="endConnector">The end connector.</param>
    public ConnectionRoute(IEnumerable<Point> points, IConnector? startConnector = null,
        IConnector? endConnector = null)
    {
        Points = points.ToList();
        StartConnector = startConnector;
        EndConnector = endConnector;
    }

    /// <summary>
    /// Gets or sets the intermediate points.
    /// </summary>
    public IList<Point> Points { get; }

    /// <summary>
    /// Gets or sets the end connector.
    /// </summary>
    public IConnector? EndConnector { get; set; }

    /// <summary>
    /// Gets or sets the start connector.
    /// </summary>
    /// <value>The start connector.</value>
    public IConnector? StartConnector { get; set; }
}