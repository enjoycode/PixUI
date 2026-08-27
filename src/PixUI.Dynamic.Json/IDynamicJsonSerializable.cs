using System.Text.Json;

namespace PixUI.Dynamic.Json;

public interface IDynamicJsonSerializable
{
    void WriteTo(Utf8JsonWriter writer);

    void ReadFrom(ref Utf8JsonReader reader, DynamicState state);
}