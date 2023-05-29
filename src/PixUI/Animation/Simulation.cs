using System;
using System.Diagnostics;

namespace PixUI;

public abstract class Simulation
{
    /// <summary>
    /// How close to the actual end of the simulation a value at a particular time
    /// must be before [isDone] considers the simulation to be "done".
    /// </summary>
    /// <remarks>
    /// A simulation with an asymptotic curve would never technically be "done",
    /// but once the difference from the value at a particular time and the
    /// asymptote itself could not be seen, it would be pointless to continue. The
    /// tolerance defines how to determine if the difference could not be seen.
    /// </remarks>
    public Tolerance Tolerance { get; private set; }

    public Simulation(Tolerance? tolerance = null)
    {
        Tolerance = tolerance ?? Tolerance.Default;
    }

    /// <summary>
    /// The position of the object in the simulation at the given time.
    /// </summary>
    public abstract double X(double time);

    /// <summary>
    /// The velocity of the object in the simulation at the given time.
    /// </summary>
    public abstract double Dx(double time);

    /// <summary>
    ///  Whether the simulation is "done" at the given time.
    /// </summary>
    public abstract bool IsDone(double time);
}

internal sealed class InterpolationSimulation : Simulation
{
    private readonly double _durationInSeconds;
    private readonly double _begin;
    private readonly double _end;
    private readonly Curve _curve;

    internal InterpolationSimulation(double begin, double end, double duration, Curve curve,
        double scale)
    {
        Debug.Assert(duration > 0);

        _begin = begin;
        _end = end;
        _curve = curve;

        _durationInSeconds = duration * scale / 1000;
    }

    public override double X(double timeInSeconds)
    {
        var t = Math.Clamp(timeInSeconds / _durationInSeconds, 0.0, 1.0);
        if (t == 0.0) return _begin;
        if (t == 1.0) return _end;
        return _begin + (_end - _begin) * _curve.Transform(t);
    }

    public override double Dx(double timeInSeconds)
    {
        var epsilon = Tolerance.Time;
        return (X(timeInSeconds + epsilon) - X(timeInSeconds - epsilon)) / (2 * epsilon);
    }

    public override bool IsDone(double timeInSeconds) => timeInSeconds > _durationInSeconds;
}

internal delegate void DirectionSetter(AnimationDirection direction);

internal sealed class RepeatingSimulation : Simulation
{
    public RepeatingSimulation(double initialValue, double min, double max, bool reverse,
        int period, DirectionSetter directionSetter)
    {
        _min = min;
        _max = max;
        _reverse = reverse;
        _directionSetter = directionSetter;

        _periodInSeconds = period / 1000f;
        _initialT = max == min ? 0 : initialValue / (max - min) * _periodInSeconds;

        Debug.Assert(_periodInSeconds > 0);
        Debug.Assert(_initialT >= 0);
    }

    private readonly double _min;
    private readonly double _max;
    private readonly bool _reverse;
    private readonly DirectionSetter _directionSetter;

    private readonly double _periodInSeconds;
    private readonly double _initialT;

    public override double X(double timeInSeconds)
    {
        Debug.Assert(timeInSeconds >= 0);

        var totalTimeInSeconds = timeInSeconds + _initialT;
        var t = (totalTimeInSeconds / _periodInSeconds) % 1.0;
        var isPlayingReverse = ((int)(totalTimeInSeconds / _periodInSeconds) & 1) == 1;

        if (_reverse && isPlayingReverse)
        {
            _directionSetter(AnimationDirection.Reverse);
            return DoubleUtils.Lerp(_max, _min, t);
        }
        else
        {
            _directionSetter(AnimationDirection.Forward);
            return DoubleUtils.Lerp(_min, _max, t);
        }
    }

    public override double Dx(double timeInSeconds) => (_max - _min) / _periodInSeconds;

    public override bool IsDone(double time) => false;
}