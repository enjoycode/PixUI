namespace CodeEditor
{
    internal sealed class BackspaceCommand : IEditCommand
    {
        public void Execute(TextEditor editor)
        {
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
            }
            else
            {
                //TODO:unicode like emoji
                //先处理AutoClosingPairs
                var ch = editor.Document.GetCharAt(caretOffset - 1);
                var closingPair = editor.Document.SyntaxParser.Language.GetAutoColsingPairs(ch);
                var len = closingPair != null &&
                          closingPair.Value == editor.Document.GetCharAt(caretOffset)
                    ? 2
                    : 1;

                editor.Document.Remove(caretOffset - 1, len);
                editor.Caret.Position = editor.Document.OffsetToPosition(caretOffset - 1);
            }
        }
    }

    internal sealed class TabCommand : IEditCommand
    {
        public void Execute(TextEditor editor)
        {
            //TODO: 暂简单实现
            // if (editor.Document.ReadOnly) return;

            var tabIndent = editor.Document.TextEditorOptions.TabIndent;
            var convertToWhitespaces = new string(' ', tabIndent);
            editor.InsertOrReplaceString(convertToWhitespaces);
        }
    }

    internal sealed class ReturnCommand : IEditCommand
    {
        public void Execute(TextEditor editor)
        {
            // if (editor.Document.ReadOnly) return;

            editor.Document.UndoStack.StartUndoGroup();
            var curLine = editor.Caret.Line;
            var curLineSegment = editor.Document.GetLineSegment(curLine);
            var leadingWhiteSpaces = curLineSegment.GetLeadingWhiteSpaces();
            if (leadingWhiteSpaces == 0)
                editor.InsertOrReplaceString("\n");
            else
                editor.InsertOrReplaceString("\n" + new string(' ', leadingWhiteSpaces));

            // editor.Document.FormattingStrategy.FormatLine(
            //     editor, curLine, editor.Caret.Offset, '\n');
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
}