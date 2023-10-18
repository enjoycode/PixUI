namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeRow() => DynamicWidgetMeta.Make<Row>(
        MaterialIcons.ViewArray,
        catalog: "Layout",
        slots: new ContainerSlot[]
        {
            new(nameof(Row.Children), ContainerType.MultiChild)
        }
    );

    private static DynamicWidgetMeta MakeColumn() => DynamicWidgetMeta.Make<Column>(
        MaterialIcons.ViewDay,
        catalog: "Layout",
        slots: new ContainerSlot[]
        {
            new(nameof(Column.Children), ContainerType.MultiChild)
        }
    );

    private static DynamicWidgetMeta MakeExpanded() => DynamicWidgetMeta.Make<Expanded>(
        MaterialIcons.AspectRatio,
        catalog: "Layout",
        slots: new ContainerSlot[]
        {
            new(nameof(Expanded.Child), ContainerType.SingleChildReversed)
        }
    );

    private static DynamicWidgetMeta MakeCenterMeta() => DynamicWidgetMeta.Make<Center>(
        MaterialIcons.CenterFocusStrong,
        catalog: "Layout",
        slots: new ContainerSlot[]
        {
            new(nameof(Center.Child), ContainerType.SingleChild)
        });

    private static DynamicWidgetMeta MakeStackMeta() => DynamicWidgetMeta.Make<Stack>(
        MaterialIcons.Layers,
        catalog: "Layout",
        slots: new ContainerSlot[]
        {
            new(nameof(Stack.Children), ContainerType.MultiChild)
        }
    );

    private static DynamicWidgetMeta MakePositionedMeta() => DynamicWidgetMeta.Make<Positioned>(
        MaterialIcons.PictureInPicture,
        catalog: string.Empty,
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Positioned.Left), typeof(State<float>), true),
            new(nameof(Positioned.Top), typeof(State<float>), true),
            new(nameof(Positioned.Right), typeof(State<float>), true),
            new(nameof(Positioned.Bottom), typeof(State<float>), true),
        },
        slots: new ContainerSlot[]
        {
            new(nameof(Positioned.Child), ContainerType.SingleChildReversed)
        }
    );
}