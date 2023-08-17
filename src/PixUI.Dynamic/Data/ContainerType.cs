namespace PixUI.Dynamic;

public enum ContainerType
{
    None = 0,
    SingleChild,
    /// <summary>
    /// 用于如Expanded, Positioned等特例，反向向上包装
    /// </summary>
    SingleChildReversed,
    MultiChild,
}
