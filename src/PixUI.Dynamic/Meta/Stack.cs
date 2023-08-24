using System;

namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeStackMeta() => new(
        "Layout", "Stack", typeof(Stack), MaterialIcons.Layers,
        slots: new ContainerSlot[]
        {
            new(nameof(Stack.Children), ContainerType.MultiChild)
        }
    );

    private static DynamicWidgetMeta MakePositionedMeta() => new(
        string.Empty, "Positioned", typeof(Positioned), MaterialIcons.PictureInPicture,
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