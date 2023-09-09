using System.Text.Json;
using System.Threading.Tasks;

namespace PixUI.Dynamic;

public interface IDynamicStateValue
{
    void WriteTo(Utf8JsonWriter writer);

    void ReadFrom(ref Utf8JsonReader reader);
}

public interface IDynamicDataSetStateValue : IDynamicStateValue
{
    ValueTask<object?> GetRuntimeDataSet();
}