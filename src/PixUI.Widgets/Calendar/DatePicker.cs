using System;
using System.Globalization;

namespace PixUI;

public sealed class DatePicker : InputBase<EditableText>
{
    public DatePicker(State<DateTime?> value) : base(new EditableText(string.Empty))
    {
        _value = Bind(value, BindingOptions.None);
        _textValue = Bind(_editor.Text, BindingOptions.None);
        if (value.Value != null) _textValue.Value = value.Value.Value.ToString(format);

        Padding = new Rx<EdgeInsets>(EdgeInsets.Only(4, 4, 0, 4));
        SuffixWidget = new Button(icon: MaterialIcons.CalendarMonth)
        {
            Style = ButtonStyle.Transparent,
            OnTap = _ => SwitchPopup(),
        };

        _editor.FocusNode.FocusChanged += OnFocusChanged;
    }

    private readonly State<DateTime?> _value;
    private readonly State<string> _textValue;
    private readonly string format = "yyyy-MM-dd";
    private bool _showing;
    private DatePickerPopup? _popup;

    public override State<bool>? Readonly { get; set; }

    private void SwitchPopup()
    {
        if (_showing) HidePopup();
        else ShowPopup();
    }

    private void ShowPopup()
    {
        if (_showing) return;
        _showing = true;
        _popup = new DatePickerPopup(Overlay!, _value, HidePopup);
        _popup.Show(this, new Offset(-4, 0), Popup.DefaultTransitionBuilder);
    }

    private void HidePopup()
    {
        if (!_showing) return;
        _showing = false;

        _popup?.Hide();
        _popup = null;
    }

    private void OnFocusChanged(FocusChangedEvent e)
    {
        if (!e.IsFocused)
        {
            // restore edit text to DateTime format
            _textValue.Value = _value.Value == null ? string.Empty : _value.Value.Value.ToString(format);
            // maybe lost focus by tap button
            if (!ReferenceEquals(e.NewFocusedWidget, SuffixWidget))
                HidePopup();
        }
        else
        {
            ShowPopup(); //TODO:属性控制是否自动弹窗
        }
    }

    public override void OnStateChanged(StateBase state, BindingOptions options)
    {
        if (ReferenceEquals(state, _value))
            _textValue.Value = _value.Value == null ? string.Empty : _value.Value.Value.ToString(format);
        if (ReferenceEquals(state, _textValue))
        {
            if (DateTime.TryParseExact(_textValue.Value, format,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                _value.Value = dateTime;
        }

        base.OnStateChanged(state, options);
    }
}