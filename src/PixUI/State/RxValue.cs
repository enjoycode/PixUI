using System.Collections.Generic;

namespace PixUI;

public sealed class RxValue<T> : State<T>
{
    public RxValue(T value)
    {
        _value = value;
    }

    private T _value;

    public override bool Readonly => false;

    public override object? BoxedValue => Value;

    public override T Value
    {
        get => _value;
        set
        {
            if (EqualityComparer<T>.Default.Equals(_value, value)) return;
            _value = value;
            NotifyValueChanged();
        }
    }

    //类似Nullable<T>暂不支持隐式转换为相应的值
    //public static implicit operator T(Rx<T> rx) => rx.Value;

    public static implicit operator RxValue<T>(T value) => new(value);
}