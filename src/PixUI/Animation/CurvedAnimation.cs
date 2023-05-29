using System;
using System.Diagnostics.CodeAnalysis;

namespace PixUI;

[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
public sealed class CurvedAnimation : AnimationWithParent<double>
{
    /// <summary>
    /// The curve to use in the forward direction.
    /// </summary>
    private readonly Curve _curve;

    /// <summary>
    /// The curve to use in the reverse direction.
    /// </summary>
    private readonly Curve? _reverseCurve;

    /// <summary>
    /// The direction used to select the current curve.
    ///
    /// The curve direction is only reset when we hit the beginning or the end of
    /// the timeline to avoid discontinuities in the value of any variables this
    /// animation is used to animate.
    /// </summary>
    private AnimationStatus? _curveDirection;

    private bool UseForwardCurve =>
        _reverseCurve == null ||
        (_curveDirection ?? Parent.Status) != AnimationStatus.Reverse;

    public CurvedAnimation(Animation<double> parent, Curve curve, Curve? reverseCurve = null)
        : base(parent)
    {
        _curve = curve;
        _reverseCurve = reverseCurve;

        UpdateCurveDirection(parent.Status);
        parent.StatusChanged += UpdateCurveDirection;
    }

    private void UpdateCurveDirection(AnimationStatus status)
    {
        switch (status)
        {
            case AnimationStatus.Dismissed:
            case AnimationStatus.Completed:
                _curveDirection = null;
                break;
            case AnimationStatus.Forward:
                _curveDirection ??= AnimationStatus.Forward;
                break;
            case AnimationStatus.Reverse:
                _curveDirection ??= AnimationStatus.Reverse;
                break;
        }
    }

    public override double Value
    {
        get
        {
            var activeCurve = UseForwardCurve ? _curve : _reverseCurve;
            var t = Parent.Value;
            if (activeCurve == null) return t;

            if (t == 0.0 || t == 1.0)
            {
                var transformedValue = activeCurve.Transform(t);
                var roundedTransformedValue = Math.Round(transformedValue);
                if (roundedTransformedValue != t)
                    throw new Exception($"Invalid curve endpoint at {t}");
                return t;
            }

            return activeCurve.Transform(t);
        }
    }
}