namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeCenterMeta() => DynamicWidgetMeta.Make<Center>(
        MaterialIcons.CenterFocusStrong,
        catalog: "Layout",
        slots: new ContainerSlot[]
        {
            new(nameof(Center.Child), ContainerType.SingleChild)
        });
}