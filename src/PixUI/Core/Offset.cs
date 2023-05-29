using System;
using System.Diagnostics.CodeAnalysis;

namespace PixUI;

[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
public readonly struct Offset : IEquatable<Offset>
{
    public static readonly Offset Empty = new Offset(0, 0);

    public readonly float Dx;
    public readonly float Dy;

    public bool IsEmpty => Dx == 0 && Dy == 0;

    public Offset(float dx, float dy)
    {
        Dx = dx;
        Dy = dy;
    }

    /// <summary>
    /// Linearly interpolate between two offsets.
    /// </summary>
    public static Offset? Lerp(Offset? a, Offset? b, double t)
    {
        if (b == null)
        {
            if (a == null)
                return null;
            return new Offset((float)(a.Value.Dx * (1.0 - t)), (float)(a.Value.Dy * (1.0 - t)));
        }

        if (a == null)
            return new Offset((float)(b.Value.Dx * t), (float)(b.Value.Dy * t));

        return new Offset(FloatUtils.Lerp(a.Value.Dx, b.Value.Dx, t),
            FloatUtils.Lerp(a.Value.Dy, b.Value.Dy, t));
    }

    public bool Equals(Offset other) => Dx == other.Dx && Dy == other.Dy;

    public override bool Equals(object? obj) => obj is Offset other && Equals(other);

    public static bool operator ==(Offset left, Offset right) => left.Equals(right);

    public static bool operator !=(Offset left, Offset right) => !left.Equals(right);

    public override int GetHashCode() => HashCode.Combine(Dx, Dy);

    public Offset Clone() => new Offset(Dx, Dy);

    public override string ToString() => $"{{Dx={Dx}, Dy={Dy}}}";
}