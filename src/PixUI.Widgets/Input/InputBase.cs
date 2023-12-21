using System;

namespace PixUI;

public abstract class InputBase<T> : Widget where T : Widget //, IFocusable
{
    protected InputBase()
    {
        _focusedDecoration = new FocusedDecoration(this, GetFocusedBorder, GetUnFocusedBorder);
    }

    private Widget? _prefix;
    private Widget? _suffix;
    private readonly T _editor = null!;

    private InputBorder? _border;
    private State<Color>? _fillColor;
    private State<EdgeInsets>? _padding;
    private readonly FocusedDecoration _focusedDecoration;

    protected T Editor
    {
        get => _editor;
        init
        {
            _editor = value;
            _editor.Parent = this;

            _focusedDecoration.AttachFocusChangedEvent(_editor);
        }
    }

    public State<EdgeInsets>? Padding
    {
        get => _padding;
        set => Bind(ref _padding, value, RelayoutOnStateChanged);
    }

    public abstract State<bool>? Readonly { get; set; }

    public bool IsReadonly => Readonly is { Value: true };

    public InputBorder? Border
    {
        get => _border;
        set
        {
            _border = value;
            Relayout();
        }
    }

    public State<Color>? FillColor
    {
        get => _fillColor;
        set => Bind(ref _fillColor, value, RepaintOnStateChanged);
    }

    protected Widget? PrefixWidget
    {
        get => _prefix;
        set
        {
            if (_prefix != null)
                _prefix.Parent = null;

            _prefix = value;
            if (_prefix == null) return;

            _prefix.Parent = this;
            Relayout();
        }
    }

    protected Widget? SuffixWidget
    {
        get => _suffix;
        set
        {
            if (_suffix != null)
                _suffix.Parent = null;

            _suffix = value;
            if (_suffix == null) return;

            _suffix.Parent = this;
            Relayout();
        }
    }

    #region ====FocusedDecoration====

    private ShapeBorder GetUnFocusedBorder() => _border ?? InputBorder.DefaultBorder;

    private ShapeBorder GetFocusedBorder()
    {
        var border = _border ?? InputBorder.DefaultBorder;
        var borderSide = new BorderSide(Theme.FocusedColor, Theme.FocusedBorderWidth);
        return border switch
        {
            OutlineInputBorder outline => new OutlineInputBorder(borderSide, outline.BorderRadius),
            UnderlineInputBorder => new UnderlineInputBorder(borderSide),
            _ => throw new NotImplementedException()
        };
    }

    #endregion

    #region ====Widget Overrides====

    protected override void OnUnmounted()
    {
        _focusedDecoration.StopAndReset();
        base.OnUnmounted();
    }

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (_prefix != null && action(_prefix)) return;

        if (action(_editor)) return;

        if (_suffix != null) action(_suffix);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        var width = maxSize.Width;
        var height = maxSize.Height;
        var padding = _padding?.Value ?? EdgeInsets.All(4);

        // 扣除padding的宽高
        var lw = width - padding.Horizontal;
        var lh = height - padding.Vertical;
        if (lw <= 0 || lh <= 0)
        {
            SetSize(width, height);
            return;
        }

        // 布局计算子组件
        if (_prefix != null)
        {
            _prefix.Layout(lw, lh);
            lw -= _prefix.W;
        }

        if (_suffix != null)
        {
            _suffix.Layout(lw, lh);
            lw -= _suffix.W;
        }

        _editor.Layout(lw, lh);

        // 设置子组件位置(暂以editor为中心上下居中对齐, TODO:考虑基线对齐)
        var maxChildHeight = _editor.H;
        _prefix?.SetPosition(padding.Left, (maxChildHeight - _prefix.H) / 2 + padding.Top);
        _suffix?.SetPosition(width - padding.Right - _suffix.W, (maxChildHeight - _suffix.H) / 2 + padding.Top);
        _editor.SetPosition(padding.Left + (_prefix?.W ?? 0), padding.Top + 1 /*offset*/);

        // 设置自身宽高
        height = Math.Min(height, maxChildHeight + padding.Vertical);
        SetSize(width, height);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var bounds = Rect.FromLTWH(0, 0, W, H);
        var border = _border ?? InputBorder.DefaultBorder;

        //画背景及边框
        var fillColor = _fillColor?.Value ?? Colors.White;
        border.Paint(canvas, bounds, IsReadonly ? Theme.DisabledBgColor : fillColor);

        PaintChildren(canvas, area);
    }

    #endregion
}