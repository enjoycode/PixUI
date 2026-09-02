using System.Text.Json;

namespace PixUI.Dynamic.Json;

internal static class JsonExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions()
    {
        Converters =
        {
            new ColorJsonConverter(),
            new EdgeInsetsJsonConverter(),
            new IconJsonConverter(),
            new InputBorderJsonConverter(),
        }
    };

    public static void WriteDynamicValue(in DynamicValue value, Utf8JsonWriter writer, DynamicPropertyMeta propertyMeta)
    {
        var valueType = propertyMeta.ValueType;
        //如果是状态值且是值类型且不可空，则需要转换为可空值类型
        if (propertyMeta.IsState && propertyMeta.ValueType.IsValueType && !propertyMeta.IsNullableValueType)
            valueType = typeof(Nullable<>).MakeGenericType(valueType);

        if (!propertyMeta.IsState)
        {
            JsonSerializer.Serialize(writer, value.Value, valueType /*必须指定类型以适配某此自定义多态序列化*/, SerializerOptions);
            return;
        }

        writer.WriteStartObject();

        switch (value.From)
        {
            case ValueSource.Const:
                writer.WritePropertyName(nameof(ValueSource.Const));
                JsonSerializer.Serialize(writer, value.Value, valueType /*必须指定类型以适配某此自定义多态序列化*/, SerializerOptions);
                break;
            case ValueSource.State:
                writer.WritePropertyName(nameof(ValueSource.State));
                writer.WriteStringValue((string?)value.Value);
                break;
            default: throw new JsonException($"Unknown ValueSource");
        }

        writer.WriteEndObject();
    }

    public static DynamicValue ReadDynamicValue(ref Utf8JsonReader reader, DynamicPropertyMeta propertyMeta)
    {
        var valueType = propertyMeta.ValueType;
        //如果是状态值且是值类型且不可空，则需要转换为可空值类型，否则下面Deserialize读取null时会报错
        if (propertyMeta.IsState && propertyMeta.ValueType.IsValueType && !propertyMeta.IsNullableValueType)
            valueType = typeof(Nullable<>).MakeGenericType(valueType);

        var v = new DynamicValue();

        if (!propertyMeta.IsState)
        {
            v.From = ValueSource.Const;
            v.Value = JsonSerializer.Deserialize(ref reader, valueType, SerializerOptions);
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
                    v.Value = JsonSerializer.Deserialize(ref reader, valueType, SerializerOptions);
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