namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeButtonMeta() => DynamicWidgetMeta.Make<Button>(
        MaterialIcons.SmartButton,
        catalog: "Common",
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Button.Text), typeof(State<string>), true, true, "Button"),
            new(nameof(Button.Icon), typeof(State<IconData>), true, true),
            new(nameof(Button.TextColor), typeof(State<Color>), true)
        }
    );

    private static DynamicWidgetMeta MakeTextMeta() => DynamicWidgetMeta.Make<Text>(
        MaterialIcons.Translate,
        catalog: "Common",
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Text.Text), typeof(State<string>), false, true, "Hello World"),
            new(nameof(Text.FontSize), typeof(State<float>), true),
            new(nameof(Text.TextColor), typeof(State<Color>), true),
        }
    );
}