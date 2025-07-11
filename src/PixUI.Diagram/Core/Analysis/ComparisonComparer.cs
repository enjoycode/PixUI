namespace PixUI.Diagram;

/// <summary>
/// <see cref="IComparer{T}"/> implementation based on a <see cref="Comparison"/>.
/// </summary>
/// <typeparam name="T">The data type being compared.</typeparam>
internal sealed class ComparisonComparer<T> : IComparer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComparisonComparer&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="comparison">The comparison.</param>
    public ComparisonComparer(Comparison<T> comparison)
    {
        Comparison = comparison;
    }

    /// <summary>
    /// Gets or sets the comparison used in this comparer.
    /// </summary>
    /// <value>The comparison used in this comparer.</value>
    public Comparison<T> Comparison { get; set; }

    /// <summary>
    /// Compares the given items.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <returns></returns>
    public int Compare(T x, T y)
    {
        return Comparison(x, y);
    }
}