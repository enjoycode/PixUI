namespace PixUI.Diagram;

/// <summary>
/// Red-Black tree data structure.
/// </summary>
/// <typeparam name="T">The data type contained in the tree.</typeparam>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
internal class RedBlackTree<T> : BinarySearchTreeBase<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RedBlackTree&lt;T&gt;"/> class.
    /// </summary>
    /// <remarks>The default comparer for the data type will be used.</remarks>
    public RedBlackTree()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RedBlackTree&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="comparer">The comparer.</param>
    public RedBlackTree(IComparer<T> comparer)
        : base(comparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RedBlackTree&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="comparison">The comparison.</param>
    public RedBlackTree(Comparison<T> comparison)
        : base(comparison)
    {
    }

    /// <summary>
    /// Adds an element with the provided key and value to the <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <param name="item">The item.</param>
    protected override void AddItem(T item)
    {
        if (Equals(item, null)) throw new ArgumentNullException("item");
        var root = Tree as RedBlackTreeNode<T>;
        var newRoot = InsertNode(root, item);
        newRoot.Color = NodeColor.Black;
        Tree = newRoot;
    }

    /// <summary>
    /// Removes the element with the specified key from the <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns>
    ///     <c>true</c> if the element is successfully removed; otherwise, <c>false</c>.  This method also returns false if key was not found in the original <see cref="IDictionary{TKey,TValue}"/>.
    /// </returns>
    /// <inheritdoc />
    protected override bool RemoveItem(T item)
    {
        if (Tree != null)
        {
            var startNode = new RedBlackTreeNode<T>(default(T));

            var childNode = startNode;
            startNode.Right = (RedBlackTreeNode<T>)Tree;

            RedBlackTreeNode<T> parent = null;
            RedBlackTreeNode<T> foundNode = null;

            var direction = true;

            while (childNode[direction] != null)
            {
                var lastDirection = direction;

                var grandParent = parent;
                parent = childNode;
                childNode = childNode[direction];

                var comparisonValue = Comparer.Compare(childNode.Data, item);

                if (comparisonValue == 0)
                {
                    foundNode = childNode;
                }

                direction = comparisonValue < 0;

                if (IsBlack(childNode) && IsBlack(childNode[direction]))
                {
                    if (IsRed(childNode[!direction]))
                    {
                        parent = parent[lastDirection] = SingleRotation(childNode, direction);
                    }
                    else if (IsBlack(childNode[direction]))
                    {
                        var sibling = parent[!lastDirection];

                        if (sibling != null)
                        {
                            if (IsBlack(sibling.Left) && IsBlack(sibling.Right))
                            {
                                parent.Color = NodeColor.Black;
                                sibling.Color = NodeColor.Red;
                                childNode.Color = NodeColor.Red;
                            }
                            else
                            {
                                var parentDirection = grandParent.Right == parent;

                                if (IsRed(sibling[lastDirection]))
                                {
                                    grandParent[parentDirection] = DoubleRotation(parent, lastDirection);
                                }
                                else if (IsRed(sibling[!lastDirection]))
                                {
                                    grandParent[parentDirection] = SingleRotation(parent, lastDirection);
                                }

                                childNode.Color = grandParent[parentDirection].Color = NodeColor.Red;
                                grandParent[parentDirection].Left.Color = NodeColor.Black;
                                grandParent[parentDirection].Right.Color = NodeColor.Black;
                            }
                        }
                    }
                }
            }

            if (foundNode != null)
            {
                foundNode.Data = childNode.Data;
                parent[parent.Right == childNode] = childNode[childNode.Left == null];
            }

            Tree = startNode.Right;

            if (Tree != null)
            {
                ((RedBlackTreeNode<T>)Tree).Color = NodeColor.Black;
            }

            if (foundNode != null)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Perform a double rotation on the node provided..
    /// </summary>
    /// <param name="node">The node on which to focus the rotation.</param>
    /// <param name="direction">The direction of the rotation.  If direction is equal to true, a right rotation is performed.  Other wise, a left rotation.</param>
    /// <returns>The new root of the cluster.</returns>
    private static RedBlackTreeNode<T> DoubleRotation(RedBlackTreeNode<T> node, bool direction)
    {
        node[!direction] = SingleRotation(node[!direction], !direction);
        return SingleRotation(node, direction);
    }

    /// <summary>
    /// Determines whether the specified node is black.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>
    ///     <c>true</c> if the specified node is black; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsBlack(RedBlackTreeNode<T> node)
    {
        return (node == null) || (node.Color == NodeColor.Black);
    }

    /// <summary>
    /// Determines whether the specified node is red.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>
    ///     <c>true</c> if the specified node is red; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsRed(RedBlackTreeNode<T> node)
    {
        return (node != null) && (node.Color == NodeColor.Red);
    }

    /// <summary>
    /// Perform a single rotation on the node provided..
    /// </summary>
    /// <param name="node">The node on which to focus the rotation.</param>
    /// <param name="direction">The direction of the rotation.  If direction is equal to true, a right rotation is performed.  Other wise, a left rotation.</param>
    /// <returns>The new root of the cluster.</returns>
    private static RedBlackTreeNode<T> SingleRotation(RedBlackTreeNode<T> node, bool direction)
    {
        var childSibling = node[!direction];

        node[!direction] = childSibling[direction];
        childSibling[direction] = node;

        node.Color = NodeColor.Red;
        childSibling.Color = NodeColor.Black;

        return childSibling;
    }

    /// <summary>
    /// A recursive implementation of insertion of a node into the tree.
    /// </summary>
    /// <param name="node">The start node.</param>
    /// <param name="item">The item.</param>
    /// <returns>The node created in the insertion.</returns>
    private RedBlackTreeNode<T> InsertNode(RedBlackTreeNode<T> node, T item)
    {
        if (node == null) node = new RedBlackTreeNode<T>(item);
        else if (Comparer.Compare(item, node.Data) != 0)
        {
            var direction = Comparer.Compare(node.Data, item) < 0;
            node[direction] = InsertNode(node[direction], item);

            if (IsRed(node[direction]))
            {
                if (IsRed(node[!direction]))
                {
                    node.Color = NodeColor.Red;
                    node.Left.Color = NodeColor.Black;
                    node.Right.Color = NodeColor.Black;
                }
                else
                {
                    if (IsRed(node[direction][direction])) node = SingleRotation(node, !direction);
                    else if (IsRed(node[direction][!direction])) node = DoubleRotation(node, !direction);
                }
            }
        }
        else throw new ArgumentException("The node is already present.");
        return node;
    }
}