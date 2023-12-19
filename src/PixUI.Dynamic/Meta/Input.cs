using System;

namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private const string CatalogInput = "Input";

    private static DynamicWidgetMeta MakeTextInputMeta() => DynamicWidgetMeta.Make<TextInput>(
        MaterialIcons.TextFields,
        catalog: CatalogInput,
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(TextInput.Text), typeof(State<string>), false, true, initValue: string.Empty),
            new(nameof(TextInput.HintText), typeof(string), true),
            new(nameof(TextInput.TextColor), typeof(State<Color>), true),
            new(nameof(TextInput.FillColor), typeof(State<Color>), true),
        }
    );

    private static DynamicWidgetMeta makeDatePickerMeta() => DynamicWidgetMeta.Make<DatePicker>(
        MaterialIcons.Today,
        catalog: CatalogInput,
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(DatePicker.Value), typeof(State<DateTime?>), false, true,
                initValue: DateTime.Today, editorName: "DateEditor"),
            new(nameof(DatePicker.TextColor), typeof(State<Color>), true),
            new(nameof(DatePicker.FillColor), typeof(State<Color>), true),
        }
    );
}