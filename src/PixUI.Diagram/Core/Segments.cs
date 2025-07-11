using static PixUI.Diagram.GeometryExtensions;

namespace PixUI.Diagram;

internal sealed class PathFigure
{
    public PathFigure()
    {
        Segments = new PathSegmentCollection();
    }

    public PathSegmentCollection Segments { get; set; }

    public bool IsClosed { get; set; }

    public bool IsFilled { get; set; } = true;

    public Point StartPoint { get; set; } = Point.Empty;
}

internal sealed class LineSegment : PathSegment
{
    public Point Point { get; set; } = Point.Empty;
}

internal sealed class BezierSegment : PathSegment
{
    public Point Point1
    {
        get;
        set;
    }

    public Point Point2
    {
        get;
        set;
    }

    public Point Point3
    {
        get;
        set;
    }

    public BezierSegment()
    {
    }

    public PathSegment Clone()
    {
        BezierSegment bezierSegment = new BezierSegment()
        {
            Point1 = Point1,
            Point2 = Point2,
            Point3 = Point3
        };
        return bezierSegment;
    }

    public void Transform(Matrix3 transformMatrix)
    {
        throw new NotImplementedException();
        //this.Point1 = transformMatrix.Transform(this.Point1);
        //this.Point2 = transformMatrix.Transform(this.Point2);
        //this.Point3 = transformMatrix.Transform(this.Point3);
    }
}

internal sealed class PolyLineSegment : PathSegment
{
    public List<Point> Points { get; set; } = [];
}

internal sealed class ArcSegment : PathSegment, IEquatable<ArcSegment>
{
    public Point Point { get; set; }
    public Size Size { get; set; }
    public float RotationAngle { get; set; }
    public bool IsLargeArc { get; set; }
    public SweepDirection SweepDirection { get; set; }
    public float StartAngle { get; set; }
    public ArcSegment()
    {

    }
    public ArcSegment(Point point, Size size, float rotationAngle, bool isLargeArc, SweepDirection sweepDirection)
    {
        Point = point;
        Size = size;
        RotationAngle = rotationAngle;
        IsLargeArc = isLargeArc;
        SweepDirection = sweepDirection;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = 17;
            result = result * 23 + Point.GetHashCode();
            return result;
        }
    }

    public bool Equals(ArcSegment? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return Point.Equals(other.Point);
    }

    public override bool Equals(object? obj) => obj is ArcSegment temp && Equals(temp);
}