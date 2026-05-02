using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PixUI;

public static class FloatUtils
{
    internal const float NearlyZero = 1.0f / (1 << 12);

    internal static bool NearlyEqual(float a, float b) =>
        Math.Abs(a - b) <= NearlyZero;

    internal static bool NearlyEqual(float a, float b, float tolerance) =>
        Math.Abs(a - b) <= tolerance;

    public static float Lerp(float a, float b, double t) //TODO: use float.Lerp()
        => (float)(a * (1.0 - t) + b * t);

    internal static float MidPoint(float a, float b)
    {
        // Use double math to avoid underflow and overflow.
        return (float)(0.5 * ((double)a + b));
    }

    internal static float Ave(float a, float b) => (a + b) * 0.5f;

    internal static float NextAfter(float x, float y)
    {
        if (float.IsNaN(x) || float.IsNaN(y)) return x + y;
        if (x == y) return y; // nextafter(0, -0) = -0

        FloatIntUnion u;
        u.i = 0;
        u.f = x; // shut up the compiler

        if (x == 0)
        {
            u.i = 1;
            return y > 0 ? u.f : -u.f;
        }

        if ((x > 0) == (y > x))
            u.i++;
        else
            u.i--;
        return u.f;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct FloatIntUnion
    {
        [FieldOffset(0)] public int i;
        [FieldOffset(0)] public float f;
    }
}

public static class DoubleUtils
{
    public static double Lerp(double a, double b, double t)
    {
        if (a == b || (double.IsNaN(a) && double.IsNaN(b))) return a;
        Debug.Assert(double.IsFinite(a),
            "Cannot interpolate between finite and non-finite values");
        Debug.Assert(double.IsFinite(b),
            "Cannot interpolate between finite and non-finite values");
        Debug.Assert(double.IsFinite(t), "t must be finite when interpolating between values");
        return a * (1.0 - t) + b * t;
    }
}