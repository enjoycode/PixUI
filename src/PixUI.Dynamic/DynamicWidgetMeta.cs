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
    public string Catelog { get; set; } = null!;

    /// <summary>
    /// 工具箱显示的名称(惟一性) eg: PieChart
    /// </summary>
    public string Name { get; set; } = null!;

    public IconData Icon { get; set; }

    public Type WidgetType { get; set; } = null!;

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
                    ctorArgs[i] = m.Value.GetRuntimeValue(m.Value.DefaultValue.Value);
                //TODO:None nullable to default value
            }

            res = (Widget)Activator.CreateInstance(WidgetType, ctorArgs);
        }

        return res;
    }

    public Widget MakeInstance(DynamicValue[] args)
    {
        if (CtorArgs == null || CtorArgs.Length != args.Length) throw new ArgumentException();

        var ctorArgs = new object?[args.Length];
        for (var i = 0; i < ctorArgs.Length; i++)
        {
            ctorArgs[i] = CtorArgs[i].Value.GetRuntimeValue(args[i]);
        }

        return (Widget)Activator.CreateInstance(WidgetType, ctorArgs);
    }
}

public sealed class DynamicValueMeta
{
    public DynamicValueMeta(Type runtimeType, DynamicValue? defautValue = null)
    {
        //先判断是否状态类型
        if (typeof(State).IsAssignableFrom(runtimeType))
        {
            if (runtimeType.IsGenericType && runtimeType.GetGenericTypeDefinition() == typeof(State<>))
            {
                ValueType = runtimeType.GenericTypeArguments[0];
                IsState = true;
            }
            else
            {
                throw new NotSupportedException("Only State<> supported");
            }
        }
        else
        {
            ValueType = runtimeType;
            IsState = false;
        }

        DefaultValue = defautValue;
    }

    public readonly Type ValueType;
    public readonly bool IsState;
    public readonly DynamicValue? DefaultValue;

    public object? GetRuntimeValue(in DynamicValue source /*, IDynamicStateProvider stateProvider*/)
    {
        if (source.From != ValueSource.Const) throw new NotImplementedException();

        //from const value, 已经在读取时转换类型为ValueType
        if (IsState && source.Value != null) //TODO: check Nullable of value
        {
            var rxType = typeof(RxValue<>).MakeGenericType(ValueType);
            return Activator.CreateInstance(rxType, source.Value);
        }
        return source.Value;
    }
}

public sealed class DynamicCtorArgMeta
{
    public DynamicCtorArgMeta(string name, Type runtimeType, bool allowNull, DynamicValue? defaultValue = null)
    {
        Name = name;
        AllowNull = allowNull;
        Value = new DynamicValueMeta(runtimeType, defaultValue);
    }

    public readonly string Name;
    public readonly DynamicValueMeta Value;
    public readonly bool AllowNull;
}

public sealed class DynamicPropertyMeta
{
    public DynamicPropertyMeta(string name, Type runtimeType, bool allowNull, DynamicValue? defaultValue = null)
    {
        Name = name;
        AllowNull = allowNull;
        Value = new DynamicValueMeta(runtimeType, defaultValue);
    }

    public readonly string Name;
    public readonly DynamicValueMeta Value;
    public readonly bool AllowNull;
}

public sealed class DynamicEventMeta
{
    public string Name { get; set; }
}