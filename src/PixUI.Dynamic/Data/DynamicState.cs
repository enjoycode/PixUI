using System.Text.Json;
using System.Threading.Tasks;

namespace PixUI.Dynamic;

public interface IDynamicState
{
    void WriteTo(Utf8JsonWriter writer);
}

public interface IDynamicValueState : IDynamicState
{
    void ReadFrom(ref Utf8JsonReader reader, DynamicState state);

    object? GetRuntimeValue();
}

public interface IDynamicDataSetState : IDynamicState
{
    void ReadFrom(ref Utf8JsonReader reader);

    ValueTask<object?> GetRuntimeDataSet();
}

public enum DynamicStateType
{
    DataSet,
    Int,
    String,
    DateTime,
}

public sealed class DynamicState
{
    public string Name { get; set; } = null!;
    public DynamicStateType Type { get; set; }
    public IDynamicState? Value { get; set; }
    public bool AllowNull { get; set; }
}