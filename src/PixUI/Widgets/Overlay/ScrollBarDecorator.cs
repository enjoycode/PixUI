using System;

namespace PixUI;

public sealed class ScrollBarDecorator<T> : FlowDecorator<T> where T : Widget, IScrollable
{
    public ScrollBarDecorator(T target, IMouseRegion mouseRegion, Func<Offset> maxOffsetGetter) : base(target, false)
    {
        _target = target;
        _mouseRegion = mouseRegion;
        _maxOffsetGetter = maxOffsetGetter;

        if (target.ScrollDirection is ScrollDirection.Both or ScrollDirection.Horizontal)
        {
            _hBar = new ScrollBar(Axis.Horizontal);
            _hBar.Parent = this;
            _hBar.MouseRegion.HoverChanged += OnBarHoverChanged;
            _hBar.NeedScroll += dx => _target.OnScroll(dx, 0);
        }

        if (target.ScrollDirection is ScrollDirection.Both or ScrollDirection.Vertical)
        {
            _vBar = new ScrollBar(Axis.Vertical);
            _vBar.Parent = this;
            _vBar.MouseRegion.HoverChanged += OnBarHoverChanged;
            _vBar.NeedScroll += dy => _target.OnScroll(0, dy);
        }
    }

    private readonly IMouseRegion _mouseRegion;
    private readonly T _target;
    private readonly Func<Offset> _maxOffsetGetter;
    private readonly ScrollBar? _hBar;
    private readonly ScrollBar? _vBar;
    private bool _hBarVisible;
    private bool _vBarVisible;
    private Overlay? _overlay;
    private Matrix3 _totalTransform;

    public void Show()
    {
        _overlay = _target.Overlay;
        _overlay?.Show(this);
    }

    public void Hide(bool force)
    {
        //如果新命中的是ScrollBar则不关闭
        var newHit = _overlay?.Window.LastHitWidget;
        if (!force && ((_vBarVisible && _vBar == newHit) || (_hBarVisible && _hBar == newHit))) return;

        _overlay?.Remove(this);
    }

    private void OnBarHoverChanged(bool isHover)
    {
        if (isHover) return;
        if (_target.ShowScrollBar != ScrollBarVisibility.Hover) return;

        var newHit = _overlay?.Window.LastHitWidget;
        if (newHit == null)
        {
            _overlay?.Remove(this);
            return;
        }

        var found = newHit.FindParent(w => w == _mouseRegion);
        if (found == null)
        {
            _overlay?.Remove(this);
        }
    }

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        if (_totalTransform == Matrix3.Empty)
            return false;

        var mp = _totalTransform.MapPoint(0, 0);
        var localPt = new Point(x + mp.X, y + mp.Y); //_totalTransform.MapPoint(x, y);
        if (localPt.X < 0 || localPt.X > _target.W || localPt.Y < 0 || localPt.Y > _target.H)
            return false;

        // Log.Debug($"Local: {localPt.X}, {localPt.Y}");

        //注意只处理ScrollBar，忽略本身
        if (_vBarVisible && _vBar!.ContainsPoint(localPt.X - _vBar.X, localPt.Y - _vBar.Y))
        {
            result.Add(_vBar);
            result.ConcatLastTransform(Matrix4.CreateIdentity(), -mp.X, -mp.Y);
            return true;
        }

        if (_hBarVisible && _hBar!.ContainsPoint(localPt.X - _hBar.X, localPt.Y - _hBar.Y))
        {
            result.Add(_hBar);
            result.ConcatLastTransform(Matrix4.CreateIdentity(), -mp.X, -mp.Y);
            return true;
        }

        return false;
    }

    protected override void PaintCore(Canvas canvas)
    {
        //TODO:新版skia直接使用Matrix4
        _totalTransform = canvas.GetTotalMatrix();
        var winScale = _overlay!.Window.ScaleFactor;
        if (winScale != 1)
        {
            var scale = Matrix3.CreateScale(1 / winScale, 1 / winScale);
            _totalTransform.Multiply(scale);
        }

        if (_totalTransform.IsInvertible)
            _totalTransform.Invert();
        else
            _totalTransform = Matrix3.Empty;

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
        // MouseRegion.HoverChanged += isHover => IsHover = isHover;
        MouseRegion.PointerDown += OnPointerDown;
        MouseRegion.PointerMove += OnPointerMove;
    }

    private readonly Axis _axis;
    private float _offset;
    private float _maxOffset;
    private const float _minThumbLength = 20f;
    private Rect _thumbRect = Rect.Empty;
    private bool _hitThumb;

    public event Action<float>? NeedScroll;

    public float Size { get; init; } = 10f;

    public MouseRegion MouseRegion { get; }

    // public bool IsHover { get; private set; }

    public void Update(float offset, float maxOffset)
    {
        _offset = offset;
        _maxOffset = maxOffset;
    }

    private void OnPointerDown(PointerEvent e)
    {
        _hitThumb = false;
        if (_thumbRect.ContainsPoint(e.X, e.Y))
        {
            _hitThumb = true;
            return;
        }

        if (_axis == Axis.Horizontal)
        {
            var thumbLength = _thumbRect.Width;
            var ratio = (W - thumbLength) / _maxOffset;
            var scrollDelta = (e.X - _thumbRect.MidX) / ratio;
            NeedScroll?.Invoke(scrollDelta);
        }
        else
        {
            var thumbLength = _thumbRect.Height;
            var ratio = (H - thumbLength) / _maxOffset;
            var scrollDelta = (e.Y - _thumbRect.MidY) / ratio;
            NeedScroll?.Invoke(scrollDelta);
        }
    }

    private void OnPointerMove(PointerEvent e)
    {
        if (e.Buttons != PointerButtons.Left) return;
        if (!_hitThumb) return;

        if (_axis == Axis.Horizontal)
        {
            var thumbLength = _thumbRect.Width;
            var ratio = (W - thumbLength) / _maxOffset;
            var scrollDelta = e.DeltaX / ratio;
            NeedScroll?.Invoke(scrollDelta);
        }
        else
        {
            var thumbLength = _thumbRect.Height;
            var ratio = (H - thumbLength) / _maxOffset;
            var scrollDelta = e.DeltaY / ratio;
            NeedScroll?.Invoke(scrollDelta);
        }
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
        if (_axis == Axis.Horizontal)
        {
            var thumbLength = Math.Max(W - _maxOffset, _minThumbLength);
            var ratio = (W - thumbLength) / _maxOffset;
            var start = _offset * ratio;
            _thumbRect = Rect.FromLTWH(start, 1, thumbLength, Size - 2);
        }
        else
        {
            var thumbLength = Math.Max(H - _maxOffset, _minThumbLength);
            var ratio = (H - thumbLength) / _maxOffset;
            var start = _offset * ratio;
            _thumbRect = Rect.FromLTWH(1, start, Size - 2, thumbLength);
        }

        paint = PixUI.Paint.Shared(Colors.Gray);
        paint.AntiAlias = true;
        using var thumb = RRect.FromRectAndRadius(_thumbRect, (Size - 2) / 2, (Size - 2) / 2);
        canvas.DrawRRect(thumb, paint);
    }
}