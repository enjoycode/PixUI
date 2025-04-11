namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private const string CatalogLayout = "Layout";

    private static DynamicWidgetMeta MakeRow() => DynamicWidgetMeta.Make<Row>(
        MaterialIcons.ViewArray,
        catalog: CatalogLayout,
        properties:
        [
            new(nameof(Row.Alignment), typeof(VerticalAlignment), false),
            new(nameof(Row.Spacing), typeof(float), false)
        ],
        slots:
        [
            new(nameof(Row.Children), ContainerType.MultiChild, ChildrenLayoutAxis.Horizontal)
        ],
        onAddToCanvas: static (element, createPlaceHolder) =>
        {
            //默认添加3个占位子级
            var defaultSlot = element.Meta!.DefaultSlot;
            for (var i = 0; i < 3; i++)
            {
                defaultSlot.TryAddChild(element.Target!,
                    createPlaceHolder(defaultSlot.PropertyName, new(100, 50), null, null));
            }
        }
    );

    private static DynamicWidgetMeta MakeColumn() => DynamicWidgetMeta.Make<Column>(
        MaterialIcons.ViewDay,
        catalog: CatalogLayout,
        properties:
        [
            new(nameof(Column.Alignment), typeof(HorizontalAlignment), false),
            new(nameof(Column.Spacing), typeof(float), false)
        ],
        slots:
        [
            new(nameof(Column.Children), ContainerType.MultiChild, ChildrenLayoutAxis.Vertical)
        ],
        onAddToCanvas: static (element, createPlaceHolder) =>
        {
            //默认添加3个占位子级
            var defaultSlot = element.Meta!.DefaultSlot;
            for (var i = 0; i < 3; i++)
            {
                defaultSlot.TryAddChild(element.Target!,
                    createPlaceHolder(defaultSlot.PropertyName, new(100, 50), null, null));
            }
        }
    );

    private static DynamicWidgetMeta MakeExpanded() => DynamicWidgetMeta.Make<Expanded>(
        MaterialIcons.AspectRatio,
        catalog: CatalogLayout,
        properties:
        [
            new(nameof(Expanded.Flex), typeof(int), false)
        ],
        slots:
        [
            new(nameof(Expanded.Child), ContainerType.SingleChildReversed)
        ]
    );

    private static DynamicWidgetMeta MakeCenterMeta() => DynamicWidgetMeta.Make<Center>(
        MaterialIcons.CenterFocusStrong,
        catalog: CatalogLayout,
        slots:
        [
            new(nameof(Center.Child), ContainerType.SingleChild)
        ]);

    private static DynamicWidgetMeta MakeCardMeta() => DynamicWidgetMeta.Make<Card>(
        MaterialIcons.FeaturedVideo,
        catalog: CatalogLayout,
        properties:
        [
            new(nameof(Card.Margin), typeof(State<EdgeInsets>), true),
            new(nameof(Card.Padding), typeof(State<EdgeInsets>), true),
            new(nameof(Card.Color), typeof(State<Color>), true),
            new(nameof(Card.ShadowColor), typeof(State<Color>), true),
            new(nameof(Card.Elevation), typeof(State<float>), true)
        ],
        slots:
        [
            new(nameof(Card.Child), ContainerType.SingleChild)
        ]);

    private static DynamicWidgetMeta MakeStackMeta() => DynamicWidgetMeta.Make<Stack>(
        MaterialIcons.Layers,
        catalog: CatalogLayout,
        slots:
        [
            new(nameof(Stack.Children), ContainerType.MultiChild, ChildrenLayoutAxis.Positioned)
        ]
    );

    private static DynamicWidgetMeta MakePositionedMeta() => DynamicWidgetMeta.Make<Positioned>(
        MaterialIcons.PictureInPicture,
        catalog: string.Empty,
        properties:
        [
            new(nameof(Positioned.Left), typeof(State<float>), true),
            new(nameof(Positioned.Top), typeof(State<float>), true),
            new(nameof(Positioned.Right), typeof(State<float>), true),
            new(nameof(Positioned.Bottom), typeof(State<float>), true)
        ],
        slots:
        [
            new(nameof(Positioned.Child), ContainerType.SingleChildReversed)
        ]
    );

    private static DynamicWidgetMeta MakeFormMeta() => DynamicWidgetMeta.Make<Form>(
        MaterialIcons.TableView,
        catalog: CatalogLayout,
        properties:
        [
            new(nameof(Form.LabelWidth), typeof(float), false)
        ],
        slots:
        [
            new(nameof(Form.Children), ContainerType.MultiChild, ChildrenLayoutAxis.Vertical)
        ],
        onAddToCanvas: static (element, createPlaceHolder) =>
        {
            var defaultSlot = element.Meta!.DefaultSlot;
            for (var i = 0; i < 3; i++)
            {
                var formItemMeta = GetByName(nameof(FormItem));
                var child = createPlaceHolder(nameof(FormItem.Child), new(100, 30), null, null);
                var formItem = new FormItem("Label:",
                    createPlaceHolder(defaultSlot.PropertyName, null, formItemMeta, child));

                defaultSlot.TryAddChild(element.Target!, formItem);
            }
        }
    );

    private static DynamicWidgetMeta MakeFormItemMeta() => DynamicWidgetMeta.Make<FormItem>(
        MaterialIcons.TableRows,
        catalog: CatalogLayout,
        properties:
        [
            new(nameof(FormItem.Label), typeof(string), false, true, initValue: string.Empty)
        ],
        slots:
        [
            new(nameof(FormItem.Child), ContainerType.SingleChildReversed)
        ]
    );
}