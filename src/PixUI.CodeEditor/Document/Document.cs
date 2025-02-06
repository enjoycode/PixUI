using System;
using System.Collections.Generic;

namespace CodeEditor;

public sealed class Document : IDisposable
{
    public Document(string fileName, ITextBuffer textBuffer, ISyntaxParser syntaxParser, string? tag = null)
    {
        _fileName = fileName;
        Tag = tag;

        TextBuffer = textBuffer;
        SyntaxParser = syntaxParser;
        if (SyntaxParser.Document != null!)
            throw new Exception("Document has already been initialized.");
        SyntaxParser.Document = this;

        _lineManager = new LineManager(this);
        FoldingManager = new FoldingManager(this);
        TextEditorOptions = new TextEditorOptions();
        UndoStack = new UndoStack();
    }

    #region ====Fields & Properties====

    private string _fileName;
    public readonly string? Tag;
    public readonly ITextBuffer TextBuffer;
    public readonly ISyntaxParser SyntaxParser;
    private readonly LineManager _lineManager;
    public readonly FoldingManager FoldingManager;
    internal readonly TextEditorOptions TextEditorOptions;
    internal readonly UndoStack UndoStack;

    public bool Readonly { get; set; }

    /// <summary>
    /// 是否有语法错误
    /// </summary>
    public bool HasSyntaxError => SyntaxParser.HasSyntaxError;

    public int TextLength => TextBuffer.Length;

    public int TotalNumberOfLines => _lineManager.TotalNumberOfLines;

    #endregion

    #region ====Events====

    public event Action<DocumentEventArgs>? DocumentChanged;

    #endregion

    #region ====Text Methods====

    public void Open(string? srcCode = null)
    {
        if (!string.IsNullOrEmpty(srcCode))
            TextBuffer.SetContent(srcCode);
        
        if (TextBuffer.Length == 0) return;
        //TODO:
        var text = TextBuffer.GetText(0, TextBuffer.Length);
        SyntaxParser.BeginEdit(0, 0, text.Length);
        _lineManager.SetContent(text);
        UndoStack.ClearAll();
        SyntaxParser.EndEdit(0, 0, text.Length);
        
        DocumentChanged?.Invoke(new DocumentEventArgs(this, 0, 0, text));
    }

    public string TextContent => GetText(0, TextBuffer.Length);

    public char GetCharAt(int offset) => TextBuffer.GetCharAt(offset);

    public string GetText(int offset, int length) => TextBuffer.GetText(offset, length);

    public void Insert(int offset, string text)
    {
        if (Readonly) return;

        SyntaxParser.BeginEdit(offset, 0, text.Length);

        TextBuffer.Insert(offset, text);
        _lineManager.Insert(offset, text);
        UndoStack.Push(new UndoableInsert(this, offset, text));

        SyntaxParser.EndEdit(offset, 0, text.Length);

        DocumentChanged?.Invoke(new DocumentEventArgs(this, offset, 0, text));
    }

    public void Remove(int offset, int length)
    {
        if (Readonly) return;

        SyntaxParser.BeginEdit(offset, length, 0);

        UndoStack.Push(new UndoableDelete(this, offset, GetText(offset, length)));
        TextBuffer.Remove(offset, length);
        _lineManager.Remove(offset, length);

        SyntaxParser.EndEdit(offset, length, 0);

        DocumentChanged?.Invoke(new DocumentEventArgs(this, offset, length, ""));
    }

    public void Replace(int offset, int length, string text)
    {
        if (Readonly) return;

        SyntaxParser.BeginEdit(offset, length, text.Length);

        UndoStack.Push(new UndoableReplace(this, offset, GetText(offset, length), text));
        TextBuffer.Replace(offset, length, text);
        _lineManager.Replace(offset, length, text);

        SyntaxParser.EndEdit(offset, length, text.Length);

        DocumentChanged?.Invoke(new DocumentEventArgs(this, offset, length, text));
    }

    public void StartUndoGroup() => UndoStack.StartUndoGroup();
    public void EndUndoGroup() => UndoStack.EndUndoGroup();

    #endregion

    #region ====Position Methods====

    /// Returns a valid line number for the given offset.
    public int GetLineNumberForOffset(int offset)
        => _lineManager.GetLineNumberForOffset(offset);

    /// Returns a [LineSegment] for the given offset.
    public LineSegment GetLineSegmentForOffset(int offset)
        => _lineManager.GetLineSegmentForOffset(offset);

    public LineSegment GetLineSegment(int lineNumber)
        => _lineManager.GetLineSegment(lineNumber);

    /// Get the first logical line for a given visible line.
    /// example : lineNumber == 100 foldings are in the linetracker
    /// between 0..1 (2 folded, invisible lines) this method returns 102
    /// the 'logical' line number
    public int GetFirstLogicalLine(int lineNumber)
        => _lineManager.GetFirstLogicalLine(lineNumber);

    /// Get the visible line for a given logical line.
    /// example : lineNumber == 100 foldings are in the linetracker
    /// between 0..1 (2 folded, invisible lines) this method returns 98
    /// the 'visible' line number
    public int GetVisibleLine(int lineNumber)
        => _lineManager.GetVisibleLine(lineNumber);

    /// returns the logical line/column position from an offset
    public TextLocation OffsetToPosition(int offset)
    {
        var lineNumber = GetLineNumberForOffset(offset);
        var line = GetLineSegment(lineNumber);
        return new TextLocation(offset - line.Offset, lineNumber);
    }

    /// returns the offset from a logical line/column position
    public int PositionToOffset(TextLocation position)
    {
        if (position.Line >= TotalNumberOfLines) return 0;

        var line = GetLineSegment(position.Line);
        return Math.Min(TextLength, line.Offset + Math.Min(line.Length, position.Column));
    }

    internal void UpdateSegmentsOnDocumentChanged<T>(IList<T> list, DocumentEventArgs e)
        where T : ISegment
    {
        var removedCharacters = e.Length > 0 ? e.Length : 0;
        var insertedCharacters = string.IsNullOrEmpty(e.Text) ? 0 : e.Text.Length;
        for (var i = 0; i < list.Count; ++i)
        {
            ISegment s = list[i];
            var segmentStart = s.Offset;
            var segmentEnd = s.Offset + s.Length;

            if (e.Offset <= segmentStart)
            {
                segmentStart -= removedCharacters;
                if (segmentStart < e.Offset) segmentStart = e.Offset;
            }

            if (e.Offset < segmentEnd)
            {
                segmentEnd -= removedCharacters;
                if (segmentEnd < e.Offset) segmentEnd = e.Offset;
            }

            // Debug.Assert(segmentStart <= segmentEnd);

            if (segmentStart == segmentEnd)
            {
                list.RemoveAt(i);
                --i;
                continue;
            }

            if (e.Offset <= segmentStart) segmentStart += insertedCharacters;
            if (e.Offset < segmentEnd) segmentEnd += insertedCharacters;

            // Debug.Assert(segmentStart < segmentEnd);

            s.Offset = segmentStart;
            s.Length = segmentEnd - segmentStart;
        }
    }

    #endregion

    public void Dispose() => SyntaxParser.Dispose();
}