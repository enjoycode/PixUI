﻿namespace PixUI.Diagram;

internal class RedBlackTreeList<TKey, TValue> : RedBlackTree<TKey, LinkedList<TValue>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RedBlackTreeList&lt;TKey, TValue&gt;"/> class.
    /// </summary>
    /// <inheritdoc />
    public RedBlackTreeList()
    {
    }

    public RedBlackTreeList(IComparer<TKey> comparer)
        : base(comparer)
    {
    }

    public RedBlackTreeList(Comparison<TKey> comparison)
        : base(comparison)
    {
    }

    private delegate bool NodeAction(TKey key, LinkedList<TValue> values);

    /// <summary>
    /// Determines whether the specified value contains value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    ///     <c>true</c> if the specified value contains value; otherwise, <c>false</c>.
    /// </returns>
    public bool ContainsValue(TValue value)
    {
        return TraverseItems((key, list) => list.Contains(value));
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetKeyEnumerator()
    {
        var stack = new Stack<BinaryTree<KeyValuePair<TKey, LinkedList<TValue>>>>();
        if (Tree != null)
        {
            stack.Push(Tree);
        }
        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();
            var list = currentNode.Data.Value;
            foreach (var item in list)
            {
                yield return new KeyValuePair<TKey, TValue>(currentNode.Data.Key, item);
            }
            if (currentNode.Left != null)
            {
                stack.Push(currentNode.Left);
            }
            if (currentNode.Right != null)
            {
                stack.Push(currentNode.Right);
            }
        }
    }

    /// <summary>
    /// Gets the value enumerator.
    /// </summary>
    /// <returns>An enumerator to enumerate through the values contained in this instance.</returns>
    public IEnumerator<TValue> GetValueEnumerator()
    {
        var stack = new Stack<BinaryTree<KeyValuePair<TKey, LinkedList<TValue>>>>();

        if (Tree != null)
        {
            stack.Push(Tree);
        }
        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();
            var list = currentNode.Data.Value;
            foreach (var item in list)
            {
                yield return item;
            }
            if (currentNode.Left != null)
            {
                stack.Push(currentNode.Left);
            }
            if (currentNode.Right != null)
            {
                stack.Push(currentNode.Right);
            }
        }
    }

    /// <summary>
    /// Removes the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key under which the item was found.</param>
    /// <returns>A value indicating whether the item was found or not.</returns>
    public bool Remove(TValue value, out TKey key)
    {
        var foundKey = default(TKey);
        var ret = TraverseItems(delegate (TKey itemKey, LinkedList<TValue> list)
        {
            if (list.Remove(value))
            {
                if (list.Count == 0)
                {
                    Remove(itemKey);
                }
                foundKey = itemKey;
                return true;
            }
            return false;
        });

        key = foundKey;
        return ret;
    }

    /// <summary>
    /// Traverses the items.
    /// </summary>
    /// <param name="shouldStop">A predicate that performs an action on the list, and indicates whether the enumeration of items should stop or not.</param>
    /// <returns>An indication of whether the enumeration was stopped prematurely.</returns>
    private bool TraverseItems(NodeAction shouldStop)
    {
        var stack = new Stack<BinaryTree<KeyValuePair<TKey, LinkedList<TValue>>>>();
        if (Tree != null)
        {
            stack.Push(Tree);
        }
        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();
            if (shouldStop(currentNode.Data.Key, currentNode.Data.Value))
            {
                return true;
            }
            if (currentNode.Left != null)
            {
                stack.Push(currentNode.Left);
            }
            if (currentNode.Right != null)
            {
                stack.Push(currentNode.Right);
            }
        }

        return false;
    }
}