using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixUI.Dynamic.Json;

public sealed class InputBorderJsonConverter : JsonConverter<InputBorder>
{
    //TODO: 暂简单实现只支持Outline及Underline

    public override InputBorder Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Read(); //Type prop
        reader.Read();
        var type = reader.GetInt32();
        InputBorder res;
        if (type == 0)
        {
            reader.Read();
            var color = JsonSerializer.Deserialize<Color>(ref reader);
            reader.Read();
            reader.Read();
            var width = reader.GetSingle();
            reader.Read();
            reader.Read();
            var radius = reader.GetSingle();
            res = new OutlineInputBorder(new BorderSide(color, width), BorderRadius.Circular(radius));
        }
        else if (type == 1)
        {
            reader.Read();
            var color = JsonSerializer.Deserialize<Color>(ref reader);
            reader.Read();
            reader.Read();
            var width = reader.GetSingle();
            res = new UnderlineInputBorder(new BorderSide(color, width));
        }
        else
        {
            throw new NotImplementedException();
        }

        reader.Read(); // }
        return res;
    }

    public override void Write(Utf8JsonWriter writer, InputBorder value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (value is OutlineInputBorder outline)
        {
            writer.WriteNumber("Type", 0);
            writer.WritePropertyName("Color");
            JsonSerializer.Serialize(writer, value.BorderSide.Color);
            writer.WriteNumber("Width", value.BorderSide.Width);
            writer.WriteNumber("Radius", outline.BorderRadius.TopLeft.X); //暂全部视为一样的
        }
        else if (value is UnderlineInputBorder)
        {
            writer.WriteNumber("Type", 1);
            writer.WritePropertyName("Color");
            JsonSerializer.Serialize(writer, value.BorderSide.Color);
            writer.WriteNumber("Width", value.BorderSide.Width);
        }
        else
        {
            throw new NotImplementedException();
        }

        writer.WriteEndObject();
    }
}