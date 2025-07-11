using System.Diagnostics.CodeAnalysis;

namespace PixUI.Diagram;

/// <summary>
/// Router used for the TreeDown, TreeLeft, TreeRight, TreeUp and TipOverTree LayoutTypes.
/// </summary>
internal class OrgTreeRouter : IRouter
{
    // Pool of routers.
    private static readonly Dictionary<TreeLayoutType, TreeRouterBase> Routers;

    static OrgTreeRouter()
    {
        Routers = new Dictionary<TreeLayoutType, TreeRouterBase>
        {
            {
                TreeLayoutType.TreeRight,
                new DirectionalRouter(new DirectionalRoutingSettings(ConnectorPosition.Right,
                    ConnectorPosition.Left, 1, 1))
            },
            {
                TreeLayoutType.TreeLeft,
                new DirectionalRouter(new DirectionalRoutingSettings(ConnectorPosition.Left,
                    ConnectorPosition.Right, 1, -1))
            },
            {
                TreeLayoutType.TreeDown,
                new DirectionalRouter(new DirectionalRoutingSettings(ConnectorPosition.Bottom,
                    ConnectorPosition.Top, -1, 1))
            },
            {
                TreeLayoutType.TreeUp,
                new DirectionalRouter(new DirectionalRoutingSettings(ConnectorPosition.Top,
                    ConnectorPosition.Bottom, -1, -1))
            },
            //{ TreeLayoutType.TipOverTree, new TipOverTreeRouter() },
        };
    }

    /// <summary>
    /// Gets or sets the type of the tree layout.
    /// </summary>
    /// <value>
    /// The type of the tree layout.
    /// </value>
    public TreeLayoutType TreeLayoutType { get; set; }

    /// <summary>
    /// Gets or sets the connection outer spacing.
    /// </summary>
    /// <value>
    /// The connection outer spacing.
    /// </value>
    public float ConnectionOuterSpacing { get; set; }

    /// <summary>
    /// Creates a TreeLayoutType -specific router and gets the route points.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <param name="showLastLine">Whether the last line segment should be shown.</param>
    /// <returns></returns>
    [SuppressMessage("Microsoft.Performance", "CA1801:ReviewUnusedParameters", MessageId = "showLastLine",
        Justification = "Part of the IRouter interface.")]
    public IList<Point> GetRoutePoints(IConnection connection, bool showLastLine)
    {
        if (!Routers.Keys.Contains(TreeLayoutType))
        {
            return new List<Point>();
        }

        var router = Routers[TreeLayoutType];
        return router.GetRoute(connection, ConnectionOuterSpacing);
    }
}