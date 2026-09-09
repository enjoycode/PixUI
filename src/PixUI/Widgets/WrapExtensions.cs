namespace PixUI;

public static class WrapExtensions
{
    public static Expanded WrapByExpanded(this Widget widget, int flex = 1) =>
        new Expanded(widget, flex);

    public static TParent WithChildren<TParent>(this TParent parent, IList<Widget> children)
        where TParent : MultiChildWidget<Widget>
    {
        parent.Children = children;
        return parent;
    }

    public static TParent WithChildren<TParent, TChild>(this TParent parent, IList<TChild> children)
        where TParent : MultiChildWidget<TChild> where TChild : Widget
    {
        parent.Children = children;
        return parent;
    }

    public static TParent WithChild<TParent>(this TParent parent, Widget child) where TParent : SingleChildWidget
    {
        parent.Child = child;
        return parent;
    }

    public static TParent AddChild<TParent>(this TParent parent, Widget child) where TParent : MultiChildWidget<Widget>
    {
        parent.Children.Add(child);
        return parent;
    }

    public static TParent AddChild<TParent, TChild>(this TParent parent, TChild child)
        where TParent : MultiChildWidget<TChild> where TChild : Widget
    {
        parent.Children.Add(child);
        return parent;
    }
}