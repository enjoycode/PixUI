using Facebook.Yoga;
using Facebook.Yoga.Debug;

namespace PixUI.Markdown;

public static class YogaExtensions
{
    public static Node AppendChild(this Node owner, Node child)
    {
        AssertFatal.AssertWithNode(owner, child.GetOwner() == null,
            "Child already has a owner, it must be removed first.");
        AssertFatal.AssertWithNode(owner, !owner.HasMeasureFunc(),
            "Cannot add child: Nodes with measure functions cannot have children.");

        owner.InsertChild(child, owner.GetChildCount());
        child.SetOwner(owner);
        owner.MarkDirtyAndPropagate();
        return owner;
    }

    public static Node SetStyleSize(this Node node, float widthPt, float heightPt)
        => node.SetStyleSize(StyleSizeLength.Points(widthPt), StyleSizeLength.Points(heightPt));

    public static Node SetStyleSize(this Node node, StyleSizeLength width, StyleSizeLength height)
    {
        node.SetStyleDimension(Dimension.Width, width);
        node.SetStyleDimension(Dimension.Height, height);
        return node;
    }

    public static Node SetStyleWidth(this Node node, StyleSizeLength width) =>
        node.SetStyleDimension(Dimension.Width, width);

    public static Node SetStyleHeight(this Node node, StyleSizeLength height) =>
        node.SetStyleDimension(Dimension.Height, height);

    private static Node SetStyleDimension(this Node node, Dimension dim, StyleSizeLength value)
    {
        if (!(node.Style.Dimension(dim) != value))
            return node;
        node.Style.SetDimension(dim, value);
        node.MarkDirtyAndPropagate();
        node.ProcessDimensions();
        return node;
    }
}