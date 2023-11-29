using System;

namespace PixUI;

/// <summary>
/// 通过取值及赋值委托代理的状态
/// </summary>
public sealed class RxProxy<T> : State<T>
{
    public RxProxy(Func<T> getter, Action<T>? setter = null, bool autoNotify = true)
    {
        _getter = getter;
        if (setter == null || !autoNotify)
            _setter = setter;
        else
            _setter = v =>
            {
                //TODO: compare old value
                setter(v);
                NotifyValueChanged();
            };
    }

    private readonly Func<T> _getter;
    private readonly Action<T>? _setter;

    public override bool Readonly => _setter == null;

    public override T Value
    {
        get => _getter();
        set
        {
            if (_setter == null) throw new NotSupportedException("状态值只读");
            _setter(value);
        }
    }
}