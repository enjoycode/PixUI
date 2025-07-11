namespace PixUI.Diagram;

/// <summary>
/// An item/node of the <see cref="RedBlackTree{TKey,TValue}"/> can obviously only have at most two children, hence the inheritance from the <see cref="BinaryTree{T}"/>.
/// </summary>
/// <typeparam name="T">The data type in the node.</typeparam>
internal class RedBlackTreeNode<T> : BinaryTree<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RedBlackTreeNode&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="data">The data contained in this node.</param>
    internal RedBlackTreeNode(T data)
        : base(data)
    {
        Color = NodeColor.Red;
    }

    /// <summary>
    /// Gets or sets the color of the current node.
    /// </summary>
    /// <value>The color of the node.</value>
    internal NodeColor Color { get; set; }

    /// <summary>
    /// Gets or sets the left subtree.
    /// </summary>
    /// <value>The left subtree.</value>
    internal new RedBlackTreeNode<T> Left
    {
        get
        {
            return (RedBlackTreeNode<T>)base.Left;
        }
        set
        {
            base.Left = value;
        }
    }

    /// <summary>
    /// Gets or sets the right subtree.
    /// </summary>
    /// <value>The right subtree.</value>
    internal new RedBlackTreeNode<T> Right
    {
        get
        {
            return (RedBlackTreeNode<T>)base.Right;
        }
        set
        {
            base.Right = value;
        }
    }

    internal RedBlackTreeNode<T> this[bool direction]
    {
        get
        {
            return direction ? Right : Left;
        }
        set
        {
            if (direction)
            {
                Right = value;
            }
            else
            {
                Left = value;
            }
        }
    }
}