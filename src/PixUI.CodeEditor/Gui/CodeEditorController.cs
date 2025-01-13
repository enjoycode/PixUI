using System;
using System.Collections.Generic;
using PixUI;

namespace CodeEditor;

public sealed class CodeEditorController : WidgetController<CodeEditorWidget>
{
    public CodeEditorController(string fileName, ITextBuffer textBuffer,
        ICompletionProvider? completionProvider = null, string? tag = null)
    {
        _editActions = MakeShortcuts();
        Theme = new TextEditorTheme();
        Document = new Document(fileName, textBuffer, tag);
        TextEditor = new TextEditor(this);
        _completionContext = new CompletionContext(this, completionProvider);

        Document.DocumentChanged += _OnDocumentChanged;
        TextEditor.Caret.PositionChanged += _OnCaretPositionChanged;
    }

    public readonly Document Document;
    internal readonly TextEditor TextEditor;
    internal readonly TextEditorTheme Theme;
    private readonly CompletionContext _completionContext;
    private readonly Dictionary<int, IEditCommand> _editActions;

    /// <summary>
    /// 用于构建文本编辑区域的右键菜单
    /// </summary>
    public Func<TextEditor, MenuItem[]>? ContextMenuBuilder { get; set; }

    private static Dictionary<int, IEditCommand> MakeShortcuts()
    {
        var controlKey = UIApplication.Current.IsMacOS()
            ? Keys.Meta
            : Keys.Control;

        var dic = new Dictionary<int, IEditCommand>()
        {
            { (int)Keys.Left, new CaretLeft() },
            { (int)Keys.Right, new CaretRight() },
            { (int)Keys.Up, new CaretUp() },
            { (int)Keys.Down, new CaretDown() },
            { (int)Keys.Back, new BackspaceCommand() },
            { (int)Keys.Return, new ReturnCommand() },
            { (int)Keys.Tab, new TabCommand() },
            { (int)(controlKey | Keys.A), new SelectAllCommand() },
            { (int)(controlKey | Keys.C), new CopyCommand() },
            { (int)(controlKey | Keys.X), new CutCommand() },
            { (int)(controlKey | Keys.V), new PasteCommand() },
            { (int)(controlKey | Keys.Z), new UndoCommand() },
            { (int)(controlKey | Keys.Y), new RedoCommand() },
        };

        return dic;
    }

    #region ====Event Handles====

    private Point _mouseDownPos;
    private bool _gotMouseDown; //primary button is down
    private bool _doDragDrop = false;
    private TextLocation _minSelection = TextLocation.Empty;
    private TextLocation _maxSelection = TextLocation.Empty;
    private bool _caretChangedByTextInput; //是否由文本输入导致的光标位置变更

    internal void OnPointerDown(PointerEvent e)
    {
        // this corrects weird problems when text is selected,
        // then a menu item is selected, then the text is
        // clicked again - it correctly synchronises the click position
        TextEditor.PointerPos.X = e.X;
        TextEditor.PointerPos.Y = e.Y;

        // 处理非文本区域
        foreach (var area in TextEditor.LeftAreas)
        {
            if (area.Bounds.ContainsPoint(e.X, e.Y))
                area.HandlePointerDown(e.X, e.Y, e.Buttons);
        }

        // 处理文本编辑区域
        if (TextEditor.TextView.Bounds.ContainsPoint(e.X, e.Y))
        {
            _gotMouseDown = true;
            TextEditor.SelectionManager.SelectFrom.Where = WhereFrom.TextArea;
            _mouseDownPos = new PixUI.Point(e.X, e.Y);
            _minSelection = TextLocation.Empty;
            _maxSelection = TextLocation.Empty;
            TextEditor.TextView.HandlePointerDown(e.X, e.Y, e.Buttons);
        }
    }

    internal void OnPointerUp(PointerEvent e)
    {
        TextEditor.SelectionManager.SelectFrom.Where = WhereFrom.None;
        _gotMouseDown = false;
        _mouseDownPos = new Point(-1, -1);
    }

    internal void OnPointerMove(PointerEvent e)
    {
        TextEditor.PointerPos.X = e.X;
        TextEditor.PointerPos.Y = e.Y;

        if (e.Buttons == PointerButtons.Left)
        {
            if (_gotMouseDown && TextEditor.SelectionManager.SelectFrom.Where == WhereFrom.TextArea)
            {
                ExtendSelectionToPointer();
            }
        }
    }

    internal Offset OnScroll(float dx, float dy)
    {
        var oldX = TextEditor.VirtualTop.X;
        var oldY = TextEditor.VirtualTop.Y;

        var newX = dx == 0 ? oldX : Math.Clamp(oldX + dx, 0, TextEditor.MaxHScrollValue);
        var newY = dy == 0 ? oldY : Math.Clamp(oldY + dy, 0, TextEditor.MaxVScrollValue);

        var offset = new Offset(newX - oldX, newY - oldY);
        if (offset.Dx != 0 || offset.Dy != 0)
        {
            TextEditor.VirtualTop = new Point(newX, newY);
            Widget.RequestInvalidate(true, null);
        }

        return offset;
    }

    internal void OnKeyDown(KeyEvent e)
    {
        //先预处理一些特殊键
        _completionContext.PreProcessKeyDown(e);

        if (_editActions.TryGetValue((int)e.KeyData, out var cmd))
        {
            cmd.Execute(TextEditor);
            e.StopPropagate(); //暂全部停止向上传播
        }
    }

    internal void OnTextInput(string text)
    {
        _caretChangedByTextInput = true;

        //先判断处理AutoClosingPairs
        var closingPair = text.Length == 1
            ? Document.SyntaxParser.Language.GetAutoClosingPairs(text[0])
            : null;
        if (closingPair == null)
        {
            TextEditor.InsertOrReplaceString(text, 0);
        }
        else
        {
            TextEditor.InsertOrReplaceString(text + new string(closingPair.Value, 1), 0);
            var oldPosition = TextEditor.Caret.Position;
            TextEditor.Caret.Position = new TextLocation(oldPosition.Column - 1, oldPosition.Line);
        }

        _caretChangedByTextInput = false;
        //处理Completion
        _completionContext.RunCompletion(text);
    }

    private void ExtendSelectionToPointer()
    {
        var mousePos = TextEditor.PointerPos;
        var realMousePos = TextEditor.TextView.GetLogicalPosition(
            Math.Max(0, mousePos.X - TextEditor.TextView.Bounds.Left),
            mousePos.Y - TextEditor.TextView.Bounds.Top);
        var y = realMousePos.Line;
        //realMousePos = TextEditor.Caret.ValidatePosition(realMousePos);
        var oldPos = TextEditor.Caret.Position;
        if (oldPos == realMousePos &&
            TextEditor.SelectionManager.SelectFrom.Where != WhereFrom.Gutter)
            return;

        // the selection is from the gutter
        if (TextEditor.SelectionManager.SelectFrom.Where == WhereFrom.Gutter)
        {
            if (realMousePos.Line < TextEditor.SelectionManager.SelectionStart.Line)
            {
                // the selection has moved above the start point
                TextEditor.Caret.Position = new TextLocation(0, realMousePos.Line);
            }
            else
            {
                // the selection has moved below the start point
                TextEditor.Caret.Position = TextEditor.SelectionManager.NextValidPosition(realMousePos.Line);
            }
        }
        else
        {
            TextEditor.Caret.Position = realMousePos;
        }

        // moves selection across whole words for double-click initiated selection
        if (!_minSelection.IsEmpty &&
            TextEditor.SelectionManager.HasSomethingSelected &&
            TextEditor.SelectionManager.SelectFrom.Where == WhereFrom.TextArea)
        {
            // Extend selection when selection was started with double-click
            var selection = TextEditor.SelectionManager.SelectionCollection[0];
            var min = SelectionManager.GreaterEqPos(_minSelection, _maxSelection)
                ? _maxSelection
                : _minSelection;
            var max = SelectionManager.GreaterEqPos(_minSelection, _maxSelection)
                ? _minSelection
                : _maxSelection;
            if (SelectionManager.GreaterEqPos(max, realMousePos) &&
                SelectionManager.GreaterEqPos(realMousePos, min))
            {
                TextEditor.SelectionManager.SetSelection(min, max);
            }
            else if (SelectionManager.GreaterEqPos(max, realMousePos))
            {
                var moff = TextEditor.Document.PositionToOffset(realMousePos);
                min = TextEditor.Document
                    .OffsetToPosition(FindWordStart(TextEditor.Document, moff));
                TextEditor.SelectionManager.SetSelection(min, max);
            }
            else
            {
                var moff = TextEditor.Document.PositionToOffset(realMousePos);
                max = TextEditor.Document
                    .OffsetToPosition(FindWordEnd(TextEditor.Document, moff));
                TextEditor.SelectionManager.SetSelection(min, max);
            }
        }
        else
        {
            TextEditor.SelectionManager
                .ExtendSelection(oldPos, TextEditor.Caret.Position);
        }
        //textArea.SetDesiredColumn();
    }

    #endregion

    #region ====重绘事件====

    private void _OnDocumentChanged(DocumentEventArgs e)
    {
        //TODO: 进一步合并LineManager改变的行数
        //var dirtyLines = Document.SyntaxParser.GetDirtyLines(this);
        if (Widget != null!)
            Widget.RequestInvalidate(true, null /*dirtyLines*/);
    }

    private void _OnCaretPositionChanged()
    {
        if (!_caretChangedByTextInput)
        {
            //TODO: set IME input rect
            _completionContext.OnCaretChangedByNoneTextInput();
        }

        Widget.RequestInvalidate(false, null);
    }

    #endregion

    #region ====Actions====

    public int GetCaretOffset() => TextEditor.Caret.Offset;

    /// <summary>
    /// 设置光标位置
    /// </summary>
    public void SetCaret(int line, int column)
    {
        TextEditor.Caret.Position = new TextLocation(column, line);

        //TODO: animate scroll to caret position
    }

    public void SetSelection(TextLocation start, TextLocation end) =>
        TextEditor.SelectionManager.SetSelection(start, end);

    public void ClearSelection() => TextEditor.SelectionManager.ClearSelection();

    public void ScrollTo(int line) => TextEditor.ScrollTo(line);

    #endregion

    #region ====Static Helpers====

    private static bool IsSelectableChar(char c) => !IsWhiteSpace(c);

    private static bool IsWhiteSpace(char c) => c == 32;

    private static int FindWordStart(Document document, int offset)
    {
        var line = document.GetLineSegmentForOffset(offset);

        if (offset > 0 &&
            IsWhiteSpace(document.GetCharAt(offset - 1)) &&
            IsWhiteSpace(document.GetCharAt(offset)))
        {
            while (offset > line.Offset &&
                   IsWhiteSpace(document.GetCharAt(offset - 1)))
            {
                --offset;
            }
        }
        else if (IsSelectableChar(document.GetCharAt(offset)) ||
                 (offset > 0 &&
                  IsWhiteSpace(document.GetCharAt(offset)) &&
                  IsSelectableChar(document.GetCharAt(offset - 1))))
        {
            while (offset > line.Offset &&
                   IsSelectableChar(document.GetCharAt(offset - 1)))
            {
                --offset;
            }
        }
        else
        {
            if (offset > 0 &&
                !IsWhiteSpace(document.GetCharAt(offset - 1)) &&
                !IsSelectableChar(document.GetCharAt(offset - 1)))
            {
                return Math.Max(0, offset - 1);
            }
        }

        return offset;
    }

    private static int FindWordEnd(Document document, int offset)
    {
        var line = document.GetLineSegmentForOffset(offset);
        if (line.Length == 0) return offset;
        var endPos = line.Offset + line.Length;
        offset = Math.Min(offset, endPos - 1);

        if (IsSelectableChar(document.GetCharAt(offset)))
        {
            while (
                offset < endPos && IsSelectableChar(document.GetCharAt(offset)))
            {
                ++offset;
            }
        }
        else if (IsWhiteSpace(document.GetCharAt(offset)))
        {
            if (offset > 0 && IsWhiteSpace(document.GetCharAt(offset - 1)))
            {
                while (offset < endPos && IsWhiteSpace(document.GetCharAt(offset)))
                {
                    ++offset;
                }
            }
        }
        else
        {
            return Math.Max(0, offset + 1);
        }

        return offset;
    }

    #endregion
}