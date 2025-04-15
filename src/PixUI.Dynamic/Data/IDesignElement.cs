namespace PixUI.Dynamic;

public interface IDesignElement
{
    DynamicWidgetMeta? Meta { get; }

    DynamicWidgetData Data { get; }

    Widget? Target { get; }
    
    Widget? Child { get; set; }

    /// <summary>
    /// 设计时创建占位用的DesignElement的委托
    /// </summary>
    /// <param name="slotName"></param>
    /// <param name="size"></param>
    /// <param name="meta">是否指定类型，否则表示占位</param>
    /// <param name="child">是否指定子级实例</param>
    Widget CreatePlaceHolder(string slotName, Size? size, DynamicWidgetMeta? meta, Widget? child);
}