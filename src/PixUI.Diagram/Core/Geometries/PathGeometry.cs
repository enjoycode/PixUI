namespace PixUI.Diagram;

internal sealed class PathGeometry : Geometry
{
    public PathGeometry()
    {
        Figures = [];
    }

    public List<PathFigure> Figures { get; }

    /// <summary>
    /// Gets or sets a value that determines how the intersecting areas contained
    /// in this System.Windows.Media.PathGeometry are combined.
    /// </summary>
    /// <returns>
    /// Indicates how the intersecting areas of this System.Windows.Media.PathGeometry
    /// are combined. The default value is EvenOdd.
    /// </returns>
    public FillRule FillRule { get; set; }
}

/// <summary>
///  Specifies how the intersecting areas of System.Windows.Media.PathFigure objects
///  contained in a System.Windows.Media.Geometry are combined to form the area
///  of the System.Windows.Media.Geometry.
/// </summary>
public enum FillRule
{
    /// <summary>
    ///  Rule that determines whether a point is in the fill region by drawing a ray
    ///  from that point to infinity in any direction and counting the number of path
    ///  segments within the given shape that the ray crosses. If this number is odd,
    ///  the point is inside; if even, the point is outside.
    /// </summary>
    EvenOdd = 0,

    /// <summary>
    ///  Rule that determines whether a point is in the fill region of the path by
    ///  rawing a ray from that point to infinity in any direction and then examining
    ///  the places where a segment of the shape crosses the ray. Starting with a
    ///  count of zero, add one each time a segment crosses the ray from left to right
    ///  and subtract one each time a path segment crosses the ray from right to left.
    ///  After counting the crossings, if the result is zero then the point is outside
    ///  the path. Otherwise, it is inside.
    /// </summary>
    Nonzero = 1,
}