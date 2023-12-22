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

    public List<EventValue>? Events { get; private set; }

    /// <summary>
    /// 指定属性是否已绑定至状态
    /// </summary>
    public bool HasBindToState(string propertyName, out string stateName)
    {
        stateName = null!;
        if (TryGetPropertyValue(propertyName, out var value))
        {
            if (value!.Value.From == ValueSource.State)
            {
                stateName = value.Value.StateName;
                return true;
            }
        }

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

    public bool TryGetEventValue(string name, out EventValue value)
    {
        value = null!;
        if (Events == null || Events.Count == 0)
            return false;

        var exists = Events.FirstOrDefault(p => p.Name == name);
        if (exists != null)
            value = exists;

        return exists != null;
    }

    public void SetEventValue(string name, IEventAction action)
    {
        Events ??= new List<EventValue>();
        Events.RemoveAll(e => e.Name == name);
        Events.Add(new EventValue { Name = name, Action = action });
    }

    public void RemoveEventValue(string name) => Events?.RemoveAll(e => e.Name == name);
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

    public string StateName => From == ValueSource.State ? (Value as string ?? string.Empty) : string.Empty;

    public void Write(Utf8JsonWriter writer, DynamicPropertyMeta propertyMeta)
    {
        var valueType = propertyMeta.ValueType;
        //如果是状态值且是值类型且不可空，则需要转换为可空值类型
        if (propertyMeta.IsState && propertyMeta.ValueType.IsValueType && !propertyMeta.IsNullableValueType)
            valueType = typeof(Nullable<>).MakeGenericType(valueType);

        if (!propertyMeta.IsState)
        {
            JsonSerializer.Serialize(writer, Value, valueType /*必须指定类型以适配某此自定义多态序列化*/);
            return;
        }

        writer.WriteStartObject();

        switch (From)
        {
            case ValueSource.Const:
                writer.WritePropertyName(nameof(ValueSource.Const));
                JsonSerializer.Serialize(writer, Value, valueType /*必须指定类型以适配某此自定义多态序列化*/);
                break;
            case ValueSource.State:
                writer.WritePropertyName(nameof(ValueSource.State));
                writer.WriteStringValue((string?)Value);
                break;
            default: throw new JsonException($"Unknown ValueSource");
        }

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
            switch (sourceName)
            {
                case nameof(ValueSource.Const):
                    v.From = ValueSource.Const;
                    v.Value = JsonSerializer.Deserialize(ref reader, valueType /*TODO: options*/);
                    break;
                case nameof(ValueSource.State):
                    reader.Read();
                    v.From = ValueSource.State;
                    v.Value = reader.GetString();
                    break;
                default:
                    throw new JsonException($"Unknown ValueSource: [{sourceName}]");
            }

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

/// <summary>
/// 设计时组件的事件行为定义
/// </summary>
public sealed class EventValue
{
    public string Name { get; set; } = null!;

    public IEventAction Action { get; set; } = null!;
}