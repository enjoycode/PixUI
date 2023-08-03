using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixUI.Dynamic;

/// <summary>
/// 描述设计元素的数据，用于序列化及生成相应的组件实例
/// </summary>
public sealed class DynamicWidgetData
{
    public string Type { get; set; }

    public DynamicValue[]? CtorArgs { get; set; }

    public List<PropertyValue>? Properties { get; private set; }

    public void AddPropertyValue(PropertyValue propertyValue)
    {
        Properties ??= new List<PropertyValue>();
        Properties.Add(propertyValue);
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

    public static implicit operator DynamicValue(string any) => new() { From = ValueSource.Const, Value = any };

    public void Write(Utf8JsonWriter writer)
    {
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

    public void Read(ref Utf8JsonReader reader, DynamicValueMeta valueMeta)
    {
        reader.Read(); // {
        reader.Read(); // ValueSource
        var sourceName = reader.GetString()!;
        From = sourceName switch
        {
            nameof(ValueSource.Const) => ValueSource.Const,
            nameof(ValueSource.State) => ValueSource.State,
            _ => throw new JsonException($"Unknown ValueSource: [{sourceName}]")
        };

        var valueType = valueMeta.ValueType;
        if (valueMeta.ValueType.IsValueType && valueMeta.IsState) //TODO:排除本身就是Nullable<>
            valueType = typeof(Nullable<>).MakeGenericType(valueType);

        Value = JsonSerializer.Deserialize(ref reader, valueType /*TODO: options*/);
        reader.Read(); // }
    }
}

/// <summary>
/// 设计时组件的属性数据
/// </summary>
public sealed class PropertyValue
{
    public string Name { get; set; }
    public DynamicValue Value { get; set; }
}