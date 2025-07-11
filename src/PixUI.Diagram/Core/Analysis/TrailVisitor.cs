namespace PixUI.Diagram;

/// <summary>
/// This visitor keeps a trail of the visited item in the <see cref="Trail"/>.
/// </summary>
/// <typeparam name="TData">The data type of the visitor.</typeparam>
internal sealed class TrailVisitor<TData> : IVisitor<TData>
{
    private readonly List<TData> _trail;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrailVisitor&lt;TData&gt;"/> class.
    /// </summary>
    public TrailVisitor()
    {
        _trail = new List<TData>();
    }

    /// <summary>
    /// Gets a value indicating whether this visitor has completed.
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
    /// Gets the tracking list.
    /// </summary>
    /// <value>The tracking list.</value>        
    public IList<TData> Trail
    {
        get
        {
            return _trail;
        }
    }

    /// <summary>
    /// Visits the specified object.
    /// </summary>
    /// <param name="obj">The object.</param>
    public void Visit(TData obj)
    {
        _trail.Add(obj);
    }
}