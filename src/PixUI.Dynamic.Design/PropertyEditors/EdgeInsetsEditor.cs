using System;
using System.Globalization;

namespace PixUI.Dynamic.Design;

public sealed class EdgeInsetsEditor : ValueEditorBase
{
    public EdgeInsetsEditor(State<EdgeInsets?> state, DesignElement element) : base(element)
    {
        Child = new EdgeInsetsInput(state);
    }

    private sealed class EdgeInsetsInput : InputBase<EditableText>
    {
        public EdgeInsetsInput(State<EdgeInsets?> state)
        {
            Editor = new EditableText { Text = string.Empty /*must assign*/ };
            Editor.CommitChanges = OnCommitChanges;

            Bind(ref _state!, state, OnValueChanged, true);
        }

        private readonly State<EdgeInsets?> _state;

        public override State<bool>? Readonly { get; set; }

        private void OnValueChanged(State state)
        {
            if (_state.Value == null)
            {
                Editor.Text.Value = string.Empty;
                return;
            }

            var v = _state.Value.Value;
            if (_state.Value.Value.IsAllSame)
            {
                Editor.Text.Value = v.Left.ToString(CultureInfo.InvariantCulture);
                return;
            }

            Editor.Text.Value = $"{v.Left}, {v.Top}, {v.Right}, {v.Bottom}";
        }

        private void OnCommitChanges(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                _state.Value = EdgeInsets.All(0);
                return;
            }

            try
            {
                if (text.Contains(','))
                {
                    var sr = text.Split(',');
                    var left = float.Parse(sr[0].Trim());
                    var top = float.Parse(sr[1].Trim());
                    var right = float.Parse(sr[2].Trim());
                    var bottom = float.Parse(sr[3].Trim());
                    _state.Value = EdgeInsets.Only(left, top, right, bottom);
                }
                else
                {
                    _state.Value = EdgeInsets.All(float.Parse(text.Trim()));
                }
            }
            catch (Exception)
            {
                Notification.Warn("EdgeInsets格式错误");
                OnValueChanged(_state); //restore it
            }
        }
    }
}