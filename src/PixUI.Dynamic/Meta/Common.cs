namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private const string CatalogCommon = "Common";

    private static DynamicWidgetMeta MakeButtonMeta() => DynamicWidgetMeta.Make<Button>(
        MaterialIcons.SmartButton,
        catalog: CatalogCommon,
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Button.Text), typeof(State<string>), true, true, "Button"),
            new(nameof(Button.Icon), typeof(State<IconData>), true, true),
            new(nameof(Button.TextColor), typeof(State<Color>), true)
        },
        events: new DynamicEventMeta[]
        {
            new(nameof(Button.OnTap))
        }
    );

    private static DynamicWidgetMeta MakeTextMeta() => DynamicWidgetMeta.Make<Text>(
        MaterialIcons.Translate,
        catalog: CatalogCommon,
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Text.Text), typeof(State<string>), false, true, "Hello World"),
            new(nameof(Text.FontSize), typeof(State<float>), true),
            new(nameof(Text.TextColor), typeof(State<Color>), true),
        }
    );
}