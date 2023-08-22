using System;

namespace PixUI;

/// <summary>
/// 通过源状态计算/转换而来的状态
/// </summary>
public sealed class RxComputed<T> : State<T>, IStateBindable
{
    private readonly Func<T> _getter;
    private readonly Action<T>? _setter;

    private RxComputed(Func<T> getter, Action<T>? setter)
    {
        _getter = getter;
        _setter = setter;
    }

    public static RxComputed<string> MakeAsString<TR>(State<TR> s,
        Func<TR, string>? formatter = null, Func<string, TR>? parser = null)
    {
        var computed = new RxComputed<string>(
            formatter == null ? s.ToString : () => formatter(s.Value),
            parser == null ? null : v => s.Value = parser(v)
        );
        s.AddBinding(computed, BindingOptions.None);
        return computed;
    }

    [TSRename("Make1")]
    public static RxComputed<TR> Make<TS, TR>(State<TS> source, Func<TS, TR> getter,
        Action<TR>? setter = null)
    {
        var computed = new RxComputed<TR>(() => getter(source.Value), setter);
        source.AddBinding(computed, BindingOptions.None);
        return computed;
    }

    [TSRename("Make2")]
    public static RxComputed<TR> Make<T1, T2, TR>(State<T1> s1, State<T2> s2,
        Func<T1, T2, TR> getter,
        Action<TR>? setter = null)
    {
        var computed = new RxComputed<TR>(() => getter(s1.Value, s2.Value), setter);
        s1.AddBinding(computed, BindingOptions.None);
        s2.AddBinding(computed, BindingOptions.None);
        return computed;
    }

    public override bool Readonly => _setter == null;

    public override T Value
    {
        get => _getter();
        set
        {
            try
            {
                _setter?.Invoke(value);
            }
            catch (Exception e)
            {
                Log.Warn($"Set value error: {e.Message}");
            }
        }
    }

    public void OnStateChanged(State state, BindingOptions options) => NotifyValueChanged();
}