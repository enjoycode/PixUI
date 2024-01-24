using System;

namespace PixUI;

public sealed class ScrollBarDecorator<T> : FlowDecorator<T> where T : Widget, IScrollable
{
    public ScrollBarDecorator(T target, Func<Offset> maxOffsetGetter) : base(target, false)
    {
        _target = target;
        _maxOffsetGetter = maxOffsetGetter;

        if (target.ScrollDirection is ScrollDirection.Both or ScrollDirection.Horizontal)
        {
            _hBar = new ScrollBar(Axis.Horizontal);
            _hBar.Parent = this;
        }

        if (target.ScrollDirection is ScrollDirection.Both or ScrollDirection.Vertical)
        {
            _vBar = new ScrollBar(Axis.Vertical);
            _vBar.Parent = this;
        }
    }

    private readonly T _target;
    private readonly Func<Offset> _maxOffsetGetter;
    private readonly ScrollBar? _hBar;
    private readonly ScrollBar? _vBar;
    private bool _hBarVisible;
    private bool _vBarVisible;
    private Overlay? _overlay;

    public void Show()
    {
        _overlay = _target.Overlay;
        _overlay?.Show(this);
    }

    public void Hide() => _overlay?.Remove(this);

    protected override void PaintCore(Canvas canvas)
    {
        var maxOffset = _maxOffsetGetter();
        _hBarVisible = _hBar != null && maxOffset.Dx != 0;
        _vBarVisible = _vBar != null && maxOffset.Dy != 0;
        var allVisible = _hBarVisible && _vBarVisible;

        if (_hBarVisible)
        {
            _hBar!.Update(_target.ScrollOffsetX, maxOffset.Dx);

            _hBar.Layout(_target.W - (allVisible ? _vBar!.Size : 0), _target.H);
            _hBar.SetPosition(0, _target.H - _hBar.Size);
            _hBar.BeforePaint(canvas, true);
            _hBar.Paint(canvas);
            _hBar.AfterPaint(canvas);
        }

        if (_vBarVisible)
        {
            _vBar!.Update(_target.ScrollOffsetY, maxOffset.Dy);

            _vBar.Layout(_target.W, _target.H - (allVisible ? _hBar!.Size : 0));
            _vBar.SetPosition(_target.W - _vBar.Size, 0);
            _vBar.BeforePaint(canvas, true);
            _vBar.Paint(canvas);
            _vBar.AfterPaint(canvas);
        }
    }
}

public sealed class ScrollBar : Widget, IMouseRegion
{
    public ScrollBar(Axis axis)
    {
        _axis = axis;
        MouseRegion = new MouseRegion();
    }

    private readonly Axis _axis;
    private float _offset;
    private float _maxOffset;
    private const float _minThumbLength = 20f;

    public float Size { get; init; } = 10f;

    public MouseRegion MouseRegion { get; }

    public void Update(float offset, float maxOffset)
    {
        _offset = offset;
        _maxOffset = maxOffset;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        if (_axis == Axis.Horizontal)
            SetSize(availableWidth, Size);
        else
            SetSize(Size, availableHeight);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        // draw background
        Color bgColor = 0x555F6368;
        var rect = Rect.FromLTWH(0, 0, W, H);
        var paint = PixUI.Paint.Shared(bgColor);
        canvas.DrawRect(rect, paint);

        // draw thumb
        Rect thumbRect;
        if (_axis == Axis.Horizontal)
        {
            var thumbLength = Math.Max(W - _maxOffset, _minThumbLength);
            var ratio = (W - thumbLength) / _maxOffset;
            var start = _offset * ratio;
            thumbRect = Rect.FromLTWH(start, 1, thumbLength, Size - 2);
        }
        else
        {
            var thumbLength = Math.Max(H - _maxOffset, _minThumbLength);
            var ratio = (H - thumbLength) / _maxOffset;
            var start = _offset * ratio;
            thumbRect = Rect.FromLTWH(1, start, Size - 2, thumbLength);
        }

        paint = PixUI.Paint.Shared(Colors.Gray);
        paint.AntiAlias = true;
        using var thumb = RRect.FromRectAndRadius(thumbRect, (Size - 2) / 2, (Size - 2) / 2);
        canvas.DrawRRect(thumb, paint);
    }
}