namespace PixUI;

public static class WrapExtensions
{
    public static Expanded WrapByExpanded(this Widget widget, int flex = 1) =>
        new Expanded(widget, flex);
}