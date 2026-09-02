using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixUI.Dynamic.Json;

public sealed class EdgeInsetsJsonConverter : JsonConverter<EdgeInsets>
{
    public override EdgeInsets Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
            return EdgeInsets.All(reader.GetSingle());

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new Exception("EdgeInserts json format error");

        reader.Read();
        var left = reader.GetSingle();
        reader.Read();
        var top = reader.GetSingle();
        reader.Read();
        var right = reader.GetSingle();
        reader.Read();
        var bottom = reader.GetSingle();

        reader.Read();
        return EdgeInsets.Only(left, top, right, bottom);
    }

    public override void Write(Utf8JsonWriter writer, EdgeInsets value, JsonSerializerOptions options)
    {
        if (value.IsAllSame)
        {
            writer.WriteNumberValue(value.Left);
        }
        else
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Left);
            writer.WriteNumberValue(value.Top);
            writer.WriteNumberValue(value.Right);
            writer.WriteNumberValue(value.Bottom);
            writer.WriteEndArray();
        }
    }
}