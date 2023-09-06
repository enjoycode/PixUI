namespace PixUI.Dynamic;

public enum DynamicStateType
{
    DataSet,
    Int,
    String,
}

public sealed class DynamicState
{
    public string Name { get; set; } = null!;
    public DynamicStateType Type { get; set; }
    public IDynamicStateValue? Value { get; set; }
}