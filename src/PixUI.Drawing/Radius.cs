using System;
using System.Diagnostics.CodeAnalysis;

namespace PixUI;

/// <summary>
/// A radius for either circular or elliptical shapes.
/// </summary>
[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
public readonly struct Radius : IEquatable<Radius>
{
    public static readonly Radius Empty = new Radius(0, 0);

    public static Radius Circular(float radius) => new Radius(radius, radius);

    public static Radius Elliptical(float x, float y) => new Radius(x, y);

    /// <summary>
    /// The radius value on the horizontal axis.
    /// </summary>
    public readonly float X;

    /// <summary>
    /// The radius value on the vertical axis.
    /// </summary>
    public readonly float Y;

    private Radius(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Radius? Lerp(Radius? a, Radius? b, double t)
    {
        if (b == null)
        {
            if (a == null) return null;

            var k = (float)(1.0 - t);
            return Radius.Elliptical(a.Value.X * k, a.Value.Y * k);
        }

        if (a == null)
            return Radius.Elliptical((float)(b.Value.X * t), (float)(b.Value.Y * t));
        return Radius.Elliptical(FloatUtils.Lerp(a.Value.X, b.Value.X, t),
            FloatUtils.Lerp(a.Value.Y, b.Value.Y, t));
    }

    public static Radius operator *(Radius pt, float operand) =>
        new Radius(pt.X * operand, pt.Y * operand);

    public static bool operator ==(Radius left, Radius right) => left.Equals(right);

    public static bool operator !=(Radius left, Radius right) => !left.Equals(right);

    public bool Equals(Radius other) => X == other.X && Y == other.Y;

    public Radius Clone() => new Radius(X, Y);

    public override int GetHashCode() => HashCode.Combine(X, Y);
}