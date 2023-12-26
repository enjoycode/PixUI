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
            new(nameof(TextInput.Border), typeof(InputBorder), true),
        },
        initWidth: 111
    );

    private static DynamicWidgetMeta MakeSelectMeta() => DynamicWidgetMeta.Make<Select<string>>(
        MaterialIcons.EditAttributes,
        catalog: CatalogInput,
        name: "Select",
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Select<string>.Options), typeof(string[]), false),
            new(nameof(Select<string>.Value), typeof(State<string?>), false, true,
                initValue: string.Empty),
            new(nameof(Select<string>.TextColor), typeof(State<Color>), true),
            new(nameof(Select<string>.FillColor), typeof(State<Color>), true),
            new(nameof(Select<string>.Border), typeof(InputBorder), true),
        },
        initWidth: 111
    );

    private static DynamicWidgetMeta MakeDatePickerMeta() => DynamicWidgetMeta.Make<DatePicker>(
        MaterialIcons.Today,
        catalog: CatalogInput,
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(DatePicker.Value), typeof(State<DateTime?>), false, true,
                initValue: DateTime.Today, editorName: "DateEditor"),
            new(nameof(DatePicker.TextColor), typeof(State<Color>), true),
            new(nameof(DatePicker.FillColor), typeof(State<Color>), true),
            new(nameof(DatePicker.Border), typeof(InputBorder), true),
        },
        initWidth: 111
    );
}