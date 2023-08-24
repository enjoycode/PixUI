namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeButtonMeta() => new(
        "Common", "Button", typeof(Button),
        MaterialIcons.SmartButton,
        ctorArgs: new DynamicCtorArgMeta[]
        {
            new("Text", typeof(State<string>), true, "Button"),
            new("Icon", typeof(State<IconData>), true)
        },
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Button.TextColor), typeof(State<Color>), true)
        }
    );
}