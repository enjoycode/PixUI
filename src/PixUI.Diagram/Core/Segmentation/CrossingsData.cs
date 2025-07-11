namespace PixUI.Diagram;

/// <summary>
/// Stores a collection of PointCollection, each PointCollection
/// holding the crossing points detected for correspondent link segment.
/// </summary>
public sealed class CrossingsData
{
    /// <summary>
    /// Initializes a new instance of the CrossingsData class.
    /// </summary>
    public CrossingsData(int segments)
    {
        SegmentCrossings = new List<PointCollection>(segments);
        for (var i = 0; i < segments; ++i)
            SegmentCrossings.Add(new PointCollection());
    }

    /// <summary>
    /// Gets or sets the segment crossings.
    /// </summary>
    public IList<PointCollection> SegmentCrossings { get; set; }
}