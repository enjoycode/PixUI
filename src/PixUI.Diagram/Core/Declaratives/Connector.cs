namespace PixUI.Diagram;

internal sealed class Connector : IConnector
{
    public string Name { get; set; }

    public Connector() { }

    //public bool IsActive { get; set; }

    public Point AbsolutePosition
    {
        get
        {
            if (Shape == null) return new Point();
            //var parentContainer = this.Shape.ParentContainer;
            //if (parentContainer != null && parentContainer.IsCollapsed)
            //{
            //    // Cheating the connections that the hidden shape is actually having the connectors of the Parent Container (that is collapsed). 
            //    if (parentContainer.Connectors[this.Name] != null)
            //        return parentContainer.Connectors[this.Name].AbsolutePosition;

            //    if (parentContainer.Connectors[ConnectorPosition.Auto] != null)
            //        return parentContainer.Connectors[ConnectorPosition.Auto].AbsolutePosition;

            //    if (parentContainer.Connectors.Count > 0)
            //        return parentContainer.Connectors[0].AbsolutePosition;
            //}
            var bounds = Shape.Bounds;
            var shapePosition = bounds.Location; //bounds.TopLeft();
            var relativePosition = CalculateRelativePosition(bounds.Size);
            var position = new Point(shapePosition.X + relativePosition.X, shapePosition.Y + relativePosition.Y);

            //var parentElement = this.Shape as FrameworkElement;
            //if (this.parentElement != null)
            //{
            //    var origin = parentElement.RenderTransformOrigin;
            //    return position.Rotate(bounds.Pivot(origin), this.Shape.RotationAngle);
            //}

            return position; //return position.Rotate(bounds.Center(), this.Shape.RotationAngle);
        }
    }

    /// <summary>
    /// Gets or sets the offset of the top-left corner of the shape.
    ///  A value of zero corresponds to the upper-left corner, 
    /// while a value of one corresponds to the right side of the shape.
    /// Values outside the <c>[0,1]</c> range will position the connector outside the shape.
    /// </summary>
    /// <value>
    /// The connector's offset.
    /// </value>
    public Point Offset { get; set; }

    /// <summary>
    /// Gets the associated shape.
    /// </summary>
    public IShape Shape { get; internal set; }

    /// <summary>
    /// Calculate the relative position of this connector.
    /// </summary>
    /// <param name="shapeSize">Size of the shape.</param>
    /// <returns>
    /// The desired position of the connector's center. 
    /// This position is relative to the parent shape's (connectorsControl's) position.
    /// </returns>
    public Point CalculateRelativePosition(Size shapeSize)
    {
        return new Point(shapeSize.Width * Offset.X, shapeSize.Height * Offset.Y);
    }
}