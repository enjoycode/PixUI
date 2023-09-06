using System.Text.Json;

namespace PixUI.Dynamic;

public interface IDynamicStateValue
{
    void WriteTo(Utf8JsonWriter writer);

    void ReadFrom(ref Utf8JsonReader reader);
}