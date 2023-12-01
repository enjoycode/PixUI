using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PixUI;

// ReSharper disable once UnusedType.Global
public abstract class RxObjectBase<T> where T : class
{
    protected T _target = null!;

    /// <summary>
    /// 代理的目标对象实例
    /// </summary>
    public T Target
    {
        get => _target;
        set
        {
            var old = _target;
            _target = value;
            OnTargetChanged(old);
        }
    }

#if __WEB__
        [TSRawScript(@"
        protected OnTargetChanged(old: T) {
            const props = Object.getOwnPropertyNames(this);
            for(let prop of props) {
                // @ts-ignore
                if (this[prop] instanceof PixUI.RxProperty) {
                    // @ts-ignore
                    this[prop].NotifyValueChanged();
                }
            }
        }
")]
        private void OnTargetChanged(T old) {}
#else
    protected virtual void OnTargetChanged(T old)
    {
        //默认使用反射处理, TODO:
        //var rxPropertyType = typeof(RxProxy<>);

        var fields = GetType().GetFields(System.Reflection.BindingFlags.Instance |
                                         System.Reflection.BindingFlags.GetField |
                                         System.Reflection.BindingFlags.Public);
        foreach (var field in fields)
        {
            var fieldType = field.FieldType;
            if (fieldType.Name == "RxProxy`1")
            {
                var state = field.GetValue(this) as State;
                state?.NotifyValueChanged();
            }
        }
    }
#endif
}

/// <summary>
/// 用于监听INotifyPropertyChanged对象实例及属性的变更
/// </summary>
public sealed class RxObject<T> : RxObjectBase<T> where T : class, INotifyPropertyChanged, new()
{
    public RxObject()
    {
        _target = new T();
        _target.PropertyChanged += OnTargetPropertyChanged;
    }
    
    private readonly Dictionary<string, State> _ds = new();
    
    public State<TMember> Observe<TMember>(string propertyName, Func<T, TMember> getter, Action<T, TMember> setter)
    {
        if (_ds.TryGetValue(propertyName, out var state))
            return (State<TMember>)state;

        var proxy = new RxProxy<TMember>(
            () => getter(Target),
            v => setter(Target, v),
            false
        );
        _ds[propertyName] = proxy;
        return proxy;
    }
    
    private void OnTargetPropertyChanged(object? _, PropertyChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.PropertyName)) return;
        
        if (_ds.TryGetValue(e.PropertyName, out var state))
            state.NotifyValueChanged();
    }
    
    protected override void OnTargetChanged(T old)
    {
        old.PropertyChanged -= OnTargetPropertyChanged;
        _target.PropertyChanged += OnTargetPropertyChanged;

        //TODO:考虑比较新旧值是否产生变更，暂全部通知
        foreach (var state in _ds.Values)
        {
            state.NotifyValueChanged();
        }
    }
}