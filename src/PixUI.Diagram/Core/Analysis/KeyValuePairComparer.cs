namespace PixUI.Diagram;

/// <summary>
/// A comparer of key-value pairs based on a comparison of the respective keys.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
internal class KeyValuePairComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
{
    private readonly IComparer<TKey> _comparer;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyValuePairComparer&lt;TKey, TValue&gt;"/> class.
    /// </summary>
    public KeyValuePairComparer()
    {
        _comparer = Comparer<TKey>.Default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyValuePairComparer&lt;TKey, TValue&gt;"/> class.
    /// </summary>
    /// <param name="comparer">The comparer.</param>
    public KeyValuePairComparer(IComparer<TKey> comparer)
    {
        if (comparer == null) throw new ArgumentNullException(nameof(comparer));
        _comparer = comparer;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyValuePairComparer&lt;TKey, TValue&gt;"/> class.
    /// </summary>
    /// <param name="comparison">The comparison.</param>
    public KeyValuePairComparer(Comparison<TKey> comparison)
    {
        if (comparison == null) throw new ArgumentNullException(nameof(comparison));
        _comparer = new ComparisonComparer<TKey>(comparison);
    }

    /// <summary>
    /// Compares the two key pairs.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <returns></returns>
    public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
    {
        return _comparer.Compare(x.Key, y.Key);
    }

    /// <summary>
    /// Compares the two values.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <returns></returns>
    public int Compare(TKey x, TKey y)
    {
        return _comparer.Compare(x, y);
    }
}