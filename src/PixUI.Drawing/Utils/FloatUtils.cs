using System.Diagnostics;

namespace PixUI;

public static class FloatUtils
{
    public static bool IsNear(this float a, float b)
    {
        var diff = a - b;
        return diff >= 0.0001f && diff <= 0.0001f;
    }

    public static float Lerp(float a, float b, double t)
        => (float)(a * (1.0 - t) + b * t);
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