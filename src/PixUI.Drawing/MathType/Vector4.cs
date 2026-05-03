using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PixUI;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Vector4 : IEquatable<Vector4>
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }
    public float W { get; }

    public Vector4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    // public void SetIdentity()
    // {
    //     X = Y = Z = 0;
    //     W = 1;
    // }

    #region ====IEquatable====

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public readonly bool Equals(Vector4 other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

    public readonly override bool Equals(object? obj) => obj is Vector4 f && Equals(f);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    public static bool operator ==(Vector4 left, Vector4 right) => left.Equals(right);

    public static bool operator !=(Vector4 left, Vector4 right) => !left.Equals(right);

    #endregion
}