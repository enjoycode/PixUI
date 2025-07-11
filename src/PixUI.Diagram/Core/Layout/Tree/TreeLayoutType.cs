namespace PixUI.Diagram;

/// <summary>
/// The different layout algorithms supported.
/// </summary>
internal enum TreeLayoutType
{
    /// <summary>
    /// The standard mind mapping layout.
    /// </summary>
    MindmapHorizontal,

    /// <summary>
    /// The standard mind mapping layout but with the two wings laid out vertically.
    /// </summary>
    MindmapVertical,

    /// <summary>
    /// Standard tree layout with the children positioned at the right of the root.
    /// </summary>
    TreeRight,

    /// <summary>
    /// Standard tree layout with the children positioned at the left of the root.
    /// </summary>
    TreeLeft,

    /// <summary>
    ///  Standard tree layout with the children positioned on top of the root.
    /// </summary>
    TreeUp,

    /// <summary>
    /// Standard tree layout with the children positioned below the root.
    /// </summary>
    TreeDown,

    /// <summary>
    /// Top-down layout with the children on the second level positioned as a tree view underneath the first level.
    /// </summary>
    TipOverTree,

    /// <summary>
    /// Experimental radial tree layout.
    /// </summary>
    RadialTree,

    /// <summary>
    /// Unspecified layout. This is not an algorithm but just a tag for the host application to tell that the user has not specified any layout yet.
    /// </summary>
    Undefined
}