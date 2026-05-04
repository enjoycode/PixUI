using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PixUI;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Matrix3 : IEquatable<Matrix3>
{
    public Matrix3()
    {
        this = Identity;
    }

    public Matrix3(
        float scaleX, float skewX, float transX,
        float skewY, float scaleY, float transY,
        float persp0, float persp1, float persp2)
    {
        ScaleX = scaleX;
        SkewX = skewX;
        TransX = transX;
        SkewY = skewY;
        ScaleY = scaleY;
        TransY = transY;
        Persp0 = persp0;
        Persp1 = persp1;
        Persp2 = persp2;
    }

    public const float DegreesToRadians = (float)Math.PI / 180.0f;

    public static readonly Matrix3 Empty;
    public static readonly Matrix3 Identity = new() { ScaleX = 1, ScaleY = 1, Persp2 = 1 };

    #region ====Fields====

    //[ScaleX, SkewX,  TransX]
    //[SkewY,  ScaleY, TransY]
    //[Persp0, Persp1, Persp2]
    
    public float ScaleX { get; private set; }
    public float SkewX { get; private set; }
    public float TransX { get; private set; }
    public float SkewY { get; private set; }
    public float ScaleY { get; private set; }
    public float TransY { get; private set; }
    public float Persp0 { get; private set; }
    public float Persp1 { get; private set; }
    public float Persp2 { get; private set; }

    private int Type;

    #endregion

    public readonly bool IsIdentity => Equals(Identity);

    #region ====Create Methods====

    public static Matrix3 CreateIdentity() => new() { ScaleX = 1, ScaleY = 1, Persp2 = 1 };

    public static Matrix3 CreateTranslation(float x, float y)
    {
        if (x == 0 && y == 0)
            return Identity;

        return new Matrix3
        {
            ScaleX = 1,
            ScaleY = 1,
            TransX = x,
            TransY = y,
            Persp2 = 1,
        };
    }

    public static Matrix3 CreateScale(float x, float y)
    {
        if (x == 1 && y == 1)
            return Identity;

        return new Matrix3
        {
            ScaleX = x,
            ScaleY = y,
            Persp2 = 1,
        };
    }

    public static Matrix3 CreateScale(float x, float y, float pivotX, float pivotY)
    {
        if (x == 1 && y == 1)
            return Identity;

        var tx = pivotX - x * pivotX;
        var ty = pivotY - y * pivotY;

        return new Matrix3
        {
            ScaleX = x,
            ScaleY = y,
            TransX = tx,
            TransY = ty,
            Persp2 = 1,
        };
    }

    public static Matrix3 CreateRotation(float radians)
    {
        if (radians == 0)
            return Identity;

        var sin = (float)Math.Sin(radians);
        var cos = (float)Math.Cos(radians);

        var matrix = Identity;
        SetSinCos(ref matrix, sin, cos);
        return matrix;
    }

    public static Matrix3 CreateRotation(float radians, float pivotX, float pivotY)
    {
        if (radians == 0)
            return Identity;

        var sin = (float)Math.Sin(radians);
        var cos = (float)Math.Cos(radians);

        var matrix = Identity;
        SetSinCos(ref matrix, sin, cos, pivotX, pivotY);
        return matrix;
    }

    public static Matrix3 CreateRotationDegrees(float degrees)
    {
        if (degrees == 0)
            return Identity;

        return CreateRotation(degrees * DegreesToRadians);
    }

    public static Matrix3 CreateRotationDegrees(float degrees, float pivotX, float pivotY)
    {
        if (degrees == 0)
            return Identity;

        return CreateRotation(degrees * DegreesToRadians, pivotX, pivotY);
    }

    public static Matrix3 CreateSkew(float x, float y)
    {
        if (x == 0 && y == 0)
            return Identity;

        return new Matrix3
        {
            ScaleX = 1,
            SkewX = x,
            SkewY = y,
            ScaleY = 1,
            Persp2 = 1,
        };
    }

    public static Matrix3 CreateScaleTranslation(float sx, float sy, float tx, float ty)
    {
        if (sx == 0 && sy == 0 && tx == 0 && ty == 0)
            return Identity;

        return new Matrix3
        {
            ScaleX = sx,
            SkewX = 0,
            TransX = tx,

            SkewY = 0,
            ScaleY = sy,
            TransY = ty,

            Persp0 = 0,
            Persp1 = 0,
            Persp2 = 1,
        };
    }

    #endregion

    public readonly bool IsInvertible => TryInvert(out _);

    public readonly bool TryInvert(out Matrix3 inverse)
    {
        //TODO: use Matrix4 logic now
        Matrix4 matrix4 = this;
        var result = Matrix4.TryInvert(matrix4);
        inverse = result?.ToMatrix3() ?? Empty;
        return result.HasValue;
    }

    public void Invert()
    {
        if (TryInvert(out var inverse))
            this = inverse;
    }

    // *Concat
    public void Multiply(in Matrix3 arg)
    {
        var m00 = ScaleX;
        var m01 = SkewY;
        var m02 = Persp0;
        var m10 = SkewX;
        var m11 = ScaleY;
        var m12 = Persp1;
        var m20 = TransX;
        var m21 = TransY;
        var m22 = Persp2;
        var n00 = arg.ScaleX;
        var n01 = arg.SkewY;
        var n02 = arg.Persp0;
        var n10 = arg.SkewX;
        var n11 = arg.ScaleY;
        var n12 = arg.Persp1;
        var n20 = arg.TransX;
        var n21 = arg.TransY;
        var n22 = arg.Persp2;
        ScaleX = (m00 * n00) + (m01 * n10) + (m02 * n20);
        SkewY = (m00 * n01) + (m01 * n11) + (m02 * n21);
        Persp0 = (m00 * n02) + (m01 * n12) + (m02 * n22);
        SkewX = (m10 * n00) + (m11 * n10) + (m12 * n20);
        ScaleY = (m10 * n01) + (m11 * n11) + (m12 * n21);
        Persp1 = (m10 * n02) + (m11 * n12) + (m12 * n22);
        TransX = (m20 * n00) + (m21 * n10) + (m22 * n20);
        TransY = (m20 * n01) + (m21 * n11) + (m22 * n21);
        Persp2 = (m20 * n02) + (m21 * n12) + (m22 * n22);
    }

    // MapPoints

    public readonly Point MapPoint(float x, float y)
    {
        //TODO: reuse Matrix4 logic now
        return ((Matrix4)this).MapPoint(x, y);
    }

    public readonly PointI MapPointI(int x, int y)
    {
        var pt = MapPoint(x, y);
        return new PointI((int)pt.X, (int)pt.Y);
    }

    private static void SetSinCos(ref Matrix3 matrix, float sin, float cos)
    {
        matrix.ScaleX = cos;
        matrix.SkewX = -sin;
        matrix.TransX = 0;
        matrix.SkewY = sin;
        matrix.ScaleY = cos;
        matrix.TransY = 0;
        matrix.Persp0 = 0;
        matrix.Persp1 = 0;
        matrix.Persp2 = 1;
    }

    private static void SetSinCos(ref Matrix3 matrix, float sin, float cos, float pivotX, float pivotY)
    {
        float oneMinusCos = 1 - cos;

        matrix.ScaleX = cos;
        matrix.SkewX = -sin;
        matrix.TransX = Dot(sin, pivotY, oneMinusCos, pivotX);
        matrix.SkewY = sin;
        matrix.ScaleY = cos;
        matrix.TransY = Dot(-sin, pivotX, oneMinusCos, pivotY);
        matrix.Persp0 = 0;
        matrix.Persp1 = 0;
        matrix.Persp2 = 1;
    }

    private static float Dot(float a, float b, float c, float d) =>
        a * b + c * d;

    private static float Cross(float a, float b, float c, float d) =>
        a * b - c * d;

    #region ====Overrides====

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public readonly bool Equals(Matrix3 obj) =>
        ScaleX == obj.ScaleX && SkewX == obj.SkewX && TransX == obj.TransX &&
        SkewY == obj.SkewY && ScaleY == obj.ScaleY && TransY == obj.TransY &&
        Persp0 == obj.Persp0 && Persp1 == obj.Persp1 && Persp2 == obj.Persp2;

    public readonly override bool Equals(object? obj) => obj is Matrix3 f && Equals(f);

    public static bool operator ==(Matrix3 left, Matrix3 right) => left.Equals(right);

    public static bool operator !=(Matrix3 left, Matrix3 right) => !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(ScaleX);
        hash.Add(SkewX);
        hash.Add(TransX);
        hash.Add(SkewY);
        hash.Add(ScaleY);
        hash.Add(TransY);
        hash.Add(Persp0);
        hash.Add(Persp1);
        hash.Add(Persp2);
        return hash.ToHashCode();
    }

    #endregion
}