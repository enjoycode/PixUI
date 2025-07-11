namespace PixUI.Diagram;

internal static class PointExtensions
{
    /// <summary>
    /// Adds the specified points together.
    /// </summary>
    /// <param name="point">A point.</param>
    /// <param name="point2">The point2.</param>
    /// <returns>
    /// The augmented point.
    /// </returns>
    public static Point Add(this Point point, Point point2) => new(point.X + point2.X, point.Y + point2.Y);

    /// <summary>
    /// Subtracts point2 from point1.
    /// </summary>
    public static Point Substract(this Point point1, Point point2) => new(point1.X - point2.X, point1.Y - point2.Y);

    /// <summary>
    /// Returns the distance between the specified points.
    /// </summary>
    /// <param name="startPoint">The start point.</param>
    /// <param name="endPoint">The end point.</param>
    /// <returns></returns>
    public static float Distance(this Point startPoint, Point endPoint) =>
        (float)Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2));

    /// <summary>
    /// Rotates the point.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <param name="pivot">The pivot.</param>
    /// <param name="angle">The angle.</param>
    /// <returns></returns>
    public static Point Rotate(this Point point, Point pivot, float angle)
    {
        if (angle.IsEqual(0)) return point;

        var matrix = Matrix3.CreateRotationDegrees(angle, pivot.X, pivot.Y);
        return matrix.MapPoint(point.X, point.Y);
    }
}