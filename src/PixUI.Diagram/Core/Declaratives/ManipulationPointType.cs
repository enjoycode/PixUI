namespace PixUI.Diagram;

/// <summary>
/// The type of a manipulation point.
/// </summary>
internal enum ManipulationPointType
{
    /// <summary>
    /// Intermediate connection point.
    /// </summary>
    Intermediate,

    /// <summary>
    /// The first manipulation point.
    /// </summary>
    First,

    /// <summary>
    /// The last manipulation point.
    /// </summary>
    Last,

    /// <summary>
    /// The handle to translate a segment horizontally and vertically.
    /// </summary>
    SegmentHandle,

    /// <summary>
    /// The bezier start point handle.
    /// </summary>
    BezierStartHandle,

    /// <summary>
    /// The bezier end point handle.
    /// </summary>
    BezierEndHandle,

    /// <summary>
    /// The bezier handle of an intermediate point.
    /// </summary>
    BezierHandle
}