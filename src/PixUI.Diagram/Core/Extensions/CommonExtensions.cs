namespace PixUI.Diagram;

internal static class CommonExtensions
{

    /// <summary>
    /// Executes the action for each item in the collection.
    /// </summary>      
    /// <param name="collection">The collection to iterate.</param>
    /// <param name="action">The action to execute on each item.</param>
    /// <exception cref="ArgumentNullException"> Will be raised if either the <paramref name="collection"/> or the <paramref name="action"/> is null.</exception>
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        if (action == null) throw new ArgumentNullException(nameof(action));
        foreach (var item in collection) action(item);
    }

    /// <summary>
    /// Clones the list.
    /// </summary>
    /// <typeparam name="T">The data type of the list.</typeparam>
    /// <param name="list">The list to clone.</param>
    /// <returns>
    /// The cloned list.
    /// </returns>
    /// <exception cref="ArgumentNullException">Will be raised if the underlying <paramref name="list" /> is <c>null</c>.</exception>
    public static IList<T> Clone<T>(this IEnumerable<T> list)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        var clone = new List<T>();
        foreach (var item in list) clone.Add(item);
        return clone;
    }

}