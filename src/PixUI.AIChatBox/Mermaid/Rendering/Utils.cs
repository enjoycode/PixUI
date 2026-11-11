namespace PixUI.Mermaid;

internal static class Utils
{
    /// <summary>
    /// Returns the middle point between the given points.
    /// </summary>
    /// <param name="point1">A point.</param>
    /// <param name="point2">Another point.</param>
    /// <returns>Halfway between the two given points.</returns>
    internal static Point MiddlePoint(Point point1, Point point2) =>
        new((point1.X + point2.X) / 2f, (point1.Y + point2.Y) / 2f);

    /// <summary>
    /// Gets the points of an arrow.
    /// </summary>
    /// <param name="type">The type of arrow.</param>
    /// <param name="startPoint">The start point - the tip of the arrow.</param>
    /// <param name="endPoint">The end of the line of the arrow.</param>
    /// <param name="arrowWidth">The width of the arrow.</param>
    /// <param name="arrowHeight">The height of the arrow.</param>
    /// <returns>
    /// Returns the points of an arrow in the order:
    /// base arrow point, left arrow head point, right arrow head point.
    /// </returns>
    internal static (Point arrowBase, Point left, Point right) GetArrowPoints(CapType type,
        Point startPoint, Point endPoint, float arrowWidth, float arrowHeight)
    {
        arrowHeight = arrowHeight / 2f;
        var theta = Math.Atan2(startPoint.Y - endPoint.Y, startPoint.X - endPoint.X);
        var sint = Math.Round(Math.Sin(theta), 2);
        var cost = Math.Round(Math.Cos(theta), 2);

        // ge the side points of the arrow's head:
        var leftPoint = new Point((float)(endPoint.X + ((arrowWidth * cost) - (arrowHeight * sint)))
            , (float)(endPoint.Y + ((arrowWidth * sint) + (arrowHeight * cost))));
        var rightPoint = new Point((float)(endPoint.X + ((arrowWidth * cost) + (arrowHeight * sint)))
            , (float)(endPoint.Y - ((arrowHeight * cost) - (arrowWidth * sint))));
        var arrowBase = Utils.MiddlePoint(leftPoint, rightPoint);

        switch (type)
        {
            case CapType.Arrow2:
            case CapType.Arrow2Filled:
                var deltaX = endPoint.X - arrowBase.X;
                var deltaY = endPoint.Y - arrowBase.Y;
                //// the arrow base point is opposite of the tip of the arrow and the incoming line stops in it:
                arrowBase = new Point(arrowBase.X + (deltaX * 0.2f), arrowBase.Y + (deltaY * 0.2f));
                break;
            case CapType.Arrow3:
                //// arrow3 is not closed, so the base of the arrow is the tip of the arrow:
                arrowBase = endPoint;
                break;
        }

        return (arrowBase, leftPoint, rightPoint);
    }

    /// <summary>
    /// Creates an arrow based from the specified points.
    /// </summary>
    /// <param name="startPoint">The start point - the tip of the arrow.</param>
    /// <param name="arrowPoint1">The arrow point1 - the left end point from the start-base line.</param>
    /// <param name="arrowPoint2">The arrow point2 - the right end points from the start-base line.</param>
    /// <param name="basePoint">The base point of the arrow - the end of the arrow's head.</param>
    /// <param name="isClosed">If set to <c>true</c> the arrow will be closed.</param>
    /// <returns>Returns a path of an arrow.</returns>
    internal static IPath CreateArrow(Point startPoint, Point arrowPoint1, Point arrowPoint2, Point basePoint,
        bool isClosed)
    {
        using var pb = PathBuilder.Create();
        if (isClosed)
        {
            pb.MoveTo(startPoint.X, startPoint.Y);
            pb.LineTo(arrowPoint1.X, arrowPoint1.Y);
            pb.LineTo(basePoint.X, basePoint.Y);
            pb.LineTo(arrowPoint2.X, arrowPoint2.Y);
            pb.Close();
            return pb.Detach();
        }

        pb.MoveTo(arrowPoint1.X, arrowPoint1.Y);
        pb.LineTo(startPoint.X, startPoint.Y);
        pb.LineTo(arrowPoint2.X, arrowPoint2.Y);
        return pb.Detach();
    }
}