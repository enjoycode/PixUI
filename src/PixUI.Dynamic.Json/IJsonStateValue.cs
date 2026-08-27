using System.Text.Json;

namespace PixUI.Dynamic.Json;

/// <summary>
/// 支持Json序列化的DynamicStateValue
/// </summary>
public interface IJsonStateValue : IDynamicStateValue
{
    void WriteTo(Utf8JsonWriter writer);

    void ReadFrom(ref Utf8JsonReader reader, DynamicState state);
}