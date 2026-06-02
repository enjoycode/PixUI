namespace PixUI.Diagram;

public sealed class RoutingService
{
    /// <summary>
    /// Gets or sets the connection router.
    /// </summary>
    public IRouter Router { get; set; }

    public RoutingService(DiagramSurface diagram)
    {
        Router = new GridRouter(diagram);
    }

    /// <summary>
    /// Creates the connection route.
    /// </summary>
    /// <param name="connection">The connection which should be routed.</param>
    /// <remarks>The routing works only for the <see cref="ConnectionType.Spline"/> and <see cref="ConnectionType.Polyline"/> types.</remarks>
    /// <returns>A list of intermediate points defining the route and the start and end connectors.</returns>
    public ConnectionRoute FindExtendedRoute(IConnection? connection)
    {
        var result = new ConnectionRoute();
        if (connection == null) return result;
        if (connection.IsModified) return new ConnectionRoute(connection.ConnectionPoints.ToList());

        if (connection.ConnectionType != ConnectionType.Bezier)
        {
            var extendedRouter = Router as IExtendedRouter;
            var router = Router;
            if (connection.Source == null || connection.Target == null)
            {
                return new ConnectionRoute(connection.ConnectionPoints.ToList());
                // extendedRouter = FreeRouter as IExtendedRouter;
                // router = FreeRouter;
            }

            if (extendedRouter != null)
                result = extendedRouter.GetRoutePoints(connection);
            else if (router != null!)
                result = new ConnectionRoute(router.GetRoutePoints(connection, false));
        }

        return result;
    }
}