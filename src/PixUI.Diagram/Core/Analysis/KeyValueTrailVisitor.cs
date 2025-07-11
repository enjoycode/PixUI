namespace PixUI.Diagram;

/// <summary>
/// A visitor that tracks (stores) keys from KeyValuePairs in the order they were visited.
/// </summary>
/// <typeparam name="TKey">The type of key of the KeyValuePair.</typeparam>
/// <typeparam name="TValue">The type of value of the KeyValuePair.</typeparam>
internal sealed class KeyValueTrailVisitor<TKey, TValue> : IVisitor<KeyValuePair<TKey, TValue>>
{
    private readonly List<TValue> _values;
    private readonly List<TKey> _keys;

    public KeyValueTrailVisitor()
    {
        _values = new List<TValue>();
        _keys = new List<TKey>();
    }

    /// <summary>
    /// Gets a value indicating whether this instance has completed.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance has completed; otherwise, <c>false</c>.
    /// </value>
    public bool HasCompleted
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the trail of the values.
    /// </summary>
    /// <value>The value list.</value>
    public IList<TValue> Values
    {
        get
        {
            return _values;
        }
    }

    /// <summary>
    /// Gets the trail of the keys.
    /// </summary>
    /// <value>The keys list.</value>
    public IList<TKey> Keys
    {
        get
        {
            return _keys;
        }
    }

    /// <summary>
    /// Visits the specified key pair.
    /// </summary>
    /// <param name="obj">The object.</param>
    public void Visit(KeyValuePair<TKey, TValue> obj)
    {
        _values.Add(obj.Value);
        _keys.Add(obj.Key);
    }
}