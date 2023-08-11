using System;

namespace PixUI;

public sealed class RxProperty<T> : State<T>
{
    public RxProperty(Func<T> getter, Action<T>? setter = null, bool autoNotify = true)
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
            if (_setter == null) throw new NotSupportedException();
            _setter(value);
        }
    }
}

// ReSharper disable once UnusedType.Global
public abstract class RxObject<T> where T : class
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
        //var rxPropertyType = typeof(RxProperty<>);

        var fields = GetType().GetFields(System.Reflection.BindingFlags.Instance |
                                         System.Reflection.BindingFlags.GetField |
                                         System.Reflection.BindingFlags.Public);
        foreach (var field in fields)
        {
            var fieldType = field.FieldType;
            if (fieldType.Name == "RxProperty`1")
            {
                var state = (State)field.GetValue(this);
                state.NotifyValueChanged();
            }
        }
    }
#endif
}