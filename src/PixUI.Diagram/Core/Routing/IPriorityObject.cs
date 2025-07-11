namespace PixUI.Diagram;

/// <summary>
/// Defines API for objects with priority.
/// </summary>
internal interface IPriorityObject
{
    /// <summary>
    /// Gets or sets the priority.
    /// </summary>
    /// <value>
    /// The priority.
    /// </value>
    int Priority { get; set; }
}