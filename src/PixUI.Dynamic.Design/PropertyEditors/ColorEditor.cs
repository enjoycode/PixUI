using System.Globalization;

namespace PixUI.Dynamic.Design;

public sealed class ColorEditor : ValueEditorBase
{
    //TODO: 暂简单实现输入ARGB值

    public ColorEditor(State<Color?> state, DesignElement element) : base(element)
    {
        Bind(ref _state!, state, OnValueChanged, true);

        var colorState = state.ToNoneNullable(Color.Empty);
        Child = new TextInput(_text)
        {
            Suffix = new Icon(MaterialIcons.Square) { Color = colorState },
            OnCommitChanges = OnCommitChanges,
        };
    }

    private readonly State<Color?> _state;
    private readonly State<string> _text = string.Empty;

    private void OnValueChanged(State state)
    {
        _text.Value = _state.Value == null ? string.Empty : ((uint)_state.Value).ToString("X8");
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
}