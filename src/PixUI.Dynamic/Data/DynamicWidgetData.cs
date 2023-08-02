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

    public ValueSource[]? CtorArgs { get; set; }

    public List<PropertyValue>? Properties { get; private set; }

    public void AddPropertyValue(PropertyValue propertyValue)
    {
        Properties ??= new List<PropertyValue>();
        Properties.Add(propertyValue);
    }
}

public enum ValueFrom
{
    Const,
    State
}

[JsonConverter(typeof(ValueSourceJsonConverter))]
public struct ValueSource
{
    public ValueFrom From { get; set; }
    public object? Value { get; set; }

    public void Read(ref Utf8JsonReader reader, DynamicValueMeta valueMeta)
    {
        reader.Read(); // {
        reader.Read(); //From
        reader.Read();
        From = (ValueFrom)reader.GetInt32();
        reader.Read(); //Value
        var valueType = valueMeta.ValueType;
        if (valueMeta.ValueType.IsValueType && valueMeta.ValueNullable)
            valueType = typeof(Nullable<>).MakeGenericType(valueType);
        if (valueMeta.IsState)
            
        Value = JsonSerializer.Deserialize(ref reader, valueMeta.ValueType /*TODO: options*/);
        reader.Read(); // }
    }
}

public sealed class ValueSourceJsonConverter : JsonConverter<ValueSource>
{
    public override ValueSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, ValueSource value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber(nameof(ValueSource.From), (int)value.From);
        writer.WritePropertyName(nameof(ValueSource.Value));
        JsonSerializer.Serialize(writer, value.Value, options);
        writer.WriteEndObject();
    }
}

/// <summary>
/// 设计时组件的属性数据
/// </summary>
public sealed class PropertyValue
{
    public string Name { get; set; }
    public ValueSource Value { get; set; }
}