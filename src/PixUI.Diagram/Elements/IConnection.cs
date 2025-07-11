namespace PixUI.Diagram;

public interface IConnection : IDiagramItem
{
    /// <summary>
    /// Gets or sets the position of where this connection starts.
    /// </summary>
    Point StartPoint { get; set; }

    /// <summary>
    /// Gets or sets the position of where this connection ends.
    /// </summary>
    Point EndPoint { get; set; }

    /// <summary>
    /// Gets or sets the source shape of this connection.
    /// </summary>
    IShape? Source { get; set; }

    string SourceConnectorPosition { get; set; }

    /// <summary>
    /// Gets the actual source connector of this connection if the connector is dynamically assigned (<see cref="ConnectorPosition.Auto"/>).
    /// </summary>
    IConnector SourceConnectorResult { get; }

    /// <summary>
    /// Gets or sets the target shape of this connection.
    /// </summary>
    IShape? Target { get; set; }

    string TargetConnectorPosition { get; set; }

    /// <summary>
    /// Gets the actual target connector of this connection if the connector is dynamically assigned (<see cref="ConnectorPosition.Auto"/>).
    /// </summary>
    IConnector TargetConnectorResult { get; }

    /// <summary>
    /// Gets the connection points.
    /// </summary>
    /// <remarks>The positions are absolute coordinates with respect to the canvas.</remarks>
    IList<Point> ConnectionPoints { get; }

    /// <summary>
    /// Gets or sets the type of the connection.
    /// </summary>
    ConnectionType ConnectionType { get; set; }

    /// <summary>
    /// Attaches the connection to specific source and target.
    /// </summary>
    /// <param name="source">The source connector to attach to.</param>
    /// <param name="target">The target connector to attach to.</param>
    void Attach(IConnector? source = null, IConnector? target = null);

    /// <summary>
    /// Updates this connection.
    /// </summary>
    /// <param name="isManipulating">Should be set to <c>true</c>
    /// if any of the manipulation services is active as a result of a user action.
    /// </param>
    void Update(bool isManipulating = false);
}