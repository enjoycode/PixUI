using System.Collections;

namespace PixUI.Diagram;

/// <summary>
/// Base implementation of the <see cref="ISearchTree{T}"/> interface.
/// </summary>
/// <typeparam name="T">The data type contained in this collection.</typeparam>
/// <seealso cref="RedBlackTree{TKey,TValue}"/>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
internal abstract class BinarySearchTreeBase<T> : ISearchTree<T>
{
    private readonly IComparer<T> _comparer;

    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySearchTreeBase{T}"/> class.
    /// </summary>
    protected BinarySearchTreeBase()
    {
        _comparer = Comparer<T>.Default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySearchTreeBase{T}"/> class.
    /// </summary>
    /// <param name="comparer">The comparer to use when comparing items.</param>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
    protected BinarySearchTreeBase(IComparer<T> comparer)
    {
        _comparer = comparer;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BinarySearchTreeBase&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="comparison">The comparison.</param>
    protected BinarySearchTreeBase(Comparison<T> comparison)
    {
        _comparer = new ComparisonComparer<T>(comparison);
    }

    /// <summary>
    /// A custom comparison between some search value and the type of item that is kept in the tree.
    /// </summary>
    /// <typeparam name="TSearch">The type of the search.</typeparam>
    protected delegate int CustomComparison<in TSearch>(TSearch value, T item);

    /// <summary>
    /// Gets the comparer.
    /// </summary>
    /// <value>The comparer.</value>
    public IComparer<T> Comparer
    {
        get
        {
            return _comparer;
        }
    }

    /// <summary>
    /// Gets the count.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance is empty.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
    /// </value>
    public bool IsEmpty
    {
        get
        {
            return Count == 0;
        }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is read only.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance is read only; otherwise, <c>false</c>.
    /// </value>
    public bool IsReadOnly
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height
    {
        get
        {
            if (Tree.IsEmpty) return 0;
            return Math.Max(Tree.Left == null ? 0 : Tree.Left.Height, Tree.Right == null ? 0 : Tree.Right.Height);
        }
    }

    /// <summary>
    /// Gets the maximum.
    /// </summary>
    public virtual T Maximum
    {
        get
        {
            ValidateEmpty();
            return FindMaximumNode().Data;
        }
    }

    /// <summary>
    /// Gets the minimum.
    /// </summary>
    public virtual T Minimum
    {
        get
        {
            ValidateEmpty();
            return FindMinimumNode().Data;
        }
    }

    /// <summary>
    /// Gets or sets the binary tree.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    protected BinaryTree<T> Tree { get; set; }

    /// <summary>
    /// Adds the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    public void Add(T item)
    {
        AddItem(item);
        Count++;
    }

    /// <summary>
    /// Clears this instance.
    /// </summary>
    public void Clear()
    {
        ClearItems();
    }

    /// <summary>
    /// Determines whether the item is in this tree.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool Contains(T item)
    {
        var node = FindNode(item);
        return node != null;
    }

    /// <summary>
    /// Copies to.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <param name="arrayIndex">Index of the array.</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        if ((array.Length - arrayIndex) < Count) throw new ArgumentException("Index should not exceed array.Lenght minus Count.", "array");
        foreach (var association in Tree) array[arrayIndex++] = association;
    }

    /// <summary>
    /// Depth first search traversal.
    /// </summary>
    /// <param name="visitor">The visitor.</param>
    public void DepthFirstTraversal(IVisitor<T> visitor)
    {
        VisitNode(Tree, visitor);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<T> GetEnumerator()
    {
        if (Tree != null)
        {
            var stack = new Stack<BinaryTree<T>>();
            stack.Push(Tree);
            while (stack.Count > 0)
            {
                var binaryTree = stack.Pop();
                yield return binaryTree.Data;
                if (binaryTree.Left != null) stack.Push(binaryTree.Left);
                if (binaryTree.Right != null) stack.Push(binaryTree.Right);
            }
        }
    }

    /// <summary>
    /// Gets the ordered enumerator.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetOrderedEnumerator()
    {
        if (Tree != null)
        {
            var trackingVisitor = new TrailVisitor<T>();
            Tree.DepthFirstTraversal(trackingVisitor);
            var trackingList = trackingVisitor.Trail;
            foreach (var t in trackingList) yield return t;
        }
    }

    /// <summary>
    /// Removes the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    public bool Remove(T item)
    {
        var itemRemoved = RemoveItem(item);
        if (itemRemoved) Count--;
        return itemRemoved;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Finds the maximum node.
    /// </summary>
    /// <param name="startNode">The start node.</param>
    /// <returns>The maximum node below this node.</returns>
    protected static BinaryTree<T> FindMaximumNode(BinaryTree<T> startNode)
    {
        var searchNode = startNode;
        while (searchNode.Right != null) searchNode = searchNode.Right;
        return searchNode;
    }

    /// <summary>
    /// Finds the minimum node.
    /// </summary>
    /// <param name="startNode">The start node.</param>
    /// <returns>The minimum node below this node.</returns>
    protected static BinaryTree<T> FindMinimumNode(BinaryTree<T> startNode)
    {
        var searchNode = startNode;
        while (searchNode.Left != null) searchNode = searchNode.Left;
        return searchNode;
    }

    /// <summary>
    /// Adds the item.
    /// </summary>
    /// <param name="item">The item.</param>
    protected abstract void AddItem(T item);

    /// <summary>
    /// Clears all the objects in this instance.
    /// </summary>
    /// <remarks>
    /// <b>Notes to Inheritors: </b>
    ///  Derived classes can override this method to change the behavior of the <see cref="Clear"/> method.
    /// </remarks>
    protected virtual void ClearItems()
    {
        Tree = null;
        Count = 0;
    }

    /// <summary>
    /// Find the maximum node.
    /// </summary>
    /// <returns>The maximum node.</returns>
    protected BinaryTree<T> FindMaximumNode()
    {
        return FindMaximumNode(Tree);
    }

    /// <summary>
    /// Find the minimum node.
    /// </summary>
    /// <returns>The minimum node.</returns>
    protected BinaryTree<T> FindMinimumNode()
    {
        return FindMinimumNode(Tree);
    }

    /// <summary>
    /// Finds the node containing the specified data key.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    /// The node with the specified key if found.  If the key is not in the tree, this method returns null.
    /// </returns>
    protected virtual BinaryTree<T> FindNode(T item)
    {
        if (Tree == null) return null;
        var currentNode = Tree;
        while (currentNode != null)
        {
            var nodeResult = _comparer.Compare(item, currentNode.Data);
            if (nodeResult == 0) return currentNode;
            currentNode = nodeResult < 0 ? currentNode.Left : currentNode.Right;
        }

        return null;
    }

    /// <summary>
    /// Finds the node that matches the custom delegate.
    /// </summary>
    /// <typeparam name="TSearch">The type of the search.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="customComparison">The custom comparison.</param>
    /// <returns>The item if  found, else null.</returns>
    protected virtual BinaryTree<T> FindNode<TSearch>(TSearch value, CustomComparison<TSearch> customComparison)
    {
        if (Tree == null) return null;
        var currentNode = Tree;
        while (currentNode != null)
        {
            var nodeResult = customComparison(value, currentNode.Data);
            if (nodeResult == 0) return currentNode;
            currentNode = nodeResult < 0 ? currentNode.Left : currentNode.Right;
        }
        return null;
    }

    /// <summary>
    /// Removes the item from the tree.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns>An indication of whether the item has been removed from the tree.</returns>
    protected abstract bool RemoveItem(T item);

    /// <summary>
    /// Visits the node in an in-order fashion.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="visitor">The visitor.</param>
    private static void VisitNode(BinaryTree<T> node, IVisitor<T> visitor)
    {
        if (node == null) return;
        var pair = node.Data;
        var prepost = visitor as IPrePostVisitor<T>;
        if (prepost != null) prepost.PreVisit(pair);
        VisitNode(node.Left, visitor);
        visitor.Visit(pair);
        VisitNode(node.Right, visitor);
        if (prepost != null) prepost.PostVisit(pair);
    }

    private void ValidateEmpty()
    {
        if (Count == 0) throw new InvalidOperationException("The tree is empty.");
    }
}