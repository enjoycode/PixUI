using System.Runtime.InteropServices;

namespace PixUI;

[StructLayout(LayoutKind.Sequential)]
public partial struct Size : IEquatable<Size>
{
    public float Width { get; set; }

    public float Height { get; set; }

    public readonly bool Equals(Size obj) =>
        FloatUtils.NearlyEqual(Width, obj.Width) && FloatUtils.NearlyEqual(Height, obj.Height);

    public readonly override bool Equals(object? obj) =>
        obj is Size f && Equals(f);

    public static bool operator ==(Size left, Size right) =>
        left.Equals(right);

    public static bool operator !=(Size left, Size right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Width);
        hash.Add(Height);
        return hash.ToHashCode();
    }
}

[StructLayout(LayoutKind.Sequential)]
public partial struct SizeI : IEquatable<SizeI>
{
    public int Width { get; set; }

    public int Height { get; set; }

    public readonly bool Equals(SizeI obj) =>
        Width == obj.Width && Height == obj.Height;

    public readonly override bool Equals(object? obj) =>
        obj is SizeI f && Equals(f);

    public static bool operator ==(SizeI left, SizeI right) => left.Equals(right);

    public static bool operator !=(SizeI left, SizeI right) => !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Width);
        hash.Add(Height);
        return hash.ToHashCode();
    }
}