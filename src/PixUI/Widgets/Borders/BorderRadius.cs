using System;

namespace PixUI;

public readonly struct BorderRadius : IEquatable<BorderRadius>
{
    public static readonly BorderRadius Empty = BorderRadius.All(Radius.Empty);

    #region ====Factory====

    public static BorderRadius All(Radius radius)
        => new BorderRadius(radius, radius, radius, radius);

    public static BorderRadius Circular(float radius)
        => BorderRadius.All(Radius.Circular(radius));

    public static BorderRadius Vertical(Radius top, Radius bottom)
        => new BorderRadius(top, top, bottom, bottom);

    public static BorderRadius Horizontal(Radius left, Radius right)
        => new BorderRadius(left, right, left, right);

    #endregion

    public readonly Radius TopLeft;
    public readonly Radius TopRight;
    public readonly Radius BottomLeft;
    public readonly Radius BottomRight;

    private BorderRadius(Radius topLeft, Radius topRight, Radius bottomLeft, Radius bottomRight)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomLeft = bottomLeft;
        BottomRight = bottomRight;
    }

    public static BorderRadius? Lerp(BorderRadius? a, BorderRadius? b, double t)
    {
        if (a == null && b == null)
            return null;
        if (a == null)
            return b!.Value * (float)t;
        if (b == null)
            return a.Value * (float)(1.0 - t);

        return new BorderRadius(Radius.Lerp(a.Value.TopLeft, b.Value.TopLeft, t)!.Value,
            Radius.Lerp(a.Value.TopRight, b.Value.TopRight, t)!.Value,
            Radius.Lerp(a.Value.BottomLeft, b.Value.BottomLeft, t)!.Value,
            Radius.Lerp(a.Value.BottomRight, b.Value.BottomRight, t)!.Value);
    }

    public static BorderRadius operator *(BorderRadius pt, float operand) =>
        new BorderRadius(pt.TopLeft * operand, pt.TopRight * operand,
            pt.BottomLeft * operand, pt.BottomRight * operand);

    public bool Equals(BorderRadius other) =>
        TopLeft == other.TopLeft && TopRight == other.TopRight &&
        BottomLeft == other.BottomLeft && BottomRight == other.BottomRight;

    public static bool operator ==(BorderRadius left, BorderRadius right) => left.Equals(right);

    public static bool operator !=(BorderRadius left, BorderRadius right) => !left.Equals(right);

    public RRect ToRRect(in Rect rect) =>
        RRect.FromRectAndCorner(rect, TopLeft, TopRight, BottomLeft, BottomRight);

    public BorderRadius Clone() => new BorderRadius(TopLeft, TopRight, BottomLeft, BottomRight);
}