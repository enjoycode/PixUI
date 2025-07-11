namespace PixUI.Diagram;

public interface IShape : IDiagramItem, IRotatable
{
    /// <summary>
    /// Gets the incoming links (connections).
    /// </summary>
    /// <remarks>These are the links or connection ending in this shape.</remarks>
    IEnumerable<IConnection> IncomingLinks { get; }

    /// <summary>
    /// Gets the outgoing links (connections).
    /// </summary>
    /// <remarks>These are the links or connection originating from this shape.</remarks>
    IEnumerable<IConnection> OutgoingLinks { get; }

    /// <summary>
    /// Gets the connectors of this shape.
    /// </summary>
    /// <seealso cref="IConnector"/>
    ConnectorCollection Connectors { get; }

    bool CanConnect(bool isStartPoint, IConnection connection);

    ///// <summary>
    ///// Gets or sets a value indicating whether the connector adorner is visible.
    ///// </summary>
    ///// <value>
    /////     <c>true</c> if this instance is connectors adorner visible; otherwise, <c>false</c>.
    ///// </value>
    //bool IsConnectorsAdornerVisible { get; set; }
}