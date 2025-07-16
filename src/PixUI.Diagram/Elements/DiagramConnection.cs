using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace PixUI.Diagram;

public class DiagramConnection : DiagramItem, IConnection
{
    #region ====Fields & Properties====

    private bool _isDirty;
    private bool _isSuppressingConnectionUpdate;
    private bool _isInnerChange;

    private Point _startPoint;
    private Point _endPoint;
    private Point _editingPoint;
    private Point _position;

    private readonly ObservableCollection<Point> _connectionPoints = [];

    private Geometry _geometry;
    private PathFigure? _sourceCap, _targetCap;

    private string _title = string.Empty;
    private Paragraph? _cachedLayout;

    #region ----文字相关属性----

    public virtual string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                _cachedLayout = null;
            }
        }
    }

    private Paragraph? CachedLayout
    {
        get
        {
            if (string.IsNullOrEmpty(Title))
                return null;

            if (_cachedLayout == null)
            {
                using var ps = new ParagraphStyle();
                using var pb = new ParagraphBuilder(ps);
                pb.PushStyle(new TextStyle() { Color = ForeColor }); //TODO: fix font
                pb.AddText(Title);
                _cachedLayout = pb.Build();
                _cachedLayout.Layout(float.MaxValue);
            }

            return _cachedLayout;
        }
    }

    private Size CachedTextSize
    {
        get
        {
            var layout = CachedLayout;
            if (layout == null)
                return new Size(0, 13f);
            return new Size(layout.MaxIntrinsicWidth, layout.Height);
        }
    }

    #endregion

    #region ----形状相关属性----

    [Browsable(false)]
    public Geometry Geometry
    {
        get => _geometry;
        set
        {
            if (_geometry != value) _geometry = value;
        }
    }

    /// <summary>
    /// Gets the connection points of the connection.
    /// </summary>
    /// <remarks>The positions are absolute coordinates with respect to the canvas.</remarks>
    [Browsable(false)]
    public IList<Point> ConnectionPoints => _connectionPoints;

    [Browsable(false)]
    public Point StartPoint
    {
        get
        {
            if (SourceConnectorResult == null)
                return _startPoint;
            if (SourceConnectorResult.Name == ConnectorPosition.Gliding)
            {
                // don't use the connector's absolute position but get the crossing point
                // between the line (defined by the centers of the shapes) and the bounds
                Point endp;

                // if there are intermediate points, we need to use the almost last point
                if (ConnectionPoints.Any())
                    endp = ConnectionPoints[0];
                else
                    endp = Target == null ? _endPoint : Target.Bounds.Center();

                var r = SourceConnectorResult.AbsolutePosition;
                if (Source is DiagramShape shape)
                {
                    // since the center does not sit inside the shape and doesn't give intersections
                    var startp = shape.GlidingStyle == GlidingStyle.RightTriangle
                        ? new Point(shape.Bounds.X + (shape.Bounds.Width / 5),
                            shape.Bounds.Y + (2 * shape.Bounds.Height / 3))
                        : SourceConnectorResult.AbsolutePosition;
                    r = shape.Bounds.FindIntersectionPoint(shape.RotationAngle, startp, endp,
                        shape.GlidingStyle);
                }
                else
                {
                    Utils.IntersectionPointOnRectangle(Source!.Bounds, SourceConnectorResult.AbsolutePosition,
                        endp, ref r);
                }

                return _startPoint = r;
            }

            _startPoint = SourceConnectorResult.AbsolutePosition;
            return _startPoint;
        }
        set
        {
            if (Source == null)
            {
                if (_startPoint != value)
                {
                    // move the handle along with the endpoint
                    if (ConnectionType == ConnectionType.Bezier)
                        ConnectionPoints[0] = new Point(
                            ConnectionPoints[0].X + value.X - _startPoint.X,
                            ConnectionPoints[0].Y + value.Y - _startPoint.Y);
                    _startPoint = value;
                    SetPosition(Utils.GetTopLeftPoint(this.AllPoints()));
                    //this.OnPropertyChanged(nameof(StartPoint));
                }
            }

            //TODO: maybe need repaint
        }
    }

    [Browsable(false)]
    public Point EndPoint
    {
        get
        {
            // the logic is basically that the 'endPoint' is returned if the connection floats,
            // otherwise 'endPoint' is irrelevant and the connector's position is returned
            if (TargetConnectorResult == null)
                return _endPoint;

            if (TargetConnectorResult.Name == ConnectorPosition.Gliding)
            {
                Point startp;

                // if there are intermediate points we need to use the almost last point
                if (ConnectionPoints.Any())
                    startp = ConnectionPoints.Last();
                else
                    startp = Source == null ? _startPoint : Source.Bounds.Center();

                var r = new Point(0, 0);
                if (Target is DiagramShape shape)
                {
                    // the center (=TargetConnectorResult.AbsolutePosition) will not do in the right triangle case
                    // since the center does not sit inside the shape and doesn't give intersections
                    var endp = (shape.GlidingStyle == GlidingStyle.RightTriangle)
                        ? new Point(shape.Bounds.X + (shape.Bounds.Width / 5),
                            shape.Bounds.Y + (2 * shape.Bounds.Height / 3))
                        : TargetConnectorResult.AbsolutePosition;
                    r = shape.Bounds.FindIntersectionPoint(shape.RotationAngle, endp, startp,
                        shape.GlidingStyle);
                }
                else
                {
                    Utils.IntersectionPointOnRectangle(Target!.Bounds, startp,
                        TargetConnectorResult.AbsolutePosition, ref r);
                }

                return _endPoint = r;
            }

            _endPoint = TargetConnectorResult.AbsolutePosition;
            return _endPoint;
        }
        set
        {
            if (Target == null)
            {
                if (_endPoint != value)
                {
                    if (ConnectionType == ConnectionType.Bezier)
                        ConnectionPoints[1] = new Point(
                            ConnectionPoints[1].X + value.X - _endPoint.X,
                            ConnectionPoints[1].Y + value.Y - _endPoint.Y);
                    _endPoint = value;
                    SetPosition(Utils.GetTopLeftPoint(this.AllPoints()));
                    //this.OnPropertyChanged(DiagramPropertyName.EndPoint);
                }
            }

            //TODO: maybe repaint
        }
    }

    /// <summary>
    /// Gets or sets the editing point of the connection. This points indicates the position of the editing element.
    /// </summary>
    internal Point EditingPoint
    {
        get => _editingPoint;
        set
        {
            if (_editingPoint != value)
            {
                _editingPoint = value;
                //this.OnEditingPointPropertyChanged();
            }
        }
    }

    #endregion

    #region ----布局相关属性----

    [Browsable(false)]
    public Point Position
    {
        get => _position;
        set
        {
            if (_position != value)
                _position = value;
        }
    }

    [Browsable(false)]
    public override Rect Bounds
    {
        get
        {
            //TODO: 优化: Cache path if not changed && 如果是ConnectionType == Polyline
            using var path = GetPath(true);
            //TODO: var tightBounds = path.TightBounds;
            var pathBounds = path.Bounds;
            return Rect.FromLTWH(Position.X, Position.Y, pathBounds.Width, pathBounds.Height);
        }
        set { }
    }

    #endregion

    #region ----连接相关属性----

    /// <summary>
    /// Gets or sets a value indicating whether the default Bezier definition (Bezier tangents) has been altered.
    /// </summary>
    /// <remarks>
    ///   <list type="bullet">
    ///   <item> This property is set to <c>true</c> as soon as one of the Bezier handles has been manually modified.
    /// This means that the automatically calculated handle positions in the function of the connector will be halted.
    /// You re-enable this auto-handle calculation by re-setting this property to <c>false</c>.
    ///   </item>
    ///   <item>
    ///     Setting this property has no effect if the <see cref="ConnectionType" />
    ///     is not <see cref="ConnectionType.Bezier" />.</item>
    ///   </list>
    /// </remarks>
    public bool IsModified { get; set; }

    private IShape? _source;

    public IShape? Source
    {
        get => _source;
        set
        {
            if (_source != value)
            {
                var oldSource = _source;
                _source = value;
                OnSourceChanged(oldSource);
            }
        }
    }

    private IShape? _target;

    public IShape? Target
    {
        get => _target;
        set
        {
            if (_target != value)
            {
                var oldTarget = _target;
                _target = value;
                OnTargetChanged(oldTarget);
            }
        }
    }

    private string? _sourceConnectorPosition;

    /// <summary>
    /// Gets or sets the source connector position.
    /// </summary>
    /// <value>
    /// The source connector position.
    /// </value>
    public string? SourceConnectorPosition
    {
        get => _sourceConnectorPosition;
        set
        {
            if (_sourceConnectorPosition != value)
            {
                var oldValue = _sourceConnectorPosition;
                _sourceConnectorPosition = value;
                OnSourceConnectorPositionChanged(value, oldValue);
            }
        }
    }

    private string? _targetConnectorPosition;

    /// <summary>
    /// Gets or sets the target connector position.
    /// </summary>
    /// <value>
    /// The target connector position.
    /// </value>
    public string? TargetConnectorPosition
    {
        get => _targetConnectorPosition;
        set
        {
            if (_targetConnectorPosition != value)
            {
                var oldValue = _targetConnectorPosition;
                _targetConnectorPosition = value;
                OnTargetConnectorPositionChanged(value, oldValue);
            }
        }
    }

    /// <summary>
    /// Gets the source connector result.
    /// </summary>
    [Browsable(false)]
    public IConnector? SourceConnectorResult { get; private set; }

    /// <summary>
    /// Gets the target connector result.
    /// </summary>
    [Browsable(false)]
    public IConnector? TargetConnectorResult { get; private set; }

    /// <summary>
    /// Gets or sets whether this connection will be automatically routed. 
    /// </summary>
    /// <remarks>Setting this property effects only <see cref="ConnectionType.Spline"/> 
    /// and <see cref="ConnectionType.Polyline"/> connections.</remarks>
    public bool Route { get; set; }

    #endregion

    #region ----绘制样式相关属性----

    /// <summary>
    /// Gets or sets the type of the connection.
    /// </summary>
    /// <value>
    /// The type of the connection.
    /// </value>
    public ConnectionType ConnectionType { get; set; }

    /// <summary>
    /// Gets or sets the type of the source cap.
    /// </summary>
    /// <value>
    /// The type of the source cap.
    /// </value>
    [Browsable(true)]
    public CapType SourceCapType { get; set; }

    /// <summary>
    /// Gets or sets the type of the target cap.
    /// </summary>
    /// <value>
    /// The type of the target cap.
    /// </value>
    [Browsable(true)]
    public CapType TargetCapType { get; set; }

    /// <summary>
    /// Gets or sets the size of the source cap.
    /// </summary>
    /// <value>
    /// The size of the source cap.
    /// </value>
    public Size SourceCapSize { get; set; } = new Size(12, 12);

    /// <summary>
    /// Gets or sets the size of the target cap.
    /// </summary>
    /// <value>
    /// The size of the target cap.
    /// </value>
    public Size TargetCapSize { get; set; } = new Size(12, 12);

    /// <summary>
    /// Gets or sets the Bezier tension.
    /// </summary>
    /// <value>
    /// A value or zero turns the Bezier connection into a straight line,
    /// a value of one and above increase the sharpness of the Bezier curve.
    /// </value>
    [Browsable(false)]
    public float BezierTension { get; set; } = 0.5f;

    [Browsable(false)] public Color BackColor { get; set; } = Colors.DarkGray;

    [Browsable(false)] public Color ForeColor { get; set; } = Colors.Black;

    #endregion

    #endregion

    #region ====Property Changed====

    protected virtual void OnSourceChanged(IShape? oldSource)
    {
        if (!_isSuppressingConnectionUpdate)
            ResolveSourceConnector();
        //if (Source != null && Source.ParentContainer != null)
        //    EnsureZIndex(Source.ParentContainer);

        //this.OnPropertyChanged(nameof(Source));
    }

    protected virtual void OnTargetChanged(IShape? oldTarget)
    {
        if (!_isSuppressingConnectionUpdate)
            ResolveTargetConnector();
        //if (Target != null && Target.ParentContainer != null)
        //    EnsureZIndex(Target.ParentContainer);

        //this.OnPropertyChanged(nameof(Target));
    }

    protected virtual void OnSourceConnectorPositionChanged(string? newPosition, string? oldPosition)
    {
        ResolveSourceConnector();
        Update();
    }

    protected virtual void OnTargetConnectorPositionChanged(string? newPosition, string? oldPosition)
    {
        ResolveTargetConnector();
        Update();
    }

    #endregion

    #region ====连接路径计算方法====

    /// <summary>
    /// Attaches the connection to specific source and target.
    /// </summary>
    public void Attach(IConnector? source = null, IConnector? target = null)
    {
        _isSuppressingConnectionUpdate = true;

        if (source != null)
        {
            Source = source.Shape;
            if (SourceConnectorPosition != source.Name)
                SourceConnectorPosition = source.Name;
            else
                ResolveSourceConnector();
        }

        if (target != null)
        {
            Target = target.Shape;
            if (TargetConnectorPosition != target.Name)
                TargetConnectorPosition = target.Name;
            else
                ResolveTargetConnector();
        }

        /* if both source and target belong to the same top group the new connection should be part of the (supremum aka first common parent) group so that
         * intermediate points are dragged along. Same for the container.
         */
        //if (this.Source != null && this.Target != null)
        //{
        //    var commonGroup = this.Source.GetSupremumGroup(this.Target);

        //    // the common group can be null and should be set to null in case the parent was not null before but it now due to a re-attachment
        //    this.ParentGroup = commonGroup;

        //    var commonContainer = this.Source.GetSupremumContainer(this.Target) as IContainerShape;
        //    var currentContainer = DiagramContainerShape.GetParentContainer(this) as IContainerShape;

        //    // we can only update the Items, not the ItemsSource since the binding is one-way only on this level (for now)
        //    if (commonContainer != null)
        //    {
        //        if (commonContainer != currentContainer)
        //            commonContainer.AddItem(this.Diagram.GetItemFromContainer(this));
        //    }
        //    else
        //    {
        //        if (currentContainer != null)
        //            currentContainer.RemoveItem(this.Diagram.GetItemFromContainer(this));
        //    }
        //}

        _isSuppressingConnectionUpdate = false;
        Update();
    }

    /// <summary>
    /// Will attempt to resolve the source connector on the basis of the set <see cref="SourceConnectorPosition"/>.
    /// </summary>
    private void ResolveSourceConnector()
    {
        if (Source != null)
        {
            if (Source.Connectors.Count == 0)
                throw new InvalidOperationException("Shape Has No Connectors");
            if (SourceConnectorPosition != ConnectorPosition.Auto)
            {
                SourceConnectorResult = Source.Connectors[SourceConnectorPosition!];
                if (SourceConnectorResult == null)
                    throw new InvalidOperationException($"Connector Doesnt Exist: {SourceConnectorPosition}");
            }
        }
        else
            SourceConnectorResult = null;
    }

    /// <summary>
    /// Will attempt to resolve the target connector on the basis of the set <see cref="TargetConnectorPosition"/>.
    /// </summary>
    private void ResolveTargetConnector()
    {
        if (Target != null)
        {
            if (Target.Connectors.Count == 0)
                throw new InvalidOperationException("Shape Has No Connectors");
            if (TargetConnectorPosition != ConnectorPosition.Auto)
            {
                TargetConnectorResult = Target.Connectors[TargetConnectorPosition];
                if (TargetConnectorResult == null)
                    throw new InvalidOperationException($"Connector Doesnt Exist: {TargetConnectorResult}");
            }
        }
        else
            TargetConnectorResult = null;
    }

    /// <summary>
    /// Calculates the connector to which this connection is attached if the <see cref="ConnectorPosition.Auto"/> connector is used.
    /// </summary>
    private void CalculateAutoConnectors(bool calculateStart = true, bool calculateEnd = true)
    {
        if (calculateStart && Source != null && SourceConnectorPosition == ConnectorPosition.Auto)
        {
            // if there are intermediate points, we need to use the almost last point,
            // except for Bezier where the handles are actually defined through the connector's position
            Point target;
            if (ConnectionPoints.Any() && ConnectionType != ConnectionType.Bezier)
                target = ConnectionPoints[0];
            else
                target = Target == null ? _endPoint : Target.Bounds.Center();

            SourceConnectorResult = ShapeUtilities.GetNearestConnector(Source, target) ??
                                    Source.Connectors[ConnectorPosition.Auto];
        }

        if (calculateEnd && Target != null && TargetConnectorPosition == ConnectorPosition.Auto)
        {
            Point source;
            if (ConnectionPoints.Any() && ConnectionType != ConnectionType.Bezier)
                source = ConnectionPoints.Last();
            else
                source = Source == null ? _endPoint : Source.Bounds.Center();

            TargetConnectorResult = ShapeUtilities.GetNearestConnector(Target, source) ??
                                    Target.Connectors[ConnectorPosition.Auto];
        }
    }

    public void Update(bool isManipulating = false)
    {
        //先清空adorner的缓存
        var adorner = SelectionAdorner as ConnectionSelectionAdorner;
        adorner?.ResetCache();

        if (_isSuppressingConnectionUpdate)
            return;
        //if (GetIsAutoUpdateSuppressed(this))
        //    return;
        var updatedPending = isManipulating &&
                             Source is { IsSelected: true } &&
                             Target is { IsSelected: true } &&
                             Source != Target;

        if (_isDirty || !updatedPending)
        {
            // cache start and end points:
            if (!updatedPending)
            {
                CalculateAutoConnectors();
                UpdateAutoBezierHandles();
            }

            UpdateGeometryOverride();
            //if (this.IsSelectedInGroup)
            //    this.UpdateSelectedGeometryOverride();
            //this.UpdateDeferredGeometryOverride(null);

            if (!string.IsNullOrWhiteSpace(Title))
                CalculateEditingPoint();
            _isDirty = false;
        }
        else if (updatedPending)
        {
            _isDirty = true;
        }

        if (Parent != null) //TODO:更有效率的重绘
        {
            //this.InvalidateMeasure();
            Parent.Invalidate();
        }
        else
        {
            Surface?.Repaint();
        }

        _isDirty = true;
    }

    /// <summary>
    /// Updates the Bezier handles according to the position of the endpoints.
    /// </summary>
    private void UpdateAutoBezierHandles()
    {
        // auto-update only works if the user hasn't altered the handles
        if (ConnectionType != ConnectionType.Bezier || IsModified)
            return;

        var startHandle = CalculateHandlerOffset(SourceConnectorResult, true);
        if (ConnectionPoints.Count > 0)
            ConnectionPoints[0] = startHandle;
        else
            ConnectionPoints.Add(startHandle);

        var endHandle = CalculateHandlerOffset(TargetConnectorResult, false);
        if (ConnectionPoints.Count > 1)
            ConnectionPoints[1] = endHandle;
        else
            ConnectionPoints.Add(endHandle);
    }

    private Point CalculateHandlerOffset(IConnector? connector, bool isStartHandler)
    {
        var relativePoint = isStartHandler ? StartPoint : EndPoint;
        Point offset;
        if (connector != null && connector.Shape != null)
        {
            offset = GetBezierAutoHandleOffset(connector.Shape, connector.Name);
        }
        else
        {
            var delta = isStartHandler ? DiagramConstants.BezierAutoOffset : -DiagramConstants.BezierAutoOffset;
            offset = StartPoint.X < EndPoint.X ? new Point(delta, 0) : new Point(-delta, 0);
        }

        return new Point(relativePoint.X + offset.X, relativePoint.Y + offset.Y);
    }

    /// <summary>
    /// Gets the Bezier handle offset in function of the connector's position.
    /// </summary>
    /// <remarks>
    /// <item> The default offset is defined in the <see cref="DiagramConstants.BezierAutoOffset"/>.</item> 
    /// <item> This method accepts the predefined <see cref="ConnectorPosition"/> enumeration but will also 'guess' anything containing the words 'left', 'up' and so on. </item>
    /// </remarks>
    /// <param name="shape">The shape to which the connection is attached.</param>
    /// <param name="connectorName">Name of the connector.</param>
    /// <returns></returns>
    private static Point GetBezierAutoHandleOffset(IShape shape, string connectorName)
    {
        var offset = new Point();
        if (string.IsNullOrEmpty(connectorName))
            return offset;

        // we're creating some loose interpretation here in case a custom connector has words referring to a position
        var lowerConnector = connectorName.ToLower(CultureInfo.InvariantCulture);
        if (lowerConnector.Contains("left"))
            connectorName = ConnectorPosition.Left;
        if (lowerConnector.Contains("right"))
            connectorName = ConnectorPosition.Right;
        if (lowerConnector.Contains("up") || lowerConnector.Contains("top"))
            connectorName = ConnectorPosition.Top;
        if (lowerConnector.Contains("bottom") || lowerConnector.Contains("down"))
            connectorName = ConnectorPosition.Bottom;
        switch (connectorName)
        {
            case ConnectorPosition.Auto:
                // the resulting connector position should never end up here
                break;
            case ConnectorPosition.Bottom:
                offset.Y = DiagramConstants.BezierAutoOffset;
                break;
            case ConnectorPosition.Top:
                offset.Y = -DiagramConstants.BezierAutoOffset;
                break;
            case ConnectorPosition.Right:
                offset.X = DiagramConstants.BezierAutoOffset;
                break;
            case ConnectorPosition.Left:
                offset.X = -DiagramConstants.BezierAutoOffset;
                break;
            case ConnectorPosition.Gliding:
                break;
        }

        // the offset vector is relative to the shape hence the zero-pivot
        if (Math.Abs(shape.RotationAngle) > Utils.Epsilon)
            offset = offset.Rotate(new Point(0, 0), shape.RotationAngle);
        return offset;
    }

    /// <summary>
    /// When overridden, provides the geometry for the connection.
    /// </summary>
    /// <remarks>This is called whenever the connection is redrawn.</remarks>
    private void UpdateGeometryOverride()
    {
        // this method needs to work even if the Diagram is null, e.g. displaying the cap types in combo
        if (Surface == null)
        {
            SetPosition(Utils.GetTopLeftPoint(this.AllPoints()));
            Geometry = CreateGeometry(BridgeType.None, false);
            //    if (this.geometryPath != null)
            //        this.geometryPath.Data = this.Geometry;
        }
        else
        {
            //if (this.ServiceLocator.ManipulationPointService.IsManipulating ||
            //    (this.ParentContainer != null && this.ParentContainer.IsCollapsed))
            //    return;

            if (Route && Surface.RoutingService != null)
            {
                ConnectionRoute route = Surface.RoutingService.FindExtendedRoute(this);

                // Note that the routing only returns the intermediate points(ConnectionPoints), not the start/end points.
                ConnectionPoints.Clear();
                route.Points.ForEach(point => ConnectionPoints.Add(point));

                bool calculateStart = true, calculateEnd = true;
                if (route.StartConnector != null)
                {
                    SourceConnectorResult = route.StartConnector;
                    calculateStart = false;
                }

                if (route.EndConnector != null)
                {
                    TargetConnectorResult = route.EndConnector;
                    calculateEnd = false;
                }

                CalculateAutoConnectors(calculateStart, calculateEnd);
                //this.UpdateAutoBezierHandles();

                // the route can go wildly off the rectangle defined by the start/end points
                route.Points.Add(StartPoint);
                route.Points.Add(EndPoint);
                SetPosition(Utils.GetTopLeftPoint(route.Points));
            }
            else
            {
                SetPosition(Utils.GetTopLeftPoint(this.AllPoints()));
            }

            var connectionBridge = Surface?.ConnectionBridge ?? BridgeType.None;
            var connectionRoundedCorners = Surface?.ConnectionRoundedCorners ?? false;
            Geometry = CreateGeometry(connectionBridge, connectionRoundedCorners);

            //if (this.geometryPath != null)
            //    this.geometryPath.Data = this.Geometry;

            // if the crossing geometry of upper/lower connections depends on the geometry of
            // this connection we need to redraw these connections.
            // This is in effect a bubbling up or down of the changes.

            //if (this.Diagram.ConnectionBridge != BridgeType.None)
            //{
            //    var collectUpperOnes = this.Diagram.ConnectionBridge == BridgeType.Gap;
            //    var toredraw = this.FetchZConnections(collectUpperOnes);
            //    toredraw.ForEach(c => c.Update());
            //}
            //}
        }
    }

    internal void UpdateGeometryPoint(int index, float offsetX, float offsetY)
    {
        //暂简单更新
        if (index == 0)
        {
            StartPoint = new Point(StartPoint.X + offsetX, StartPoint.Y + offsetY);
        }
        else if (index == ConnectionPoints.Count() + 1)
        {
            EndPoint = new Point(EndPoint.X + offsetX, EndPoint.Y + offsetY);
        }
        else
        {
            var pt = ConnectionPoints[index - 1];
            ConnectionPoints[index - 1] = new Point(pt.X + offsetX, pt.Y + offsetY);
        }

        IsModified = true;
        Update();
    }

    //internal void UpdateDeferredGeometry(Point start, Point end, Point[] points)
    //{
    //}

    /// <summary>
    /// Returns the information related to the crossing of this connection with other connections.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// The positions in the returned data are local coordinates with respect to the bounding rectangle of this connection. 
    /// </item> 
    /// <item>The returned data contains a collection of points for each segment. This collection starts with the startpoint of the segment and ends with the endpoint of the segment. If there is a gap or crossing then for each gap a pair of points will
    /// denote the begin/end of the gap. So, each segment collection has always at least two points.</item>
    /// </list>
    /// </remarks>
    public CrossingsData GetCrossings()
    {
        throw new NotImplementedException();
        //var gapRadius = DiagramConstants.CrossingRadius;
        //var gapRadiusSquared = gapRadius * gapRadius;
        //var intersections = new List<Point>();
        //var intersection = new Point();

        //// this includes the endpoints and the intermediate points
        //var points = this.AllPoints();

        //// the resulting crossing data is a collection of points, one for each segment.
        //var crossingsData = new CrossingsData(this.SegmentCount);

        //var connectionBounds = this.GetActualBounds();
        //if (this.Diagram == null)
        //    return crossingsData;
        //var collectLowerOnes = this.Diagram.ConnectionBridge == BridgeType.Bow;

        //// the bow will run over the underlying cons, while the gap will stop for the upper cons. Hence the switch.
        //var connections = this.FetchZConnections(collectLowerOnes);
        //if (!connections.Any())
        //{
        //    for (var k = 0; k < this.SegmentCount(); k++)
        //    {
        //        var segmentStartPoint = points[k];
        //        var segmentEndPoint = points[k + 1];
        //        crossingsData.SegmentCrossings[k].Add(segmentStartPoint);
        //        crossingsData.SegmentCrossings[k].Add(segmentEndPoint);
        //    }
        //}
        //else
        //{
        //    // IMPORTANT: all points in the loop are in global coordinates, they are shifted afterwards to local coordinates
        //    for (var segmentIndex = 0; segmentIndex < SegmentCount; segmentIndex++)
        //    {
        //        // note that a segment can be both a gap or a line
        //        var segmentStartPoint = points[segmentIndex];
        //        var segmentEndPoint = points[segmentIndex + 1];
        //        if (segmentStartPoint == segmentEndPoint)
        //            continue;
        //        foreach (var connection in connections)
        //        {
        //            if (connection.Visibility == ElementVisibility.Visible && connection.ConnectionType == ConnectionType.Polyline)
        //            {
        //                var bounds = connection.GetActualBounds();
        //                if (bounds.IntersectsWith(connectionBounds))
        //                {
        //                    var pts = connection.AllPoints();
        //                    for (var k = 0; k < connection.SegmentCount(); k++)
        //                    {
        //                        var otherSegmentStartPoint = pts[k];
        //                        var otherSegmentEndPoint = pts[k + 1];
        //                        if (otherSegmentStartPoint == otherSegmentEndPoint)
        //                            continue;
        //                        if (Utils.SegmentIntersect(
        //                            segmentStartPoint, segmentEndPoint, otherSegmentStartPoint, otherSegmentEndPoint, ref intersection) &&
        //                            Utils.DistanceSquared(segmentStartPoint, intersection) > gapRadiusSquared &&
        //                            Utils.DistanceSquared(segmentEndPoint, intersection) > gapRadiusSquared &&
        //                            Utils.DistanceSquared(otherSegmentStartPoint, intersection) > gapRadiusSquared &&
        //                            Utils.DistanceSquared(otherSegmentEndPoint, intersection) > gapRadiusSquared)
        //                            intersections.Add(intersection);
        //                    }
        //                }
        //            }
        //        }
        //        var comparer = new DistanceToPointComparer(segmentStartPoint);

        //        // reorder the points from the start to the end point
        //        intersections.Sort(comparer);

        //        var currentSegmentCrossings = crossingsData.SegmentCrossings[segmentIndex];

        //        // the first point is the start point of the segment
        //        currentSegmentCrossings.Add(segmentStartPoint);
        //        foreach (var p in intersections)
        //        {
        //            var unit = segmentEndPoint.Delta(segmentStartPoint);
        //            unit.Normalize();

        //            // the vector at the crossing with a length equal to the gap radius
        //            var rho = new Vector(unit.X * gapRadius, unit.Y * gapRadius);

        //            // the start of the gap
        //            currentSegmentCrossings.Add(new Point(p.X - rho.X, p.Y - rho.Y));

        //            // the end of the gap
        //            currentSegmentCrossings.Add(new Point(p.X + rho.X, p.Y + rho.Y));
        //        }

        //        // the last points is of cours the end point of the segment
        //        currentSegmentCrossings.Add(segmentEndPoint);

        //        // Check if there are intersection that overlap
        //        for (var i = 1; i < currentSegmentCrossings.Count - 2; i++)
        //        {
        //            var p1 = currentSegmentCrossings[i];
        //            var p2 = currentSegmentCrossings[i + 1];

        //            if (comparer.Compare(p1, p2) > 0)//|| p1.Distance(p2) < DiagramConstants.CrossingRadius)
        //            {
        //                // Remove these points
        //                currentSegmentCrossings.RemoveAt(i);
        //                currentSegmentCrossings.RemoveAt(i);
        //                i--;
        //            }
        //        }

        //        // move onwards to the next segment
        //        intersections.Clear();
        //        intersection = new Point();
        //    }
        //}

        //// shift to local coordinates
        //var delta = new Vector(this.Position.X, this.Position.Y);
        //foreach (var crossings in crossingsData.SegmentCrossings)
        //{
        //    for (var index = 0; index < crossings.Count; index++)
        //    {
        //        var p = crossings[index];
        //        crossings[index] = new Point(p.X - delta.X, p.Y - delta.Y);
        //    }
        //}
        //return crossingsData;
    }

    /// <summary>
    /// Creates the connection's geometry.
    /// </summary>
    private Geometry CreateGeometry(BridgeType bridgeType, bool roundedCorners)
    {
        var sourcePoint = StartPoint.Substract(Position);
        var targetPoint = EndPoint.Substract(Position);
        var transformedPoints = this.TranslateConnectionPoints(false).ToList();

        Point sourceCapSecondPoint;
        Point targetCapSecondPoint;
        if (ConnectionType == ConnectionType.Spline)
        {
            // we need to know the current tangent
            var points = new List<Point> { sourcePoint };
            points.AddRange(transformedPoints);
            points.Add(targetPoint);
            GeometryExtensions.GetSplineFigureTangents(points, out sourceCapSecondPoint, out targetCapSecondPoint);
        }
        else
        {
            sourceCapSecondPoint = transformedPoints.Count == 0 ? targetPoint : transformedPoints[0];
            targetCapSecondPoint = transformedPoints.Count == 0
                ? sourcePoint
                : transformedPoints[transformedPoints.Count - 1];
        }

        var newSource = sourcePoint;
        var newEnd = targetPoint;

        _sourceCap = CreateSourceCapGeometry(sourcePoint, sourceCapSecondPoint, ref newSource);
        _targetCap = CreateTargetCapGeometry(targetPoint, targetCapSecondPoint, ref newEnd);

        var baseGeometry = GeometryExtensions.CreateBaseLineGeometry(new LineSpecification()
        {
            StartPoint = newSource,
            EndPoint = newEnd,
            Points = transformedPoints,
            ConnectionType = ConnectionType,
            BezierTension = BezierTension,
            BridgeType = bridgeType,
            Crossings = bridgeType != BridgeType.None ? GetCrossings() : null,
            Bounds = Rect.FromLTWH(Position.X, Position.Y, 100, 100), //TODO:***** fix thi
            RoundedCorners = roundedCorners
        });

        return baseGeometry;
    }

    /// <summary>
    /// Creates the source cap geometry.
    /// </summary>
    /// <param name="startPt">The start point.</param>
    /// <param name="endPt">The end point.</param>
    /// <param name="baseLineStart">The new start of the baseline.</param>
    private PathFigure CreateSourceCapGeometry(Point startPt, Point endPt, ref Point baseLineStart)
    {
        return GeometryExtensions.CreateConnectionCapFigure(startPt, endPt
            , SourceCapType, SourceCapSize.Width
            , SourceCapSize.Height, ref baseLineStart);
    }

    /// <summary>
    /// Creates the target cap geometry.
    /// </summary>
    /// <param name="startPt">The start point.</param>
    /// <param name="endPt">The end point.</param>
    /// <param name="baseLineEnd">The new end of the baseline.</param>
    private PathFigure CreateTargetCapGeometry(Point startPt, Point endPt, ref Point baseLineEnd)
    {
        return GeometryExtensions.CreateConnectionCapFigure(startPt, endPt
            , TargetCapType, TargetCapSize.Width
            , TargetCapSize.Height, ref baseLineEnd);
    }

    /// <summary>
    /// Calculates the point where the editing of the label occurs.
    /// </summary>
    private void CalculateEditingPoint()
    {
        // the endpoints in local connection coordinates
        var endPoints = this.GetConnectionEndPoints(true);

        // the intermediate points in local coordinates
        var points = new List<Point>();
        if (ConnectionType == ConnectionType.Bezier)
        {
            throw new NotImplementedException();
            //var bezierCurve = Utils.ApproximateBezierCurve(new[] { this.StartPoint, Utils.Lerp(this.StartPoint, this.ConnectionPoints[0], this.BezierTension), Utils.Lerp(this.EndPoint, this.ConnectionPoints[1], this.BezierTension), this.EndPoint }, 10);
            //points = bezierCurve.Select(x => x = x.Substract(this.Bounds.TopLeft())).ToList();
        }
        else
        {
            points = ConnectionPoints.Select(x => x = x.Substract(Bounds.TopLeft())).ToList();
        }

        // the middlepoint in local coordinates; note that the Canvas wherein the label sits will move with the Bounds of this connection
        var defaultEditingPoint = ConnectionUtilities.CalculateMiddlePointOfLine(endPoints, points);

        defaultEditingPoint = new Point(defaultEditingPoint.X - CachedTextSize.Width / 2f,
            defaultEditingPoint.Y -
            CachedTextSize.Height * 2f /
            2f); //*2 because we want to text appear on the top on the horizontal connection

        //switch (this.HorizontalContentAlignment)
        //{
        //    case HorizontalAlignment.Center:
        //        defaultEditingPoint.X = defaultEditingPoint.X;
        //        break;
        //    case HorizontalAlignment.Left:
        //        defaultEditingPoint.X = 0;
        //        break;
        //    case HorizontalAlignment.Right:
        //        defaultEditingPoint.X = this.Bounds.Width;
        //        break;
        //}

        //switch (this.VerticalContentAlignment)
        //{
        //    case VerticalAlignment.Bottom:
        //        defaultEditingPoint.Y = this.Bounds.Height;
        //        break;
        //    case VerticalAlignment.Center:
        //        defaultEditingPoint.Y = defaultEditingPoint.Y;
        //        break;
        //    case VerticalAlignment.Top:
        //        defaultEditingPoint.Y = 0;
        //        break;
        //}

        EditingPoint = defaultEditingPoint.Add( /*this.Bounds.TopLeft()*/Position.Substract(Position));
    }

    private void SetPosition(Point newPosition)
    {
        _isInnerChange = true;
        Position = newPosition;
        _isInnerChange = false;

        //Console.WriteLine("SetPosition to: {0}, bounds.Location:D {1}", newPosition, this.Bounds.Location);
    }

    #endregion

    #region ====Overrides Methods====

    protected internal override void Invalidate()
    {
        Surface?.Repaint();
    }

    protected internal override ISelectionAdorner GetSelectionAdorner(DesignAdorners adorners) =>
        SelectionAdorner ??= new ConnectionSelectionAdorner(adorners, this);

    protected internal override void OnAddToSurface()
    {
        base.OnAddToSurface();

        Update();
    }

    protected override void SetBounds(float x, float y, float width, float height, BoundsSpecified specified) { }

    public override void Paint(Canvas canvas)
    {
        if (_isDirty)
        {
            Update();
            _isDirty = false;
        }

        DrawConnectionLine(canvas);
        DrawConnectionCap(canvas, _targetCap);
        DrawConnectionCap(canvas, _sourceCap);
        //DrawDeferredConnectionLine(canvas);
        DrawConnectionText(canvas);
    }

    #endregion

    #region ====Paint Methods====

    private void DrawConnectionLine(Canvas canvas)
    {
        using var path = GetPath();

        if (StrokeDashArray is { Length: > 0 })
            canvas.DrawPathDashed(ForeColor, StrokeThickness, StrokeDashArray, path);
        else
            canvas.DrawPath(ForeColor, StrokeThickness, path);

        //canvas.DrawRectangle(Colors.Red, 1f, Rect.FromLTWH(0, 0, Bounds.Width, Bounds.Height));
    }

    private void DrawConnectionCap(Canvas canvas, PathFigure? capFigure)
    {
        if (capFigure == null)
            return;

        var path = GetPathFromCap(capFigure);
        if (capFigure.IsClosed)
            path.Close();

        if (StrokeDashArray is { Length: > 0 })
            canvas.DrawPathDashed(ForeColor, StrokeThickness, StrokeDashArray, path);
        else
            canvas.DrawPath(ForeColor, StrokeThickness, path);

        if (capFigure.IsFilled)
            canvas.FillPath(BackColor, path);
    }

    private Path GetPath(bool transforms = false)
    {
        if (Geometry is PathGeometry geo)
            return GetPathCore(geo, transforms);

        return new Path();
    }

    private Path GetPathCore(PathGeometry? geometry, bool transforms = false)
    {
        if (geometry == null) return new Path();

        var segment = geometry.Figures[0].Segments[0];
        if (segment is LineSegment lineSegment)
        {
            var path = new Path();
            var lastFigure = geometry.Figures.Last();

            foreach (var figure in geometry.Figures)
            {
                var startPoint = figure.StartPoint;

                if (Surface != null && Surface.ConnectionBridge == BridgeType.Bow)
                {
                    throw new NotImplementedException();
                    //return GetBowPath(figure, transforms, path, startPoint);
                }

                var points = new List<Point>();
                foreach (PathSegment seg in figure.Segments)
                {
                    if (seg is LineSegment lineSeg) points.Add(lineSeg.Point);

                    if (seg is BezierSegment bSeg)
                    {
                        using var arcPath = new Path();
                        arcPath.AddBezier(startPoint, bSeg.Point1, bSeg.Point2, bSeg.Point3);
                        arcPath.Points.ForEach(p => points.Add(new Point(p.X, p.Y)));
                    }
                }

                Point[] arrayPoints = new Point[points.Count + 1];
                arrayPoints[0] = figure.StartPoint;

                for (int i = 0; i < points.Count; ++i)
                    arrayPoints[i + 1] = points[i];

                if (transforms)
                {
                    //TODO:***** throw new NotImplementedException();
                    //this.TotalTransform.TransformPoints(arrayPoints);
                }

                if (arrayPoints.Length > 1) path.AddLines(arrayPoints);

                if (figure != lastFigure) path.Close();
            }

            return path;
        }

        if (segment is PolyLineSegment polyLineSegment)
        {
            var points = polyLineSegment.Points;

            var path = new Path();

            Point[] arrayPoints = new Point[points.Count + 1];
            arrayPoints[0] = geometry.Figures[0].StartPoint;

            for (int i = 0; i < points.Count; ++i)
                arrayPoints[i + 1] = points[i];

            if (transforms)
            {
                //TODO:**** throw new NotImplementedException();
                //this.TotalTransform.TransformPoints(arrayPoints);
            }

            if (arrayPoints.Length > 1)
                path.AddLines(arrayPoints);

            return path;
        }

        if (segment is BezierSegment bezierSegment)
        {
            var path = new Path();
            var arrayPoints = new Point[4];
            arrayPoints[0] = geometry.Figures[0].StartPoint;
            arrayPoints[1] = new Point(bezierSegment.Point1.X, bezierSegment.Point1.Y);
            arrayPoints[2] = new Point(bezierSegment.Point2.X, bezierSegment.Point2.Y);
            arrayPoints[3] = new Point(bezierSegment.Point3.X, bezierSegment.Point3.Y);

            if (transforms)
            {
                //TODO:***
                //this.TotalTransform.TransformPoints(arrayPoints);
            }

            if (arrayPoints.Length > 1)
                path.AddBezier(arrayPoints[0], arrayPoints[1], arrayPoints[2], arrayPoints[3]);

            return path;
        }

        return new Path();
    }

    private static Path GetPathFromCap(PathFigure figure)
    {
        var path = new Path();
        var start = new Point(figure.StartPoint.X, figure.StartPoint.Y);
        foreach (var segment in figure.Segments)
        {
            if (segment is LineSegment lineSegment)
            {
                //var pathFromLine = GetPathFromLineSegment(lineSegment, false, figure, ref start);
                //path.AddPath(pathFromLine, true);
                GetPathFromLineSegment(path, lineSegment, false, figure, ref start);
                continue;
            }

            if (segment is ArcSegment arcSegment)
            {
                //var pathFromArc = GetPathFromArcSegment(arcSegment, start);
                //path.AddPath(pathFromArc, true);
                GetPathFromArcSegment(path, arcSegment, start);
                break;
            }
        }

        return path;
    }

    private static void GetPathFromLineSegment(Path path, LineSegment lineSegment, bool transforms,
        PathFigure figure, ref Point start)
    {
        Point[] points = new Point[2];
        points[0] = start;
        points[1] = new Point(lineSegment.Point.X, lineSegment.Point.Y);

        if (transforms)
        {
            throw new NotImplementedException();
            //this.TotalTransform.TransformPoints(points);
        }

        path.AddLine(points[0], points[1]);
        start = points[1];
    }

    private static void GetPathFromArcSegment(Path path, ArcSegment arcSegment, Point startPoint)
    {
        var endPoint = arcSegment.Point;
        var dist = (float)Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) +
                                    Math.Pow(startPoint.Y - endPoint.Y, 2));
        var size = new Size(dist, dist);
        var location = new Point((endPoint.X + startPoint.X - size.Width) / 2f,
            (endPoint.Y + startPoint.Y - size.Height) / 2f);
        path.AddEllipse(Rect.FromLTWH(location.X, location.Y, dist, dist));
    }

    private void DrawConnectionText(Canvas canvas)
    {
        if (string.IsNullOrEmpty(Title))
            return;
        if ( /*this.IsInEditMode ||*/ string.IsNullOrWhiteSpace(Title))
            return;

        var size = CachedTextSize;
        var rect = Rect.FromLTWH(_editingPoint.X + 0.5f, _editingPoint.Y + 1f, size.Width, size.Height);
        canvas.FillRoundRectangle(Colors.White, rect, 2, 2);
        canvas.DrawParagraph(CachedLayout!, _editingPoint.X, _editingPoint.Y);
    }

    #endregion
}