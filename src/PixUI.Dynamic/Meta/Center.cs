namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeCenterMeta() => new(
        "Layout", "Center", typeof(Center), MaterialIcons.CenterFocusStrong,
        slots: new ContainerSlot[]
        {
            new(nameof(Center.Child), ContainerType.SingleChild)
        }
    );
}