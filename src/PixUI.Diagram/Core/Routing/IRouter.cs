namespace PixUI.Diagram;

public interface IRouter
{
    /// <summary>
    /// Gets the route points.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <param name="showLastLine">Whether the last line segment should be shown.</param>
    /// <returns></returns>
    IList<Point> GetRoutePoints(IConnection connection, bool showLastLine);
}

/// <summary>
///  Represents an extended connection router.
/// </summary>
public interface IExtendedRouter : IRouter
{
    /// <summary>
    /// Gets the route points and the start and end connectors.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <returns></returns>
    ConnectionRoute GetRoutePoints(IConnection connection);
}