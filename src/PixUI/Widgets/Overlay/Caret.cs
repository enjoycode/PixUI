using System;
using System.Threading;

namespace PixUI;

public sealed class Caret
{
    public Caret(Widget widget, Func<Rect> boundsBuilder, Func<Color>? colorBuilder)
    {
        _widget = widget;
        ColorBuilder = colorBuilder;
        BoundsBuilder = boundsBuilder;
    }

    private readonly Widget _widget; //拥有caret的Widget
    internal readonly Func<Color>? ColorBuilder;
    internal readonly Func<Rect> BoundsBuilder;
    private CaretDecorator? _decorator;
    private Timer? timer;
    private bool _isHidden;

    public bool IsHidden
    {
        get => _isHidden;
        set
        {
            if (_isHidden == value)
                return;
            if (value) Hide();
            else Show();
        }
    }

    internal Widget Target => _widget;

    public void Show()
    {
        _isHidden = false;
        _decorator ??= new CaretDecorator(this);
        _widget.Overlay?.Show(_decorator);
    }

    public void Hide()
    {
        _isHidden = true;
        (_decorator?.Parent as Overlay)?.Remove(_decorator!);
    }

    public void NotifyPositionChanged() => _decorator?.Invalidate(InvalidAction.Repaint);

    public void StartBlinking()
    {
        Show();
        timer?.Dispose();
        var blinkTime = TimeSpan.FromMilliseconds(530);
        timer = new Timer(s => { UIApplication.Current.BeginInvoke(() => IsHidden = !IsHidden); }, null,
            blinkTime, blinkTime);
    }

    public void StopBlinking()
    {
        timer!.Dispose();
        Hide();
    }
}

internal sealed class CaretDecorator : FlowDecorator<Widget>
{
    private readonly Caret _owner;

    public CaretDecorator(Caret owner) : base(owner.Target, true)
    {
        _owner = owner;
    }

    protected override void PaintCore(Canvas canvas)
    {
        var paint = PaintUtils.Shared();
        if (_owner.ColorBuilder == null)
        {
            paint.Color = new Color(0xFFFFFFFF);
            paint.BlendMode = BlendMode.Difference;
        }
        else
        {
            paint.Color = _owner.ColorBuilder();
        }

        var bounds = _owner.BoundsBuilder();
        canvas.DrawRect(Rect.FromLTWH(bounds.Left, bounds.Top, bounds.Width, bounds.Height), paint);
    }
}