#if !__WEB__
using System;
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
        float skewY, float scaleY, float m5,
        float persp0, float persp1, float persp2)
    {
        this.M0 = scaleX;
        this.M1 = skewX;
        this.M2 = transX;
        this.M3 = skewY;
        this.M4 = scaleY;
        this.M5 = m5;
        this.M6 = persp0;
        this.M7 = persp1;
        this.M8 = persp2;
    }


    public const float DegreesToRadians = (float)Math.PI / 180.0f;

    public readonly static Matrix3 Empty;
    public readonly static Matrix3 Identity = new() { M0 = 1, M4 = 1, M8 = 1 };

    #region ====Fields====

    //[ScaleX, SkewX,  TransX]
    //[SkewY,  ScaleY, TransY]
    //[Persp0, Persp1, Persp2]

    /// <summary>
    /// ScaleX
    /// </summary>
    public float M0 { get; private set; }

    /// <summary>
    /// SkewX
    /// </summary>
    public float M1 { get; private set; }

    /// <summary>
    /// TransX
    /// </summary>
    public float M2 { get; private set; }

    /// <summary>
    /// SkewY
    /// </summary>
    public float M3 { get; private set; }

    /// <summary>
    /// ScaleY
    /// </summary>
    public float M4 { get; private set; }

    /// <summary>
    /// TransY
    /// </summary>
    public float M5 { get; private set; }

    /// <summary>
    /// Persp0
    /// </summary>
    public float M6 { get; private set; }

    /// <summary>
    /// Persp1
    /// </summary>
    public float M7 { get; private set; }

    /// <summary>
    /// Persp2
    /// </summary>
    public float M8 { get; private set; }

    #endregion

    public readonly bool IsIdentity => Equals(Identity);


    #region ====Create Methods====

    public static Matrix3 CreateIdentity() => new() { M0 = 1, M4 = 1, M8 = 1 };

    public static Matrix3 CreateTranslation(float x, float y)
    {
        if (x == 0 && y == 0)
            return Identity;

        return new Matrix3
        {
            M0 = 1,
            M4 = 1,
            M2 = x,
            M5 = y,
            M8 = 1,
        };
    }

    public static Matrix3 CreateScale(float x, float y)
    {
        if (x == 1 && y == 1)
            return Identity;

        return new Matrix3
        {
            M0 = x,
            M4 = y,
            M8 = 1,
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
            M0 = x,
            M4 = y,
            M2 = tx,
            M5 = ty,
            M8 = 1,
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
            M0 = 1,
            M1 = x,
            M3 = y,
            M4 = 1,
            M8 = 1,
        };
    }

    public static Matrix3 CreateScaleTranslation(float sx, float sy, float tx, float ty)
    {
        if (sx == 0 && sy == 0 && tx == 0 && ty == 0)
            return Identity;

        return new Matrix3
        {
            M0 = sx,
            M1 = 0,
            M2 = tx,

            M3 = 0,
            M4 = sy,
            M5 = ty,

            M6 = 0,
            M7 = 0,
            M8 = 1,
        };
    }

    #endregion

    public readonly bool IsInvertible
    {
        get
        {
            fixed (Matrix3* t = &this)
            {
                return SkiaApi.sk_matrix_try_invert(t, null);
            }
        }
    }

    public readonly bool TryInvert(out Matrix3 inverse)
    {
        fixed (Matrix3* i = &inverse)
        fixed (Matrix3* t = &this)
        {
            return SkiaApi.sk_matrix_try_invert(t, i);
        }
    }

    public void Invert()
    {
        if (TryInvert(out var matrix))
            this = matrix;
    }

    // *Concat
    public void Multiply(in Matrix3 arg)
    {
        var m00 = M0;
        var m01 = M3;
        var m02 = M6;
        var m10 = M1;
        var m11 = M4;
        var m12 = M7;
        var m20 = M2;
        var m21 = M5;
        var m22 = M8;
        var n00 = arg.M0;
        var n01 = arg.M3;
        var n02 = arg.M6;
        var n10 = arg.M1;
        var n11 = arg.M4;
        var n12 = arg.M7;
        var n20 = arg.M2;
        var n21 = arg.M5;
        var n22 = arg.M8;
        M0 = (m00 * n00) + (m01 * n10) + (m02 * n20);
        M3 = (m00 * n01) + (m01 * n11) + (m02 * n21);
        M6 = (m00 * n02) + (m01 * n12) + (m02 * n22);
        M1 = (m10 * n00) + (m11 * n10) + (m12 * n20);
        M4 = (m10 * n01) + (m11 * n11) + (m12 * n21);
        M7 = (m10 * n02) + (m11 * n12) + (m12 * n22);
        M2 = (m20 * n00) + (m21 * n10) + (m22 * n20);
        M5 = (m20 * n01) + (m21 * n11) + (m22 * n21);
        M8 = (m20 * n02) + (m21 * n12) + (m22 * n22);
    }

    // MapPoints

    public readonly Point MapPoint(float x, float y)
    {
        Point result;
        fixed (Matrix3* t = &this)
        {
            SkiaApi.sk_matrix_map_xy(t, x, y, &result);
        }

        return result;
    }

    public readonly PointI MapPointI(int x, int y)
    {
        var pt = MapPoint(x, y);
        return new PointI((int)pt.X, (int)pt.Y);
    }

    public readonly void MapPointsI(PointI[] points)
    {
        for (var i = 0; i < points.Length; i++)
        {
            points[i] = MapPointI(points[i].X, points[i].Y);
        }
    }

    // MapVectors

    public readonly Point MapVector(Point vector) =>
        MapVector(vector.X, vector.Y);

    public readonly Point MapVector(float x, float y)
    {
        Point result;
        fixed (Matrix3* t = &this)
        {
            SkiaApi.sk_matrix_map_vector(t, x, y, &result);
        }

        return result;
    }

    public readonly void MapVectors(Point[] result, Point[] vectors)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));
        if (vectors == null)
            throw new ArgumentNullException(nameof(vectors));
        if (result.Length != vectors.Length)
            throw new ArgumentException("Buffers must be the same size.");

        fixed (Matrix3* t = &this)
        fixed (Point* rp = result)
        fixed (Point* pp = vectors)
        {
            SkiaApi.sk_matrix_map_vectors(t, rp, pp, result.Length);
        }
    }

    public readonly Point[] MapVectors(Point[] vectors)
    {
        if (vectors == null)
            throw new ArgumentNullException(nameof(vectors));

        var res = new Point[vectors.Length];
        MapVectors(res, vectors);
        return res;
    }

    // MapRadius

    public readonly float MapRadius(float radius)
    {
        fixed (Matrix3* t = &this)
        {
            return SkiaApi.sk_matrix_map_radius(t, radius);
        }
    }

    // private

    private static void SetSinCos(ref Matrix3 matrix, float sin, float cos)
    {
        matrix.M0 = cos;
        matrix.M1 = -sin;
        matrix.M2 = 0;
        matrix.M3 = sin;
        matrix.M4 = cos;
        matrix.M5 = 0;
        matrix.M6 = 0;
        matrix.M7 = 0;
        matrix.M8 = 1;
    }

    private static void SetSinCos(ref Matrix3 matrix, float sin, float cos, float pivotx,
        float pivoty)
    {
        float oneMinusCos = 1 - cos;

        matrix.M0 = cos;
        matrix.M1 = -sin;
        matrix.M2 = Dot(sin, pivoty, oneMinusCos, pivotx);
        matrix.M3 = sin;
        matrix.M4 = cos;
        matrix.M5 = Dot(-sin, pivotx, oneMinusCos, pivoty);
        matrix.M6 = 0;
        matrix.M7 = 0;
        matrix.M8 = 1;
    }

    private static float Dot(float a, float b, float c, float d) =>
        a * b + c * d;

    private static float Cross(float a, float b, float c, float d) =>
        a * b - c * d;

    #region ====Overrides====

    public readonly bool Equals(Matrix3 obj) =>
        M0 == obj.M0 && M1 == obj.M1 && M2 == obj.M2 &&
        M3 == obj.M3 && M4 == obj.M4 && M5 == obj.M5 &&
        M6 == obj.M6 && M7 == obj.M7 && M8 == obj.M8;

    public readonly override bool Equals(object obj) => obj is PixUI.Matrix3 f && Equals(f);

    public static bool operator ==(Matrix3 left, Matrix3 right) => left.Equals(right);

    public static bool operator !=(Matrix3 left, Matrix3 right) => !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(M0);
        hash.Add(M1);
        hash.Add(M2);
        hash.Add(M3);
        hash.Add(M4);
        hash.Add(M5);
        hash.Add(M6);
        hash.Add(M7);
        hash.Add(M8);
        return hash.ToHashCode();
    }

    #endregion
}
#endif