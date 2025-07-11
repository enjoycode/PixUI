namespace PixUI.Diagram;

/// <summary>
/// Describes a visitor to a data structure.
/// </summary>
/// <typeparam name="T">The type of objects to be visited.</typeparam>
internal interface IVisitor<in T>
{
    /// <summary>
    /// Gets whether this visitor has finished.
    /// </summary>
    /// <remarks>Assigning this value is important to break the traversals when searching.</remarks>
    /// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
    bool HasCompleted { get; }

    /// <summary>
    /// Visits the specified object.
    /// </summary>
    /// <param name="obj">The object to visit.</param>
    void Visit(T obj);
}