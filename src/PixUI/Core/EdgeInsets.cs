using System;

namespace PixUI;

public readonly struct EdgeInsets : IEquatable<EdgeInsets>
{
    public readonly float Left;
    public readonly float Top;
    public readonly float Right;
    public readonly float Bottom;

    public float Horizontal => Left + Right;
    public float Vertical => Top + Bottom;

    private EdgeInsets(float left, float top, float right, float bottom)
    {
        Left = Math.Max(0, left);
        Top = Math.Max(0, top);
        Right = Math.Max(0, right);
        Bottom = Math.Max(0, bottom);
    }

    public static implicit operator EdgeInsets(float value) => new(value, value, value, value);

    public static EdgeInsets All(float value) => new(value, value, value, value);

    public static EdgeInsets Only(float left, float top, float right, float bottom) =>
        new(left, top, right, bottom);

    public EdgeInsets Clone() => new EdgeInsets(Left, Top, Right, Bottom);

    public static bool operator ==(EdgeInsets left, EdgeInsets right) => left.Equals(right);

    public static bool operator !=(EdgeInsets left, EdgeInsets right) => !left.Equals(right);

    public bool Equals(EdgeInsets other) => Left == other.Left && Top == other.Top &&
                                            Right == other.Right && Bottom == other.Bottom;

#if !__WEB__
    public override bool Equals(object? obj) => obj is EdgeInsets other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);
#endif
}