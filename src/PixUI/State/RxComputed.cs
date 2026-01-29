using System;

namespace PixUI;

/// <summary>
/// 通过源状态计算/转换而来的状态
/// </summary>
public sealed class RxComputed<T> : State<T>
{
    private RxComputed(Func<T> getter, Action<T>? setter /*, Func<bool>? notifier = null*/)
    {
        _getter = getter;
        _setter = setter;
        // _notifier = notifier;
    }

    private readonly Func<T> _getter;

    private readonly Action<T>? _setter;

    // /// <summary>
    // /// 是否允许在源变更时通知自身变更的委托
    // /// </summary>
    // private readonly Func<bool>? _notifier;

    public static RxComputed<string> MakeAsString<R>(State<R> s,
        Func<R, string>? formatter = null, Func<string, R>? parser = null)
    {
        var computed = new RxComputed<string>(
            formatter == null ? s.ToString : () => formatter(s.Value),
            parser == null ? null : v => s.Value = parser(v)
        );
        s.AddListener(computed.OnStateChanged);
        return computed;
    }

    public static RxComputed<R> Make<TS, R>(State<TS> source, Func<TS, R> getter,
        Action<R>? setter = null /*, Func<bool>? notifier = null*/)
    {
        var computed = new RxComputed<R>(() => getter(source.Value), setter /*, notifier*/);
        source.AddListener(computed.OnStateChanged);
        return computed;
    }

    public static RxComputed<TS?> MakeNullable<TS>(State<TS> source) where TS : struct
    {
        var computed = new RxComputed<TS?>(() => source.Value, v => source.Value = v ?? default(TS));
        source.AddListener(computed.OnStateChanged);
        return computed;
    }

    public static RxComputed<R> Make<T1, T2, R>(State<T1> s1, State<T2> s2,
        Func<T1, T2, R> getter, Action<R>? setter = null /*, Func<bool>? notifier = null*/)
    {
        var computed = new RxComputed<R>(() => getter(s1.Value, s2.Value), setter /*, notifier*/);
        s1.AddListener(computed.OnStateChanged);
        s2.AddListener(computed.OnStateChanged);
        return computed;
    }

    public override bool Readonly => _setter == null;

    public override object? BoxedValue => Value;

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

    private void OnStateChanged(State state)
    {
        // if (_notifier == null || _notifier.Invoke())
        NotifyValueChanged();
    }
}