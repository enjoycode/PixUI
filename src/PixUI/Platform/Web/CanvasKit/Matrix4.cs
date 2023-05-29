#if __WEB__
using System;

namespace PixUI
{
    [TSType("PixUI.Matrix4")]
    public struct Matrix4 : IEquatable<Matrix4>
    {
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

        public bool IsIdentity => true;

        public static Matrix4 CreateIdentity() => new Matrix4();

        public static Matrix4 CreateTranslation(float x, float y, float z) => new Matrix4();

        public static Matrix4 CreateScale(float x, float y, float z) => new Matrix4();

        public static Matrix4? TryInvert(Matrix4 other) => null;

        public void Translate(float x, float y = 0.0f, float z = 0.0f) { }

        public void RotateZ(float angle) { }

        public void PreConcat(in Matrix4 m) { }

        public void Multiply(in Matrix4 arg) { }

        public void SetColumn(int column, Vector4 arg) { }

        public void SetRow(int row, Vector4 arg) { }

        public bool Equals(Matrix4 other) => true;
        
        public static bool operator ==(Matrix4 left, Matrix4 right) => left.Equals(right);

        public static bool operator !=(Matrix4 left, Matrix4 right) => !left.Equals(right);
    }
}
#endif