using System;

namespace PixUI;

public sealed class Button : Widget, IMouseRegion, IFocusable
{
    public Button()
    {
        Height = DefaultHeight; //TODO: 默认字体高+padding

        MouseRegion = new MouseRegion(() => Cursors.Hand);
        MouseRegion.PointerDown += OnPointerDown;
        MouseRegion.PointerUp += OnPointerUp;

        FocusNode = new FocusNode();
        _hoverDecoration = new HoverDecoration(this, GetHoverShaper, GetHoverBounds);
        _hoverDecoration.AttachHoverChangedEvent(this);
    }

    public Button(State<string>? text = null, State<IconData>? icon = null) : this()
    {
        Text = text;
        Icon = icon;
    }

    public const float DefaultHeight = 30;
    internal const float StandardRadius = 4;

    private readonly State<string>? _text;
    private readonly State<IconData>? _icon;
    private State<float>? _borderWidth;
    private State<Color>? _textColor;
    private State<Color>? _fillColor;
    private State<float>? _fontSize;
    private Text? _textWidget;
    private Icon? _iconWidget;
    private readonly HoverDecoration _hoverDecoration;
    
    private bool _drawMask;

    public State<string>? Text
    {
        get => _text;
        init => Bind(ref _text, value, RelayoutOnStateChanged);
    }

    public State<IconData>? Icon
    {
        get => _icon;
        init => Bind(ref _icon, value, RelayoutOnStateChanged);
    }

    public ButtonStyle Style { get; set; } = ButtonStyle.Solid;
    public ButtonShape Shape { get; set; } = ButtonShape.Standard;

    public State<Color>? TextColor
    {
        get => _textColor;
        set
        {
            _textColor = value ?? (Style == ButtonStyle.Solid ? Colors.White : Colors.Black);
            if (_textWidget != null) _textWidget.TextColor = _textColor;
            if (_iconWidget != null) _iconWidget.Color = _textColor;
        }
    }

    public State<Color>? FillColor
    {
        get => _fillColor;
        set => Bind(ref _fillColor, value, RepaintOnStateChanged);
    }

    public State<float>? FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            if (_textWidget != null) _textWidget.FontSize = value;
            if (_iconWidget != null) _iconWidget.Size = value;
        }
    }
    
    public MouseRegion MouseRegion { get; }
    public FocusNode FocusNode { get; }

    public Action<PointerEvent> OnTap
    {
        set => MouseRegion.PointerTap += value;
    }

    #region ====EventHandlers====

    private void OnPointerDown(PointerEvent e)
    {
        if (e.Buttons != PointerButtons.Left) return;

        _drawMask = true;
        Repaint();
    }

    private void OnPointerUp(PointerEvent e)
    {
        if (e.Buttons != PointerButtons.Left) return;

        _drawMask = false;
        Repaint();
    }

    #endregion

    #region ====HoverDecoration====

    private ShapeBorder GetHoverShaper()
    {
        switch (Shape)
        {
            case ButtonShape.Square:
                return new RoundedRectangleBorder(); //TODO: use RectangleBorder
            case ButtonShape.Standard:
                return new RoundedRectangleBorder(null, BorderRadius.All(Radius.Circular(StandardRadius)));
            case ButtonShape.Pills:
                return new RoundedRectangleBorder(null, BorderRadius.All(Radius.Circular(H / 2)));
            default:
                throw new NotImplementedException();
        }
    }

    private Rect GetHoverBounds()
    {
        //Icon only 特殊处理
        if (_iconWidget != null && _textWidget == null && Style == ButtonStyle.Transparent)
            return Rect.FromLTWH(_iconWidget.X, _iconWidget.Y, _iconWidget.W, _iconWidget.H);
        return Rect.FromLTWH(0, 0, W, H);
    }

    #endregion

    #region ====Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (_textWidget != null && action(_textWidget)) return;
        if (_iconWidget != null) action(_iconWidget);
    }

    /// <summary>
    /// 没有指定宽高充满可用空间, 仅指定高则使用Icon+Text的宽度
    /// </summary>
    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        var width = maxSize.Width;
        var height = maxSize.Height;

        TryBuildContent();
        _iconWidget?.Layout(width, height);
        _textWidget?.Layout(width - (_iconWidget?.W ?? 0), height);
        var contentWidth = (_iconWidget?.W ?? 0) + (_textWidget?.W ?? 0);
        SetSize(Width == null ? Math.Max(DefaultHeight, contentWidth + 16 /*padding*/) : width, height);

        //TODO: 根据icon位置计算
        // var contentHeight = Math.Max(_iconWidget?.H ?? 0, _textWidget?.H ?? 0);
        var contentOffsetX = (W - contentWidth) / 2;
        // var contentOffsetY = (H - contentHeight) / 2;
        _iconWidget?.SetPosition(contentOffsetX, (H - _iconWidget!.H) / 2);
        _textWidget?.SetPosition(contentOffsetX + (_iconWidget?.W ?? 0), (H - _textWidget!.H) / 2);
    }

    private void TryBuildContent()
    {
        if (Text == null && Icon == null) return;

        _textColor ??= Style == ButtonStyle.Solid ? Colors.White : Colors.Black;

        if (Text != null && _textWidget == null)
        {
            _textWidget = new Text(Text) { TextColor = _textColor, FontSize = _fontSize };
            _textWidget.Parent = this;
        }

        if (Icon != null && _iconWidget == null)
        {
            _iconWidget = new Icon(Icon) { Color = _textColor, Size = _fontSize };
            _iconWidget.Parent = this;
        }
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        PaintShape(canvas);

        if (_iconWidget != null)
        {
            canvas.Translate(_iconWidget.X, _iconWidget.Y);
            _iconWidget.Paint(canvas, area);
            canvas.Translate(-_iconWidget.X, -_iconWidget.Y);
        }

        if (_textWidget != null)
        {
            canvas.Translate(_textWidget.X, _textWidget.Y);
            _textWidget.Paint(canvas, area);
            canvas.Translate(-_textWidget.X, -_textWidget.Y);
        }

        PaintMask(canvas);
    }

    private void PaintShape(Canvas canvas)
    {
        if (Style == ButtonStyle.Transparent) return;

        var paint = PixUI.Paint.Shared();
        paint.Style = Style == ButtonStyle.Solid ? PaintStyle.Fill : PaintStyle.Stroke;
        paint.StrokeWidth = Style == ButtonStyle.Outline ? (_borderWidth?.Value ?? 2) : 0;
        paint.AntiAlias = Shape != ButtonShape.Square;
        paint.Color = _fillColor?.Value ?? new Color(0xFF3880FF);

        switch (Shape)
        {
            case ButtonShape.Square:
                canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), paint);
                break;
            case ButtonShape.Standard:
            {
                using var rrect = RRect.FromRectAndRadius(Rect.FromLTWH(0, 0, W, H),
                    StandardRadius, StandardRadius);
                canvas.DrawRRect(rrect, paint);
                break;
            }
            default:
                throw new NotImplementedException();
        }
    }

    private void PaintMask(Canvas canvas)
    {
        if (!_drawMask) return;

        var paint = PixUI.Paint.Shared(Colors.Gray.WithAlpha(128));
        paint.AntiAlias = Shape != ButtonShape.Square;

        var x = 0f;
        var y = 0f;
        var w = W;
        var h = H;
        if (_iconWidget != null && _textWidget == null && Style == ButtonStyle.Transparent)
        {
            x = _iconWidget.X;
            y = _iconWidget.Y;
            w = _iconWidget.W;
            h = _iconWidget.H;
        }

        switch (Shape)
        {
            case ButtonShape.Standard:
            {
                using var rrect = RRect.FromRectAndRadius(Rect.FromLTWH(x, y, w, h),
                    StandardRadius, StandardRadius);
                canvas.DrawRRect(rrect, paint);
                break;
            }
            case ButtonShape.Pills:
            {
                using var rrect = RRect.FromRectAndRadius(Rect.FromLTWH(x, y, w, h),
                    h / 2f, h / 2f);
                canvas.DrawRRect(rrect, paint);
                break;
            }
            default:
                canvas.DrawRect(Rect.FromLTWH(x, y, w, h), paint);
                break;
        }
    }

    protected override void OnUnmounted()
    {
        base.OnUnmounted();
        _hoverDecoration.Hide();
    }

    public override string ToString()
    {
        if (DebugLabel != null || Text == null)
            return base.ToString();
        return $"{nameof(Button)}[\"{Text.Value}\"]";
    }

    public override void Dispose()
    {
        _textWidget?.Dispose();
        _iconWidget?.Dispose();
        base.Dispose();
    }

    #endregion
}