namespace PixUI.Diagram;

/// <summary>
/// Defines the different shapes along which a gliding connection will glide.
/// </summary>
/// <seealso cref="ConnectorPosition"/>
public enum GlidingStyle
{
    /// <summary>
    /// The connections attached to a gliding connector will glide along a rectangle with dimensions equal to the bounds of the shape.
    /// </summary>
    Rectangle,

    /// <summary>
    /// The connections attached to a gliding connector will glide along an ellipse with dimensions equal to the bounds of the shape.
    /// </summary>
    Ellipse,

    /// <summary>
    /// The connections attached to a gliding connector will glide along a rhombus (diamond polygon) with dimensions equal to the bounds of the shape.
    /// </summary>
    Diamond,

    /// <summary>
    /// The connections attached to a gliding connector will glide along a right triangle with dimensions equal to the bounds of the shape.
    /// </summary>
    RightTriangle,

    /// <summary>
    /// The connections attached to a gliding connector will glide along a triangle with dimensions equal to the bounds of the shape.
    /// </summary>
    Triangle
}