using System.Runtime.CompilerServices;

namespace PixUI;

/// <summary>
/// A radius for either circular or elliptical shapes.
/// </summary>
[InlineArray(2)]
public struct Radius : IEquatable<Radius>
{
    #region ====Statics====

    public static readonly Radius Empty = new Radius(0, 0);

    public static Radius Circular(float radius) => new Radius(radius, radius);

    public static Radius Elliptical(float x, float y) => new Radius(x, y);

    #endregion

    private float _values;

    /// <summary>
    /// The radius value on the horizontal axis.
    /// </summary>
    public float X => this[0];

    /// <summary>
    /// The radius value on the vertical axis.
    /// </summary>
    public float Y => this[1];

    private Radius(float x, float y)
    {
        this[0] = x;
        this[1] = y;
    }

    public static Radius? Lerp(Radius? a, Radius? b, double t)
    {
        if (b == null)
        {
            if (a == null) return null;

            var k = (float)(1.0 - t);
            return Elliptical(a.Value.X * k, a.Value.Y * k);
        }

        if (a == null)
            return Elliptical((float)(b.Value.X * t), (float)(b.Value.Y * t));
        return Elliptical(FloatUtils.Lerp(a.Value.X, b.Value.X, t), FloatUtils.Lerp(a.Value.Y, b.Value.Y, t));
    }

    public static Radius operator *(Radius pt, float operand) => new(pt.X * operand, pt.Y * operand);

    public static bool operator ==(Radius left, Radius right) => left.Equals(right);

    public static bool operator !=(Radius left, Radius right) => !left.Equals(right);

    public bool Equals(Radius other) => FloatUtils.NearlyEqual(X, other.X) && FloatUtils.NearlyEqual(Y, other.Y);

    public override bool Equals(object? obj) => obj is Radius other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);
}