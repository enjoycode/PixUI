using System;

namespace PixUI;

public sealed class HoverDecoration
{
    public HoverDecoration(Widget widget, Func<ShapeBorder> shapeBuilder,
        Func<Rect>? boundsGetter = null, float elevation = 4,
        Color? hoverColor = null)
    {
        Widget = widget;
        ShapeBuilder = shapeBuilder;
        BoundsGetter = boundsGetter;
        Elevation = elevation;
        HoverColor = hoverColor;
    }

    internal readonly Widget Widget;
    internal readonly Func<ShapeBorder> ShapeBuilder;
    internal readonly Func<Rect>? BoundsGetter;
    internal readonly float Elevation;
    internal readonly Color? HoverColor;
    private HoverDecorator? _decorator;

    public void Show()
    {
        _decorator = new HoverDecorator(this);
        Widget.Overlay?.Show(_decorator);
    }

    public void Hide()
    {
        if (_decorator == null) return;
        ((Overlay)_decorator.Parent!).Remove(_decorator);
        _decorator = null;
    }

    public void AttachHoverChangedEvent(IMouseRegion widget) =>
        widget.MouseRegion.HoverChanged += _OnHoverChanged;

    private void _OnHoverChanged(bool hover)
    {
        if (hover)
            Show();
        else
            Hide();
    }
}

internal sealed class HoverDecorator : FlowDecorator<Widget>
{
    private readonly HoverDecoration _owner;
    private readonly ShapeBorder _shape;

    internal HoverDecorator(HoverDecoration owner): base(owner.Widget, true)
    {
        _owner = owner;
        _shape = owner.ShapeBuilder();
    }

    protected override void PaintCore(Canvas canvas)
    {
        Rect bounds;
        if (_owner.BoundsGetter == null)
        {
            var widget = _owner.Widget;
            bounds = Rect.FromLTWH(0, 0, widget.W, widget.H);
        }
        else
        {
            bounds = _owner.BoundsGetter();
        }

        using var path = _shape.GetOuterPath(bounds);

        // draw shadow
        if (_owner.Elevation > 0)
        {
            canvas.Save();
            canvas.ClipPath(path, ClipOp.Difference, false);
            canvas.DrawShadow(path, Colors.Black, _owner.Elevation, false,
                Root!.Window.ScaleFactor);
            canvas.Restore();
        }

        // draw hover color
        if (_owner.HoverColor != null)
        {
            canvas.Save();
            canvas.ClipPath(path, ClipOp.Intersect, false);
            var paint = PaintUtils.Shared(_owner.HoverColor.Value);
            // paint.BlendMode = _owner.BlendMode;
            canvas.DrawPath(path, paint);
            canvas.Restore();
        }
    }
}