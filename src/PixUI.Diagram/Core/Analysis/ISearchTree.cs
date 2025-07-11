namespace PixUI.Diagram;

/// <summary>
/// Describes data structures used for searching.
/// </summary>
/// <typeparam name="T">The data type.</typeparam>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
internal interface ISearchTree<T> : ICollection<T>
{
    /// <summary>
    /// Gets the maximal item in the tree.
    /// </summary>
    /// <value>The maximum item in the tree.</value>
    T Maximum { get; }

    /// <summary>
    /// Gets the smallest item in the tree.
    /// </summary>
    /// <value>The smallest item in the tree.</value>
    /// <exception cref="InvalidOperationException">The <see cref="ISearchTree{T}"/> is empty.</exception>
    T Minimum { get; }

    /// <summary>
    /// Performs a depth first traversal on the search tree.
    /// </summary>
    /// <param name="visitor">The visitor to use.</param>
    /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
    void DepthFirstTraversal(IVisitor<T> visitor);

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
    /// </returns>
    IEnumerator<T> GetOrderedEnumerator();
}