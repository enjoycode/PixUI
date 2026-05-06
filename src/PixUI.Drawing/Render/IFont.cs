using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PixUI;

[StructLayout(LayoutKind.Sequential)]
public struct FontMetrics : IEquatable<FontMetrics>
{
    public uint Flags;
    public float Top;
    public float Ascent;
    public float Descent;
    public float Bottom;
    public float Leading;
    public float AvgCharWidth;
    public float MaxCharWidth;
    public float XMin;
    public float XMax;
    public float XHeight;
    public float CapHeight;
    public float UnderlineThickness;
    public float UnderlinePosition;
    public float StrikeoutThickness;
    public float StrikeoutPosition;

    public bool HasUnderlineThickness => (Flags & 1) == 1;
    public bool HasUnderlinePosition => (Flags & 2) == 2;
    public bool HasStrikeoutThickness => (Flags & 4) == 4;
    public bool HasStrikeoutPosition => (Flags & 8) == 8;

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public readonly bool Equals(FontMetrics obj) =>
        Flags == obj.Flags && Top == obj.Top && Ascent == obj.Ascent &&
        Descent == obj.Descent && Bottom == obj.Bottom && Leading == obj.Leading &&
        AvgCharWidth == obj.AvgCharWidth && MaxCharWidth == obj.MaxCharWidth &&
        XMin == obj.XMin && XMax == obj.XMax && XHeight == obj.XHeight &&
        CapHeight == obj.CapHeight && UnderlineThickness == obj.UnderlineThickness &&
        UnderlinePosition == obj.UnderlinePosition &&
        StrikeoutThickness == obj.StrikeoutThickness &&
        StrikeoutPosition == obj.StrikeoutPosition;

    public readonly override bool Equals(object? obj) =>
        obj is FontMetrics f && Equals(f);

    public static bool operator ==(FontMetrics left, FontMetrics right) =>
        left.Equals(right);

    public static bool operator !=(FontMetrics left, FontMetrics right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Flags);
        hash.Add(Top);
        hash.Add(Ascent);
        hash.Add(Descent);
        hash.Add(Bottom);
        hash.Add(Leading);
        hash.Add(AvgCharWidth);
        hash.Add(MaxCharWidth);
        hash.Add(XMin);
        hash.Add(XMax);
        hash.Add(XHeight);
        hash.Add(CapHeight);
        hash.Add(UnderlineThickness);
        hash.Add(UnderlinePosition);
        hash.Add(StrikeoutThickness);
        hash.Add(StrikeoutPosition);
        return hash.ToHashCode();
    }
}

public interface IFont : IDisposable
{
    string Name { get; }

    ITypeface? Typeface { get; set; }

    /// <summary>
    /// 以像素为单位的行距
    /// </summary>
    /// <remarks>
    /// 行距是两个连续文本行的基线之间的垂直距离。 因此，行距包括行之间的空格以及字符本身的高度。
    /// </remarks>
    int Height { get; }

    /// <summary>
    /// Size in points
    /// </summary>
    float Size { get; set; }

    ushort GetGlyphId(int codepoint);

    FontMetrics GetMetrics();

    Size MeasureString(string text);
}