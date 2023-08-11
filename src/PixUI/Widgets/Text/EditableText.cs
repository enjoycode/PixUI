using System;

namespace PixUI;

public sealed class EditableText : TextBase, IMouseRegion, IFocusable
{
    public EditableText(State<string> text) : base(text)
    {
        MouseRegion = new MouseRegion(() => Cursors.IBeam);
        MouseRegion.PointerDown += _OnPointerDown;

        FocusNode = new FocusNode();
        FocusNode.FocusChanged += _OnFocusChanged;
        FocusNode.TextInput += _OnTextInput;
        FocusNode.KeyDown += _OnKeyDown;

        _caret = new Caret(this, GetCaretBounds, GetCaretColor);
    }

    private readonly Caret _caret;
    private int _caretPosition = 0;

    public readonly State<bool> Focused = false;

    private Paragraph? _hintParagraph;

    private State<bool>? _readonly;

    public State<bool>? Readonly
    {
        get => _readonly;
        set => _readonly = Rebind(_readonly, value, BindingOptions.None);
    }

    internal bool IsReadonly => _readonly != null && _readonly.Value;

    public MouseRegion MouseRegion { get; }
    public FocusNode FocusNode { get; }

    /// <summary>
    /// 字体行高
    /// </summary>
    private float FontHeight => (FontSize?.Value ?? Theme.DefaultFontSize) + 4;

    protected override bool ForceHeight => true;

    public bool IsObscure { get; set; }

    public string? HintText { get; set; }

    #region ====Event Handlers====

    private void _OnFocusChanged(FocusChangedEvent e)
    {
        Focused.Value = e.IsFocused;
        if (e.IsFocused)
        {
            if (!IsReadonly)
                Root!.Window.StartTextInput();
            _caret.Show();
        }
        else
        {
            if (!IsReadonly)
                Root!.Window.StopTextInput();
            _caret.Hide();
        }
    }

    private void _OnTextInput(string text)
    {
        if (IsReadonly) return;

        if (Text.Value != null)
            Text.Value = Text.Value.Insert(_caretPosition, text);
        else
            Text.Value = text;

        _caretPosition += text.Length;
    }

    private void _OnPointerDown(PointerEvent theEvent)
    {
        var newPos = 0;
        if (CachedParagraph != null)
        {
            var pos = CachedParagraph.GetGlyphPositionAtCoordinate(theEvent.X, theEvent.Y);
            Console.WriteLine($"pos={pos.Position} affinity={pos.Affinity}");
            newPos = pos.Position;
        }

        if (newPos != _caretPosition)
        {
            _caretPosition = newPos;
            _caret.NotifyPositionChanged(); //需要通知刷新
        }
    }

    private void _OnKeyDown(KeyEvent keyEvent)
    {
        switch (keyEvent.KeyCode)
        {
            case Keys.Back:
                DeleteBack();
                break;
            case Keys.Left:
                MoveLeft();
                break;
            case Keys.Right:
                MoveRight();
                break;
        }
    }

    private void DeleteBack()
    {
        if (IsReadonly || _caretPosition == 0) return;

        Text.Value = Text.Value.Remove(_caretPosition - 1, 1);
        _caretPosition--;
    }

    private void MoveLeft()
    {
        if (_caretPosition == 0) return;

        _caretPosition--;
        _caret.NotifyPositionChanged();
    }

    private void MoveRight()
    {
        if (_caretPosition == Text.Value.Length)
            return;

        _caretPosition++;
        _caret.NotifyPositionChanged();
    }

    #endregion

    #region ====IWidgetWithCaret====

    public Rect GetCaretBounds()
    {
        //TODO: cache result
        const float caretWidth = 2;
        const float halfCaretWidth = caretWidth / 2;

        TryBuildParagraph();

        var pos = new Point(0, 0);
        if (_caretPosition != 0)
        {
            if (_caretPosition == Text.Value.Length)
            {
                var textbox = CachedParagraph!.GetRectForPosition(_caretPosition - 1,
                    BoxHeightStyle.Tight, BoxWidthStyle.Tight);
                pos.Offset(textbox.Rect.Left + textbox.Rect.Width, 0);
            }
            else
            {
                var textbox = CachedParagraph!.GetRectForPosition(_caretPosition,
                    BoxHeightStyle.Tight, BoxWidthStyle.Tight);
                pos.Offset(textbox.Rect.Left, 0);
            }
        }

        return Rect.FromLTWH(pos.X - halfCaretWidth, pos.Y, caretWidth, H);
    }

    public Color GetCaretColor() => Theme.CaretColor; //TODO:

    #endregion

    #region ====Overrides====

    private void TryBuildParagraph()
    {
        if (CachedParagraph != null) return;
        if (Text.Value == null || Text.Value.Length == 0) return;

        var text = IsObscure ? new string('●', Text.Value.Length) : Text.Value;
        BuildParagraph(text, float.PositiveInfinity);
    }

    public override void OnStateChanged(State state, BindingOptions options)
    {
        if (ReferenceEquals(state, _readonly))
        {
            if (IsReadonly)
                Root?.Window.StopTextInput();
            else
                Root?.Window.StartTextInput();
            return;
        }

        base.OnStateChanged(state, options);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        TryBuildParagraph();
        SetSize(width, Math.Min(height, FontHeight));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (Text.Value == null || Text.Value.Length == 0)
        {
            if (string.IsNullOrEmpty(HintText)) return;

            _hintParagraph ??=
                BuildParagraphInternal(HintText, float.PositiveInfinity, Colors.Gray);
            canvas.DrawParagraph(_hintParagraph, 0, 2 /*offset*/);
        }
        else
        {
            TryBuildParagraph();
            if (CachedParagraph == null) return;
            canvas.DrawParagraph(CachedParagraph, 0, 2 /*offset*/);
        }
    }

    #endregion
}