﻿namespace PixUI.Diagram;

/// <summary>
/// An interface for the tree data structure.
/// </summary>
/// <typeparam name="T">The type of elements in the tree.</typeparam>
internal interface ITree<T>
{
    /// <summary>
    /// Gets the data held in this node.
    /// </summary>
    /// <value>The data.</value>
    T Data { get; }

    /// <summary>
    /// Gets the degree of this node.
    /// </summary>
    /// <value>The degree of this node.</value>
    int Degree { get; }

    /// <summary>
    /// Gets the height of this tree.
    /// </summary>
    /// <value>The height of this tree.</value>
    int Height { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is leaf node.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance is leaf node; otherwise, <c>false</c>.
    /// </value>
    bool IsLeafNode { get; }

    /// <summary>
    /// Gets the parent of the current node.
    /// </summary>
    /// <value>The parent of the current node.</value>
    ITree<T> Parent { get; }

    /// <summary>
    /// Adds the specified child to the tree.
    /// </summary>
    /// <param name="child">The child to add..</param>
    void Add(ITree<T> child);

    /// <summary>
    /// Finds the node for which the given predicate holds true.
    /// </summary>
    /// <param name="condition">The condition to test on the data item.</param>
    /// <returns>The fist node that matches the condition if found, otherwise null.</returns>
    ITree<T> FindNode(Predicate<T> condition);

    /// <summary>
    /// Gets the child at the specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The child at the specified index.</returns>
    ITree<T> GetChild(int index);

    /// <summary>
    /// Removes the specified child.
    /// </summary>
    /// <param name="child">The child.</param>
    /// <returns>An indication of whether the child was found (and removed) from this tree.</returns>
    bool Remove(ITree<T> child);
}