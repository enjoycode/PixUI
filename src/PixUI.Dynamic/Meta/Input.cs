using System;

namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeTextInputMeta() => DynamicWidgetMeta.Make<TextInput>(
        MaterialIcons.TextFields,
        catalog: "Input",
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(TextInput.Text), typeof(State<string>), false, true, initValue: string.Empty),
        }
    );

    private static DynamicWidgetMeta makeDatePickerMeta() => DynamicWidgetMeta.Make<DatePicker>(
        MaterialIcons.Today,
        catalog: "Input",
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(DatePicker.Value), typeof(State<DateTime?>), false, true, initValue: DateTime.Today),
        }
    );
}