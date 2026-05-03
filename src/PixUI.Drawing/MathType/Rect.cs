using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PixUI;

[StructLayout(LayoutKind.Sequential)]
public partial struct Rect : IEquatable<Rect>
{
    public float Left { get; set; }

    public float Top { get; set; }

    public float Right { get; set; }

    public float Bottom { get; set; }

    public float X
    {
        readonly get => Left;
        set
        {
            var diff = value - Left;
            Left = value;
            Right += diff;
        }
    }

    public float Y
    {
        readonly get => Top;
        set
        {
            var diff = value - Top;
            Top = value;
            Bottom += diff;
        }
    }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public readonly bool Equals(Rect obj) =>
        Left == obj.Left && Top == obj.Top && Right == obj.Right && Bottom == obj.Bottom;

    public readonly override bool Equals(object? obj) => obj is Rect f && Equals(f);

    public static bool operator ==(Rect left, Rect right) => left.Equals(right);

    public static bool operator !=(Rect left, Rect right) => !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Left);
        hash.Add(Top);
        hash.Add(Right);
        hash.Add(Bottom);
        return hash.ToHashCode();
    }
}

[StructLayout(LayoutKind.Sequential)]
public partial struct RectI : IEquatable<RectI>
{
    public int X
    {
        get => Left;
        set
        {
            var diff = value - Left;
            Left = value;
            Right += diff;
        }
    }

    public int Y
    {
        get => Top;
        set
        {
            var diff = value - Top;
            Top = value;
            Bottom += diff;
        }
    }

    public int Left { get; set; }

    public int Top { get; set; }

    public int Right { get; set; }

    public int Bottom { get; set; }

    public readonly bool Equals(RectI obj) =>
        Left == obj.Left && Top == obj.Top && Right == obj.Right && Bottom == obj.Bottom;

    public readonly override bool Equals(object? obj) => obj is RectI f && Equals(f);

    public static bool operator ==(RectI left, RectI right) => left.Equals(right);

    public static bool operator !=(RectI left, RectI right) => !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Left);
        hash.Add(Top);
        hash.Add(Right);
        hash.Add(Bottom);
        return hash.ToHashCode();
    }
}