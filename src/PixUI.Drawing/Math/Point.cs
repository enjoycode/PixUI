using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PixUI;

[StructLayout(LayoutKind.Sequential)]
public partial struct Point : IEquatable<Point>
{
    public float X { get; set; }

    public float Y { get; set; }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public readonly bool Equals(Point obj) => X == obj.X && Y == obj.Y;

    public readonly override bool Equals(object? obj) => obj is Point f && Equals(f);

    public static bool operator ==(Point left, Point right) => left.Equals(right);

    public static bool operator !=(Point left, Point right) => !left.Equals(right);

    public readonly override int GetHashCode() => HashCode.Combine(X, Y);
}

[StructLayout(LayoutKind.Sequential)]
public partial struct PointI : IEquatable<PointI>
{
    public int X { get; set; }

    public int Y { get; set; }

    public readonly bool Equals(PointI obj) => X == obj.X && Y == obj.Y;

    public readonly override bool Equals(object? obj) => obj is PointI f && Equals(f);

    public static bool operator ==(PointI left, PointI right) => left.Equals(right);

    public static bool operator !=(PointI left, PointI right) => !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(X);
        hash.Add(Y);
        return hash.ToHashCode();
    }
}

[StructLayout(LayoutKind.Sequential)]
public partial struct Point3 : IEquatable<Point3>
{
    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public readonly bool Equals(Point3 obj) => X == obj.X && Y == obj.Y && Z == obj.Z;

    public readonly override bool Equals(object? obj) => obj is Point3 f && Equals(f);

    public static bool operator ==(Point3 left, Point3 right) => left.Equals(right);

    public static bool operator !=(Point3 left, Point3 right) => !left.Equals(right);

    public readonly override int GetHashCode() => HashCode.Combine(X, Y, Z);
}