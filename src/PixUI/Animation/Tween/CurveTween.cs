using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace PixUI;

[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
public sealed class CurveTween : Animatable<double>
{
    private readonly Curve _curve;

    public CurveTween(Curve curve)
    {
        _curve = curve;
    }

    public override double Transform(double t)
    {
        if (t == 0.0 || t == 1.0)
        {
            Debug.Assert(Math.Round(_curve.Transform(t)) == t);
            return t;
        }

        return _curve.Transform(t);
    }
}