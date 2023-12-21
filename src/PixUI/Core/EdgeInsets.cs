using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixUI;

[JsonConverter(typeof(EdgeInsetsJsonConverter))]
public readonly struct EdgeInsets : IEquatable<EdgeInsets>
{
    public readonly float Left;
    public readonly float Top;
    public readonly float Right;
    public readonly float Bottom;

    public float Horizontal => Left + Right;
    public float Vertical => Top + Bottom;
    public bool IsAllSame => Left == Right && Left == Top && Left == Bottom;

    private EdgeInsets(float left, float top, float right, float bottom)
    {
        Left = Math.Max(0, left);
        Top = Math.Max(0, top);
        Right = Math.Max(0, right);
        Bottom = Math.Max(0, bottom);
    }

    public static implicit operator EdgeInsets(float value) => new(value, value, value, value);

    public static EdgeInsets All(float value) => new(value, value, value, value);

    public static EdgeInsets Only(float left, float top, float right, float bottom) =>
        new(left, top, right, bottom);

    public EdgeInsets Clone() => new EdgeInsets(Left, Top, Right, Bottom);

    public static bool operator ==(EdgeInsets left, EdgeInsets right) => left.Equals(right);

    public static bool operator !=(EdgeInsets left, EdgeInsets right) => !left.Equals(right);

    public bool Equals(EdgeInsets other) => Left == other.Left && Top == other.Top &&
                                            Right == other.Right && Bottom == other.Bottom;

#if !__WEB__
    public override bool Equals(object? obj) => obj is EdgeInsets other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);
#endif
}

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