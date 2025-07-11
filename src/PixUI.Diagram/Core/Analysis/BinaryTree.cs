using System.Collections;

namespace PixUI.Diagram;

/// <summary>
/// Standard implementation of a binary tree.
/// </summary>
/// <typeparam name="TData">The data type on which the tree is based.</typeparam>
/// <seealso cref="RedBlackTree{TKey,TValue}"/>
internal class BinaryTree<TData> : ICollection<TData>, ITree<TData>
{
    private BinaryTree<TData> _leftSubtree;

    private BinaryTree<TData> _rightSubtree;

    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryTree&lt;TData&gt;"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    public BinaryTree(TData data, TData left, TData right)
        : this(data, new BinaryTree<TData>(left), new BinaryTree<TData>(right))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryTree&lt;TData&gt;"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    public BinaryTree(TData data, BinaryTree<TData> left = null, BinaryTree<TData> right = null)
    {
        _leftSubtree = left;
        if (left != null) left.Parent = this;
        _rightSubtree = right;
        if (right != null) right.Parent = this;
        Data = data;
    }

    /// <summary>
    /// Gets the number of children at this level, which can be at most two.
    /// </summary>
    public int Count
    {
        get
        {
            var count = 0;
            if (_leftSubtree != null) count++;
            if (_rightSubtree != null) count++;
            return count;
        }
    }

    /// <summary>
    /// Gets or sets the data of this tree.
    /// </summary>
    /// <value>
    /// The data.
    /// </value>
    public TData Data { get; set; }

    /// <summary>
    /// Gets the degree.
    /// </summary>
    public int Degree
    {
        get
        {
            return Count;
        }
    }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public virtual int Height
    {
        get
        {
            if (Degree == 0) return 0;
            return 1 + FindMaximumChildHeight();
        }
    }

    /// <summary>
    /// Gets whether both sides are occupied, i.e. the left and right positions are filled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is full; otherwise, <c>false</c>.
    /// </value>
    public bool IsComplete
    {
        get
        {
            return (_leftSubtree != null) && (_rightSubtree != null);
        }
    }

    /// <summary>
    /// Gets a value indicating whether this tree is empty.
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
    /// Gets whether this is a leaf node, i.e. it doesn't have children nodes.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance is leaf node; otherwise, <c>false</c>.
    /// </value>
    public virtual bool IsLeafNode
    {
        get
        {
            return Degree == 0;
        }
    }

    /// <summary>
    /// Returns <c>false</c>; this tree is never read-only.
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
    /// Gets or sets the left subtree.
    /// </summary>
    /// <value>The left subtree.</value>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual BinaryTree<TData> Left
    {
        get
        {
            return _leftSubtree;
        }
        set
        {
            if (_leftSubtree != null) RemoveLeft();
            if (value != null)
            {
                if (value.Parent != null) value.Parent.Remove(value);
                value.Parent = this;
            }

            _leftSubtree = value;
        }
    }

    /// <summary>
    /// Gets the parent of the current node.
    /// </summary>
    /// <value>The parent of the current node.</value>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public BinaryTree<TData> Parent { get; set; }

    /// <summary>
    /// Gets or sets the right subtree.
    /// </summary>
    /// <value>The right subtree.</value>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual BinaryTree<TData> Right
    {
        get
        {
            return _rightSubtree;
        }
        set
        {
            if (_rightSubtree != null) RemoveRight();
            if (value != null)
            {
                if (value.Parent != null) value.Parent.Remove(value);
                value.Parent = this;
            }
            _rightSubtree = value;
        }
    }

    /// <summary>
    /// Gets the root of the binary tree.
    /// </summary>
    public BinaryTree<TData> Root
    {
        get
        {
            var runner = Parent;
            while (runner != null)
            {
                if (runner.Parent != null) runner = runner.Parent;
                else return runner;
            }
            return this;
        }
    }

    /// <summary>
    /// Gets the parent.
    /// </summary>
    ITree<TData> ITree<TData>.Parent
    {
        get
        {
            return Parent;
        }
    }

    /// <summary>
    /// Gets the <see cref="BinaryTree{T}"/> at the specified index.
    /// </summary>
    public BinaryTree<TData> this[int index]
    {
        get
        {
            return GetChild(index);
        }
    }

    /// <summary>
    /// Adds the given item to this tree.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public virtual void Add(TData item)
    {
        AddItem(new BinaryTree<TData>(item));
    }

    /// <summary>
    /// Adds an item to the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <param name="subtree">The subtree.</param>
    /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
    /// <exception cref="InvalidOperationException">The <see cref="BinaryTree{T}"/> is full.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="subtree"/> is null (Nothing in Visual Basic).</exception>
    public void Add(BinaryTree<TData> subtree)
    {
        AddItem(subtree);
    }

    /// <summary>
    /// Performs a breadth first traversal on this tree with the specified visitor.
    /// </summary>
    /// <param name="visitor">The visitor.</param>
    /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
    public virtual void BreadthFirstTraversal(IVisitor<TData> visitor)
    {
        var queue = new Queue<BinaryTree<TData>>();

        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            if (visitor.HasCompleted)
            {
                break;
            }
            var binaryTree = queue.Dequeue();
            visitor.Visit(binaryTree.Data);

            for (var i = 0; i < binaryTree.Degree; i++)
            {
                var child = binaryTree.GetChild(i);
                if (child != null)
                {
                    queue.Enqueue(child);
                }
            }
        }
    }

    /// <summary>
    /// Clears this tree of its content.
    /// </summary>
    public virtual void Clear()
    {
        if (_leftSubtree != null)
        {
            _leftSubtree.Parent = null;
            _leftSubtree = null;
        }
        if (_rightSubtree != null)
        {
            _rightSubtree.Parent = null;
            _rightSubtree = null;
        }
    }

    /// <summary>
    /// Returns whether the given item is contained in this collection.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    ///   <c>true</c> if is contained in this collection; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(TData item)
    {
        return Enumerable.Contains(this, item);
    }

    /// <summary>
    /// Copies the tree to the given array.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <param name="arrayIndex">Index of the array.</param>
    public void CopyTo(TData[] array, int arrayIndex)
    {
        foreach (var item in this)
        {
            if (arrayIndex >= array.Length)
            {
                throw new ArgumentException("ArrayIndex should not exceed array.Length", "array");
            }
            array[arrayIndex++] = item;
        }
    }

    /// <summary>
    /// Performs a depth first traversal on this tree with the specified visitor.
    /// </summary>
    /// <param name="visitor">The ordered visitor.</param>
    /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
    public virtual void DepthFirstTraversal(IVisitor<TData> visitor)
    {
        if (visitor.HasCompleted) return;
        var prepost = visitor as IPrePostVisitor<TData>;
        if (prepost != null)
            prepost.PreVisit(Data);
        if (_leftSubtree != null) _leftSubtree.DepthFirstTraversal(visitor);
        visitor.Visit(Data);
        if (_rightSubtree != null) _rightSubtree.DepthFirstTraversal(visitor);
        if (prepost != null)
            prepost.PostVisit(Data);
    }

    /// <summary>
    /// Seeks the tree node containing the given data.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public BinaryTree<TData> Find(TData value)
    {
        // we do it the easy way here and use an ad hoc BFT 
        var queue = new Queue<BinaryTree<TData>>();
        queue.Enqueue(Root);
        while (queue.Count > 0)
        {
            var binaryTree = queue.Dequeue();
            if (EqualityComparer<TData>.Default.Equals(binaryTree.Data, value))
            {
                return binaryTree;
            }
            for (var i = 0; i < binaryTree.Degree; i++)
            {
                var child = binaryTree.GetChild(i);
                if (child != null)
                {
                    queue.Enqueue(child);
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Finds the node with the specified condition.  If a node is not found matching
    /// the specified condition, null is returned.
    /// </summary>
    /// <param name="condition">The condition to test.</param>
    /// <returns>The first node that matches the condition supplied.  If a node is not found, null is returned.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="condition"/> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
    public BinaryTree<TData> FindNode(Predicate<TData> condition)
    {
        if (condition(Data))
        {
            return this;
        }
        if (_leftSubtree != null)
        {
            var ret = _leftSubtree.FindNode(condition);
            if (ret != null) return ret;
        }
        if (_rightSubtree != null)
        {
            var ret = _rightSubtree.FindNode(condition);
            if (ret != null) return ret;
        }
        return null;
    }

    /// <summary>
    /// Gets the left (index zero) or right (index one) subtree.
    /// </summary>
    /// <param name="index">The index of the child in question.</param>
    /// <returns>The child at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>There are at most two children at each level of a binary tree, the index can hence only be zero or one.</exception>
    public BinaryTree<TData> GetChild(int index)
    {
        switch (index)
        {
            case 0:
                return _leftSubtree;
            case 1:
                return _rightSubtree;
            default:
                throw new ArgumentOutOfRangeException("index");
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<TData> GetEnumerator()
    {
        var stack = new Stack<BinaryTree<TData>>();
        stack.Push(this);
        while (stack.Count > 0)
        {
            var tree = stack.Pop();
            yield return tree.Data;
            if (tree._leftSubtree != null)
            {
                stack.Push(tree._leftSubtree);
            }
            if (tree._rightSubtree != null)
            {
                stack.Push(tree._rightSubtree);
            }
        }
    }

    /// <summary>
    /// Removes the specified item from the tree.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns></returns>
    public virtual bool Remove(TData item)
    {
        if (_leftSubtree != null)
        {
            if (_leftSubtree.Data.Equals(item))
            {
                RemoveLeft();
                return true;
            }
        }

        if (_rightSubtree != null)
        {
            if (_rightSubtree.Data.Equals(item))
            {
                RemoveRight();
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes the specified child.
    /// </summary>
    /// <param name="child">The child.</param>
    /// <returns>Returns whether the child was found (and removed) from this tree.</returns>
    public virtual bool Remove(BinaryTree<TData> child)
    {
        if (_leftSubtree != null)
        {
            if (_leftSubtree == child)
            {
                RemoveLeft();
                return true;
            }
        }

        if (_rightSubtree != null)
        {
            if (_rightSubtree == child)
            {
                RemoveRight();
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes the left child.
    /// </summary>
    public virtual void RemoveLeft()
    {
        if (_leftSubtree == null)
        {
            return;
        }
        _leftSubtree.Parent = null;
        _leftSubtree = null;
    }

    /// <summary>
    /// Removes the left child.
    /// </summary>
    public virtual void RemoveRight()
    {
        if (_rightSubtree == null)
        {
            return;
        }
        _rightSubtree.Parent = null;
        _rightSubtree = null;
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        string msg = null;
        switch (Count)
        {
            case 0:
                msg = "No children";
                break;
            case 1:
                msg = Left == null ? "One right child." : "One left child.";
                break;
            case 2:
                msg = "Is full (two children).";
                break;
        }
        return $"{Data}; {msg}";
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    void ITree<TData>.Add(ITree<TData> child)
    {
        AddItem((BinaryTree<TData>)child);
    }

    ITree<TData> ITree<TData>.FindNode(Predicate<TData> condition)
    {
        return FindNode(condition);
    }

    ITree<TData> ITree<TData>.GetChild(int index)
    {
        return GetChild(index);
    }

    /// <summary>
    /// Removes the specified child.
    /// </summary>
    /// <param name="child">The child.</param>
    /// <returns></returns>
    bool ITree<TData>.Remove(ITree<TData> child)
    {
        return Remove((BinaryTree<TData>)child);
    }

    /// <summary>
    /// Finds the maximum height between the child nodes.
    /// </summary>
    /// <returns>The maximum height of the tree between all paths from this node and all leaf nodes.</returns>
    protected virtual int FindMaximumChildHeight()
    {
        var leftHeight = _leftSubtree != null ? _leftSubtree.Height : 0;
        var rightHeight = _rightSubtree != null ? _rightSubtree.Height : 0;
        return leftHeight > rightHeight ? leftHeight : rightHeight;
    }

    /// <summary>
    /// Adds an item to the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <param name="subtree">The sub tree.</param>
    private void AddItem(BinaryTree<TData> subtree)
    {
        if (_leftSubtree == null)
        {
            if (subtree.Parent != null) subtree.Parent.Remove(subtree);
            _leftSubtree = subtree;
            subtree.Parent = this;
        }
        else if (_rightSubtree == null)
        {
            if (subtree.Parent != null) subtree.Parent.Remove(subtree);
            _rightSubtree = subtree;
            subtree.Parent = this;
        }
        else throw new InvalidOperationException("This binary tree is full.");
    }
}