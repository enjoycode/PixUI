using System;

namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeStackMeta() => new()
    {
        Catelog = "Layout",
        Name = "Stack",
        WidgetType = typeof(Stack),
        ContainerType = ContainerType.MultiChild,
        Icon = MaterialIcons.Layers,
        AddChildAction = (parent, child) =>
        {
            var stack = (Stack)parent;
            if (child is Positioned positioned)
                stack.Children.Add(positioned);
            else
                throw new NotSupportedException("Only Positioned can be add to Stack");
        }
    };

    private static DynamicWidgetMeta MakePositionedMeta() => new()
    {
        Catelog = "Layout",
        Name = "Positioned",
        WidgetType = typeof(Positioned),
        ContainerType = ContainerType.SingleChildReversed,
        Icon = MaterialIcons.PictureInPicture,
        AddChildAction = (parent, child) => ((Positioned)parent).Child = child,
        Properties = new DynamicPropertyMeta[]
        {
            new(nameof(Positioned.Left), typeof(State<float>), true),
            new(nameof(Positioned.Top), typeof(State<float>), true),
            new(nameof(Positioned.Right), typeof(State<float>), true),
            new(nameof(Positioned.Bottom), typeof(State<float>), true),
        }
    };
}