using System;

namespace PixUI;

public abstract class Animation<T>
{
    public abstract event Action? ValueChanged;

    public abstract event Action<AnimationStatus>? StatusChanged;

    public abstract T Value { get; }

    public abstract AnimationStatus Status { get; }

    public bool IsDismissed => Status == AnimationStatus.Dismissed;
    public bool IsCompleted => Status == AnimationStatus.Completed;
}

public abstract class AnimationWithParent<T> : Animation<T>
{
    protected readonly Animation<double> Parent;

    public AnimationWithParent(Animation<double> parent)
    {
        Parent = parent;
    }

    public override AnimationStatus Status => Parent.Status;

#if __WEB__
        [TSRawScript("public get ValueChanged(): System.Event { return this.Parent.ValueChanged; }")]
        public override event Action? ValueChanged;

        [TSRawScript(
            "public get StatusChanged(): System.Event<PixUI.AnimationStatus> { return this.Parent.StatusChanged; }")]
        public override event Action<AnimationStatus>? StatusChanged;
#else

    public override event Action? ValueChanged
    {
        add => Parent.ValueChanged += value;
        remove => Parent.ValueChanged -= value;
    }

    public override event Action<AnimationStatus>? StatusChanged
    {
        add => Parent.StatusChanged += value;
        remove => Parent.StatusChanged -= value;
    }
#endif
}

internal sealed class AnimatedEvaluation<T> : AnimationWithParent<T>
{
    private readonly Animatable<T> _evaluatable;

    internal AnimatedEvaluation(Animation<double> parent, Animatable<T> evaluatable)
        : base(parent)
    {
        _evaluatable = evaluatable;
    }

    public override T Value => _evaluatable.Evaluate(Parent);
}