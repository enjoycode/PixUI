using System;

namespace PixUI;

public sealed class EditableText : TextBase, IMouseRegion, IFocusable
{
    public EditableText()
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
    private int _caretPosition;
    private bool _changeByTextInput;

    public readonly State<bool> Focused = false;

    private Paragraph? _hintParagraph;
    private string? _hintText;
    private State<bool>? _readonly;

    public State<bool>? Readonly
    {
        get => _readonly;
        set => Bind(ref _readonly, value, OnReadonlyChanged);
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

    public string? HintText
    {
        get => _hintText;
        set
        {
            _hintText = value;
            _hintParagraph = null;
            Repaint();
        }
    }

    /// <summary>
    /// 验证输入的内容，返回false不接受输入
    /// </summary>
    public Func<string, bool>? PreviewInput { get; set; }

    /// <summary>
    /// 按下回车键或失去焦点时递交变更
    /// </summary>
    public Action<string>? CommitChanges { get; set; }

    #region ====Event Handlers====

    private void _OnFocusChanged(FocusChangedEvent e)
    {
        Focused.Value = e.IsFocused;
        if (e.IsFocused)
        {
            if (!IsReadonly)
            {
                e.Window.StartTextInput();
                var winPt = LocalToWindow(0, 0);
                var inputRect = Rect.FromLTWH(winPt.X, winPt.Y, W, H);
                e.Window.SetTextInputRect(inputRect);
            }

            _caret.Show();
        }
        else
        {
            if (!IsReadonly)
                e.Window.StopTextInput();
            _caret.Hide();
            Commit();
        }
    }

    private void _OnTextInput(string text)
    {
        if (IsReadonly) return;

        var newText = string.IsNullOrEmpty(Text.Value) ? text : Text.Value.Insert(_caretPosition, text);
        if (PreviewInput != null && !PreviewInput(newText))
            return;

        _changeByTextInput = true;
        Text.Value = newText;
        _changeByTextInput = false;
        _caretPosition += text.Length;
    }

    protected override void OnTextChanged(State state)
    {
        if (!_changeByTextInput)
            _caretPosition = Text.Value.Length;

        base.OnTextChanged(state);
    }

    private void _OnPointerDown(PointerEvent theEvent)
    {
        var newPos = 0;
        if (CachedParagraph != null)
        {
            var pos = CachedParagraph.GetGlyphPositionAtCoordinate(theEvent.X, theEvent.Y);
            Log.Debug($"pos={pos.Position} affinity={pos.Affinity}");
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
            case Keys.Return:
                Commit();
                break;
        }
    }

    private void DeleteBack()
    {
        if (IsReadonly || _caretPosition == 0) return;

        _changeByTextInput = true;
        Text.Value = Text.Value.Remove(_caretPosition - 1, 1);
        _changeByTextInput = false;
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

    private void Commit()
    {
        CommitChanges?.Invoke(Text.Value);
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
        if (string.IsNullOrEmpty(Text.Value) || Text.Value.Length == 0) return;

        var text = IsObscure ? new string('●', Text.Value.Length) : Text.Value;
        BuildParagraph(text, float.PositiveInfinity);
    }

    private void OnReadonlyChanged(State state)
    {
        if (IsReadonly)
            Root?.Window.StopTextInput();
        else
            Root?.Window.StartTextInput();
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        TryBuildParagraph();
        SetSize(maxSize.Width, Math.Min(maxSize.Height, FontHeight));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (string.IsNullOrEmpty(Text.Value) || Text.Value.Length == 0)
        {
            if (string.IsNullOrEmpty(HintText)) return;

            _hintParagraph ??= BuildParagraphInternal(HintText, float.PositiveInfinity, Colors.Gray);

            canvas.Save();
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
            canvas.DrawParagraph(_hintParagraph, 0, 2 /*offset*/);
            canvas.Restore();
        }
        else
        {
            TryBuildParagraph();
            if (CachedParagraph == null) return;

            canvas.Save();
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
            canvas.DrawParagraph(CachedParagraph, 0, 2 /*offset*/);
            canvas.Restore();
        }
    }

    #endregion
}