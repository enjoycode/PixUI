namespace PixUI.Diagram;

public class DiagramShape : DiagramItem, IShape
{
    public DiagramShape()
    {
        //this.Connectors.CollectionChanged += this.ConnectorsCollectionChanged;
        EnsureDefaultConnectors();
    }

    private Rect _bounds = Rect.Empty;
    private GlidingStyle _glidingStyle = GlidingStyle.Rectangle;

    public Point Position
    {
        get => _bounds.Location;
        set => SetBounds(value.X, value.Y, _bounds.Width, _bounds.Height, BoundsSpecified.Location);
    }

    public override Rect Bounds
    {
        get => _bounds;
        set => SetBounds(value.X, value.Y, value.Width, value.Height, BoundsSpecified.All);
    }

    /// <summary>
    /// Gets or sets the style of outline the gliding connection should follow.
    /// </summary>
    /// <remarks>This property has only an effect if the <see cref="UseGlidingConnector"/> is set to <c>true</c>.</remarks>
    /// <seealso cref="ConnectorPosition"/>
    /// <seealso cref="UseGlidingConnector"/>
    public GlidingStyle GlidingStyle
    {
        get => _glidingStyle;
        set
        {
            if (_glidingStyle != value)
            {
                _glidingStyle = value;
                OnGlidingStyleChanged();
            }
        }
    }

    #region ====Property Changed====

    private void OnGlidingStyleChanged()
    {
        EnsureGlidingConnector();
    }

    #endregion

    #region ====Overrides Methods====

    protected override void SetBounds(float x, float y, float width, float height, BoundsSpecified specified)
    {
        var oldBounds = Bounds;

        _bounds.X = x;
        _bounds.Y = y;
        _bounds.Width = width;
        _bounds.Height = height;

        //通知Canvas刷新相关区域
        InvalidateOnBoundsChanged(oldBounds);

        //位置更新时，刷新相应的连接线
        if (Surface != null)
        {
            var connections = Surface.GetConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].Source == this || connections[i].Target == this)
                    connections[i].Update(false);
            }
        }
    }

    protected internal override void Paint(Canvas g)
    {
        //TODO: 测试代码
        var clientRectangle = Rect.FromLTWH(0, 0, _bounds.Width, _bounds.Height);
        g.FillRectangle(Colors.Silver, clientRectangle);
        g.DrawRectangle(Colors.DarkGray, 1.5f, clientRectangle);
    }

    #endregion

    #region ====IShape Implements====

    private float _rotationAngleCache;

    /// <summary>
    /// Gets or sets the rotation angle.
    /// </summary>
    /// <value>
    /// The rotation angle.
    /// </value>
    public float RotationAngle
    {
        get => _rotationAngleCache;
        set
        {
            if (Math.Abs(_rotationAngleCache - value) > Utils.Epsilon)
                _rotationAngleCache = value;
        }
    }

    /// <summary>
    /// Gets the connectors of this shape.
    /// </summary>
    public ConnectorCollection Connectors { get; } = new();

    public virtual bool CanConnect(bool isStartPoint, IConnection connection) => true;

    public IEnumerable<IConnection> IncomingLinks => Surface == null
        ? Enumerable.Empty<IConnection>()
        : Surface.GetIncomingConnectionsForShape(this);

    public IEnumerable<IConnection> OutgoingLinks => Surface == null
        ? Enumerable.Empty<IConnection>()
        : Surface.GetOutgoingConnectionsForShape(this);

    #endregion

    #region ====Connectors====

    /// <summary>
    /// Gets or sets whether the default connectors should be defined on the shape.
    /// </summary>
    public bool UseDefaultConnectors { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the gliding connector should be defined on the shape.
    /// </summary>
    public bool UseGlidingConnector { get; set; }

    /// <summary>
    /// Ensures that the known/predefined/default connectors are added to the shape.
    /// </summary>
    private void EnsureDefaultConnectors()
    {
        if (UseDefaultConnectors)
        {
            //todo: fix
            //if (this.GlidingConnectorHasConnections()) throw new InvalidOperationException("Cannot Remove Gliding Connector");

            EnsureConnector(ConnectorPosition.Auto);
            EnsureConnector(ConnectorPosition.Left);
            EnsureConnector(ConnectorPosition.Top);
            EnsureConnector(ConnectorPosition.Right);
            EnsureConnector(ConnectorPosition.Bottom);

            // the usage of the default connectors is mutually exclusive with the gliding connector
            if (Connectors.Contains(ConnectorPosition.Gliding))
            {
                Connectors.Remove(Connectors[ConnectorPosition.Gliding]!);
                UseGlidingConnector = false;
            }
        }
        else
        {
            //todo: fix
            //if (this.DefaultConnectorsHaveConnections()) throw new InvalidOperationException("Cannot Remove Default Connector");

            // custom or gliding connector remain valid
            var defaultConnectors = Connectors
                .Where(c => c.Name != ConnectorPosition.Gliding && !c.IsCustom())
                .ToArray();
            defaultConnectors.ForEach(c => Connectors.Remove(c));
        }
    }

    /// <summary>
    /// Ensures that the gliding connector is added to the shape.
    /// </summary>
    private void EnsureGlidingConnector()
    {
        if (UseGlidingConnector)
        {
            // attempting to move the connections to the gliding one
            EnsureConnector(ConnectorPosition.Gliding);
            MoveConnectionEndPointsToGlidingConnector();

            // can safely remove them since checked above that no connections are present
            var defaultConnectors = Connectors.Where(c => c.Name != ConnectorPosition.Gliding && !c.IsCustom())
                .ToArray();
            defaultConnectors.ForEach(c => Connectors.Remove(c));
            UseDefaultConnectors = false;
        }
        else
        {
            if (GlidingConnectorHasConnections())
                throw new InvalidOperationException("Cannot remove gliding connector");

            // custom and default connectors remain valid in this case
            var glidingConnector = Connectors.SingleOrDefault(c => c.Name == ConnectorPosition.Gliding);

            // safe to remove since we checked there are no gliding connections above
            if (glidingConnector != null)
                Connectors.Remove(glidingConnector);
        }
    }

    private void EnsureConnector(string name)
    {
        if (Connectors.All(c => c.Name != name))
        {
            var connector = new Connector
            {
                Name = name,
                Offset = ConnectorPosition.GetKnownOffset(name),
                Shape = this
            };
            Connectors.Add(connector);
        }
    }

    private bool GlidingConnectorHasConnections()
    {
        if (Connectors.Contains(ConnectorPosition.Gliding))
        {
            var glider = Connectors[ConnectorPosition.Gliding];
            return IncomingLinks.Any(l => l.TargetConnectorResult == glider) ||
                   OutgoingLinks.Any(l => l.SourceConnectorResult == glider);
        }

        return false;
    }

    private void MoveConnectionEndPointsToGlidingConnector()
    {
        if (Connectors.Any(c => c.Name != ConnectorPosition.Gliding && !c.IsCustom()))
        {
            var standards = Connectors.Where(c => c.Name != ConnectorPosition.Gliding && !c.IsCustom()).ToList();
            var incomers = IncomingLinks.Where(l =>
                l.TargetConnectorPosition == ConnectorPosition.Auto || standards.Contains(l.TargetConnectorResult));
            foreach (var con in incomers)
            {
                con.TargetConnectorPosition = ConnectorPosition.Gliding;
                con.Target = this;
            }

            var outgoers = OutgoingLinks.Where(l =>
                l.SourceConnectorPosition == ConnectorPosition.Auto || standards.Contains(l.SourceConnectorResult));
            foreach (var con in outgoers)
            {
                con.SourceConnectorPosition = ConnectorPosition.Gliding;
                con.Source = this;
            }
        }
    }

    #endregion
}