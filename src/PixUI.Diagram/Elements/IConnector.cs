namespace PixUI.Diagram;

public interface IConnector
{
    /// <summary>
    /// Gets the name of the connector.
    /// </summary>
    /// <remarks>This name is supposedly unique across a shape since it's used to access a connector.</remarks>
    string Name { get; set; }

    ///// <summary>
    ///// Gets or sets a value indicating whether this instance is active.
    ///// </summary>
    ///// <value>
    /////   <c>True</c> if this instance is active; otherwise, <c>false</c>.
    ///// </value>
    //bool IsActive { get; set; }

    /// <summary>
    /// Gets the shape to which this connector belongs.
    /// </summary>
    IShape Shape { get; }

    /// <summary>
    /// Gets or sets the offset of the top-left corner of the shape. Its value range from 0 to 1.
    /// </summary>
    /// <value>
    /// The offset.
    /// </value>
    Point Offset { get; set; }

    /// <summary>
    /// Gets the absolute or actual position of the connector with respect to the diagramming surface.
    /// </summary>
    /// <returns></returns>
    Point AbsolutePosition { get; }

    /// <summary>
    /// Calculates the relative position of the connector.
    /// </summary>
    /// <param name="shapeSize">Size of the shape.</param>
    /// <returns></returns>
    Point CalculateRelativePosition(Size shapeSize);
}