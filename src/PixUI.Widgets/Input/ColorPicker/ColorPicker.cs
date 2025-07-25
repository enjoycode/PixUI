using System.Globalization;

namespace PixUI;

public sealed class ColorPicker : InputBase<EditableText>
{
    public ColorPicker(State<Color> state)
    {
        Bind(ref _state!, state, OnValueChanged, true);

        Editor = new EditableText();
        Editor.Text = _hexValue;
        Editor.Readonly = _hexValue.Readonly;
        Editor.CommitChanges = OnCommitChanges;

        PrefixWidget = new ColorRect(_state);
        SuffixWidget = new Button(icon: MaterialIcons.ColorLens)
        {
            Style = ButtonStyle.Transparent,
            TextColor = Colors.Black,
            OnTap = _ => SwitchPopup(),
        };
    }

    private readonly State<Color> _state;
    private readonly State<string> _hexValue = string.Empty;

    private bool _showing;
    private ColorPickerPopup? _popup;

    public override State<bool>? Readonly
    {
        get => Editor.Readonly;
        set => Editor.Readonly = value;
    }

    private void OnValueChanged(State state)
    {
        _hexValue.Value = ((uint)_state.Value).ToString("X8");
    }

    private void OnCommitChanges(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            OnValueChanged(_state);
            return;
        }

        if (uint.TryParse(text, NumberStyles.HexNumber, null, out var value))
        {
            _state.Value = new Color(value);
        }
        else
        {
            Notification.Warn("Color hex格式错误");
            OnValueChanged(_state); //restore it
        }
    }

    protected override void OnUnmounted() => HidePopup();

    private void SwitchPopup()
    {
        if (_showing) HidePopup();
        else ShowPopup();
    }

    private void ShowPopup()
    {
        if (_showing) return;
        _showing = true;
        _popup = new ColorPickerPopup(Overlay!, _state, HidePopup);
        _popup.Show(this, new Offset(-4, 0), Popup.DefaultTransitionBuilder);
    }

    private void HidePopup()
    {
        if (!_showing) return;
        _showing = false;

        _popup?.Hide();
        _popup = null;
    }

    private class ColorRect : Widget
    {
        public ColorRect(State<Color> state)
        {
            _state = state;
            _state.AddListener(_ => Repaint());
        }

        private readonly State<Color> _state;
        private const float SIZE = 16;
        private const float RADIUS = 3;

        public override void Layout(float availableWidth, float availableHeight)
        {
            CacheAndGetMaxSize(availableWidth, availableHeight);
            SetSize(SIZE, SIZE);
        }

        public override void Paint(Canvas canvas, IDirtyArea? area = null)
        {
            var rect = Rect.FromLTWH(0, 0, W, H);
            using var roundRect = RRect.FromRectAndRadius(rect, RADIUS, RADIUS);
            canvas.Save();
            canvas.ClipRRect(roundRect, ClipOp.Intersect, true);

            OpacitySlider.DrawChessBoards(canvas, rect);

            var paint = PixUI.Paint.Shared(_state.Value);
            canvas.DrawRect(rect, paint);

            canvas.Restore();
        }
    }
}