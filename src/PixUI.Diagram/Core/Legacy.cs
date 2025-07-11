namespace PixUI.Diagram;

public abstract class Geometry { }

public abstract class PathSegment { }

/// <summary>
/// Represents a collection of points
/// </summary>
public class PointCollection : List<Point> { }

public enum SweepDirection : int
{
    Clockwise = 0,
    Counterclockwise = 1,
}

public struct Vector : IEquatable<Vector>
{
    public double Length => Math.Sqrt(X * X + Y * Y);

    public double LengthSquared => X * X + Y * Y;

    public double X { get; set; }

    public double Y { get; set; }

    public Vector(double x, double y)
    {
        X = x;
        Y = y;
    }

    public static Vector Add(Vector vector1, Vector vector2) => new(vector1.X + vector2.X, vector1.Y + vector2.Y);

    public static Point Add(Vector vector, Point point) =>
        new((float)(point.X + vector.X), (float)(point.Y + vector.Y));

    public static double AngleBetween(Vector vector1, Vector vector2)
    {
        double num = vector1.X * vector2.Y - vector2.X * vector1.Y;
        double num1 = vector1.X * vector2.X + vector1.Y * vector2.Y;
        return Math.Atan2(num, num1) * 57.2957795130823;
    }

    public static double CrossProduct(Vector vector1, Vector vector2) => vector1.X * vector2.Y - vector1.Y * vector2.X;

    public static double Determinant(Vector vector1, Vector vector2) => vector1.X * vector2.Y - vector1.Y * vector2.X;

    public static Vector Divide(Vector vector, double scalar) => vector * 1 / scalar;

    public static bool Equals(Vector vector1, Vector vector2)
    {
        double x = vector1.X;
        if (!x.Equals(vector2.X))
            return false;

        double y = vector1.Y;
        return y.Equals(vector2.Y);
    }

    public override bool Equals(object o)
    {
        if (o == null || !(o is Vector))
            return false;

        Vector vector = (Vector)o;
        return Equals(this, vector);
    }

    public bool Equals(Vector value) => Equals(this, value);

    public override int GetHashCode()
    {
        double x = X;
        double y = Y;
        return x.GetHashCode() ^ y.GetHashCode();
    }

    public static Vector Multiply(Vector vector, double scalar) => new(vector.X * scalar, vector.Y * scalar);

    public static Vector Multiply(double scalar, Vector vector) => new(vector.X * scalar, vector.Y * scalar);

    public static double Multiply(Vector vector1, Vector vector2) => vector1.X * vector2.X + vector1.Y * vector2.Y;

    public void Negate()
    {
        X = -X;
        Y = -Y;
    }

    public void Normalize()
    {
        this /= Math.Max(Math.Abs(X), Math.Abs(Y));
        this /= Length;
    }

    public static Vector operator +(Vector vector1, Vector vector2)
    {
        return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
    }

    public static Point operator +(Vector vector, Point point)
    {
        return new Point((float)(point.X + vector.X), (float)(point.Y + vector.Y));
    }

    public static Vector operator /(Vector vector, double scalar)
    {
        return vector * (1d / scalar);
    }

    public static bool operator ==(Vector vector1, Vector vector2)
    {
        if (vector1.X != vector2.X)
            return false;
        return vector1.Y == vector2.Y;
    }

    public static explicit operator Size(Vector vector) => new((float)Math.Abs(vector.X), (float)Math.Abs(vector.Y));

    public static explicit operator Point(Vector vector) => new((float)vector.X, (float)vector.Y);

    public static bool operator !=(Vector vector1, Vector vector2) => !(vector1 == vector2);

    public static Vector operator *(Vector vector, double scalar) => new(vector.X * scalar, vector.Y * scalar);

    public static Vector operator *(double scalar, Vector vector) => new(vector.X * scalar, vector.Y * scalar);

    public static double operator *(Vector vector1, Vector vector2) => vector1.X * vector2.X + vector1.Y * vector2.Y;

    public static Vector operator -(Vector vector1, Vector vector2) =>
        new(vector1.X - vector2.X, vector1.Y - vector2.Y);

    public static Vector operator -(Vector vector) => new(-vector.X, -vector.Y);

    public static Vector Subtract(Vector vector1, Vector vector2) => new(vector1.X - vector2.X, vector1.Y - vector2.Y);
}