using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace PixUI;

public abstract class ParametricCurve<T>
{
    public virtual T Transform(double t)
    {
        Debug.Assert(t >= 0.0 && t <= 1.0);
        return TransformInternal(t);
    }

    protected virtual T TransformInternal(double t) => throw new NotSupportedException();
}

public abstract class Curve : ParametricCurve<double>
{
    public Curve Flipped => new FlippedCurve(this);

    public override double Transform(double t)
    {
        if (t == 0.0 || t == 1.0) return t;

        return base.Transform(t);
    }
}

internal sealed class Linear : Curve
{
    protected override double TransformInternal(double t) => t;
}

internal sealed class BounceInOutCurve : Curve
{
    protected override double TransformInternal(double t)
    {
        if (t < 0.5)
            return (1.0 - Curves.Bounce(1.0 - t * 2.0)) * 0.5;
        return Curves.Bounce(t * 2.0 - 1.0) * 0.5 + 0.5;
    }
}

public sealed class FlippedCurve : Curve
{
    public readonly Curve Curve;

    public FlippedCurve(Curve curve)
    {
        Curve = curve;
    }

    protected override double TransformInternal(double t)
        => 1.0 - Curve.Transform(1.0 - t);
}

/// <summary>
/// A curve that is 0.0 until [begin], then curved (according to [curve]) from
/// 0.0 at [begin] to 1.0 at [end], then remains 1.0 past [end].
/// </summary>
/// <remarks>
/// An [Interval] can be used to delay an animation. For example, a six second
/// animation that uses an [Interval] with its [begin] set to 0.5 and its [end]
/// set to 1.0 will essentially become a three-second animation that starts
/// three seconds later.
/// </remarks>
[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
public sealed class Interval : Curve
{
    /// <summary>
    /// The largest value for which this interval is 0.0.
    /// From t=0.0 to t=`begin`, the interval's value is 0.0.
    /// </summary>
    private readonly double _begin;

    /// <summary>
    /// The smallest value for which this interval is 1.0.
    ///
    /// From t=`end` to t=1.0, the interval's value is 1.0.
    /// </summary>
    private readonly double _end;

    /// <summary>
    /// The curve to apply between [Begin] and [End].
    /// </summary>
    private readonly Curve _curve;

    public Interval(double begin, double end, Curve? curve = null)
    {
        if (!(begin >= 0 && begin <= 1 && end >= 0 && end <= 1 && end >= begin))
            throw new ArgumentOutOfRangeException();

        _begin = begin;
        _end = end;
        _curve = curve ?? Curves.Linear;
    }

    protected override double TransformInternal(double t)
    {
        t = Math.Clamp((t - _begin) / (_end - _begin), 0, 1);
        if (t == 0.0 || t == 1.0)
            return t;
        return _curve.Transform(t);
    }
}

/// <summary>
/// A sawtooth curve that repeats a given number of times over the unit interval.
/// </summary>
/// <remarks>
/// The curve rises linearly from 0.0 to 1.0 and then falls discontinuously back
/// to 0.0 each iteration.
/// </remarks>
public sealed class SawTooth : Curve
{
    public SawTooth(int count)
    {
        _count = count;
    }

    // The number of repetitions of the sawtooth pattern in the unit interval.
    private readonly int _count;

    protected override double TransformInternal(double t)
    {
        t *= _count;
        return t - Math.Truncate(t);
    }
}

public sealed class Cubic : Curve
{
    private const double CubicErrorBound = 0.001;

    private readonly double _a;
    private readonly double _b;
    private readonly double _c;
    private readonly double _d;

    public Cubic(double a, double b, double c, double d)
    {
        _a = a;
        _b = b;
        _c = c;
        _d = d;
    }

    private static double EvaluateCubic(double a, double b, double m)
    {
        return 3 * a * (1 - m) * (1 - m) * m +
               3 * b * (1 - m) * m * m +
               m * m * m;
    }

    protected override double TransformInternal(double t)
    {
        var start = 0.0;
        var end = 1.0;
        while (true)
        {
            var midpoint = (start + end) / 2;
            var estimate = EvaluateCubic(_a, _c, midpoint);
            if (Math.Abs(t - estimate) < CubicErrorBound)
                return EvaluateCubic(_b, _d, midpoint);
            if (estimate < t)
                start = midpoint;
            else
                end = midpoint;
        }
    }
}

public static class Curves
{
    public static readonly Curve Linear = new Linear();

    public static readonly Curve BounceInOut = new BounceInOutCurve();

    public static readonly Cubic EaseInOutCubic = new Cubic(0.645, 0.045, 0.355, 1.0);

    public static readonly Cubic FastOutSlowIn = new Cubic(0.4, 0.0, 0.2, 1.0);

    internal static double Bounce(double t)
    {
        if (t < 1.0 / 2.75)
        {
            return 7.5625 * t * t;
        }

        if (t < 2 / 2.75)
        {
            t -= 1.5 / 2.75;
            return 7.5625 * t * t + 0.75;
        }

        if (t < 2.5 / 2.75)
        {
            t -= 2.25 / 2.75;
            return 7.5625 * t * t + 0.9375;
        }

        t -= 2.625 / 2.75;
        return 7.5625 * t * t + 0.984375;
    }
}