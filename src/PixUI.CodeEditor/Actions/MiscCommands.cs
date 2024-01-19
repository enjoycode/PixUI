namespace CodeEditor;

internal sealed class BackspaceCommand : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        if (editor.Document.Readonly) return;

        if (editor.SelectionManager.HasSomethingSelected)
        {
            editor.DeleteSelection();
            return;
        }

        var caretOffset = editor.Caret.Offset;
        if (caretOffset <= 0) return;

        var curLineNr = editor.Document.GetLineNumberForOffset(caretOffset);
        var curLine = editor.Document.GetLineSegment(curLineNr);
        var curLineOffset = curLine.Offset;
        if (curLineOffset == caretOffset)
        {
            var preLine = editor.Document.GetLineSegment(curLineNr - 1);
            var preLineEndOffset = preLine.Offset + preLine.Length;
            editor.Document.Remove(preLineEndOffset, curLineOffset - preLineEndOffset);
            editor.Caret.Position = new TextLocation(preLine.Length, curLineNr - 1);
            return;
        }

        //TODO:unicode like emoji

        //判断之前是否Tab缩进
        var tabIndent = editor.Document.TextEditorOptions.TabIndent;
        if (caretOffset - curLineOffset >= tabIndent)
        {
            var preTab = true;
            for (var i = 0; i < tabIndent; i++)
            {
                if (editor.Document.GetCharAt(caretOffset - 1 - i) != ' ')
                {
                    preTab = false;
                    break;
                }
            }

            if (preTab)
            {
                editor.Document.Remove(caretOffset - tabIndent, tabIndent);
                editor.Caret.Position = editor.Document.OffsetToPosition(caretOffset - tabIndent);
                return;
            }
        }

        //处理AutoClosingPairs
        var ch = editor.Document.GetCharAt(caretOffset - 1);
        var closingPair = editor.Document.SyntaxParser.Language.GetAutoColsingPairs(ch);
        var len = closingPair != null && closingPair.Value == editor.Document.GetCharAt(caretOffset)
            ? 2
            : 1;

        editor.Document.Remove(caretOffset - 1, len);
        editor.Caret.Position = editor.Document.OffsetToPosition(caretOffset - 1);
    }
}

internal sealed class TabCommand : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        //TODO: 暂简单实现
        if (editor.Document.Readonly) return;

        var tabIndent = editor.Document.TextEditorOptions.TabIndent;
        var convertToWhitespaces = new string(' ', tabIndent);
        editor.InsertOrReplaceString(convertToWhitespaces);
    }
}

internal sealed class ReturnCommand : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        if (editor.Document.Readonly) return;

        editor.Document.UndoStack.StartUndoGroup();
        var curLine = editor.Caret.Line;
        var curLineSegment = editor.Document.GetLineSegment(curLine);
        var leadingWhiteSpaces = curLineSegment.GetLeadingWhiteSpaces();
        var caretOffset = editor.Caret.Offset;
        var language = editor.Document.SyntaxParser.Language;
        var isBlockBracket = caretOffset > 0 &&
                             language.IsBlockStartBracket(editor.Document.GetCharAt(caretOffset - 1)) &&
                             language.IsBlockEndBracket(editor.Document.GetCharAt(caretOffset));
        var tabIndent = editor.Document.TextEditorOptions.TabIndent;

        var insertText = "\n";
        if (isBlockBracket)
            insertText += new string(' ', leadingWhiteSpaces + tabIndent) + "\n";
        if (leadingWhiteSpaces > 0)
            insertText += new string(' ', leadingWhiteSpaces);

        editor.InsertOrReplaceString(insertText);
        if (isBlockBracket)
            editor.Caret.Position = new TextLocation(leadingWhiteSpaces + tabIndent, curLine + 1);
        // editor.Document.FormattingStrategy.FormatLine(editor, curLine, editor.Caret.Offset, '\n');
        editor.Document.UndoStack.EndUndoGroup();
    }
}

internal sealed class UndoCommand : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        // if (editor.Document.ReadOnly) return;

        editor.Document.UndoStack.Undo();
    }
}

internal sealed class RedoCommand : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        // if (editor.Document.ReadOnly) return;

        editor.Document.UndoStack.Redo();
    }
}

internal sealed class SelectAllCommand : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        var start = editor.Document.OffsetToPosition(0);
        var end = editor.Document.OffsetToPosition(editor.Document.TextLength);
        editor.SelectionManager.SetSelection(start, end);
        editor.Controller.Widget.RequestInvalidate(false, null); //暂需要重绘选择区域
    }
}