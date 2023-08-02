using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixUI.Dynamic;

public sealed class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString()!;
        var intValue = uint.Parse(stringValue, NumberStyles.HexNumber);
        return new Color(intValue);
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}