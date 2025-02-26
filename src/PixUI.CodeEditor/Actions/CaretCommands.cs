namespace CodeEditor;

internal sealed class CaretLeft : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        var caret = editor.Caret;
        if (caret.Line == 0 && caret.Column == 0)
            return;

        var position = caret.Position;
        var caretOffset = caret.Offset;
        var foldings = editor.Document.FoldingManager.FindOverlapping(caretOffset - 1);
        FoldingSegment? justBeforeCaret = null;
        foreach (var fs in foldings)
        {
            if (fs.EndOffset == caretOffset && fs.IsFolded)
            {
                justBeforeCaret = fs;
                // the first folding found is the folding with the smallest Start position
                break;
            }
        }

        if (justBeforeCaret != null)
        {
            var foldStartLine = editor.Document.GetLineSegmentByOffset(justBeforeCaret.StartOffset);
            var foldStartColumn = justBeforeCaret.StartOffset - foldStartLine.Offset;
            position.Line = foldStartLine.LineNumber;
            position.Column = foldStartColumn;
        }
        else
        {
            if (position.Column > 0)
            {
                position.Column -= 1;
            }
            else if (position.Line > 0)
            {
                var lineAbove = editor.Document.GetLineSegment(position.Line - 1);
                position.Column = lineAbove.Length;
                position.Line -= 1;
            }
        }

        caret.Position = position;
        //textArea.setDesiredColumn();
    }
}

internal sealed class CaretRight : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        var caret = editor.Caret;
        var curLine = editor.Document.GetLineSegment(caret.Line);
        var position = caret.Position;
        var foldings = editor.Document.FoldingManager.GetFoldingsAt(caret.Offset);
        FoldingSegment? justBehindCaret = null;
        foreach (var fs in foldings)
        {
            if (fs.IsFolded)
            {
                justBehindCaret = fs;
                break;
            }
        }

        if (justBehindCaret != null)
        {
            var foldEndLine = editor.Document.GetLineSegmentByOffset(justBehindCaret.EndOffset);
            var foldEndColumn = justBehindCaret.EndOffset - foldEndLine.Offset;
            position.Line = foldEndLine.LineNumber;
            position.Column = foldEndColumn;
        }
        else
        {
            // no folding is interesting
            if (position.Column < curLine.Length || editor.Document.TextEditorOptions.AllowCaretBeyondEOL)
            {
                position.Column += 1;
            }
            else if (position.Line + 1 < editor.Document.TotalNumberOfLines)
            {
                position.Line += 1;
                position.Column = 0;
            }
        }

        caret.Position = position;
        //textArea.setDesiredColumn();
    }
}

internal sealed class CaretUp : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        var position = editor.Caret.Position;
        var visualLine = editor.Document.GetVisibleLine(position.Line);
        if (visualLine > 0)
        {
            //暂用模拟点击位置
            var vx = editor.TextView.GetDrawingXPos(position.Line, position.Column) + editor.VirtualTop.X;
            var vy = editor.TextView.Bounds.Top + (visualLine - 1) * editor.TextView.FontHeight -
                     editor.VirtualTop.Y;
            var logicalLine = editor.TextView.GetLogicalLine(vy);
            var logicalColumn = editor.TextView.GetLogicalColumn(logicalLine, vx);
            editor.Caret.Position = logicalColumn.Location;
        }
    }
}

internal sealed class CaretDown : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        var position = editor.Caret.Position;
        var visualLine = editor.Document.GetVisibleLine(position.Line);
        if (visualLine < editor.Document.GetVisibleLine(editor.Document.TotalNumberOfLines))
        {
            //暂用模拟点击位置
            var vx = editor.TextView.GetDrawingXPos(position.Line, position.Column) + editor.VirtualTop.X;
            var vy = editor.TextView.Bounds.Top + (visualLine + 1) * editor.TextView.FontHeight -
                     editor.VirtualTop.Y;
            var logicalLine = editor.TextView.GetLogicalLine(vy);
            var logicalColumn = editor.TextView.GetLogicalColumn(logicalLine, vx);
            editor.Caret.Position = logicalColumn.Location;
        }
    }
}