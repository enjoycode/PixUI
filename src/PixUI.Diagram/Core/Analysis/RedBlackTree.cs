using System.Collections.ObjectModel;

namespace PixUI.Diagram;

/// <summary>
/// A red–black tree is a type of self-balancing binary search tree, a data structure used in computer science, typically to implement associative arrays.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
internal class RedBlackTree<TKey, TValue> : RedBlackTree<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RedBlackTree&lt;TKey, TValue&gt;"/> class.
    /// </summary>
    public RedBlackTree()
        : base(new KeyValuePairComparer<TKey, TValue>())
    {
        // Do nothing - the default Comparer will be used by the base class.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RedBlackTree&lt;TKey, TValue&gt;"/> class.
    /// </summary>
    /// <param name="comparer">The comparer.</param>
    public RedBlackTree(IComparer<TKey> comparer)
        : base(new KeyValuePairComparer<TKey, TValue>(comparer))
    {
        // Do nothing else.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RedBlackTree&lt;TKey, TValue&gt;"/> class.
    /// </summary>
    /// <param name="comparison">The comparison.</param>
    public RedBlackTree(Comparison<TKey> comparison)
        : base(new KeyValuePairComparer<TKey, TValue>(comparison))
    {
        // Do nothing else.
    }

    /// <summary>
    /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </summary>
    /// <value></value>
    public ICollection<TKey> Keys
    {
        get
        {
            // Get the keys in sorted order
            var visitor = new KeyValueTrailVisitor<TKey, TValue>();
            DepthFirstTraversal(visitor);
            return new ReadOnlyCollection<TKey>(visitor.Keys);
        }
    }

    /// <summary>
    /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </summary>
    /// <value></value>
    public ICollection<TValue> Values
    {
        get
        {
            var visitor = new KeyValueTrailVisitor<TKey, TValue>();
            DepthFirstTraversal(visitor);
            return new ReadOnlyCollection<TValue>(visitor.Values);
        }
    }

    /// <summary>
    /// Gets or sets the value with the specified key.
    /// </summary>
    /// <value>The key of the item to set or get.</value>
    public TValue this[TKey key]
    {
        get
        {
            var node = FindNode(key);
            if (node == null) throw new KeyNotFoundException("key");
            return node.Data.Value;
        }
        set
        {
            var node = FindNode(key);
            if (node == null)
            {
                throw new KeyNotFoundException("key");
            }
            node.Data = new KeyValuePair<TKey, TValue>(key, value);
        }
    }

    /// <summary>
    /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </summary>
    /// <param name="key">The object to use as the key of the element to add.</param>
    /// <param name="value">The object to use as the value of the element to add.</param>
    public void Add(TKey key, TValue value)
    {
        if (Equals(key, null)) throw new ArgumentNullException(nameof(key));
        Add(new KeyValuePair<TKey, TValue>(key, value));
    }

    /// <summary>
    /// Determines whether this tree contains the given item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    ///   <c>true</c> if the item is in this tree; otherwise, <c>false</c>.
    /// </returns>
    public override bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return Contains(item, true);
    }

    /// <summary>
    /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param>
    /// <returns>
    /// True if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="key"/> is null.
    /// </exception>
    public bool ContainsKey(TKey key)
    {
        return Contains(new KeyValuePair<TKey, TValue>(key, default(TValue)), false);
    }

    /// <summary>
    /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>
    /// True if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="key"/> is null.
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">
    /// The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
    /// </exception>
    public bool Remove(TKey key)
    {
        return Remove(new KeyValuePair<TKey, TValue>(key, default(TValue)));
    }

    /// <summary>
    /// Attempts to the get value.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool TryGetValue(TKey key, out TValue value)
    {
        var node = FindNode(new KeyValuePair<TKey, TValue>(key, default(TValue)));

        if (node == null)
        {
            value = default(TValue);
            return false;
        }

        value = node.Data.Value;
        return true;
    }

    private bool Contains(KeyValuePair<TKey, TValue> item, bool checkValue)
    {
        var node = FindNode(item);

        if ((node != null) && !checkValue)
        {
            return true;
        }

        return node != null && Equals(item.Value, node.Data.Value);
    }

    private RedBlackTreeNode<KeyValuePair<TKey, TValue>> FindNode(TKey key)
    {
        return base.FindNode(new KeyValuePair<TKey, TValue>(key, default(TValue))) as RedBlackTreeNode<KeyValuePair<TKey, TValue>>;
    }
}