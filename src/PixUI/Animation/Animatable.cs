namespace PixUI;

/// <summary>
/// 根据Animation<double>生成指定T的值
/// </summary>
public abstract class Animatable<T>
{
    /// <summary>
    /// Returns the value of the object at point `t`.
    /// </summary>
    public abstract T Transform(double t);

    /// <summary>
    /// The current value of this object for the given [Animation].
    /// </summary>
    public T Evaluate(Animation<double> animation) => Transform(animation.Value);

    /// <summary>
    /// Returns a new [Animation] that is driven by the given animation but that
    /// takes on values determined by this object.
    /// </summary>
    public Animation<T> Animate(Animation<double> parent) =>
        new AnimatedEvaluation<T>(parent, this);

    /// <summary>
    /// Returns a new [Animatable] whose value is determined by first evaluating
    /// the given parent and then evaluating this object.
    /// </summary>
    public Animatable<T> Chain(Animatable<double> parent) =>
        new ChainedEvaluation<T>(parent, this);
}

internal sealed class ChainedEvaluation<T> : Animatable<T>
{
    private readonly Animatable<double> _parent;
    private readonly Animatable<T> _evaluatable;

    internal ChainedEvaluation(Animatable<double> parent, Animatable<T> evaluatable)
    {
        _parent = parent;
        _evaluatable = evaluatable;
    }

    public override T Transform(double t) => _evaluatable.Transform(_parent.Transform(t));
}