#if !__WEB__
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PixUI;

/// <summary>
/// Matrix 4x4
/// </summary>
/// <remarks>
/// Values are stored in column major order.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public struct Matrix4 : IEquatable<Matrix4>
{
    public bool IsIdentity => Equals(CreateIdentity());

    #region ====Fields====

    public float M00 { get; private set; }
    public float M01 { get; private set; }
    public float M02 { get; private set; }
    public float M03 { get; private set; }

    public float M10 { get; private set; }
    public float M11 { get; private set; }
    public float M12 { get; private set; }
    public float M13 { get; private set; }

    public float M20 { get; private set; }
    public float M21 { get; private set; }
    public float M22 { get; private set; }
    public float M23 { get; private set; }

    public float M30 { get; private set; }
    public float M31 { get; private set; }
    public float M32 { get; private set; }
    public float M33 { get; private set; }

    #endregion

    #region ====Ctor====

    public Matrix4(
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33)
    {
        M00 = m00;
        M01 = m01;
        M02 = m02;
        M03 = m03;

        M10 = m10;
        M11 = m11;
        M12 = m12;
        M13 = m13;

        M20 = m20;
        M21 = m21;
        M22 = m22;
        M23 = m23;

        M30 = m30;
        M31 = m31;
        M32 = m32;
        M33 = m33;
    }

    public Matrix4(Matrix3 src)
    {
        M00 = src.ScaleX;
        M01 = src.SkewX;
        M02 = 0;
        M03 = src.Persp0;

        M10 = src.SkewY;
        M11 = src.ScaleY;
        M12 = 0;
        M13 = src.Persp1;

        M20 = 0;
        M21 = 0;
        M22 = 1;
        M23 = 0;

        M30 = src.TransX;
        M31 = src.TransY;
        M32 = 0;
        M33 = src.Persp2;
    }

    #endregion

    #region ====Create====

    public static Matrix4 CreateIdentity() =>
        new(1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);

    public static Matrix4 CreateTranslation(float x, float y, float z = 0) =>
        new(1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            x, y, z, 1);

    public static Matrix4 CreateScale(float x, float y, float z = 1) =>
        new(x, 0, 0, 0,
            0, y, 0, 0,
            0, 0, z, 0,
            0, 0, 0, 1);

    public static Matrix4 CreateScaleTranslation(float sx, float sy, float tx, float ty)
        => new Matrix4(Matrix3.CreateScaleTranslation(sx, sy, tx, ty));

    public static Matrix4 CreateRotation(float x, float y, float z, float radians)
    {
        var matrix = new Matrix4();
        matrix.SetRotationAbout(x, y, z, radians);
        return matrix;
    }

    public static Matrix4 CreateRotationDegrees(float x, float y, float z, float degrees)
    {
        var matrix = new Matrix4();
        matrix.SetRotationAboutDegrees(x, y, z, degrees);
        return matrix;
    }

    public static Matrix4 CreateRotationDegrees(float degrees, float x = 0, float y = 0) =>
        CreateRotationDegrees(x, y, 0, degrees);

    #endregion

    #region ====FromXXX====

    // public static Matrix4 FromRowMajor(ReadOnlySpan<float> src)
    // {
    //     var matrix = new Matrix4();
    //     matrix.SetRowMajor(src);
    //     return matrix;
    // }
    //
    // public static Matrix4 FromColumnMajor(ReadOnlySpan<float> src)
    // {
    //     var matrix = new Matrix4();
    //     matrix.SetColumnMajor(src);
    //     return matrix;
    // }

    #endregion

    #region ====ToXXX====

    // public readonly float[] ToColumnMajor()
    // {
    //     var dst = new float[16];
    //     ToColumnMajor(dst);
    //     return dst;
    // }
    //
    // public readonly void ToColumnMajor(Span<float> dst)
    // {
    //     if (dst == null)
    //         throw new ArgumentNullException(nameof(dst));
    //     if (dst.Length != 16)
    //         throw new ArgumentException("The destination array must be 16 entries.",
    //             nameof(dst));
    //
    //     dst[0] = M0;
    //     dst[1] = M1;
    //     dst[2] = M2;
    //     dst[3] = M3;
    //
    //     dst[4] = M4;
    //     dst[5] = M5;
    //     dst[6] = M6;
    //     dst[7] = M7;
    //
    //     dst[8] = M8;
    //     dst[9] = M9;
    //     dst[10] = M10;
    //     dst[11] = M11;
    //
    //     dst[12] = M12;
    //     dst[13] = M13;
    //     dst[14] = M14;
    //     dst[15] = M15;
    // }
    //
    // public readonly float[] ToRowMajor()
    // {
    //     var dst = new float[16];
    //     ToRowMajor(dst);
    //     return dst;
    // }
    //
    // public readonly void ToRowMajor(Span<float> dst)
    // {
    //     if (dst == null)
    //         throw new ArgumentNullException(nameof(dst));
    //     if (dst.Length != 16)
    //         throw new ArgumentException("The destination array must be 16 entries.",
    //             nameof(dst));
    //
    //     dst[0] = M0;
    //     dst[1] = M4;
    //     dst[2] = M8;
    //     dst[3] = M12;
    //
    //     dst[4] = M1;
    //     dst[5] = M5;
    //     dst[6] = M9;
    //     dst[7] = M13;
    //
    //     dst[8] = M2;
    //     dst[9] = M6;
    //     dst[10] = M10;
    //     dst[11] = M14;
    //
    //     dst[12] = M3;
    //     dst[13] = M7;
    //     dst[14] = M11;
    //     dst[15] = M15;
    // }

    #endregion

    #region ====SetXXX====

    /// <summary>
    /// Return index in storage for [row], [col] value.
    /// </summary>
    private static int GetIndex(int row, int col) => (col * 4) + row;

    public void SetIdentity() => this = CreateIdentity();

    public void SetColumnMajor(ReadOnlySpan<float> src)
    {
        if (src == null)
            throw new ArgumentNullException(nameof(src));
        if (src.Length != 16)
            throw new ArgumentException("The source array must be 16 entries.", nameof(src));

        M00 = src[0];
        M01 = src[1];
        M02 = src[2];
        M03 = src[3];

        M10 = src[4];
        M11 = src[5];
        M12 = src[6];
        M13 = src[7];

        M20 = src[8];
        M21 = src[9];
        M22 = src[10];
        M23 = src[11];

        M30 = src[12];
        M31 = src[13];
        M32 = src[14];
        M33 = src[15];
    }

    public void SetRowMajor(ReadOnlySpan<float> src)
    {
        if (src == null)
            throw new ArgumentNullException(nameof(src));
        if (src.Length != 16)
            throw new ArgumentException("The source array must be 16 entries.", nameof(src));

        M00 = src[0];
        M10 = src[1];
        M20 = src[2];
        M30 = src[3];

        M01 = src[4];
        M11 = src[5];
        M21 = src[6];
        M31 = src[7];

        M02 = src[8];
        M12 = src[9];
        M22 = src[10];
        M32 = src[11];

        M03 = src[12];
        M13 = src[13];
        M23 = src[14];
        M33 = src[15];
    }

    public unsafe void SetColumn(int column, Vector4 arg)
    {
        var entry = column * 4;
        fixed (Matrix4* mptr = &this)
        {
            var ptr = (float*)mptr;
            ptr[entry + 3] = arg.W;
            ptr[entry + 2] = arg.Z;
            ptr[entry + 1] = arg.Y;
            ptr[entry] = arg.X;
        }
    }

    public unsafe void SetRow(int row, Vector4 arg)
    {
        fixed (Matrix4* mptr = &this)
        {
            var ptr = (float*)mptr;

            ptr[GetIndex(row, 0)] = arg.X;
            ptr[GetIndex(row, 1)] = arg.Y;
            ptr[GetIndex(row, 2)] = arg.Z;
            ptr[GetIndex(row, 3)] = arg.W;
        }
    }

    public void Set3x3ColumnMajor(ReadOnlySpan<float> src)
    {
        if (src == null)
            throw new ArgumentNullException(nameof(src));
        if (src.Length != 9)
            throw new ArgumentException("The source array must be 9 entries.", nameof(src));

        M00 = src[0];
        M10 = src[1];
        M20 = 0;
        M30 = src[2];

        M01 = src[3];
        M11 = src[4];
        M21 = 0;
        M31 = src[5];

        M02 = 0;
        M12 = 0;
        M22 = 1;
        M32 = 0;

        M03 = src[6];
        M13 = src[7];
        M23 = 0;
        M33 = src[8];
    }

    public void Set3x3RowMajor(ReadOnlySpan<float> src)
    {
        if (src == null)
            throw new ArgumentNullException(nameof(src));
        if (src.Length != 9)
            throw new ArgumentException("The source array must be 9 entries.", nameof(src));

        M00 = src[0];
        M01 = src[1];
        M02 = 0;
        M03 = src[2];

        M10 = src[3];
        M11 = src[4];
        M12 = 0;
        M13 = src[5];

        M20 = 0;
        M21 = 0;
        M22 = 1;
        M23 = 0;

        M30 = src[6];
        M31 = src[7];
        M32 = 0;
        M33 = src[8];
    }

    public void SetTranslation(float dx, float dy, float dz) =>
        this = CreateTranslation(dx, dy, dz);

    public void SetScale(float sx, float sy, float sz) =>
        this = CreateScale(sx, sy, sz);

    public void SetRotationAboutDegrees(float x, float y, float z, float degrees) =>
        SetRotationAbout(x, y, z, degrees * Matrix3.DegreesToRadians);

    public void SetRotationAbout(float x, float y, float z, float radians)
    {
        var length = x * x + y * y + z * z;

        if (length == 0)
        {
            SetIdentity();
            return;
        }

        if (length != 1)
        {
            var scale = 1 / (float)MathF.Sqrt(length);
            x *= scale;
            y *= scale;
            z *= scale;
        }

        SetRotationAboutUnit(x, y, z, radians);
    }

    public void SetRotationAboutUnit(float x, float y, float z, float radians)
    {
        var sa = (float)MathF.Sin(radians);
        var ca = (float)MathF.Cos(radians);
        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var xy = x * y;
        var xz = x * z;
        var yz = y * z;

        M00 = xx + ca * (1.0f - xx);
        M01 = xy - ca * xy + sa * z;
        M02 = xz - ca * xz - sa * y;
        M03 = 0;

        M10 = xy - ca * xy - sa * z;
        M11 = yy + ca * (1.0f - yy);
        M12 = yz - ca * yz + sa * x;
        M13 = 0;

        M20 = xz - ca * xz + sa * y;
        M21 = yz - ca * yz - sa * x;
        M22 = zz + ca * (1.0f - zz);
        M23 = 0;

        M30 = 0;
        M31 = 0;
        M32 = 0;
        M33 = 1;
    }

    public void SetConcat(Matrix4 a, Matrix4 b) =>
        this = new(
            a.M00 * b.M00 + a.M01 * b.M10 + a.M02 * b.M20 + a.M03 * b.M30,
            a.M00 * b.M01 + a.M01 * b.M11 + a.M02 * b.M21 + a.M03 * b.M31,
            a.M00 * b.M02 + a.M01 * b.M12 + a.M02 * b.M22 + a.M03 * b.M32,
            a.M00 * b.M03 + a.M01 * b.M13 + a.M02 * b.M23 + a.M03 * b.M33,
            a.M10 * b.M00 + a.M11 * b.M10 + a.M12 * b.M20 + a.M13 * b.M30,
            a.M10 * b.M01 + a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31,
            a.M10 * b.M02 + a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32,
            a.M10 * b.M03 + a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33,
            a.M20 * b.M00 + a.M21 * b.M10 + a.M22 * b.M20 + a.M23 * b.M30,
            a.M20 * b.M01 + a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31,
            a.M20 * b.M02 + a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32,
            a.M20 * b.M03 + a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33,
            a.M30 * b.M00 + a.M31 * b.M10 + a.M32 * b.M20 + a.M33 * b.M30,
            a.M30 * b.M01 + a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31,
            a.M30 * b.M02 + a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32,
            a.M30 * b.M03 + a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33);

    #endregion

    #region ====Pre* and Post*====

    public void PreTranslate(float dx, float dy, float dz) =>
        PreConcat(CreateTranslation(dx, dy, dz));

    public void PostTranslate(float dx, float dy, float dz) =>
        PostConcat(CreateTranslation(dx, dy, dz));

    public void PreScale(float sx, float sy, float sz) =>
        PreConcat(CreateScale(sx, sy, sz));

    public void PostScale(float sx, float sy, float sz) =>
        PostConcat(CreateScale(sx, sy, sz));

    public void PreConcat(Matrix4 m) => SetConcat(this, m);

    public void PostConcat(Matrix4 m) => SetConcat(m, this);

    #endregion

    #region ====Operations====

    public void Translate(float x, float y = 0.0f, float z = 0.0f)
    {
        M30 = M00 * x + M10 * y + M20 * z + M30;
        M31 = M01 * x + M11 * y + M21 * z + M31;
        M32 = M02 * x + M12 * y + M22 * z + M32;
        M33 = M03 * x + M13 * y + M23 * z + M33;
    }

    public void Scale(float x, float y = 1.0f, float z = 1.0f)
    {
        M00 *= x;
        M01 *= x;
        M02 *= x;
        M03 *= x;
        M10 *= y;
        M11 *= y;
        M12 *= y;
        M13 *= y;
        M20 *= z;
        M21 *= z;
        M22 *= z;
        M23 *= z;
    }

    public void RotateX(float angle)
    {
        var cosAngle = (float)Math.Cos(angle);
        var sinAngle = (float)Math.Sin(angle);
        var t1 = M10 * cosAngle + M20 * sinAngle;
        var t2 = M11 * cosAngle + M21 * sinAngle;
        var t3 = M12 * cosAngle + M22 * sinAngle;
        var t4 = M13 * cosAngle + M23 * sinAngle;
        var t5 = M10 * -sinAngle + M20 * cosAngle;
        var t6 = M11 * -sinAngle + M21 * cosAngle;
        var t7 = M12 * -sinAngle + M22 * cosAngle;
        var t8 = M13 * -sinAngle + M23 * cosAngle;
        M10 = t1;
        M11 = t2;
        M12 = t3;
        M13 = t4;
        M20 = t5;
        M21 = t6;
        M22 = t7;
        M23 = t8;
    }

    public void RotateZ(float angle)
    {
        var cosAngle = (float)Math.Cos(angle);
        var sinAngle = (float)Math.Sin(angle);
        var t1 = M00 * cosAngle + M10 * sinAngle;
        var t2 = M01 * cosAngle + M11 * sinAngle;
        var t3 = M02 * cosAngle + M12 * sinAngle;
        var t4 = M03 * cosAngle + M13 * sinAngle;
        var t5 = M00 * -sinAngle + M10 * cosAngle;
        var t6 = M01 * -sinAngle + M11 * cosAngle;
        var t7 = M02 * -sinAngle + M12 * cosAngle;
        var t8 = M03 * -sinAngle + M13 * cosAngle;
        M00 = t1;
        M01 = t2;
        M02 = t3;
        M03 = t4;
        M10 = t5;
        M11 = t6;
        M12 = t7;
        M13 = t8;
    }

    public void Multiply(in Matrix4 arg)
    {
        var aM0 = M00;
        var aM4 = M10;
        var aM8 = M20;
        var aM12 = M30;
        var aM1 = M01;
        var aM5 = M11;
        var aM9 = M21;
        var aM13 = M31;
        var aM2 = M02;
        var aM6 = M12;
        var aM10 = M22;
        var aM14 = M32;
        var aM3 = M03;
        var aM7 = M13;
        var aM11 = M23;
        var aM15 = M33;

        var bM0 = arg.M00;
        var bM4 = arg.M10;
        var bM8 = arg.M20;
        var bM12 = arg.M30;
        var bM1 = arg.M01;
        var bM5 = arg.M11;
        var bM9 = arg.M21;
        var bM13 = arg.M31;
        var bM2 = arg.M02;
        var bM6 = arg.M12;
        var bM10 = arg.M22;
        var bM14 = arg.M32;
        var bM3 = arg.M03;
        var bM7 = arg.M13;
        var bM11 = arg.M23;
        var bM15 = arg.M33;
        M00 = (aM0 * bM0) + (aM4 * bM1) + (aM8 * bM2) + (aM12 * bM3);
        M10 = (aM0 * bM4) + (aM4 * bM5) + (aM8 * bM6) + (aM12 * bM7);
        M20 = (aM0 * bM8) + (aM4 * bM9) + (aM8 * bM10) + (aM12 * bM11);
        M30 = (aM0 * bM12) + (aM4 * bM13) + (aM8 * bM14) + (aM12 * bM15);
        M01 = (aM1 * bM0) + (aM5 * bM1) + (aM9 * bM2) + (aM13 * bM3);
        M11 = (aM1 * bM4) + (aM5 * bM5) + (aM9 * bM6) + (aM13 * bM7);
        M21 = (aM1 * bM8) + (aM5 * bM9) + (aM9 * bM10) + (aM13 * bM11);
        M31 = (aM1 * bM12) + (aM5 * bM13) + (aM9 * bM14) + (aM13 * bM15);
        M02 = (aM2 * bM0) + (aM6 * bM1) + (aM10 * bM2) + (aM14 * bM3);
        M12 = (aM2 * bM4) + (aM6 * bM5) + (aM10 * bM6) + (aM14 * bM7);
        M22 = (aM2 * bM8) + (aM6 * bM9) + (aM10 * bM10) + (aM14 * bM11);
        M32 = (aM2 * bM12) + (aM6 * bM13) + (aM10 * bM14) + (aM14 * bM15);
        M03 = (aM3 * bM0) + (aM7 * bM1) + (aM11 * bM2) + (aM15 * bM3);
        M13 = (aM3 * bM4) + (aM7 * bM5) + (aM11 * bM6) + (aM15 * bM7);
        M23 = (aM3 * bM8) + (aM7 * bM9) + (aM11 * bM10) + (aM15 * bM11);
        M33 = (aM3 * bM12) + (aM7 * bM13) + (aM11 * bM14) + (aM15 * bM15);
    }

    #endregion

    #region ====Invert & Transpose====

    /// <summary>
    /// Invert this.
    /// </summary>
    public float Invert() => CopyInverse(this);

    /// <summary>
    /// Set this matrix to be the inverse of [arg]
    /// </summary>
    public float CopyInverse(in Matrix4 arg)
    {
        var a00 = arg.M00;
        var a01 = arg.M01;
        var a02 = arg.M02;
        var a03 = arg.M03;
        var a10 = arg.M10;
        var a11 = arg.M11;
        var a12 = arg.M12;
        var a13 = arg.M13;
        var a20 = arg.M20;
        var a21 = arg.M21;
        var a22 = arg.M22;
        var a23 = arg.M23;
        var a30 = arg.M30;
        var a31 = arg.M31;
        var a32 = arg.M32;
        var a33 = arg.M33;
        var b00 = a00 * a11 - a01 * a10;
        var b01 = a00 * a12 - a02 * a10;
        var b02 = a00 * a13 - a03 * a10;
        var b03 = a01 * a12 - a02 * a11;
        var b04 = a01 * a13 - a03 * a11;
        var b05 = a02 * a13 - a03 * a12;
        var b06 = a20 * a31 - a21 * a30;
        var b07 = a20 * a32 - a22 * a30;
        var b08 = a20 * a33 - a23 * a30;
        var b09 = a21 * a32 - a22 * a31;
        var b10 = a21 * a33 - a23 * a31;
        var b11 = a22 * a33 - a23 * a32;
        var det = b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06;
        if (det == 0.0)
        {
            this = arg;
            return 0;
        }

        var invDet = 1.0f / det;
        M00 = (a11 * b11 - a12 * b10 + a13 * b09) * invDet;
        M01 = (-a01 * b11 + a02 * b10 - a03 * b09) * invDet;
        M02 = (a31 * b05 - a32 * b04 + a33 * b03) * invDet;
        M03 = (-a21 * b05 + a22 * b04 - a23 * b03) * invDet;
        M10 = (-a10 * b11 + a12 * b08 - a13 * b07) * invDet;
        M11 = (a00 * b11 - a02 * b08 + a03 * b07) * invDet;
        M12 = (-a30 * b05 + a32 * b02 - a33 * b01) * invDet;
        M13 = (a20 * b05 - a22 * b02 + a23 * b01) * invDet;
        M20 = (a10 * b10 - a11 * b08 + a13 * b06) * invDet;
        M21 = (-a00 * b10 + a01 * b08 - a03 * b06) * invDet;
        M22 = (a30 * b04 - a31 * b02 + a33 * b00) * invDet;
        M23 = (-a20 * b04 + a21 * b02 - a23 * b00) * invDet;
        M30 = (-a10 * b09 + a11 * b07 - a12 * b06) * invDet;
        M31 = (a00 * b09 - a01 * b07 + a02 * b06) * invDet;
        M32 = (-a30 * b03 + a31 * b01 - a32 * b00) * invDet;
        M33 = (a20 * b03 - a21 * b01 + a22 * b00) * invDet;
        return det;
    }

    /// <summary>
    /// Returns a matrix that is the inverse of [other] if [other] is invertible,
    /// otherwise `null`.
    /// </summary>
    public static Matrix4? TryInvert(in Matrix4 other)
    {
        var res = new Matrix4();
        var determinant = res.CopyInverse(other);
        return determinant == 0 ? null : res;
    }

    public void Transpose() =>
        this = new Matrix4(
            M00, M10, M20, M30,
            M01, M11, M21, M31,
            M02, M12, M22, M32,
            M03, M13, M23, M33);

    #endregion

    #region ====MapXXX====

    // MapVector4

    public readonly float[] MapVector4(float x, float y, float z, float w)
    {
        Span<float> srcVector4 = stackalloc float[4] { x, y, z, w };
        var dstVector4 = new float[4];
        MapVector4(srcVector4, dstVector4);
        return dstVector4;
    }

    public readonly void MapVector4(ReadOnlySpan<float> srcVector4, Span<float> dstVector4)
    {
        if (srcVector4.Length % 4 != 0)
            throw new ArgumentException("The source vector array must be multiples of 4.",
                nameof(srcVector4));
        if (dstVector4.Length % 4 != 0)
            throw new ArgumentException("The destination vector array must be multiples of 4.",
                nameof(dstVector4));

        Span<float> working = stackalloc float[4];

        for (var i = 0; i < srcVector4.Length; i += 4)
        {
            var current = srcVector4.Slice(i);
            var destination = dstVector4.Slice(i);

            var c0 = current[0];
            var c1 = current[1];
            var c2 = current[2];
            var c3 = current[3];

            working[0] = M00 * c0 + M10 * c1 + M20 * c2 + M30 * c3;
            working[1] = M01 * c0 + M11 * c1 + M21 * c2 + M31 * c3;
            working[2] = M02 * c0 + M12 * c1 + M22 * c2 + M32 * c3;
            working[3] = M03 * c0 + M13 * c1 + M23 * c2 + M33 * c3;

            working.CopyTo(destination);
        }
    }

    // MapPoints

    public readonly Point MapPoint(float x, float y) => MapPoint(new Point(x, y));

    public readonly Point MapPoint(Point src)
    {
        Span<Point> s = stackalloc[] { src };
        Span<Point> d = stackalloc Point[1];

        MapPoints(s, d);

        return d[0];
    }

    public readonly PointI MapPointI(int x, int y)
    {
        var pt = MapPoint(new Point(x, y));
        return new PointI((int)pt.X, (int)pt.Y);
    }

    public readonly void MapPointsI(PointI[] points)
    {
        for (var i = 0; i < points.Length; i++)
        {
            points[i] = MapPointI(points[i].X, points[i].Y);
        }
    }

    public readonly void MapPoints(ReadOnlySpan<Point> src, Span<Point> dst)
    {
        if (src.Length != dst.Length)
            throw new ArgumentException(
                "The destination array must have the same number of entries as the source array.",
                nameof(dst));

        for (var i = 0; i < src.Length; i++)
        {
            var s = src[i];
            dst[i] = new Point(
                M00 * s.X + M10 * s.Y + M30,
                M01 * s.X + M11 * s.Y + M31);
        }
    }

    // MapVector2

    public readonly float[] MapVector2(float x, float y)
    {
        Span<float> src2 = stackalloc float[2] { x, y };
        var dst4 = new float[4];
        MapVector2(src2, dst4);
        return dst4;
    }

    public readonly void MapVector2(ReadOnlySpan<float> src2, Span<float> dst4)
    {
        if (src2.Length % 2 != 0)
            throw new ArgumentException("The source vector array must be a set of pairs.",
                nameof(src2));
        if (dst4.Length % 4 != 0)
            throw new ArgumentException("The destination vector array must be a set quads.",
                nameof(dst4));
        if (src2.Length / 2 != dst4.Length / 4)
            throw new ArgumentException(
                "The source vector array must have the same number of pairs as the destination vector array has quads.",
                nameof(dst4));

        Span<float> working = stackalloc float[4];

        for (int i = 0, j = 0; i < src2.Length; i += 2, j += 4)
        {
            var current = src2.Slice(i);
            var destination = dst4.Slice(j);

            var c0 = current[0];
            var c1 = current[1];

            working[0] = M00 * c0 + M10 * c1 + M30;
            working[1] = M01 * c0 + M11 * c1 + M31;
            working[2] = M02 * c0 + M12 * c1 + M32;
            working[3] = M03 * c0 + M13 * c1 + M33;

            working.CopyTo(destination);
        }
    }

    #endregion

    #region ====Others====

    public readonly double GetDeterminant()
    {
        float a = M00, b = M01, c = M02, d = M03;
        float e = M10, f = M11, g = M12, h = M13;
        float i = M20, j = M21, k = M22, l = M23;
        float m = M30, n = M31, o = M32, p = M33;

        var kp_lo = k * p - l * o;
        var jp_ln = j * p - l * n;
        var jo_kn = j * o - k * n;
        var ip_lm = i * p - l * m;
        var io_km = i * o - k * m;
        var in_jm = i * n - j * m;

        var a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
        var a12 = -(e * kp_lo - g * ip_lm + h * io_km);
        var a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
        var a14 = -(e * jo_kn - f * io_km + g * in_jm);

        var det = a * a11 + b * a12 + c * a13 + d * a14;

        return det;
    }

    // public static SKMatrix4x4 CreateLookAt (SKPoint3 cameraPosition, SKPoint3 cameraTarget, SKPoint3 cameraUpVector)
    // {
    //     var zaxis = SKPoint3.Normalize (cameraPosition - cameraTarget);
    //     var xaxis = SKPoint3.Normalize (SKPoint3.Cross (cameraUpVector, zaxis));
    //     var yaxis = SKPoint3.Cross (zaxis, xaxis);
    //
    //     var m411 = -SKPoint3.Dot (xaxis, cameraPosition);
    //     var m421 = -SKPoint3.Dot (yaxis, cameraPosition);
    //     var m431 = -SKPoint3.Dot (zaxis, cameraPosition);
    //
    //     return new (
    //         xaxis.X, yaxis.X, zaxis.X, 0,
    //         xaxis.Y, yaxis.Y, zaxis.Y, 0,
    //         xaxis.Z, yaxis.Z, zaxis.Z, 0,
    //         m411, m421, m431, 1);
    // }

    // CreatePerspective*

    public static Matrix4 CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio,
        float nearPlaneDistance, float farPlaneDistance)
    {
        if (fieldOfView <= 0.0f || fieldOfView >= MathF.PI)
            throw new ArgumentOutOfRangeException(nameof(fieldOfView));

        if (nearPlaneDistance <= 0.0f)
            throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

        if (farPlaneDistance <= 0.0f)
            throw new ArgumentOutOfRangeException(nameof(farPlaneDistance));

        if (nearPlaneDistance >= farPlaneDistance)
            throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

        var yScale = 1.0f / (float)MathF.Tan(fieldOfView * 0.5f);
        var xScale = yScale / aspectRatio;
        var negFarRange = float.IsPositiveInfinity(farPlaneDistance)
            ? -1.0f
            : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

        var m11 = xScale;
        var m22 = yScale;
        var m33 = negFarRange;
        var m34 = -1.0f;
        var m43 = nearPlaneDistance * negFarRange;

        return new(
            m11, 0, 0, 0,
            0, m22, 0, 0,
            0, 0, m33, m34,
            0, 0, m43, 0);
    }

    #endregion

    #region ====Overrides====

    public readonly bool Equals(Matrix4 obj) =>
        M00 == obj.M00 && M01 == obj.M01 && M02 == obj.M02 && M03 == obj.M03 &&
        M10 == obj.M10 && M11 == obj.M11 && M12 == obj.M12 && M13 == obj.M13 &&
        M20 == obj.M20 && M21 == obj.M21 && M22 == obj.M22 && M23 == obj.M23 &&
        M30 == obj.M30 && M31 == obj.M31 && M32 == obj.M32 && M33 == obj.M33;

    public readonly override bool Equals(object obj) => obj is Matrix4 f && Equals(f);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(M00);
        hash.Add(M01);
        hash.Add(M02);
        hash.Add(M03);
        hash.Add(M10);
        hash.Add(M11);
        hash.Add(M12);
        hash.Add(M13);
        hash.Add(M20);
        hash.Add(M21);
        hash.Add(M22);
        hash.Add(M23);
        hash.Add(M30);
        hash.Add(M31);
        hash.Add(M32);
        hash.Add(M33);
        return hash.ToHashCode();
    }

    public static bool operator ==(Matrix4 left, Matrix4 right) => left.Equals(right);

    public static bool operator !=(Matrix4 left, Matrix4 right) => !left.Equals(right);

    public override string ToString() =>
        $"[{M00,10:F2}, {M01,10:F2}, {M02,10:F2}, {M03,10:F2}]\n[{M10,10:F2}, {M11,10:F2}, {M12,10:F2}, {M13,10:F2}]\n[{M20,10:F2}, {M21,10:F2}, {M22,10:F2}, {M23,10:F2}]\n[{M30,10:F2}, {M31,10:F2}, {M32,10:F2}, {M33,10:F2}]\n";

    #endregion

    #region ====Type Casting====

    public Matrix3 ToMatrix3() => new Matrix3(
        M00, M10, M30,
        M01, M11, M31,
        M03, M13, M33
    );

    public static implicit operator Matrix4(Matrix3 matrix) => new(
        matrix.ScaleX, matrix.SkewY, 0, matrix.Persp0,
        matrix.SkewX, matrix.ScaleY, 0, matrix.Persp1,
        0, 0, 1, 0,
        matrix.TransX, matrix.TransY, 0, matrix.Persp2
    );

    public static implicit operator Matrix4x4(Matrix4 matrix) =>
        Unsafe.As<Matrix4, Matrix4x4>(ref matrix);

    public static implicit operator Matrix4(Matrix4x4 matrix) =>
        Unsafe.As<Matrix4x4, Matrix4>(ref matrix);

    #endregion
}
#endif