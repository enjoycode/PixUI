#if !__WEB__
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PixUI
{
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

        public float M0 { get; private set; }
        public float M1 { get; private set; }
        public float M2 { get; private set; }
        public float M3 { get; private set; }
        public float M4 { get; private set; }
        public float M5 { get; private set; }
        public float M6 { get; private set; }
        public float M7 { get; private set; }
        public float M8 { get; private set; }
        public float M9 { get; private set; }
        public float M10 { get; private set; }
        public float M11 { get; private set; }
        public float M12 { get; private set; }
        public float M13 { get; private set; }
        public float M14 { get; private set; }
        public float M15 { get; private set; }

        #endregion

        #region ====Ctor====

        public Matrix4(
            float m0, float m1, float m2, float m3,
            float m4, float m5, float m6, float m7,
            float m8, float m9, float m10, float m11,
            float m12, float m13, float m14, float m15)
        {
            M0 = m0;
            M1 = m1;
            M2 = m2;
            M3 = m3;

            M4 = m4;
            M5 = m5;
            M6 = m6;
            M7 = m7;

            M8 = m8;
            M9 = m9;
            M10 = m10;
            M11 = m11;

            M12 = m12;
            M13 = m13;
            M14 = m14;
            M15 = m15;
        }

        public Matrix4(Matrix3 src)
        {
            M0 = src.M0;
            M1 = src.M1;
            M2 = 0;
            M3 = src.M6;

            M4 = src.M3;
            M5 = src.M4;
            M6 = 0;
            M7 = src.M7;

            M8 = 0;
            M9 = 0;
            M10 = 1;
            M11 = 0;

            M12 = src.M2;
            M13 = src.M5;
            M14 = 0;
            M15 = src.M8;
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

            M0 = src[0];
            M1 = src[1];
            M2 = src[2];
            M3 = src[3];

            M4 = src[4];
            M5 = src[5];
            M6 = src[6];
            M7 = src[7];

            M8 = src[8];
            M9 = src[9];
            M10 = src[10];
            M11 = src[11];

            M12 = src[12];
            M13 = src[13];
            M14 = src[14];
            M15 = src[15];
        }

        public void SetRowMajor(ReadOnlySpan<float> src)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (src.Length != 16)
                throw new ArgumentException("The source array must be 16 entries.", nameof(src));

            M0 = src[0];
            M4 = src[1];
            M8 = src[2];
            M12 = src[3];

            M1 = src[4];
            M5 = src[5];
            M9 = src[6];
            M13 = src[7];

            M2 = src[8];
            M6 = src[9];
            M10 = src[10];
            M14 = src[11];

            M3 = src[12];
            M7 = src[13];
            M11 = src[14];
            M15 = src[15];
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

            M0 = src[0];
            M4 = src[1];
            M8 = 0;
            M12 = src[2];

            M1 = src[3];
            M5 = src[4];
            M9 = 0;
            M13 = src[5];

            M2 = 0;
            M6 = 0;
            M10 = 1;
            M14 = 0;

            M3 = src[6];
            M7 = src[7];
            M11 = 0;
            M15 = src[8];
        }

        public void Set3x3RowMajor(ReadOnlySpan<float> src)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (src.Length != 9)
                throw new ArgumentException("The source array must be 9 entries.", nameof(src));

            M0 = src[0];
            M1 = src[1];
            M2 = 0;
            M3 = src[2];

            M4 = src[3];
            M5 = src[4];
            M6 = 0;
            M7 = src[5];

            M8 = 0;
            M9 = 0;
            M10 = 1;
            M11 = 0;

            M12 = src[6];
            M13 = src[7];
            M14 = 0;
            M15 = src[8];
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

            M0 = xx + ca * (1.0f - xx);
            M1 = xy - ca * xy + sa * z;
            M2 = xz - ca * xz - sa * y;
            M3 = 0;

            M4 = xy - ca * xy - sa * z;
            M5 = yy + ca * (1.0f - yy);
            M6 = yz - ca * yz + sa * x;
            M7 = 0;

            M8 = xz - ca * xz + sa * y;
            M9 = yz - ca * yz - sa * x;
            M10 = zz + ca * (1.0f - zz);
            M11 = 0;

            M12 = 0;
            M13 = 0;
            M14 = 0;
            M15 = 1;
        }

        public void SetConcat(Matrix4 a, Matrix4 b) =>
            this = new(
                a.M0 * b.M0 + a.M1 * b.M4 + a.M2 * b.M8 + a.M3 * b.M12,
                a.M0 * b.M1 + a.M1 * b.M5 + a.M2 * b.M9 + a.M3 * b.M13,
                a.M0 * b.M2 + a.M1 * b.M6 + a.M2 * b.M10 + a.M3 * b.M14,
                a.M0 * b.M3 + a.M1 * b.M7 + a.M2 * b.M11 + a.M3 * b.M15,
                a.M4 * b.M0 + a.M5 * b.M4 + a.M6 * b.M8 + a.M7 * b.M12,
                a.M4 * b.M1 + a.M5 * b.M5 + a.M6 * b.M9 + a.M7 * b.M13,
                a.M4 * b.M2 + a.M5 * b.M6 + a.M6 * b.M10 + a.M7 * b.M14,
                a.M4 * b.M3 + a.M5 * b.M7 + a.M6 * b.M11 + a.M7 * b.M15,
                a.M8 * b.M0 + a.M9 * b.M4 + a.M10 * b.M8 + a.M11 * b.M12,
                a.M8 * b.M1 + a.M9 * b.M5 + a.M10 * b.M9 + a.M11 * b.M13,
                a.M8 * b.M2 + a.M9 * b.M6 + a.M10 * b.M10 + a.M11 * b.M14,
                a.M8 * b.M3 + a.M9 * b.M7 + a.M10 * b.M11 + a.M11 * b.M15,
                a.M12 * b.M0 + a.M13 * b.M4 + a.M14 * b.M8 + a.M15 * b.M12,
                a.M12 * b.M1 + a.M13 * b.M5 + a.M14 * b.M9 + a.M15 * b.M13,
                a.M12 * b.M2 + a.M13 * b.M6 + a.M14 * b.M10 + a.M15 * b.M14,
                a.M12 * b.M3 + a.M13 * b.M7 + a.M14 * b.M11 + a.M15 * b.M15);

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
            M12 = M0 * x + M4 * y + M8 * z + M12;
            M13 = M1 * x + M5 * y + M9 * z + M13;
            M14 = M2 * x + M6 * y + M10 * z + M14;
            M15 = M3 * x + M7 * y + M11 * z + M15;
        }

        public void Scale(float x, float y = 1.0f, float z = 1.0f)
        {
            M0 *= x;
            M1 *= x;
            M2 *= x;
            M3 *= x;
            M4 *= y;
            M5 *= y;
            M6 *= y;
            M7 *= y;
            M8 *= z;
            M9 *= z;
            M10 *= z;
            M11 *= z;
        }

        public void RotateX(float angle)
        {
            var cosAngle = (float)Math.Cos(angle);
            var sinAngle = (float)Math.Sin(angle);
            var t1 = M4 * cosAngle + M8 * sinAngle;
            var t2 = M5 * cosAngle + M9 * sinAngle;
            var t3 = M6 * cosAngle + M10 * sinAngle;
            var t4 = M7 * cosAngle + M11 * sinAngle;
            var t5 = M4 * -sinAngle + M8 * cosAngle;
            var t6 = M5 * -sinAngle + M9 * cosAngle;
            var t7 = M6 * -sinAngle + M10 * cosAngle;
            var t8 = M7 * -sinAngle + M11 * cosAngle;
            M4 = t1;
            M5 = t2;
            M6 = t3;
            M7 = t4;
            M8 = t5;
            M9 = t6;
            M10 = t7;
            M11 = t8;
        }

        public void RotateZ(float angle)
        {
            var cosAngle = (float)Math.Cos(angle);
            var sinAngle = (float)Math.Sin(angle);
            var t1 = M0 * cosAngle + M4 * sinAngle;
            var t2 = M1 * cosAngle + M5 * sinAngle;
            var t3 = M2 * cosAngle + M6 * sinAngle;
            var t4 = M3 * cosAngle + M7 * sinAngle;
            var t5 = M0 * -sinAngle + M4 * cosAngle;
            var t6 = M1 * -sinAngle + M5 * cosAngle;
            var t7 = M2 * -sinAngle + M6 * cosAngle;
            var t8 = M3 * -sinAngle + M7 * cosAngle;
            M0 = t1;
            M1 = t2;
            M2 = t3;
            M3 = t4;
            M4 = t5;
            M5 = t6;
            M6 = t7;
            M7 = t8;
        }

        public void Multiply(in Matrix4 arg)
        {
            var aM0 = M0;
            var aM4 = M4;
            var aM8 = M8;
            var aM12 = M12;
            var aM1 = M1;
            var aM5 = M5;
            var aM9 = M9;
            var aM13 = M13;
            var aM2 = M2;
            var aM6 = M6;
            var aM10 = M10;
            var aM14 = M14;
            var aM3 = M3;
            var aM7 = M7;
            var aM11 = M11;
            var aM15 = M15;

            var bM0 = arg.M0;
            var bM4 = arg.M4;
            var bM8 = arg.M8;
            var bM12 = arg.M12;
            var bM1 = arg.M1;
            var bM5 = arg.M5;
            var bM9 = arg.M9;
            var bM13 = arg.M13;
            var bM2 = arg.M2;
            var bM6 = arg.M6;
            var bM10 = arg.M10;
            var bM14 = arg.M14;
            var bM3 = arg.M3;
            var bM7 = arg.M7;
            var bM11 = arg.M11;
            var bM15 = arg.M15;
            M0 = (aM0 * bM0) + (aM4 * bM1) + (aM8 * bM2) + (aM12 * bM3);
            M4 = (aM0 * bM4) + (aM4 * bM5) + (aM8 * bM6) + (aM12 * bM7);
            M8 = (aM0 * bM8) + (aM4 * bM9) + (aM8 * bM10) + (aM12 * bM11);
            M12 = (aM0 * bM12) + (aM4 * bM13) + (aM8 * bM14) + (aM12 * bM15);
            M1 = (aM1 * bM0) + (aM5 * bM1) + (aM9 * bM2) + (aM13 * bM3);
            M5 = (aM1 * bM4) + (aM5 * bM5) + (aM9 * bM6) + (aM13 * bM7);
            M9 = (aM1 * bM8) + (aM5 * bM9) + (aM9 * bM10) + (aM13 * bM11);
            M13 = (aM1 * bM12) + (aM5 * bM13) + (aM9 * bM14) + (aM13 * bM15);
            M2 = (aM2 * bM0) + (aM6 * bM1) + (aM10 * bM2) + (aM14 * bM3);
            M6 = (aM2 * bM4) + (aM6 * bM5) + (aM10 * bM6) + (aM14 * bM7);
            M10 = (aM2 * bM8) + (aM6 * bM9) + (aM10 * bM10) + (aM14 * bM11);
            M14 = (aM2 * bM12) + (aM6 * bM13) + (aM10 * bM14) + (aM14 * bM15);
            M3 = (aM3 * bM0) + (aM7 * bM1) + (aM11 * bM2) + (aM15 * bM3);
            M7 = (aM3 * bM4) + (aM7 * bM5) + (aM11 * bM6) + (aM15 * bM7);
            M11 = (aM3 * bM8) + (aM7 * bM9) + (aM11 * bM10) + (aM15 * bM11);
            M15 = (aM3 * bM12) + (aM7 * bM13) + (aM11 * bM14) + (aM15 * bM15);
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
            var a00 = arg.M0;
            var a01 = arg.M1;
            var a02 = arg.M2;
            var a03 = arg.M3;
            var a10 = arg.M4;
            var a11 = arg.M5;
            var a12 = arg.M6;
            var a13 = arg.M7;
            var a20 = arg.M8;
            var a21 = arg.M9;
            var a22 = arg.M10;
            var a23 = arg.M11;
            var a30 = arg.M12;
            var a31 = arg.M13;
            var a32 = arg.M14;
            var a33 = arg.M15;
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
            M0 = (a11 * b11 - a12 * b10 + a13 * b09) * invDet;
            M1 = (-a01 * b11 + a02 * b10 - a03 * b09) * invDet;
            M2 = (a31 * b05 - a32 * b04 + a33 * b03) * invDet;
            M3 = (-a21 * b05 + a22 * b04 - a23 * b03) * invDet;
            M4 = (-a10 * b11 + a12 * b08 - a13 * b07) * invDet;
            M5 = (a00 * b11 - a02 * b08 + a03 * b07) * invDet;
            M6 = (-a30 * b05 + a32 * b02 - a33 * b01) * invDet;
            M7 = (a20 * b05 - a22 * b02 + a23 * b01) * invDet;
            M8 = (a10 * b10 - a11 * b08 + a13 * b06) * invDet;
            M9 = (-a00 * b10 + a01 * b08 - a03 * b06) * invDet;
            M10 = (a30 * b04 - a31 * b02 + a33 * b00) * invDet;
            M11 = (-a20 * b04 + a21 * b02 - a23 * b00) * invDet;
            M12 = (-a10 * b09 + a11 * b07 - a12 * b06) * invDet;
            M13 = (a00 * b09 - a01 * b07 + a02 * b06) * invDet;
            M14 = (-a30 * b03 + a31 * b01 - a32 * b00) * invDet;
            M15 = (a20 * b03 - a21 * b01 + a22 * b00) * invDet;
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
                M0, M4, M8, M12,
                M1, M5, M9, M13,
                M2, M6, M10, M14,
                M3, M7, M11, M15);

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

                working[0] = M0 * c0 + M4 * c1 + M8 * c2 + M12 * c3;
                working[1] = M1 * c0 + M5 * c1 + M9 * c2 + M13 * c3;
                working[2] = M2 * c0 + M6 * c1 + M10 * c2 + M14 * c3;
                working[3] = M3 * c0 + M7 * c1 + M11 * c2 + M15 * c3;

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
                    M0 * s.X + M4 * s.Y + M12,
                    M1 * s.X + M5 * s.Y + M13);
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

                working[0] = M0 * c0 + M4 * c1 + M12;
                working[1] = M1 * c0 + M5 * c1 + M13;
                working[2] = M2 * c0 + M6 * c1 + M14;
                working[3] = M3 * c0 + M7 * c1 + M15;

                working.CopyTo(destination);
            }
        }

        #endregion

        #region ====Others====

        public readonly double GetDeterminant()
        {
            float a = M0, b = M1, c = M2, d = M3;
            float e = M4, f = M5, g = M6, h = M7;
            float i = M8, j = M9, k = M10, l = M11;
            float m = M12, n = M13, o = M14, p = M15;

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
            M0 == obj.M0 && M1 == obj.M1 && M2 == obj.M2 && M3 == obj.M3 &&
            M4 == obj.M4 && M5 == obj.M5 && M6 == obj.M6 && M7 == obj.M7 &&
            M8 == obj.M8 && M9 == obj.M9 && M10 == obj.M10 && M11 == obj.M11 &&
            M12 == obj.M12 && M13 == obj.M13 && M14 == obj.M14 && M15 == obj.M15;

        public readonly override bool Equals(object obj) => obj is Matrix4 f && Equals(f);

        public override int GetHashCode()
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
            hash.Add(M9);
            hash.Add(M10);
            hash.Add(M11);
            hash.Add(M12);
            hash.Add(M13);
            hash.Add(M14);
            hash.Add(M15);
            return hash.ToHashCode();
        }

        public static bool operator ==(Matrix4 left, Matrix4 right) => left.Equals(right);

        public static bool operator !=(Matrix4 left, Matrix4 right) => !left.Equals(right);

        public override string ToString() =>
            $"[{M0,10:F2}, {M1,10:F2}, {M2,10:F2}, {M3,10:F2}]\n[{M4,10:F2}, {M5,10:F2}, {M6,10:F2}, {M7,10:F2}]\n[{M8,10:F2}, {M9,10:F2}, {M10,10:F2}, {M11,10:F2}]\n[{M12,10:F2}, {M13,10:F2}, {M14,10:F2}, {M15,10:F2}]\n";

        #endregion
    }
}
#endif