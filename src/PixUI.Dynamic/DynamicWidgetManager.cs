using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PixUI.Dynamic;

/// <summary>
/// 管理设计时与运行时的DynamicWidget
/// </summary>
public static partial class DynamicWidgetManager
{
    static DynamicWidgetManager()
    {
        Register(MakeButtonMeta());
        Register(MakeTextMeta());
        Register(MakeTextInputMeta());
        Register(MakeDatePickerMeta());
        Register(MakeSelectMeta());
        Register(MakeRow());
        Register(MakeColumn());
        Register(MakeExpanded());
        Register(MakeStackMeta());
        Register(MakePositionedMeta());
        Register(MakeCardMeta());
        Register(MakeCenterMeta());
    }

    private static readonly Dictionary<string, DynamicWidgetMeta> _dynamicWidgets = new();

    private static IEventActionManager? _eventActionManager;

    public static IEventActionManager EventActionManager => _eventActionManager!;

    public static void TryInitEventActionManager(Func<IEventActionManager> creator)
    {
        _eventActionManager ??= creator();
    }

    public static void Register(DynamicWidgetMeta dynamicWidgetMeta, bool replaceExists = false)
    {
        if (_dynamicWidgets.ContainsKey(dynamicWidgetMeta.Name) && !replaceExists)
            throw new Exception("Already exists.");

        _dynamicWidgets[dynamicWidgetMeta.Name] = dynamicWidgetMeta;
    }

    public static void Register<T>(IconData icon,
        string? catalog = null,
        string? name = null,
        DynamicPropertyMeta[]? properties = null,
        DynamicEventMeta[]? events = null,
        ContainerSlot[]? slots = null, bool replaceExists = false)
        where T : Widget, new()
    {
        Register(DynamicWidgetMeta.Make<T>(icon, catalog, name, properties, events, slots), replaceExists);
    }

    /// <summary>
    /// 注册标了DynamicWidgetAttribute的组件
    /// </summary>
    public static bool Register(Type widgetType, bool replaceExists = false)
    {
        var baseType = typeof(Widget);
        if (!widgetType.IsAssignableTo(baseType))
            return false;

        //TODO:判断是否具备无参构造

        var attr = widgetType.GetCustomAttribute<DynamicWidgetAttribute>();
        if (attr == null)
            return false;

        var catalog = string.IsNullOrEmpty(attr.Catalog) ? "Other" : attr.Catalog;
        var name = string.IsNullOrEmpty(attr.Name) ? widgetType.Name : attr.Name;
        var icon = string.IsNullOrEmpty(attr.Icon) ? MaterialIcons.Widgets : GetIconByName(attr.Icon);

        // properties
        var props = widgetType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var list = new List<DynamicPropertyMeta>();
        foreach (var prop in props)
        {
            var propAttr = prop.GetCustomAttribute<DynamicPropertyAttribute>();
            if (propAttr == null) continue;

            var propName = prop.Name;
            var propType = prop.PropertyType;
            var allowNull = propType.IsValueType
                ? propType.GetGenericTypeDefinition() == typeof(Nullable<>)
                : propAttr.AllowNull;
            DynamicValue? initValue = null;
            if (propAttr.InitValue != null)
                initValue = new DynamicValue() { Value = propAttr.InitValue };
            
            //TODO: 使用以下示例确认是否init-only属性，不再需要手工指定DynamicProperty.InitSetter属性
            //参考: https://www.meziantou.net/csharp9-init-only-properties-are-not-read-only.htm
            //prop.SetMethod!.ReturnParameter.GetRequiredCustomModifiers(),
            //check is System.Runtime.CompilerServices.IsExternalInit

            var propMeta = new DynamicPropertyMeta(propName, propType,
                allowNull, propAttr.InitSetter, initValue, propAttr.Editor);
            list.Add(propMeta);
        }

        var meta = new DynamicWidgetMeta(catalog, name, widgetType, icon,
            () => (Widget)Activator.CreateInstance(widgetType)!,
            list.Count == 0 ? null : list.ToArray());
        try
        {
            Register(meta, replaceExists);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    // public static bool IsInitOnly(this PropertyInfo propertyInfo)
    // {
    //     var setMethod = propertyInfo.SetMethod;
    //     if (setMethod == null)
    //         return false;
    //
    //     var isExternalInitType = typeof(System.Runtime.CompilerServices.IsExternalInit);
    //     return setMethod.ReturnParameter.GetRequiredCustomModifiers().Contains(isExternalInitType);
    // }

    private static IconData GetIconByName(string iconName)
    {
        var propInfo = typeof(MaterialIcons).GetProperty(iconName, BindingFlags.Static | BindingFlags.Public);
        return propInfo == null ? MaterialIcons.Widgets : (IconData)propInfo.GetValue(null)!;
    }

    public static IList<DynamicWidgetMeta> GetAll() => _dynamicWidgets.Values.ToList();

    public static DynamicWidgetMeta GetByName(string name) => _dynamicWidgets[name]!;
}