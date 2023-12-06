using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PixUI.Dynamic;

/// <summary>
/// 描述设计元素的数据，用于序列化及生成相应的组件实例
/// </summary>
public sealed class DynamicWidgetData
{
    public List<PropertyValue>? Properties { get; private set; }

    /// <summary>
    /// 指定属性是否已绑定至状态
    /// </summary>
    public bool HasBindToState(string name)
    {
        if (TryGetPropertyValue(name, out var value))
            return value!.Value.From == ValueSource.State;
        return false;
    }

    public void AddPropertyValue(PropertyValue propertyValue)
    {
        Properties ??= new List<PropertyValue>();
        Properties.Add(propertyValue);
    }

    public bool TryGetPropertyValue(string name, out PropertyValue? value)
    {
        if (Properties == null || Properties.Count == 0)
        {
            value = null;
            return false;
        }

        var exists = Properties.FirstOrDefault(p => p.Name == name);
        value = exists;
        return exists != null;
    }

    public PropertyValue SetPropertyValue(string name, DynamicValue value)
    {
        Properties ??= new List<PropertyValue>();
        var exists = Properties.FirstOrDefault(p => p.Name == name);
        if (exists != null)
        {
            exists.Value = value;
            return exists;
        }

        var newPropValue = new PropertyValue { Name = name, Value = value };
        AddPropertyValue(newPropValue);
        return newPropValue;
    }

    public void RemovePropertyValue(string name)
    {
        Properties?.RemoveAll(p => p.Name == name);
    }
}

public enum ValueSource
{
    Const,
    State
}

public struct DynamicValue
{
    public ValueSource From { get; set; }
    public object? Value { get; set; }

    public static implicit operator DynamicValue(string value) => new() { From = ValueSource.Const, Value = value };
    public static implicit operator DynamicValue(bool value) => new() { From = ValueSource.Const, Value = value };
    public static implicit operator DynamicValue(int value) => new() { From = ValueSource.Const, Value = value };
    public static implicit operator DynamicValue(float value) => new() { From = ValueSource.Const, Value = value };
    public static implicit operator DynamicValue(double value) => new() { From = ValueSource.Const, Value = value };
    public static implicit operator DynamicValue(decimal value) => new() { From = ValueSource.Const, Value = value };
    public static implicit operator DynamicValue(Guid value) => new() { From = ValueSource.Const, Value = value };
    public static implicit operator DynamicValue(DateTime value) => new() { From = ValueSource.Const, Value = value };

    public void Write(Utf8JsonWriter writer, DynamicPropertyMeta valueMeta)
    {
        if (!valueMeta.IsState)
        {
            JsonSerializer.Serialize(writer, Value);
            return;
        }

        writer.WriteStartObject();

        var propName = From switch
        {
            ValueSource.Const => nameof(ValueSource.Const),
            ValueSource.State => nameof(ValueSource.State),
            _ => throw new JsonException($"Unknown ValueSource")
        };
        writer.WritePropertyName(propName);

        JsonSerializer.Serialize(writer, Value);
        writer.WriteEndObject();
    }

    public static DynamicValue Read(ref Utf8JsonReader reader, DynamicPropertyMeta propertyMeta)
    {
        var valueType = propertyMeta.ValueType;
        //如果是状态值且是值类型且不可空，则需要转换为可空值类型，否则下面Deserialize读取null时会报错
        if (propertyMeta.IsState && propertyMeta.ValueType.IsValueType && !propertyMeta.IsNullableValueType)
            valueType = typeof(Nullable<>).MakeGenericType(valueType);

        var v = new DynamicValue();

        if (!propertyMeta.IsState)
        {
            v.From = ValueSource.Const;
            v.Value = JsonSerializer.Deserialize(ref reader, valueType);
        }
        else
        {
            reader.Read(); // {
            reader.Read(); // ValueSource
            var sourceName = reader.GetString()!;
            v.From = sourceName switch
            {
                nameof(ValueSource.Const) => ValueSource.Const,
                nameof(ValueSource.State) => ValueSource.State,
                _ => throw new JsonException($"Unknown ValueSource: [{sourceName}]")
            };
            v.Value = JsonSerializer.Deserialize(ref reader, valueType /*TODO: options*/);
            reader.Read(); // }
        }

        return v;
    }
}

/// <summary>
/// 设计时组件的属性数据
/// </summary>
public sealed class PropertyValue
{
    public string Name { get; set; } = null!;
    public DynamicValue Value { get; set; }
}