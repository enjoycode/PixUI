namespace PixUI.Diagram;

public interface IDiagramItem
{
    string TypeName { get; }

    /// <summary>
    /// Gets whether the diagram entity is selected.
    /// </summary>
    bool IsSelected { get; }

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    Point Location { get; set; }

    Rect Bounds { get; }
}