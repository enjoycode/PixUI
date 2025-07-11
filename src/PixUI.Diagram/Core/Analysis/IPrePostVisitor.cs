namespace PixUI.Diagram;

/// <summary>
/// Interface introducing pre-visit and post-visit actions.
/// </summary>
/// <typeparam name="T">The data type being visited.</typeparam>
internal interface IPrePostVisitor<in T> : IVisitor<T>
{
    /// <summary>
    /// Pre-visit action.
    /// </summary>
    /// <param name="item">The item.</param>
    void PreVisit(T item);

    /// <summary>
    /// Post-visit action.
    /// </summary>
    /// <param name="item">The item.</param>
    void PostVisit(T item);
}