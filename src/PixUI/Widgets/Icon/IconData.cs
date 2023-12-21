using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixUI;

[JsonConverter(typeof(IconJsonConverter))]
public readonly struct IconData
{
    public readonly int CodePoint;
    public readonly IconAsset Asset;

    public IconData(int codePoint, IconAsset asset)
    {
        CodePoint = codePoint;
        Asset = asset;
    }

#if __WEB__
    public IconData Clone() => new IconData(CodePoint, Asset);
#endif
}

public sealed class IconJsonConverter : JsonConverter<IconData>
{
    public override IconData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Read(); //Type prop
        reader.Read(); //Type value

        reader.Read();
        reader.Read();
        var fontFamily = reader.GetString()!;
        reader.Read();
        reader.Read();
        var assemblyName = reader.GetString()!;
        reader.Read();
        reader.Read();
        var assetPath = reader.GetString()!;
        reader.Read();
        reader.Read();
        var codePoint = reader.GetInt32();

        reader.Read(); //}
        return new IconData(codePoint, new IconAsset(fontFamily, assemblyName, assetPath));
    }

    public override void Write(Utf8JsonWriter writer, IconData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Type", 0); //类型保留，用于扩展SvgIcon等
        writer.WriteString(nameof(IconAsset.FontFamily), value.Asset.FontFamily);
        writer.WriteString(nameof(IconAsset.AssemblyName), value.Asset.AssemblyName);
        writer.WriteString(nameof(IconAsset.AssetPath), value.Asset.AssetPath);
        writer.WriteNumber(nameof(value.CodePoint), value.CodePoint);
        writer.WriteEndObject();
    }
}