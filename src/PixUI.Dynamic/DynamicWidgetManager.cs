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
        Register(new DynamicWidgetMeta
        {
            Catelog = "Common", Name = "Button", WidgetType = typeof(Button), ContainerType = ContainerType.None,
            Icon = MaterialIcons.SmartButton, CtorArgs = new[]
            {
                new DynamicCtorArgMeta()
                {
                    Name = "Text", AllowNull = true,
                    Value = new DynamicValueMeta
                        { ValueType = typeof(string), IsState = true, DefaultValue = () => new Rx<string>("Button") }
                },
                new DynamicCtorArgMeta()
                {
                    Name = "Icon", AllowNull = true,
                    Value = new DynamicValueMeta { ValueType = typeof(IconData), IsState = true }
                }
            }
        });
        Register(new DynamicWidgetMeta
        {
            Catelog = "Layout", Name = "Center", WidgetType = typeof(Center), ContainerType = ContainerType.SingleChild,
            Icon = MaterialIcons.CenterFocusStrong,
            AddChild = (parent, child) => ((Center)parent).Child = child,
        });
    }

    private static readonly Dictionary<string, DynamicWidgetMeta> _dynamicWidgets = new();

    public static void Register(DynamicWidgetMeta dynamicWidgetMeta)
    {
        if (_dynamicWidgets.ContainsKey(dynamicWidgetMeta.Name))
            throw new Exception("Already exists."); //TODO:考虑替换

        _dynamicWidgets.Add(dynamicWidgetMeta.Name, dynamicWidgetMeta);
    }

    public static IList<DynamicWidgetMeta> GetAll() => _dynamicWidgets.Values.ToList();

    public static DynamicWidgetMeta GetByName(string name) => _dynamicWidgets[name]!;
}