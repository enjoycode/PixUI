using System;
using System.Collections.Generic;
using System.Linq;

namespace PixUI.Dynamic;

/// <summary>
/// 管理设计时与运行时的DynamicWidget
/// </summary>
public static class DynamicWidgetManager
{
    static DynamicWidgetManager()
    {
        Register(new DynamicWidgetMeta { Catelog = "Common", Name = "Button", Icon = MaterialIcons.SmartButton});
        Register(new DynamicWidgetMeta { Catelog = "Layout", Name = "Center", Icon = MaterialIcons.CenterFocusStrong});
    }

    private static readonly Dictionary<string, DynamicWidgetMeta> _dynamicWidgets = new();

    public static void Register(DynamicWidgetMeta dynamicWidgetMeta)
    {
        if (_dynamicWidgets.ContainsKey(dynamicWidgetMeta.Name))
            throw new Exception("Already exists.");

        _dynamicWidgets.Add(dynamicWidgetMeta.Name, dynamicWidgetMeta);
    }

    public static IList<DynamicWidgetMeta> GetAll() => _dynamicWidgets.Values.ToList();
}