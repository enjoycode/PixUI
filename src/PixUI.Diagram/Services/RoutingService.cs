namespace PixUI.Diagram;

public sealed class RoutingService
{
    /// <summary>
    /// Gets or sets the connection router.
    /// </summary>
    public IRouter Router { get; set; }

    public RoutingService(DiagramSurface diagram)
    {
        //this.Router = new GridRouter(diagram);
    }

    /// <summary>
    /// Creates the connection route.
    /// </summary>
    /// <param name="connection">The connection which should be routed.</param>
    /// <remarks>The routing works only for the <see cref="ConnectionType.Spline"/> and <see cref="ConnectionType.Polyline"/> types.</remarks>
    /// <returns>A list of intermediate points defining the route and the start and end connectors.</returns>
    public ConnectionRoute FindExtendedRoute(IConnection connection)
    {
        var result = new ConnectionRoute();
        if (connection == null) return result;
        //if (connection.IsModified) return new ConnectionRoute(connection.ConnectionPoints.ToList());

        if (connection.ConnectionType != ConnectionType.Bezier)
        {
            //IExtendedRouter extendetRouter = this.Router as IExtendedRouter;
            IRouter router = Router;
            if (connection.Source == null || connection.Target == null)
            {
                return new ConnectionRoute(connection.ConnectionPoints.ToList());
                //extendetRouter = this.FreeRouter as IExtendedRouter;
                //router = this.FreeRouter;
            }

            //if (extendetRouter != null)
            //    result = extendetRouter.GetRoutePoints(connection);
            //else if (router != null)
            result = new ConnectionRoute(router.GetRoutePoints(connection, false));
        }
        return result;
    }
}