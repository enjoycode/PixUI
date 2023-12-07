using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PixUI;

public sealed class DatePicker : InputBase<EditableText>
{
    public DatePicker()
    {
        Padding = new RxValue<EdgeInsets>(EdgeInsets.Only(4, 4, 0, 4));
        SuffixWidget = new Button(icon: MaterialIcons.CalendarMonth)
        {
            Style = ButtonStyle.Transparent,
            OnTap = _ => SwitchPopup(),
        };
    }

    [SetsRequiredMembers]
    public DatePicker(State<DateTime?> value) : this()
    {
        Value = value;
    }

    private readonly State<DateTime?> _value = null!;
    private readonly State<string> _textValue = null!;
    private const string format = "yyyy-MM-dd";
    private bool _showing;
    private DatePickerPopup? _popup;

    public required State<DateTime?> Value
    {
        init
        {
            Editor = new EditableText(string.Empty);

            _value = Bind(value, OnValueChanged);
            _textValue = Bind(Editor.Text, OnTextChanged);
            if (value.Value != null)
                _textValue.Value = value.Value.Value.ToString(format);

            Editor.FocusNode.FocusChanged += OnFocusChanged;
        }
    }

    public override State<bool>? Readonly { get; set; }

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

    private void OnValueChanged(State state)
    {
        _textValue.Value = _value.Value == null ? string.Empty : _value.Value.Value.ToString(format);
    }

    private void OnTextChanged(State state)
    {
        if (DateTime.TryParseExact(_textValue.Value, format,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            _value.Value = dateTime;
    }
}