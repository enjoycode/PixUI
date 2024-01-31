using System;

namespace PixUI;

public sealed class SplitterBar : Widget, IMouseRegion
{
    public SplitterBar(Action<Offset> onResize)
    {
        _onResize = onResize;
        MouseRegion = new MouseRegion(GetCursor);
        MouseRegion.PointerMove += OnPointerMove;
    }

    private readonly Action<Offset> _onResize;

    public MouseRegion MouseRegion { get; }

    public Axis Orientation { get; set; } = Axis.Horizontal;

    public Color Color { get; set; } = Colors.Gray;

    private Cursor GetCursor() => Orientation == Axis.Horizontal ? Cursors.ResizeLR : Cursors.ResizeUD;

    private void OnPointerMove(PointerEvent e)
    {
        if (e.Buttons != PointerButtons.Left) return;

        _onResize(new(e.DeltaX, e.DeltaY));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //TODO:暂简单实现
        var paint = PixUI.Paint.Shared(Color);
        canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), paint);
    }
}

public sealed class Splitter : Widget
{
    public Splitter()
    {
        _bar = new SplitterBar(OnResize);
        _bar.Parent = this;
    }

    private readonly SplitterBar _bar;
    private readonly Widget? _panel1;
    private readonly Widget? _panel2;
    private readonly Axis _orientation = Axis.Horizontal;
    private readonly State<bool>? _panel1Collapsed;
    private readonly State<bool>? _paenl2Collapsed;
    private float _barDistance = float.NaN;

    public Axis Orientation
    {
        get => _orientation;
        init
        {
            _orientation = value;
            _bar.Orientation = value;
        }
    }

    public Widget? Panel1
    {
        get => _panel1;
        init
        {
            _panel1 = value;
            if (_panel1 != null)
                _panel1.Parent = this;
        }
    }

    public Widget? Panel2
    {
        get => _panel2;
        init
        {
            _panel2 = value;
            if (_panel2 != null)
                _panel2.Parent = this;
        }
    }

    /// <summary>
    /// 初始化面板大小，如果Fixed=Panel2指的是Panel2的大小
    /// </summary>
    public float Distance
    {
        get => _barDistance;
        init => _barDistance = value;
    }

    public float SplitterSize { get; init; } = 3f;

    public Color SplitterColor
    {
        get => _bar.Color;
        init => _bar.Color = value;
    }

    public State<bool>? Panel1Collapsed
    {
        get => _panel1Collapsed;
        init => Bind(ref _panel1Collapsed, value, RelayoutOnStateChanged);
    }

    private bool IsPanel1Collapsed => _panel1Collapsed is { Value: true };

    public State<bool>? Panel2Collapsed
    {
        get => _paenl2Collapsed;
        init => Bind(ref _paenl2Collapsed, value, RelayoutOnStateChanged);
    }

    private bool IsPanel2Collapsed => _paenl2Collapsed is { Value: true };

    public FixedPanel Fixed { get; init; } = FixedPanel.None;

    public enum FixedPanel
    {
        None = 0,
        Panel1 = 1,
        Panel2 = 2
    }

    private void OnResize(Offset offset)
    {
        if (Orientation == Axis.Horizontal)
        {
            if (float.IsNaN(_barDistance))
                _barDistance = W / 2;
            if (Fixed == FixedPanel.Panel2)
                _barDistance -= offset.Dx;
            else
                _barDistance += offset.Dx;
        }
        else
        {
            if (float.IsNaN(_barDistance))
                _barDistance = H / 2;
            if (Fixed == FixedPanel.Panel2)
                _barDistance -= offset.Dy;
            else
                _barDistance += offset.Dy;
        }

        Relayout();
    }

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (Panel1 != null && action(Panel1)) return;
        if (Panel2 != null && action(Panel2)) return;
        action(_bar);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var oldW = Math.Max(W, 0);
        var oldH = Math.Max(H, 0);
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        SetSize(maxSize.Width, maxSize.Height);

        if (IsPanel1Collapsed && IsPanel2Collapsed)
            return;

        if (IsPanel1Collapsed)
        {
            Panel2!.Layout(W, H);
            Panel2.SetPosition(0, 0);
            return;
        }

        if (IsPanel2Collapsed)
        {
            Panel1!.Layout(W, H);
            Panel1.SetPosition(0, 0);
            return;
        }

        if (Orientation == Axis.Horizontal)
        {
            var distance = _barDistance;
            if (float.IsNaN(distance))
                distance = W / 2;
            else if (Fixed == FixedPanel.Panel2)
                distance = W - distance - SplitterSize;

            if (oldW != 0 && Fixed == FixedPanel.None)
            {
                distance += (W - oldW) / 2;
                if (!float.IsNaN(_barDistance))
                    _barDistance += (W - oldW) / 2;
            }

            if (Panel1!.CachedAvailableWidth != distance || Panel1.CachedAvailableHeight != H)
                Panel1.Layout(distance, H);
            if (_bar.W != SplitterSize || _bar.H != H)
                _bar.Layout(SplitterSize, H);
            if (Panel2!.CachedAvailableWidth != W - distance - SplitterSize || Panel2.CachedAvailableHeight != H)
                Panel2.Layout(W - distance - SplitterSize, H);

            Panel1.SetPosition(0, 0);
            _bar.SetPosition(distance, 0);
            Panel2.SetPosition(distance + SplitterSize, 0);
        }
        else
        {
            var distance = _barDistance;
            if (float.IsNaN(distance))
                distance = H / 2;
            else if (Fixed == FixedPanel.Panel2)
                distance = H - distance - SplitterSize;

            if (oldH != 0 && Fixed == FixedPanel.None)
            {
                distance += (H - oldH) / 2;
                if (!float.IsNaN(_barDistance))
                    _barDistance += (H - oldH) / 2;
            }

            if (Panel1!.CachedAvailableWidth != W || Panel1.CachedAvailableHeight != distance)
                Panel1.Layout(W, distance);
            if (_bar.W != W || _bar.H != SplitterSize)
                _bar.Layout(W, SplitterSize);
            if (Panel2!.CachedAvailableWidth != W || Panel2.CachedAvailableHeight != H - distance - SplitterSize)
                Panel2.Layout(W, H - distance - SplitterSize);

            Panel1.SetPosition(0, 0);
            _bar.SetPosition(0, distance);
            Panel2.SetPosition(0, distance + SplitterSize);
        }
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (IsPanel1Collapsed && IsPanel2Collapsed) return;

        if (area is RepaintChild repaintChild)
        {
            repaintChild.Repaint(canvas);
            return;
        }

        if (Panel1 != null && !IsPanel1Collapsed)
            PaintChild(Panel1, canvas, area);

        if (Panel2 != null && !IsPanel2Collapsed)
            PaintChild(Panel2, canvas, area);

        if (!IsPanel1Collapsed && !IsPanel2Collapsed)
            PaintChild(_bar, canvas, area);
    }
}