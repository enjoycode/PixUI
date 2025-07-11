namespace PixUI.Diagram;

internal static class RectExtensions
{

    /// <summary>
    /// Returns the top-left point of the rectangle.
    /// </summary>
    /// <param name="rect">The current rectangle.</param>
    /// <returns></returns>
    internal static Point TopLeft(this Rect rect) => new(rect.Left, rect.Top);

    /// <summary>
    /// Returns the top-right point of the rectangle.
    /// </summary>
    /// <param name="rect">The current rectangle.</param>
    /// <returns></returns>
    internal static Point TopRight(this Rect rect) => new(rect.Right, rect.Top);

    /// <summary>
    /// Returns the bottom-left point of the rectangle.
    /// </summary>
    /// <param name="rect">The current rectangle.</param>
    /// <returns></returns>
    public static Point BottomLeft(this Rect rect) => new(rect.Left, rect.Bottom);

    /// <summary>
    /// Returns the bottom-right corner of the rectangle.
    /// </summary>
    /// <param name="rect">The current rectangle.</param>
    /// <returns></returns>
    public static Point BottomRight(this Rect rect) => new(rect.Right, rect.Bottom);

    /// <summary>
    /// Centers the left.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <returns></returns>
    public static Point CenterLeft(this Rect rect) => new(rect.Left, rect.Top + (rect.Height / 2f));

    /// <summary>
    /// Centers the top.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <returns></returns>
    public static Point CenterTop(this Rect rect) => new(rect.Left + (rect.Width / 2f), rect.Top);

    /// <summary>
    /// Centers the right.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <returns></returns>
    public static Point CenterRight(this Rect rect) => new(rect.Right, rect.Top + (rect.Height / 2f));

    /// <summary>
    /// Returns the bottom center point.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <returns></returns>
    public static Point CenterBottom(this Rect rect) => new(rect.Left + (rect.Width / 2f), rect.Bottom);

    /// <summary>
    /// Returns the center of the specified rectangle.
    /// </summary>
    internal static Point Center(this Rect rectangle)
    {
        return Utils.MiddlePoint(rectangle.TopLeft(), rectangle.BottomRight());
    }

    internal static Rect CreateByTwoPoint(Point point1, Point point2)
    {
        var rect = new Rect();
        rect.X = Math.Min(point1.X, point2.X);
        rect.Y = Math.Min(point1.Y, point2.Y);
        rect.Width = Math.Max(Math.Max(point1.X, point2.X) - rect.X, 0);
        rect.Height = Math.Max(Math.Max(point1.Y, point2.Y) - rect.Y, 0);
        return rect;
    }

    /// <summary>
    /// Intersects the line segment.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    internal static bool IntersectsLineSegment(this Rect rect, Point lineStart, Point lineEnd, ref Point point)
    {
        return Utils.AreLinesIntersecting(lineStart, lineEnd, rect.TopLeft(), rect.TopRight(), ref point) ||
               Utils.AreLinesIntersecting(lineStart, lineEnd, rect.TopRight(), rect.BottomRight(), ref point) ||
               Utils.AreLinesIntersecting(lineStart, lineEnd, rect.BottomRight(), rect.BottomLeft(), ref point) ||
               Utils.AreLinesIntersecting(lineStart, lineEnd, rect.BottomLeft(), rect.TopLeft(), ref point);
    }

}