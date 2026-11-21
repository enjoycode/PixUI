namespace PixUI;

/// <summary>
/// 可与状态绑定的属性，如果没有绑定状态内部直接存储值
/// </summary>
public readonly struct BindableProperty<T>
{
    public BindableProperty(T value)
    {
        _value = value;
        _state = null;
    }

    private BindableProperty(State<T> state)
    {
        _state = state;
        _value = state.Value;
    }

    private readonly T _value;
    private readonly State<T>? _state;

    public T Value => _state == null ? _value : _state.Value;

    internal State<T>? State => _state;

    public static implicit operator BindableProperty<T>(T value) => new(value);

    public static implicit operator BindableProperty<T>(State<T> state) => new(state);

    public override string ToString() => Value?.ToString() ?? string.Empty;
}

public sealed class PropertyValueInvalidException : Exception
{
    public PropertyValueInvalidException(string className, string propertyName, string validator)
    {
        ClassName = className;
        PropertyName = propertyName;
        Validator = validator;
    }

    public string ClassName { get; }
    public string PropertyName { get; }
    public string Validator { get; }
}