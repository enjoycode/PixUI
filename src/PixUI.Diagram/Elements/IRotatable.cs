namespace PixUI.Diagram;

/// <summary>
/// Represents an item that supports rotation.
/// </summary>
public interface IRotatable
{
    // /// <summary>
    // /// Gets the actual bounds.
    // /// </summary>
    // Rect ActualBounds { get; }

    /// <summary>
    /// Gets or sets the rotation angle.
    /// </summary>
    /// <value>
    /// The rotation angle.
    /// </value>
    float RotationAngle { get; set; }
}