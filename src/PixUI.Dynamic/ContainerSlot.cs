namespace PixUI.Dynamic;

public enum ContainerType
{
    SingleChild,
    /// <summary>
    /// 用于如Expanded, Positioned等特例，反向向上包装
    /// </summary>
    SingleChildReversed,
    MultiChild,
}

public sealed class ContainerSlot
{
    public ContainerSlot(string propertyName, ContainerType type)
    {
        PropertyName = propertyName;
        ContainerType = type;
    }

    public readonly string PropertyName;
    public readonly ContainerType ContainerType;
}