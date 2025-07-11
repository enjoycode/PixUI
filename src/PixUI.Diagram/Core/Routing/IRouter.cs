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