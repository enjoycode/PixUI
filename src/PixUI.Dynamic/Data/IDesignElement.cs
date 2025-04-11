namespace PixUI.Dynamic;

public interface IDesignElement
{
    DynamicWidgetMeta? Meta { get; }

    DynamicWidgetData Data { get; }

    Widget? Target { get; }
}