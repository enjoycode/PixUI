namespace PixUI.Diagram;

public sealed class DiagramView : Transform, IScrollable, IMouseRegion
{
    public DiagramView(IDesignService designService) : base(Matrix4.CreateIdentity())
    {
        _mouseRegion = new MouseRegion();
        _scrollBars = new ScrollBarDecorator<DiagramView>(this, this,
            () => new(MaxScrollOffsetX, MaxScrollOffsetY));

        Surface = new DiagramSurface(designService);
        Child = Surface;
    }

    private readonly MouseRegion _mouseRegion;
    private readonly ScrollBarDecorator<DiagramView> _scrollBars;
    private float _scrollOffsetX;
    private float _scrollOffsetY;
    private float _scale = 1.0f;

    public DiagramSurface Surface { get; }

    MouseRegion IMouseRegion.MouseRegion => _mouseRegion;

    #region ====IScrollable====

    ScrollDirection IScrollable.ScrollDirection => ScrollDirection.Both;
    ScrollBarVisibility IScrollable.ShowScrollBar => ScrollBarVisibility.Hover;
    float IScrollable.ScrollOffsetX => _scrollOffsetX;
    float IScrollable.ScrollOffsetY => _scrollOffsetY;

    private float MaxScrollOffsetX => Math.Max(0, W); //TODO: get max right element
    private float MaxScrollOffsetY => Math.Max(0, H); //TODO: get max bottom element

    Offset IScrollable.OnScroll(float dx, float dy)
    {
        _scrollOffsetX += dx;
        _scrollOffsetY += dy;
        //TODO:
        return new(0, 0);
    }

    #endregion
}