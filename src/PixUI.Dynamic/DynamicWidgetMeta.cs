using System;
using System.Linq;
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
    /// 根据名称查找属性定义，找不到报错
    /// </summary>
    public DynamicPropertyMeta GetPropertyMeta(string name)
    {
        if (Properties == null || Properties.Length == 0) throw new Exception();
        return Properties.First(p => p.Name == name);
    }

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

    public Widget MakeInstance(ValueSource[] args)
    {
        if (CtorArgs == null || CtorArgs.Length != args.Length) throw new ArgumentException();

        var ctorArgs = new object?[args.Length];
        for (var i = 0; i < ctorArgs.Length; i++)
        {
            ctorArgs[i] = CtorArgs[i].Value.GetValue(ref args[i]);
        }

        return (Widget)Activator.CreateInstance(WidgetType, ctorArgs);
    }
}

public sealed class DynamicValueMeta
{
    public Type ValueType { get; set; }

    public bool ValueNullable { get; set; }

    public bool IsState { get; set; }

    public Func<object>? DefaultValue { get; set; }

    public object? GetValue(ref ValueSource source /*, IDynamicStateProvider stateProvider*/)
    {
        if (source.From != ValueFrom.Const) throw new NotImplementedException();

        //from const value, 已经在读取时转换类型为ValueType
        return source.Value;
    }
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