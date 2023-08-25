namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeButtonMeta() => DynamicWidgetMeta.Make<Button>(
        MaterialIcons.SmartButton,
        catalog: "Common",
        properties: new DynamicPropertyMeta[]
        {
            new("Text", typeof(State<string>), true, true, "Button"),
            new("Icon", typeof(State<IconData>), true, true),
            new(nameof(Button.TextColor), typeof(State<Color>), true)
        }
    );
}