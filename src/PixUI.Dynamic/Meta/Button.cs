namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeButtonMeta() =>
        new()
        {
            Catelog = "Common", Name = "Button", WidgetType = typeof(Button), ContainerType = ContainerType.None,
            Icon = MaterialIcons.SmartButton,
            CtorArgs = new[]
            {
                new DynamicCtorArgMeta("Text", typeof(State<string>), true, "Button"),
                new DynamicCtorArgMeta("Icon", typeof(State<IconData>), true)
            },
            Properties = new[]
            {
                new DynamicPropertyMeta(nameof(Button.TextColor), typeof(State<Color>), true)
            },
        };
}