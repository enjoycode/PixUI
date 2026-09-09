namespace PixUI;

partial class ChainExtensions
{
    public static TWidget RefBy<TWidget>(this TWidget widget, ref TWidget by) where TWidget : Widget
    {
        by = widget;
        return widget;
    }

    public static TWidget WithWidth<TWidget>(this TWidget widget, State<float> width) where TWidget : Widget
    {
        widget.Width = width;
        return widget;
    }

    public static TWidget WithHeight<TWidget>(this TWidget widget, State<float> height) where TWidget : Widget
    {
        widget.Height = height;
        return widget;
    }
}