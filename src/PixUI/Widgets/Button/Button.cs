using System;

namespace PixUI;

public sealed class Button : Widget, IMouseRegion, IFocusable
{
    public Button(State<string>? text = null, State<IconData>? icon = null)
    {
        _text = text;
        _icon = icon;

        Height = DefaultHeight; //TODO: 默认字体高+padding

        MouseRegion = new MouseRegion(() => Cursors.Hand);
        FocusNode = new FocusNode();

        _hoverDecoration = new HoverDecoration(this, GetHoverShaper, GetHoverBounds);
        _hoverDecoration.AttachHoverChangedEvent(this);
    }

    internal const float DefaultHeight = 30;
    internal const float StandardRadius = 4;

    private State<string>? _text;
    private State<IconData>? _icon;
    private State<float>? _outlineWidth;
    private State<Color>? _textColor;
    private State<float>? _fontSize;

    public ButtonStyle Style { get; set; } = ButtonStyle.Solid;
    public ButtonShape Shape { get; set; } = ButtonShape.Standard;

    private Text? _textWidget;
    private Icon? _iconWidget;

    public State<Color>? TextColor
    {
        get => _textColor;
        set
        {
            _textColor = value;
            if (_textWidget != null) _textWidget.TextColor = value;
            if (_iconWidget != null) _iconWidget.Color = value;
        }
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

    private readonly HoverDecoration _hoverDecoration;

    public MouseRegion MouseRegion { get; }
    public FocusNode FocusNode { get; }

    public Action<PointerEvent> OnTap
    {
        set => MouseRegion.PointerTap += value;
    }

    #region ====HoverDecoration====

    private ShapeBorder GetHoverShaper()
    {
        switch (Shape)
        {
            case ButtonShape.Square:
                return new RoundedRectangleBorder(); //TODO: use RectangleBorder
            case ButtonShape.Standard:
                return new RoundedRectangleBorder(
                    null, BorderRadius.All(Radius.Circular(StandardRadius)));
            case ButtonShape.Pills:
                return new RoundedRectangleBorder(
                    null, BorderRadius.All(Radius.Circular(H / 2)));
            default:
                throw new NotImplementedException();
        }
    }

    private Rect GetHoverBounds()
    {
        //Icon only 特殊处理
        if (_iconWidget != null && _textWidget == null && Style == ButtonStyle.Transparent)
            return Rect.FromLTWH(0, 0, _iconWidget.W, _iconWidget.H);
        return Rect.FromLTWH(0, 0, W, H);
    }

    #endregion

    #region ====Overrides====

    /// <summary>
    /// 没有指定宽高充满可用空间, 仅指定高则使用Icon+Text的宽度
    /// </summary>
    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        TryBuildContent();
        _iconWidget?.Layout(width, height);
        _textWidget?.Layout(width - (_iconWidget?.W ?? 0), height);
        var contentWidth = (_iconWidget?.W ?? 0) + (_textWidget?.W ?? 0);
        if (Width == null)
            SetSize(Math.Max(DefaultHeight, contentWidth + 16 /*padding*/), height);
        else
            SetSize(width, height);

        //TODO: 根据icon位置计算
        // var contentHeight = Math.Max(_iconWidget?.H ?? 0, _textWidget?.H ?? 0);
        var contentOffsetX = (W - contentWidth) / 2;
        // var contentOffsetY = (H - contentHeight) / 2;
        _iconWidget?.SetPosition(contentOffsetX, (H - _iconWidget!.H) / 2);
        _textWidget?.SetPosition(contentOffsetX + (_iconWidget?.W ?? 0),
            (H - _textWidget!.H) / 2);
    }

    private void TryBuildContent()
    {
        if (_text == null && _icon == null) return;

        if (_textColor == null)
        {
            _textColor = Style == ButtonStyle.Solid ? Colors.White : Colors.Black;
        }

        if (_text != null && _textWidget == null)
        {
            _textWidget = new Text(_text) { TextColor = _textColor, FontSize = _fontSize };
            _textWidget.Parent = this;
        }

        if (_icon != null && _iconWidget == null)
        {
            _iconWidget = new Icon(_icon) { Color = _textColor, Size = _fontSize };
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
    }

    private void PaintShape(Canvas canvas)
    {
        if (Style == ButtonStyle.Transparent) return;

        var paint = PaintUtils.Shared();
        paint.Style = Style == ButtonStyle.Solid ? PaintStyle.Fill : PaintStyle.Stroke;
        paint.StrokeWidth = Style == ButtonStyle.Outline ? (_outlineWidth?.Value ?? 2) : 0;
        paint.AntiAlias = Shape != ButtonShape.Square;
        paint.Color = new Color(0xFF3880FF); //TODO:

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

    protected override void OnUnmounted()
    {
        base.OnUnmounted();
        _hoverDecoration.Hide();
    }

    public override void Dispose()
    {
        _textWidget?.Dispose();
        _iconWidget?.Dispose();
        base.Dispose();
    }

    #endregion
}