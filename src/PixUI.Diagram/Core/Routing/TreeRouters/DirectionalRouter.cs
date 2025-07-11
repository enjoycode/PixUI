namespace PixUI.Diagram;

/// <summary>
/// This router routes the connections when the shape layout is TreeDown, TreeUp, TreeLeft or TreeRight.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
internal class DirectionalRouter : TreeRouterBase
{
    private readonly DirectionalRoutingSettings _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectionalRouter"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public DirectionalRouter(DirectionalRoutingSettings settings)
    {
        this._settings = settings;
    }

    /// <summary>
    /// Sets the source and target connectors.
    /// </summary>
    protected override void SetSourceAndTargetConnectors()
    {
        //todo: Rick 暂取消
        //if (this.Connection.SourceConnectorPosition != ConnectorPosition.Gliding || this.settings.SourceConnectorName == ConnectorPosition.Gliding)
        //    this.Connection.SourceConnectorPosition = this.settings.SourceConnectorName;
        //if (this.Connection.TargetConnectorPosition != ConnectorPosition.Gliding || this.settings.TargetConnectorName == ConnectorPosition.Gliding)
        //    this.Connection.TargetConnectorPosition = this.settings.TargetConnectorName;
    }

    /// <summary>
    /// Gets the route points.
    /// </summary>
    /// <returns></returns>
    protected override IList<Point> GetRoutePoints()
    {
        var list = new List<Point>();
        var source = GetFirstNearPoint(SourceInflatedRect, Connection.SourceConnectorResult);
        var target = GetFirstNearPoint(TargetInflatedRect, Connection.TargetConnectorResult);

        list.Add(source);
        float xDiff = source.X - target.X;
        float yDiff = source.Y - target.Y;
        float minWidth = Math.Min(SourceInflatedRect.Width, TargetInflatedRect.Width);
        float minHeight = Math.Min(SourceInflatedRect.Height, TargetInflatedRect.Height);
        bool shouldAddSinglePointHor = xDiff * _settings.DirectionParameter <= 0 && _settings.HorizontalIndicator == 1;
        bool shouldAddSinglePointVert = yDiff * _settings.DirectionParameter <= 0 && _settings.HorizontalIndicator == -1;

        // connection is completely between source and target shape.
        if (shouldAddSinglePointHor || shouldAddSinglePointVert)
        {
            list.Add(new Point()
            {
                X = (source.X + target.X + (_settings.HorizontalIndicator * xDiff)) / 2f,
                Y = (source.Y + target.Y - (_settings.HorizontalIndicator * yDiff)) / 2f
            });
        }
        else if ((Math.Abs(yDiff) >= minHeight && _settings.HorizontalIndicator == 1) ||
                 (Math.Abs(xDiff) >= minWidth && _settings.HorizontalIndicator == -1))
        {
            // connection needs two routing points.
            float coeff = (_settings.HorizontalIndicator + 2) % 3;
            list.Add(new Point { X = source.X - (coeff * (xDiff / 2)), Y = source.Y + ((coeff - 1) * (yDiff / 2)) });
            list.Add(new Point { X = target.X + (coeff * (xDiff / 2)), Y = target.Y - ((coeff - 1) * (yDiff / 2)) });
        }
        else
        {
            // connection intersects with source shape.
            list.AddRange(GetUnionRectangleRoutePoints());
        }
        list.Add(target);
        return list;
    }

    /// <summary>
    /// Gets the router specific rectangle horizontal points.
    /// </summary>
    /// <param name="clockwise">If set to <c>false</c> returns the reversed list.</param>
    /// <param name="rectangle">The rectangle.</param>
    /// <returns></returns>
    protected override IList<Point> GetRouterSpecificRectHorizontalPoints(bool clockwise, Rect rectangle)
    {
        var list = new List<Point> { rectangle.TopLeft(), rectangle.TopRight() };
        if (!clockwise) list.Reverse();
        return list;
    }

    /// <summary>
    /// Gets the router specific rectangle vertical points.
    /// </summary>
    /// <param name="clockwise">If set to <c>false</c> returns the reversed list.</param>
    /// <param name="rectangle">The rectangle.</param>
    /// <returns></returns>
    protected override IList<Point> GetRouterSpecificRectVerticalPoints(bool clockwise, Rect rectangle)
    {
        var list = new List<Point> { rectangle.TopRight(), rectangle.BottomRight() };
        if (!clockwise) list.Reverse();
        return list;
    }
}