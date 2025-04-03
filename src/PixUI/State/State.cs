using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PixUI;

[TSRename("StateBase")]
public abstract class State : IDisposable
{
    private List<Action<State>>? _bindings;
    private static readonly EmptyState EmptyState = new EmptyState();

    public static State Empty => EmptyState;
    
    public abstract object? BoxedValue { get; }

    public abstract bool Readonly { get; }

    public void AddListener(Action<State> action)
    {
        _bindings ??= new List<Action<State>>();
        _bindings.Add(action);
    }

    public void RemoveListener(Action<State> action)
    {
        _bindings?.RemoveAll(b => ReferenceEquals(b, action));
    }

    public void RemoveByTarget(object target)
    {
        Debug.Assert(target != null);
        _bindings?.RemoveAll(b => ReferenceEquals(b.Target, target));
    }

    public void NotifyValueChanged()
    {
        if (_bindings == null || _bindings.Count == 0) return;

        for (var i = 0; i < _bindings.Count; i++)
        {
            _bindings[i](this);
        }
    }

    public virtual void Dispose() => _bindings?.Clear();
}

internal sealed class EmptyState : State
{
    public override bool Readonly => true;

    public override object? BoxedValue => null;
}

public abstract class State<T> : State
{
    public static State<T?> Default() => new RxValue<T?>(default);

    public abstract T Value { get; set; }

    public override string ToString() => Value?.ToString() ?? string.Empty;

    /// <summary>
    /// 转换为State&lt;string&gt;的计算属性
    /// </summary>
    public State<string> ToStateOfString(Func<T, string>? formatter = null, Func<string, T>? parser = null) =>
        RxComputed<string>.MakeAsString(this, formatter, parser);

    /// <summary>
    /// 转换为State&lt;bool&gt;的计算属性
    /// </summary>
    public State<bool> ToStateOfBool(Func<T, bool> getter) => RxComputed<bool>.Make(this, getter);

    public State<TR> ToComputed<TR>(Func<T, TR> getter, Action<TR>? setter = null, Func<bool>? notifier = null) =>
        RxComputed<TR>.Make(this, getter, setter, notifier);

    public State<TR> ToComputed<T1, TR>(State<T1> other, Func<T, T1, TR> getter, Action<TR>? setter = null) =>
        RxComputed<TR>.Make(this, other, getter, setter);

    public static implicit operator State<T>(T value) => new RxValue<T>(value);

#if __WEB__
        //TODO:临时解决隐式转换
        public static State<T> op_Implicit_From<T>(T value) {
            return new RxValue<T>(value);
        }
#endif
}

public static class StateExtensions
{
    /// <summary>
    /// 简化生成反转bool值的计算状态
    /// </summary>
    public static State<bool> ToReversed(this State<bool> s) =>
        RxComputed<bool>.Make(s, v => !v, v => s.Value = !v);

    public static State<T?> ToNullable<T>(this State<T> s) where T : struct =>
        RxComputed<T?>.MakeNullable(s);

    public static State<T> ToNoneNullable<T>(this State<T?> s) where T : struct =>
        RxComputed<T>.Make(s, v => v ?? default(T), v => s.Value = v);

    public static State<T> ToNoneNullable<T>(this State<T?> s, T defaultValue) where T : struct =>
        RxComputed<T>.Make(s, v => v ?? defaultValue, v => s.Value = v);

    public static State<T> ToNoneNullable<T>(this State<T?> s, T defaultValue) where T : class =>
        RxComputed<T>.Make(s, v => v ?? defaultValue, v => s.Value = v);

    public static State<string> ToNoneNullable(this State<string?> s) =>
        RxComputed<string>.Make(s, v => v ?? string.Empty, v => s.Value = v);
}