namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeCenterMeta() =>
        new()
        {
            Catelog = "Layout", Name = "Center", WidgetType = typeof(Center), ContainerType = ContainerType.SingleChild,
            Icon = MaterialIcons.CenterFocusStrong,
            AddChild = (parent, child) => ((Center)parent).Child = child,
        };
}