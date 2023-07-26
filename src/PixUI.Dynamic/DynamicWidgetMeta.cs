using System;

namespace PixUI.Dynamic;

/// <summary>
/// 动态组件定义
/// </summary>
public sealed class DynamicWidgetMeta
{
    /// <summary>
    /// 工具箱显示的分类 eg: Charts
    /// </summary>
    public string Catelog { get; set; }

    /// <summary>
    /// 工具箱显示的名称(惟一性) eg: PieChart
    /// </summary>
    public string Name { get; set; }
    
    public IconData Icon { get; set; }

    public DynamicCtorArgMeta[]? CtorArgs { get; set; }

    public DynamicPropertyMeta[]? Properties { get; set; }

    public DynamicEventMeta[]? Events { get; set; }

    /// <summary>
    /// 设计时实例创建,工具箱拖至设计界面时
    /// </summary>
    public Func<Widget> Creator { get; set; }

    // /// <summary>
    // /// 运行时解析定义创建实例
    // /// </summary>
    // public Func<string, Widget> Parser { get; set; }
}

public sealed class DynamicValueMeta
{
    public Type ValueType { get; set; }

    public bool ValueNullable { get; set; }

    public bool IsState { get; set; }
}

public sealed class DynamicCtorArgMeta
{
    public string Name { get; set; }

    public DynamicValueMeta Value { get; set; }

    public bool AllowNull { get; set; }
}

public sealed class DynamicPropertyMeta
{
    public string Name { get; set; }

    public DynamicValueMeta Value { get; set; }

    public bool AllowNull { get; set; }
}

public sealed class DynamicEventMeta
{
    public string Name { get; set; }
}