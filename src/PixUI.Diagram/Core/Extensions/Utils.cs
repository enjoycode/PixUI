using System.Runtime.CompilerServices;

namespace PixUI.Diagram;

internal static class Utils
{
    /// <summary>
    /// An infinitesimal value.
    /// </summary>
    internal const double Epsilon = 1E-06;

    /// <summary>
    /// Determines whether the specified values are equal with Epsilon approximation.
    /// </summary>
    /// <param name="value1">The first value.</param>
    /// <param name="value2">The second value.</param>
    /// <returns>
    ///   <c>True</c> if the specified values are equal with Epsilon approximation; otherwise, <c>false</c>.
    /// </returns>
    internal static bool IsEqual(this float value1, float value2)
    {
        //// Perform the calculation. Do not call the IsNotEqual because it is slower.
        var result = Math.Abs(value1 - value2) < Epsilon;
        return result;
    }

    /// <summary>
    /// Gets a point from the minimum X and Y values from the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns></returns>
    internal static Point GetTopLeftPoint(IEnumerable<Point> points) =>
        new(points.Min(p => p.X), points.Min(p => p.Y));

    /// <summary>
    /// Returns the middle point between the given points.
    /// </summary>
    /// <param name="point1">A point.</param>
    /// <param name="point2">Another point.</param>
    /// <returns>Halfway between the two given points.</returns>
    internal static Point MiddlePoint(Point point1, Point point2) =>
        new((point1.X + point2.X) / 2f, (point1.Y + point2.Y) / 2f);

    /// <summary>
    /// Returns the distance of the point to the origin.
    /// </summary>
    internal static float Distance(float x, float y)
    {
        return (float)Math.Sqrt((x * x) + (y * y));
    }

    /// <summary>
    /// Swaps the values of the two numbers.
    /// </summary>
    /// <param name="value1">A value.</param>
    /// <param name="value2">Another value.</param>
    internal static void Swap(ref float value1, ref float value2)
    {
        var t = value1;
        value1 = value2;
        value2 = t;
    }

    /// <summary>
    /// Swaps the values of the two points.
    /// </summary>
    /// <param name="point1">A point.</param>
    /// <param name="point2">Another point.</param>
    internal static void Swap(ref Point point1, ref Point point2) => (point1, point2) = (point2, point1);

    /// <summary>
    /// Converts the specified value from degrees to radians.
    /// </summary>
    internal static float ToRadians(this float degrees)
    {
        return (float)(degrees / 180 * Math.PI);
    }

    /// <summary>
    /// Converts the Cartesian coordinates to polar coordinates.
    /// </summary>
    internal static void CartesianToPolar(Point rootPoint, Point otherPoint, ref float angle, ref float rho)
    {
        if (rootPoint == otherPoint)
        {
            angle = 0;
            rho = 0;
            return;
        }

        var dx = otherPoint.X - rootPoint.X;
        var dy = otherPoint.Y - rootPoint.Y;
        rho = Distance(dx, dy);
        angle = (float)(Math.Atan(-dy / dx) * 180 / Math.PI);
        if (dx < 0) angle += 180;
    }

    /// <summary>
    /// Polar to cartesian coordinates conversion.
    /// </summary>
    /// <param name="coordinateCenter">The coordinate center.</param>
    /// <param name="angle">The angle.</param>
    /// <param name="rho">The polar radius.</param>
    /// <returns></returns>
    /// <seealso cref="CartesianToPolar"/>
    internal static Point PolarToCartesian(Point coordinateCenter, float angle, float rho)
    {
        var radians = ToRadians(angle);
        return new Point((float)(coordinateCenter.X + (Math.Cos(radians) * rho))
            , (float)(coordinateCenter.Y - (Math.Sin(radians) * rho)));
    }

    /// <summary>
    /// Normalizes the specified angle into the [0, 2Pi] interval.
    /// </summary>
    /// <param name="angle">
    /// The angle.
    /// </param>
    /// <returns>
    /// The normalize.
    /// </returns>
    internal static float NormalizeAngle(double angle)
    {
        while (angle > Math.PI * 2) angle -= 2 * Math.PI;
        while (angle < 0) angle += Math.PI * 2;
        return (float)angle;
    }

    /// <summary>
    /// Calculates the intersection point between the specified
    /// rectangle and the line segment defined by the specified
    /// points.
    /// </summary>
    internal static void IntersectionPointOnRectangle(Rect rectangle, Point lineStart, Point lineEnd,
        ref Point intersectionPoint)
    {
        var rc = RectExtensions.CreateByTwoPoint(new Point(lineStart.X, lineStart.Y),
            new Point(lineEnd.X, lineEnd.Y));

        var x1 = lineStart.X;
        var y1 = lineStart.Y;
        var x2 = lineEnd.X;
        var y2 = lineEnd.Y;

        if (Math.Abs(x1 - x2) < Epsilon)
        {
            intersectionPoint.X = x1;

            // try with the top line
            intersectionPoint.Y = rectangle.Top;
            if (intersectionPoint.X >= rectangle.Left && intersectionPoint.X <= rectangle.Right &&
                intersectionPoint.Y >= rc.Top && intersectionPoint.Y <= rc.Bottom) return;

            // try with the bottom line
            intersectionPoint.Y = rectangle.Bottom;
            if (intersectionPoint.X >= rectangle.Left && intersectionPoint.X <= rectangle.Right &&
                intersectionPoint.Y >= rc.Top && intersectionPoint.Y <= rc.Bottom) return;
        }
        else if (Math.Abs(y1 - y2) < Epsilon)
        {
            intersectionPoint.Y = y1;

            // Try with the left line segment
            intersectionPoint.X = rectangle.Left;
            if (intersectionPoint.Y >= rectangle.Top && intersectionPoint.Y <= rectangle.Bottom &&
                intersectionPoint.X >= rc.Left && intersectionPoint.X <= rc.Right) return;

            // Try with the right line segment
            intersectionPoint.X = rectangle.Right;
            if (intersectionPoint.Y >= rectangle.Top && intersectionPoint.Y <= rectangle.Bottom &&
                intersectionPoint.X >= rc.Left && intersectionPoint.X <= rc.Right) return;
        }
        else
        {
            var a = (y1 - y2) / (x1 - x2);
            var b = ((x1 * y2) - (x2 * y1)) / (x1 - x2);

            ////TOP
            intersectionPoint.Y = rectangle.Top;
            intersectionPoint.X = (intersectionPoint.Y - b) / a;
            if (intersectionPoint.X >= rectangle.Left && intersectionPoint.X <= rectangle.Right &&
                intersectionPoint.Y <= rectangle.Bottom &&
                intersectionPoint.Y >= rc.Top && intersectionPoint.Y <= rc.Bottom) return;

            //// BOTTOM
            intersectionPoint.Y = rectangle.Bottom;
            intersectionPoint.X = (intersectionPoint.Y - b) / a;
            if (intersectionPoint.X >= rectangle.Left && intersectionPoint.X <= rectangle.Right &&
                intersectionPoint.Y >= rectangle.Top &&
                intersectionPoint.Y >= rc.Top && intersectionPoint.Y <= rc.Bottom) return;

            ////LEFT
            intersectionPoint.X = rectangle.Left;
            intersectionPoint.Y = (a * intersectionPoint.X) + b;
            if (intersectionPoint.Y >= rectangle.Top && intersectionPoint.Y <= rectangle.Bottom &&
                intersectionPoint.X <= rectangle.Right &&
                intersectionPoint.X >= rc.Left && intersectionPoint.X <= rc.Right) return;

            ////RIGHT
            intersectionPoint.X = rectangle.Right;
            intersectionPoint.Y = (a * intersectionPoint.X) + b;
            if (intersectionPoint.Y >= rectangle.Top && intersectionPoint.Y <= rectangle.Bottom &&
                intersectionPoint.X >= rectangle.Left &&
                intersectionPoint.X >= rc.Left && intersectionPoint.X <= rc.Right) return;
        }
    }

    /// <summary>
    /// Returns whether the line (line segments) intersect and returns in the crossingPoint the actual crossing
    /// point if they do.
    /// </summary>
    /// <param name="line1Start">The first point of the first line.</param>
    /// <param name="line1End">The second point of the first line.</param>
    /// <param name="line2Start">The first point of the second line.</param>
    /// <param name="line2End">The second point of the second line.</param>
    /// <param name="crossingPoint">The crossing point.</param>
    internal static bool AreLinesIntersecting(Point line1Start, Point line1End, Point line2Start, Point line2End,
        ref Point crossingPoint)
    {
        var tangensdiff = ((line1End.X - line1Start.X) * (line2End.Y - line2Start.Y)) -
                          ((line1End.Y - line1Start.Y) * (line2End.X - line2Start.X));
        if (Math.Abs(tangensdiff) < Epsilon) return false;
        var num1 = ((line1Start.Y - line2Start.Y) * (line2End.X - line2Start.X)) -
                   ((line1Start.X - line2Start.X) * (line2End.Y - line2Start.Y));
        var num2 = ((line1Start.Y - line2Start.Y) * (line1End.X - line1Start.X)) -
                   ((line1Start.X - line2Start.X) * (line1End.Y - line1Start.Y));
        var r = num1 / tangensdiff;
        var s = num2 / tangensdiff;
        ////parallel cases
        if (r < 0 || r > 1 || s < 0 || s > 1) return false;
        crossingPoint = new Point(line1Start.X + (r * (line1End.X - line1Start.X)),
            line1Start.Y + (r * (line1End.Y - line1Start.Y)));
        return true;
    }

    /// <summary>
    /// Linear interpolation between the two given values.
    /// </summary>
    /// <param name="x">The value to be returned when <paramref name="alpha"></paramref> is zero.</param>
    /// <param name="y">The value to be returned when <paramref name="alpha"></paramref> is equal to one.</param>
    /// <param name="alpha">The linear interpolation parameter which usually takes a value in the interval <c>[0,1]</c> but which works outside this range nevertheless.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Lerp(double x, double y, double alpha) => x * (1.0 - alpha) + y * alpha;

    /// <summary>
    /// Linear interpolation between the two given points.
    /// </summary>
    /// <param name="pointA">The value to be returned when <paramref name="alpha"></paramref> is zero.</param>
    /// <param name="pointB">The value to be returned when <paramref name="alpha"></paramref> is equal to one.</param>
    /// <param name="alpha">The linear interpolation parameter which usually takes a value in the interval <c>[0,1]</c> but which works outside this range nevertheless.</param>
    public static Point Lerp(Point pointA, Point pointB, double alpha)
    {
        return new Point((float)Lerp(pointA.X, pointB.X, alpha), (float)Lerp(pointA.Y, pointB.Y, alpha));
    }

    /// <summary>
    /// Finds the intersection point of a shape and the line segment defined by the point
    /// <paramref name="segmentStart"/> and <paramref name="segmentEnd"/>.
    /// </summary>
    /// <param name="rec">The bounding rectangle of the shape.</param>
    /// <param name="angle">The rotation angle.</param>
    /// <param name="segmentStart">The line point which is supposedly inside the shape defined by <paramref name="rec"/>.</param>
    /// <param name="segmentEnd">The other line point which supposedly sits outside the bounds define by <paramref name="rec"/>.</param>
    /// <param name="style">The gliding style.</param>
    /// <returns></returns>
    public static Point FindIntersectionPoint(this Rect rec, float angle, Point segmentStart, Point segmentEnd,
        GlidingStyle style)
    {
        var center = rec.Center();
        if (rec.Width <= 5 && rec.Height <= 5) return center;
        IList<Point> segments;
        var result = segmentStart;
        switch (style)
        {
            case GlidingStyle.Rectangle:
                segments = new List<Point>
                {
                    rec.TopLeft(), rec.TopRight(), rec.TopRight(), rec.BottomRight(), rec.BottomRight(),
                    rec.BottomLeft(), rec.BottomLeft(), rec.TopLeft()
                };
                if (Math.Abs(angle) > Epsilon) segments = RotatePointsAt(segments, center, angle);
                NearestIntersectionPoint(segmentStart, segmentEnd, segments, ref result);
                break;
            case GlidingStyle.Ellipse:
                segments = ApproximateEllipse(rec);
                if (Math.Abs(angle) > Epsilon) segments = RotatePointsAt(segments, center, angle);
                NearestIntersectionPoint(segmentStart, segmentEnd, segments, ref result);
                break;
            case GlidingStyle.Diamond:
                segments = new List<Point>
                {
                    rec.CenterTop(), rec.CenterRight(), rec.CenterRight(), rec.CenterBottom(), rec.CenterBottom(),
                    rec.CenterLeft(), rec.CenterLeft(), rec.CenterTop()
                };
                if (Math.Abs(angle) > Epsilon) segments = RotatePointsAt(segments, center, angle);
                NearestIntersectionPoint(segmentStart, segmentEnd, segments, ref result);
                break;
            case GlidingStyle.Triangle:
                segments = new List<Point>
                {
                    rec.CenterTop(), rec.BottomRight(), rec.BottomRight(), rec.BottomLeft(), rec.BottomLeft(),
                    rec.CenterTop()
                };
                if (Math.Abs(angle) > Epsilon) segments = RotatePointsAt(segments, center, angle);
                NearestIntersectionPoint(segmentStart, segmentEnd, segments, ref result);
                break;
            case GlidingStyle.RightTriangle:
                segments = new List<Point>
                {
                    rec.TopLeft(), rec.BottomRight(), rec.BottomRight(), rec.BottomLeft(), rec.BottomLeft(),
                    rec.TopLeft()
                };
                if (Math.Abs(angle) > Epsilon)
                {
                    segments = RotatePointsAt(segments, center, angle);

                    // since s1 is not a fix-point for the rotation it means it has to be rotated as well, otherwise it will eventually be outside the triangle
                    segmentStart = RotatePointsAt(new List<Point> { segmentStart }, center, angle).Single();
                }

                NearestIntersectionPoint(segmentStart, segmentEnd, segments, ref result);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(style));
        }

        return result;
    }

    /// <summary>
    /// Approximates the ellipse defined by the given bounds.
    /// </summary>
    /// <param name="rec">The bounds of the (not rotated) ellipse.</param>
    /// <param name="minSegmentLength">The minimum length an approximating segment should have.</param>
    /// <returns></returns>
    public static IList<Point> ApproximateEllipse(this Rect rec, double minSegmentLength = 10d)
    {
        // using Ramanujan's approximation here, see http://en.wikipedia.org/wiki/Ellipse#Circumference
        var a = rec.Width / 2;
        var b = rec.Height / 2;
        var circum = Math.PI * (a + b) * (1 + ((3 * Math.Pow((a - b) / (a + b), 2)) /
                                               (10 + Math.Sqrt(4 - (3 * Math.Pow((a - b) / (a + b), 2))))));
        var segmentCount = (int)Math.Floor(circum / minSegmentLength);
        if (segmentCount % 2 != 0) segmentCount++;
        var deltaAngle = Math.Max(360 / segmentCount, 1);
        var segments = new List<Point>();
        for (var i = 0; i < segmentCount; i++)
        {
            if (i == segmentCount - 1)
            {
                segments.Add(GetEllipsePointAtAngle(rec, (segmentCount - 1) * deltaAngle));
                segments.Add(GetEllipsePointAtAngle(rec, 0));
            }
            else
            {
                segments.Add(GetEllipsePointAtAngle(rec, i * deltaAngle));
                segments.Add(GetEllipsePointAtAngle(rec, (i + 1) * deltaAngle));
            }
        }

        return segments;
    }

    /// <summary>
    /// Returns the point at an angle on the ellipse defined by the specified rectangle.
    /// </summary>
    /// <param name="rect">The rect.</param>
    /// <param name="angle">The angle in degrees.</param>
    /// <returns></returns>
    public static Point GetEllipsePointAtAngle(this Rect rect, float angle)
    {
        var rads = ToRadians(angle);
        var cos = Math.Cos(rads);
        var rx = rect.Width / 2;
        var ry = rect.Height / 2;
        double x, y;

        if (Math.Abs(cos) < Epsilon)
        {
            x = 0;
            y = ry * Math.Sin(rads);
        }
        else
        {
            var sin = Math.Sin(rads);
            var r1 = 1 / Math.Sqrt(Square(cos / rx) + Square(sin / ry));
            x = r1 * cos;
            y = r1 * sin;
        }

        return new Point((float)(rect.Left + rx + x), (float)(rect.Top + ry + y));
    }

    /// <summary>
    /// Squares the specified value.
    /// </summary>
    public static double Square(double value) => value * value;

    /// <summary>
    /// Returns the vector between the two points.
    /// </summary>
    /// <param name="point1">The point where the vector starts.</param>
    /// <param name="point2">The points where the vector ends.</param>
    public static Vector Delta(this Point point1, Point point2) => new(point1.X - point2.X, point1.Y - point2.Y);

    /// <summary>
    /// Returns the perpendicular of the specified vector.
    /// </summary>
    public static Vector Perpendicular(this Vector vector) => new(vector.Y, -vector.X);

    /// <summary>
    /// Calculates the intersection points with each of the given segments and determines the one nearest to the first point.
    /// </summary>
    /// <returns>Returns <c>true</c> if a point (non-NaN values) is found, otherwise <c>false</c>.</returns>
    public static bool NearestIntersectionPoint(Point segmentStart, Point segmentEnd, IList<Point> points,
        ref Point result)
    {
        if (points == null) throw new ArgumentNullException("points");
        if (points.Count < 2) throw new ArgumentException("ListShouldContainAtLeastTwoPoints", "points");
        if (points.Count % 2 != 0) throw new ArgumentException("NotACollectionOfSegments", "points");

        var resultDistance = double.PositiveInfinity;
        var found = false;
        for (var i = 0; i < points.Count - 1; i++)
        {
            var l1 = points[i];
            var l2 = points[i + 1];
            var p = segmentEnd;
            if (SegmentIntersect(segmentStart, segmentEnd, l1, l2, ref p))
            {
                found = true;
                var distance = p.Distance(segmentStart);
                if (distance < resultDistance)
                {
                    result = p;
                    resultDistance = distance;
                }
            }
        }

        return found;
    }

    /// <summary>
    /// Checks whether the segments defined by the specified
    /// point pairs intersect and returns the intersection point.
    /// </summary>
    public static bool SegmentIntersect(Point segmentStart, Point segmentEnd, Point lineStart, Point lineEnd,
        ref Point result)
    {
        result = FindLinesIntersection(segmentStart, segmentEnd, lineStart, lineEnd, true);
        if (double.IsNaN(result.X) || double.IsNaN(result.Y)) return false;
        var p1 = (result.X - segmentStart.X) * (result.X - segmentEnd.X);
        var p2 = (result.Y - segmentStart.Y) * (result.Y - segmentEnd.Y);
        if (p1 > Epsilon || p2 > Epsilon) return false;
        var pl1 = (result.X - lineStart.X) * (result.X - lineEnd.X);
        var pl2 = (result.Y - lineStart.Y) * (result.Y - lineEnd.Y);
        return pl1 <= Epsilon && pl2 <= Epsilon;
    }

    /// <summary>
    /// Finds the intersection point of the lines defined by the point pairs.
    /// </summary>
    /// <returns>
    /// The intersection point. If acceptNaN is <line2Start>true</line2Start> line1Start <line2Start>double.NaN</line2Start> is returned if they don't intersect.
    /// </returns>
    public static Point FindLinesIntersection(Point line1Start, Point line1End, Point line2Start, Point line2End,
        bool acceptNaN = false)
    {
        var pt = acceptNaN ? new Point(float.NaN, float.NaN) : new Point(float.MinValue, float.MinValue);
        if (Math.Abs(line1Start.X - line1End.X) < Epsilon && Math.Abs(line2Start.X - line2End.X) < Epsilon) return pt;

        if (Math.Abs(line1Start.X - line1End.X) < Epsilon)
        {
            pt.X = line1Start.X;
            pt.Y = ((line2Start.Y - line2End.Y) / (line2Start.X - line2End.X) * pt.X) +
                   (((line2Start.X * line2End.Y) - (line2End.X * line2Start.Y)) / (line2Start.X - line2End.X));
            return pt;
        }

        if (Math.Abs(line2Start.X - line2End.X) < Epsilon)
        {
            pt.X = line2Start.X;
            pt.Y = ((line1Start.Y - line1End.Y) / (line1Start.X - line1End.X) * pt.X) +
                   (((line1Start.X * line1End.Y) - (line1End.X * line1Start.Y)) / (line1Start.X - line1End.X));
            return pt;
        }

        var a1 = (line1Start.Y - line1End.Y) / (line1Start.X - line1End.X);
        var b1 = ((line1Start.X * line1End.Y) - (line1End.X * line1Start.Y)) / (line1Start.X - line1End.X);
        var a2 = (line2Start.Y - line2End.Y) / (line2Start.X - line2End.X);
        var b2 = ((line2Start.X * line2End.Y) - (line2End.X * line2Start.Y)) / (line2Start.X - line2End.X);
        if ((Math.Abs(a1 - a2) > Epsilon) || acceptNaN)
        {
            pt.X = (b2 - b1) / (a1 - a2);
            pt.Y = (a1 * (b2 - b1) / (a1 - a2)) + b1;
        }

        return pt;
    }

    /// <summary>
    /// Rotates the points.
    /// </summary>
    public static IList<Point> RotatePointsAt(IList<Point> points, Point pivot, float angle)
    {
        var rotation = Matrix3.CreateRotationDegrees(angle, pivot.X, pivot.Y);
        return points.Select(p => rotation.MapPoint(p.X, p.Y)).ToList();
    }
}