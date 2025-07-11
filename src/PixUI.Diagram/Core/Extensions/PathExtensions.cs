using System.Runtime.CompilerServices;

namespace PixUI.Diagram;

internal static class PathExtensions
{
    public static void AddLine(this Path path, Point a, Point b)
    {
        if (!path.TryGetLastPoint(out var lastPoint) || !IsNear(lastPoint.X, a.X) || !IsNear(lastPoint.Y, a.Y))
            path.MoveTo(a.X, a.Y);

        path.LineTo(b.X, b.Y);
    }

    public static void AddBezier(this Path path, Point pt1, Point pt2, Point pt3, Point pt4)
    {
        var hasLastPoint = path.TryGetLastPoint(out var lastPoint);
        if (!hasLastPoint || !IsNear(lastPoint.X, pt1.X) || !IsNear(lastPoint.Y, pt2.Y))
            path.MoveTo(pt1.X, pt1.Y);
        path.CubicTo(pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
    }

    public static void AddEllipse(this Path path, Rect rect) => path.AddOval(rect);

    public static void AddLines(this Path path, Point[] points)
    {
        var hasLastPt = path.TryGetLastPoint(out var lastPt);
        if (!hasLastPt || !IsNear(lastPt.X, points[0].X) || !IsNear(lastPt.Y, points[0].Y))
            path.MoveTo(points[0].X, points[0].Y);
        else
            path.LineTo(points[0].X, points[0].Y);

        for (var i = 1; i < points.Length; i++)
        {
            path.LineTo(points[i].X, points[i].Y);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsNear(float a, float b)
    {
        var v = a - b;
        return v >= -0.0001f && v <= 0.0001f;
    }
}