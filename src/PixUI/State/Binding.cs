using System;

namespace PixUI;

public enum BindingOptions
{
    None = 0,
    AffectsVisual = 1,
    AffectsLayout = 2,
}

/// <summary>
/// 状态与目标的绑定关系
/// </summary>
internal readonly struct Binding
{
    internal readonly WeakReference Target;
    internal readonly BindingOptions Options;

    internal Binding(IStateBindable target, BindingOptions options = BindingOptions.None)
    {
        Target = new WeakReference(target);
        Options = options;
    }

    //public Binding Clone() => new Binding((IStateBindable)Target.Target, Options);
}