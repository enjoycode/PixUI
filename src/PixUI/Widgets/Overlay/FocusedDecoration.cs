using System;

namespace PixUI;

public sealed class FocusedDecoration
{
    public FocusedDecoration(Widget widget, Func<ShapeBorder> focusedBorderBuilder,
        Func<ShapeBorder?>? unfocusedBorderBuilder = null)
    {
        Widget = widget;
        _focusedBorderBuilder = focusedBorderBuilder;
        _unfocusedBorderBuilder = unfocusedBorderBuilder;
    }

    internal readonly Widget Widget;

    // Focus时的Border,用于动画结束
    private readonly Func<ShapeBorder> _focusedBorderBuilder;

    // 未Focus时的Border,用于动画开始
    private readonly Func<ShapeBorder?>? _unfocusedBorderBuilder;

    private FocusedDecorator? _decorator;

    public void AttachFocusChangedEvent(Widget widget)
    {
        if (widget is IFocusable focusable)
            focusable.FocusNode.FocusChanged += _OnFocusChanged;
    }

    private void _OnFocusChanged(bool focused)
    {
        if (focused)
        {
            _decorator = new FocusedDecorator(this);
            Widget.Overlay?.Show(_decorator);
        }
        else
        {
            _decorator?.Hide();
        }
    }

    internal ShapeBorder? GetUnfocusedBorder() => _unfocusedBorderBuilder?.Invoke();

    internal ShapeBorder GetFocusedBorder() => _focusedBorderBuilder();

    internal void StopAndReset() => _decorator?.Reset(); //will remove overlay

    internal void RemoveOverlayEntry()
    {
        if (_decorator == null) return;
        ((Overlay)_decorator.Parent!).Remove(_decorator);
        _decorator = null;
    }
}

internal sealed class FocusedDecorator : FlowDecorator<Widget>
{
    private readonly FocusedDecoration _owner;
    private readonly ShapeBorder? _from;
    private readonly ShapeBorder _to;
    private readonly ShapeBorder? _tween;
    private AnimationController? _controller;

    internal FocusedDecorator(FocusedDecoration owner) : base(owner.Widget, true)
    {
        _owner = owner;
        _from = owner.GetUnfocusedBorder();
        _to = owner.GetFocusedBorder();
        if (_from != null)
            _tween = _from.Clone();
    }

    internal void Hide()
    {
        if (_from == null)
        {
            _owner.RemoveOverlayEntry();
            return;
        }

        _controller?.Reverse();
    }

    internal void Reset() => _controller?.Reset();

    protected override void PaintCore(Canvas canvas)
    {
        var widget = _owner.Widget;
        var bounds = Rect.FromLTWH(0, 0, widget.W, widget.H);
        if (_from == null)
            _to.Paint(canvas, bounds);
        else
            _tween!.Paint(canvas, bounds);
    }

    protected override void OnMounted()
    {
        if (_from == null) return;

        if (_controller == null)
        {
            _controller = new AnimationController(200);
            _controller.ValueChanged += OnAnimationValueChanged;
            _controller.StatusChanged += OnAnimationStateChanged;
        }

        _controller.Forward();
    }

    private void OnAnimationValueChanged()
    {
        _from!.LerpTo(_to, _tween!, _controller!.Value);
        Invalidate(InvalidAction.Repaint);
    }

    private void OnAnimationStateChanged(AnimationStatus status)
    {
        if (status == AnimationStatus.Dismissed)
        {
            _owner.RemoveOverlayEntry();
        }
    }

    public override void Dispose()
    {
        if (_controller != null)
        {
            _controller.ValueChanged -= OnAnimationValueChanged;
            _controller.StatusChanged -= OnAnimationStateChanged;
            _controller.Dispose();
        }

        base.Dispose();
    }
}