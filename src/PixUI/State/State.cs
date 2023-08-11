using System;
using System.Linq;
using System.Collections.Generic;

namespace PixUI;

/// <summary>
/// 允许绑定状态的对象
/// </summary>
public interface IStateBindable
{
    /// <summary>
    /// 绑定的状态发生变更
    /// </summary>
    void OnStateChanged(State state, BindingOptions options);
}

[TSRename("StateBase")]
public abstract class State
{
    private List<Binding>? _bindings;

    public abstract bool Readonly { get; }

    public void AddBinding(IStateBindable target, BindingOptions options)
    {
        if (_bindings == null)
        {
            _bindings = new List<Binding> { new Binding(target, options) };
        }
        else
        {
            if (!_bindings.Any(b => ReferenceEquals(b.Target.Target, target)))
            {
                _bindings.Add(new Binding(target, options));
            }
        }
    }

    public void RemoveBinding(IStateBindable target)
    {
        _bindings?.RemoveAll(b => ReferenceEquals(b.Target.Target, target));
    }

    public void NotifyValueChanged()
    {
        if (_bindings == null) return;

        for (var i = 0; i < _bindings.Count; i++)
        {
            var target = _bindings[i].Target.Target;
            if (target == null) /*DoNot use binding.Target.IsAlive*/
            {
                _bindings.RemoveAt(i); //remove none alive binding
                i--;
            }
            else
            {
                ((IStateBindable)target).OnStateChanged(this, _bindings[i].Options);
            }
        }
    }
}

public abstract class State<T> : State
{
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
    public State<bool> ToStateOfBool(Func<T, bool> getter) => RxComputed<bool>.Make<T, bool>(this, getter);

    public static implicit operator State<T>(T value) => new RxValue<T>(value);

#if __WEB__
        //TODO:临时解决隐式转换
        public static State<T> op_Implicit_From<T>(T value) {
            return new Rx<T>(value);
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

    public static State<T> ToNoneNullable<T>(this State<T?> s) where T : struct =>
        RxComputed<T>.Make(s, v => v ?? default(T), v => s.Value = v);

    public static State<T> ToNoneNullable<T>(this State<T?> s, T defaultValue) where T : struct =>
        RxComputed<T>.Make(s, v => v ?? defaultValue, v => s.Value = v);
}

// public sealed class StateProxy<T> : State<T>, IStateBindable
// {
//     private State<T>? _source;
//     private readonly T _defaultValue;
//
//     public StateProxy(State<T>? source, T defaultValue)
//     {
//         _source = source;
//         _source?.AddBinding(this, BindingOptions.None);
//         _defaultValue = defaultValue;
//     }
//
//     public State<T>? Source
//     {
//         set
//         {
//             _source?.RemoveBinding(this);
//             _source = value;
//             _source?.AddBinding(this, BindingOptions.None);
//         }
//     }
//
//     public override bool Readonly => _source?.Readonly ?? false;
//
//     public override T Value
//     {
//         get => _source == null ? _defaultValue : _source.Value;
//         set
//         {
//             if (_source == null)
//                 throw new InvalidOperationException();
//             _source.Value = value;
//         }
//     }
//
//     public void OnStateChanged(StateBase state, BindingOptions options) => NotifyValueChanged();
// }