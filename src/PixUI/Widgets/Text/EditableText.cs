using System;

namespace PixUI;

public sealed class EditableText : TextBase, IMouseRegion, IFocusable
{
    public EditableText()
    {
        MouseRegion = new MouseRegion(() => Cursors.IBeam);
        MouseRegion.PointerDown += _OnPointerDown;
        MouseRegion.PointerMove += _OnPointerMove;

        FocusNode = new FocusNode();
        FocusNode.FocusChanged += _OnFocusChanged;
        FocusNode.TextInput += _OnTextInput;
        FocusNode.KeyDown += _OnKeyDown;

        _caret = new Caret(this, GetCaretBounds, GetCaretColor);
    }

    private readonly Caret _caret;
    private int _caretPosition;
    private int _selectionStart;
    private int _selectionEnd;
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
            ClearSelection();
            Commit();
        }
    }

    private void _OnTextInput(string text)
    {
        if (IsReadonly) return;

        var insertPos = _caretPosition;
        var oldText = Text.Value;
        if (_selectionStart != _selectionEnd)
        {
            oldText = oldText.Remove(_selectionStart, _selectionEnd - _selectionStart);
            insertPos = _selectionStart;
        }

        var newText = oldText.Insert(insertPos, text);
        if (PreviewInput != null && !PreviewInput(newText))
            return;

        _changeByTextInput = true;
        Text.Value = newText;
        _changeByTextInput = false;
        _caretPosition = insertPos + text.Length;
        _selectionStart = _selectionEnd = _caretPosition;
    }

    protected override void OnTextChanged(State state)
    {
        if (!_changeByTextInput)
            _caretPosition = Text.Value == null! ? 0 : Text.Value.Length;

        base.OnTextChanged(state);
    }

    private int GetCaretPosition(float x, float y)
    {
        var res = 0;
        if (CachedParagraph != null)
        {
            var pos = CachedParagraph.GetGlyphPositionAtCoordinate(x, y);
            Log.Debug($"pos={pos.Position} affinity={pos.Affinity}");
            res = pos.Position;
        }

        return res;
    }

    private void ClearSelection()
    {
        if (_selectionStart != _selectionEnd)
        {
            _selectionStart = _selectionEnd = _caretPosition;
            Repaint();
        }
    }

    private void _OnPointerDown(PointerEvent e)
    {
        if (e.Buttons != PointerButtons.Left) return;

        var newPos = GetCaretPosition(e.X, e.Y);
        if (newPos != _caretPosition)
        {
            _caretPosition = newPos;
            ClearSelection();
            _caret.NotifyPositionChanged(); //需要通知刷新
        }
    }

    private void _OnPointerMove(PointerEvent e)
    {
        if (e.Buttons != PointerButtons.Left) return;

        var oldStart = _selectionStart;
        var oldEnd = _selectionEnd;

        var pos = GetCaretPosition(e.X, e.Y);
        if (pos < _caretPosition)
        {
            _selectionStart = pos;
            _selectionEnd = _caretPosition;
        }
        else if (pos > _caretPosition)
        {
            _selectionStart = _caretPosition;
            _selectionEnd = pos;
        }
        else
            _selectionStart = _selectionEnd = _caretPosition;

        if (oldStart != _selectionStart || oldEnd != _selectionEnd)
            Repaint();
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

    private bool DeleteSelection()
    {
        if (_selectionStart == _selectionEnd) return false;

        Text.Value = Text.Value.Remove(_selectionStart, _selectionEnd - _selectionStart);
        _caretPosition = _selectionEnd = _selectionStart;
        return true;
    }

    private void DeleteBack()
    {
        if (IsReadonly) return;

        _changeByTextInput = true;
        if (!DeleteSelection() && _caretPosition != 0)
        {
            Text.Value = Text.Value.Remove(_caretPosition - 1, 1);
            _caretPosition--;
        }

        _changeByTextInput = false;
    }

    private void MoveLeft()
    {
        if (_caretPosition == 0) return;

        ClearSelection();
        _caretPosition--;
        _caret.NotifyPositionChanged();
    }

    private void MoveRight()
    {
        if (_caretPosition == Text.Value.Length)
            return;

        ClearSelection();
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
        const float offsetY = 2;
        if (string.IsNullOrEmpty(Text.Value) || Text.Value.Length == 0)
        {
            if (string.IsNullOrEmpty(HintText)) return;

            _hintParagraph ??= BuildParagraphInternal(HintText, float.PositiveInfinity, Colors.Gray);

            canvas.Save();
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
            canvas.DrawParagraph(_hintParagraph, 0, offsetY);
            canvas.Restore();
        }
        else
        {
            TryBuildParagraph();
            if (CachedParagraph == null) return;

            canvas.Save();
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
            canvas.DrawParagraph(CachedParagraph, 0, offsetY);
            DrawSelection(canvas);
            canvas.Restore();
        }
    }

    private void DrawSelection(Canvas canvas)
    {
        if (_selectionStart == _selectionEnd) return;

        var s = CachedParagraph!.GetRectForPosition(_selectionStart, BoxHeightStyle.Tight, BoxWidthStyle.Tight);
        var e = CachedParagraph!.GetRectForPosition(_selectionEnd - 1, BoxHeightStyle.Tight, BoxWidthStyle.Tight);
        var rect = new Rect(s.Rect.Left, 0, e.Rect.Right, H);
        var paint = PixUI.Paint.Shared(0xFFB3D7FF);
        paint.BlendMode = BlendMode.Difference;
        paint.AntiAlias = true;
        canvas.DrawRect(rect, paint);
    }

    #endregion
}