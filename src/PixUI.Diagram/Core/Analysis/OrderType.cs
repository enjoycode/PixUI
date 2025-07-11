namespace PixUI.Diagram;

/// <summary>
/// Enumerates the two ways a data structure orders its elements.
/// </summary>
public enum OrderType
{
    /// <summary>
    /// Specifies that the element with the minimum priority will pop first in the data structure.
    /// </summary>
    Ascending = 0,

    /// <summary>
    /// Specifies that the element with the maximum priority will pop first in the data structure.
    /// </summary>
    Descending = 1
}