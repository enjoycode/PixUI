namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeButtonMeta() => new()
    {
        Catelog = "Common",
        Name = "Button",
        WidgetType = typeof(Button),
        ContainerType = ContainerType.None,
        Icon = MaterialIcons.EditAttributes,
        CtorArgs = new DynamicCtorArgMeta[]
        {
            new("Text", typeof(State<string>), true, "Button"),
            new("Icon", typeof(State<IconData>), true)
        },
        Properties = new DynamicPropertyMeta[]
        {
            new(nameof(Button.TextColor), typeof(State<Color>), true)
        },
    };
}