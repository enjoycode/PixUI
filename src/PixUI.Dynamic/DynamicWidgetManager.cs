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
        Register(MakeRow());
        Register(MakeColumn());
        Register(MakeExpanded());
        Register(MakeStackMeta());
        Register(MakePositionedMeta());
        Register(MakeCenterMeta());
    }

    private static readonly Dictionary<string, DynamicWidgetMeta> _dynamicWidgets = new();

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

        //TODO: properties

        var meta = new DynamicWidgetMeta(catalog, name, widgetType, icon,
            () => (Widget)Activator.CreateInstance(widgetType)!);
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

    private static IconData GetIconByName(string iconName)
    {
        var propInfo = typeof(MaterialIcons).GetProperty(iconName, BindingFlags.Static | BindingFlags.Public);
        return propInfo == null ? MaterialIcons.Widgets : (IconData)propInfo.GetValue(null)!;
    }

    public static IList<DynamicWidgetMeta> GetAll() => _dynamicWidgets.Values.ToList();

    public static DynamicWidgetMeta GetByName(string name) => _dynamicWidgets[name]!;
}