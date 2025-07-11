namespace PixUI.Diagram;

public interface IDiagramItem
{
    /// <summary>
    /// Gets or sets whether the diagram entity is selected.
    /// </summary>
    /// <value>
    /// 	<c>True</c> if selected; otherwise, <c>false</c>.
    /// </value>
    bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    Point Position { get; set; }

    Rect Bounds { get; }
}