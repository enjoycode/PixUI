namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private const string CatalogLayout = "Layout";

    private static DynamicWidgetMeta MakeRow() => DynamicWidgetMeta.Make<Row>(
        MaterialIcons.ViewArray,
        catalog: CatalogLayout,
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Row.Alignment), typeof(VerticalAlignment), false),
            new(nameof(Row.Spacing), typeof(float), false)
        },
        slots: new ContainerSlot[]
        {
            new(nameof(Row.Children), ContainerType.MultiChild)
        }
    );

    private static DynamicWidgetMeta MakeColumn() => DynamicWidgetMeta.Make<Column>(
        MaterialIcons.ViewDay,
        catalog: CatalogLayout,
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Column.Alignment), typeof(HorizontalAlignment), false),
            new(nameof(Column.Spacing), typeof(float), false)
        },
        slots: new ContainerSlot[]
        {
            new(nameof(Column.Children), ContainerType.MultiChild)
        }
    );

    private static DynamicWidgetMeta MakeExpanded() => DynamicWidgetMeta.Make<Expanded>(
        MaterialIcons.AspectRatio,
        catalog: CatalogLayout,
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Expanded.Flex), typeof(int), false)
        },
        slots: new ContainerSlot[]
        {
            new(nameof(Expanded.Child), ContainerType.SingleChildReversed)
        }
    );

    private static DynamicWidgetMeta MakeCenterMeta() => DynamicWidgetMeta.Make<Center>(
        MaterialIcons.CenterFocusStrong,
        catalog: CatalogLayout,
        slots: new ContainerSlot[]
        {
            new(nameof(Center.Child), ContainerType.SingleChild)
        });

    private static DynamicWidgetMeta MakeCardMeta() => DynamicWidgetMeta.Make<Card>(
        MaterialIcons.FeaturedVideo,
        catalog: CatalogLayout,
        properties: new DynamicPropertyMeta[]
        {
            new(nameof(Card.Margin), typeof(State<EdgeInsets>), true),
            new(nameof(Card.Padding), typeof(State<EdgeInsets>), true),
            new(nameof(Card.Color), typeof(State<Color>), true),
            new(nameof(Card.ShadowColor), typeof(State<Color>), true),
            new(nameof(Card.Elevation), typeof(State<float>), true),
        },
        slots: new ContainerSlot[]
        {
            new(nameof(Card.Child), ContainerType.SingleChild)
        });

    private static DynamicWidgetMeta MakeStackMeta() => DynamicWidgetMeta.Make<Stack>(
        MaterialIcons.Layers,
        catalog: CatalogLayout,
        slots: new ContainerSlot[]
        {
            new(nameof(Stack.Children), ContainerType.MultiChild, ChildrenLayoutAxis.Positioned)
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