using System;
using System.Reflection;

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

    public Type WidgetType { get; set; }

    public ContainerType ContainerType { get; set; }

    /// <summary>
    /// 用于如Expanded特例，反向向上包装
    /// </summary>
    public bool IsWrapReversed { get; set; }

    public DynamicCtorArgMeta[]? CtorArgs { get; set; }

    public DynamicPropertyMeta[]? Properties { get; set; }

    public DynamicEventMeta[]? Events { get; set; }

    public Action<Widget, Widget>? AddChild { get; set; }


    /// <summary>
    /// 设计时实例创建,工具箱拖至设计界面时
    /// </summary>
    public Widget MakeDefaultInstance()
    {
        //TODO:异常转换为ErrorWidget
        Widget res;

        if (CtorArgs == null || CtorArgs.Length == 0)
        {
            res = (Widget)Activator.CreateInstance(WidgetType);
        }
        else
        {
            var ctorArgs = new object?[CtorArgs.Length];
            for (var i = 0; i < CtorArgs.Length; i++)
            {
                var m = CtorArgs[i];
                if (m.Value.DefaultValue != null)
                    ctorArgs[i] = m.Value.DefaultValue();
                //TODO:None nullable to default value
            }

            res = (Widget)Activator.CreateInstance(WidgetType, ctorArgs);
        }

        return res;
    }
}

public sealed class DynamicValueMeta
{
    public Type ValueType { get; set; }

    public bool ValueNullable { get; set; }

    public bool IsState { get; set; }

    public Func<object>? DefaultValue { get; set; }
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