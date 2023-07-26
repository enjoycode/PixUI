namespace PixUI.Design;

/// <summary>
/// 描述设计元素的信息，用于序列化及生成相应的组件实例
/// </summary>
public sealed class DesignElementData
{
    

    public ContainerType ContainerType { get; set; }

    /// <summary>
    /// 用于如Expanded特例，反向向上包装
    /// </summary>
    public bool IsWrapReversed { get; set; }
}

/// <summary>
/// 组件构造参数信息
/// </summary>
public sealed class CtorArgInfo { }

/// <summary>
/// 设计时组件的属性信息
/// </summary>
public sealed class PropertyInfo { }