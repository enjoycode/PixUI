using System;
using System.Linq;

namespace PixUI.Dynamic;

/// <summary>
/// 设计时创建占位用的DesignElement的委托
/// </summary>
public delegate Widget CreateDesignElement(string slotName, Size? size, DynamicWidgetMeta? meta, Widget? child);

/// <summary>
/// 动态组件定义
/// </summary>
public sealed class DynamicWidgetMeta
{
    internal DynamicWidgetMeta(string catalog, string name, Type widgetType, IconData icon,
        Func<Widget> instanceMaker,
        DynamicPropertyMeta[]? properties = null,
        DynamicEventMeta[]? events = null,
        ContainerSlot[]? slots = null,
        float? initWidth = null,
        float? initHeight = null,
        Action<IDesignElement, CreateDesignElement>? onAddToCanvas = null)
    {
        _instanceMaker = instanceMaker;
        Catalog = catalog;
        Name = name;
        WidgetType = widgetType;
        Icon = icon;
        Events = events;
        Slots = slots;
        OnAddToCanvas = onAddToCanvas ?? OnAddToCanvasDefault;

        // 暂简单根据是否反向包装来判断，可考虑添加Sizable参数来判断
        if (IsReversedWrapElement)
        {
            Properties = properties;
        }
        else
        {
            var len = properties?.Length ?? 0;
            Properties = new DynamicPropertyMeta[len + 2];
            Properties[0] = new DynamicPropertyMeta(nameof(Widget.Width), typeof(State<float>),
                true, initValue: initWidth);
            Properties[1] = new DynamicPropertyMeta(nameof(Widget.Height), typeof(State<float>),
                true, initValue: initHeight);
            for (var i = 0; i < len; i++)
            {
                Properties[i + 2] = properties![i];
            }
        }
    }

    public static DynamicWidgetMeta Make<T>(IconData icon,
        string? catalog = null,
        string? name = null,
        DynamicPropertyMeta[]? properties = null,
        DynamicEventMeta[]? events = null,
        ContainerSlot[]? slots = null,
        float? initWidth = null,
        float? initHeight = null,
        Action<IDesignElement, CreateDesignElement>? onAddToCanvas = null)
        where T : Widget
    {
        var widgetType = typeof(T);
        return new DynamicWidgetMeta(catalog ?? string.Empty, name ?? widgetType.Name,
            widgetType, icon, Activator.CreateInstance<T> /*use Emit?*/,
            properties, events, slots, initWidth, initHeight, onAddToCanvas);
    }

    private readonly Func<Widget> _instanceMaker;

    /// <summary>
    /// 工具箱显示的分类 eg: Charts
    /// </summary>
    public readonly string Catalog;

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

    /// <summary>
    /// 设计时添加至画布后的操作(一般用于添加子级占位)
    /// </summary>
    public readonly Action<IDesignElement, CreateDesignElement>? OnAddToCanvas;

    public bool ShowOnToolbox => Catalog != string.Empty;
    public bool IsContainer => Slots is { Length: > 0 };

    public bool IsReversedWrapElement => Slots is { Length: 1 } &&
                                         Slots[0].ContainerType == ContainerType.SingleChildReversed;

    public ContainerSlot DefaultSlot
    {
        get
        {
            if (!IsContainer)
                throw new Exception($"[{WidgetType.Name}] is not a container");
            return Slots![0];
        }
    }

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

    public bool IsSlot(string name, out ContainerSlot? slot)
    {
        if (IsContainer)
        {
            slot = Slots!.FirstOrDefault(s => s.PropertyName == name);
            return slot != null;
        }

        slot = null;
        return false;
    }

    /// <summary>
    /// 创建目标组件的实例
    /// </summary>
    public Widget CreateInstance() => _instanceMaker();

    private static void OnAddToCanvasDefault(IDesignElement designElement, CreateDesignElement createPlaceHolder)
    {
        if (designElement.Target is SingleChildWidget { IsLayoutTight: true })
        {
            var meta = designElement.Meta!;
            if (meta.IsContainer)
            {
                var defaultSlot = meta.DefaultSlot;
                defaultSlot.SetChild(designElement.Target!,
                    createPlaceHolder(defaultSlot.PropertyName, new(100, 100), null, null));
            }
        }
    }
}