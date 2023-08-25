using System;
using System.Linq;
using System.Reflection;

namespace PixUI.Dynamic;

/// <summary>
/// 动态组件定义
/// </summary>
public sealed class DynamicWidgetMeta
{
    private DynamicWidgetMeta(string catalog, string name, Type widgetType, IconData icon,
        Func<Widget> instanceMaker,
        DynamicPropertyMeta[]? properties = null,
        DynamicEventMeta[]? events = null,
        ContainerSlot[]? slots = null)
    {
        _instanceMaker = instanceMaker;
        Catelog = catalog;
        Name = name;
        WidgetType = widgetType;
        Icon = icon;
        Properties = properties;
        Events = events;
        Slots = slots;
    }

    public static DynamicWidgetMeta Make<T>(IconData icon,
        string? catalog = null,
        string? name = null,
        DynamicPropertyMeta[]? properties = null,
        DynamicEventMeta[]? events = null,
        ContainerSlot[]? slots = null)
        where T : Widget, new()
    {
        var widgetType = typeof(T);
        return new DynamicWidgetMeta(catalog ?? string.Empty, name ?? widgetType.Name,
            widgetType, icon, () => new T(),
            properties, events, slots);
    }

    private readonly Func<Widget> _instanceMaker;

    /// <summary>
    /// 工具箱显示的分类 eg: Charts
    /// </summary>
    public readonly string Catelog;

    /// <summary>
    /// 工具箱显示的名称(惟一性) eg: PieChart
    /// </summary>
    public readonly string Name;

    public readonly IconData Icon;

    public readonly Type WidgetType;

    // public readonly DynamicCtorArgMeta[]? CtorArgs;
    public readonly DynamicPropertyMeta[]? Properties;
    public readonly DynamicEventMeta[]? Events;
    public readonly ContainerSlot[]? Slots;

    public bool ShowOnToolbox => Catelog != string.Empty;
    public bool IsContainer => Slots is { Length: > 0 };

    public bool IsReversedWrapElement => Slots is { Length: 1 } &&
                                         Slots[0].ContainerType == ContainerType.SingleChildReversed;

    /// <summary>
    /// 根据名称查找属性定义，找不到报错
    /// </summary>
    public DynamicPropertyMeta GetPropertyMeta(string name)
    {
        if (Properties == null || Properties.Length == 0) throw new Exception();
        return Properties.First(p => p.Name == name);
    }

    public ContainerSlot GetSlot(string name)
    {
        if (!IsContainer) 
            throw new Exception($"[{WidgetType.Name}] can't get slot [{name}]");
        return Slots!.First(s => s.PropertyName == name);
    }

    /// <summary>
    /// 创建目标组件的实例
    /// </summary>
    public Widget CreateInstance() => _instanceMaker();
}

// public sealed class DynamicValueMeta
// {
//     public DynamicValueMeta(Type runtimeType, DynamicValue? defautValue = null)
//     {
//         //先判断是否状态类型
//         if (typeof(State).IsAssignableFrom(runtimeType))
//         {
//             if (runtimeType.IsGenericType && runtimeType.GetGenericTypeDefinition() == typeof(State<>))
//             {
//                 ValueType = runtimeType.GenericTypeArguments[0];
//                 IsState = true;
//             }
//             else
//             {
//                 throw new NotSupportedException("Only State<> supported");
//             }
//         }
//         else
//         {
//             ValueType = runtimeType;
//             IsState = false;
//         }
//
//         DefaultValue = defautValue;
//     }
//
//     public readonly Type ValueType;
//     public readonly bool IsState;
//     public readonly DynamicValue? DefaultValue;
//
//     public object? GetRuntimeValue(in DynamicValue source /*, IDynamicStateProvider stateProvider*/)
//     {
//         if (source.From != ValueSource.Const) throw new NotImplementedException();
//
//         //from const value, 已经在读取时转换类型为ValueType
//         if (IsState && source.Value != null) //TODO: check Nullable of value
//         {
//             var rxType = typeof(RxValue<>).MakeGenericType(ValueType);
//             return Activator.CreateInstance(rxType, source.Value);
//         }
//
//         return source.Value;
//     }
// }

// public sealed class DynamicCtorArgMeta
// {
//     public DynamicCtorArgMeta(string name, Type runtimeType, bool allowNull, DynamicValue? defaultValue = null)
//     {
//         Name = name;
//         AllowNull = allowNull;
//         Value = new DynamicValueMeta(runtimeType, defaultValue);
//     }
//
//     public readonly string Name;
//     public readonly DynamicValueMeta Value;
//     public readonly bool AllowNull;
// }

public sealed class DynamicPropertyMeta
{
    public DynamicPropertyMeta(string name, Type runtimeType, bool allowNull, bool initSetter = false,
        DynamicValue? initValue = null)
    {
        Name = name;
        AllowNull = allowNull;
        IsInitSetter = initSetter;
        InitValue = initValue;
        //判断是否状态类型
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
    }

    public readonly string Name;
    public readonly Type ValueType;
    public readonly bool AllowNull;
    public readonly bool IsInitSetter;
    public readonly bool IsState;

    /// <summary>
    /// 仅用于设计时创建实例时的初始化值
    /// </summary>
    public readonly DynamicValue? InitValue;

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

public sealed class DynamicEventMeta
{
    public DynamicEventMeta(string name)
    {
        Name = name;
    }

    public readonly string Name;
}