namespace CodeEditor;

internal sealed class CaretLeft : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        var position = editor.Caret.Position;
        var foldings = editor.Document.FoldingManager.GetFoldedFoldingsWithEnd(position.Line);
        FoldMarker? justBeforeCaret = null;
        foreach (var fm in foldings)
        {
            if (fm.EndColumn == position.Column)
            {
                justBeforeCaret = fm;
                // the first folding found is the folding with the smallest Start position
                break;
            }
        }

        if (justBeforeCaret != null)
        {
            position.Line = justBeforeCaret.StartLine;
            position.Column = justBeforeCaret.StartColumn;
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
                position.Line = position.Line - 1;
            }
        }

        editor.Caret.Position = position;
        //textArea.setDesiredColumn();
    }
}

internal sealed class CaretRight : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        var curLine = editor.Document.GetLineSegment(editor.Caret.Line);
        var position = editor.Caret.Position;
        var foldings = editor.Document.FoldingManager.GetFoldedFoldingsWithStart(position.Line);
        FoldMarker? justBehindCaret = null;
        foreach (var fm in foldings)
        {
            if (fm.StartColumn == position.Column)
            {
                justBehindCaret = fm;
                break;
            }
        }

        if (justBehindCaret != null)
        {
            position.Line = justBehindCaret.EndLine;
            position.Column = justBehindCaret.EndColumn;
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

        editor.Caret.Position = position;
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