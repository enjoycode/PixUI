using System;
using System.Diagnostics;

namespace CodeEditor;

public sealed class LineManager
{
    public LineManager(Document document)
    {
        _document = document;
        _lineCollection = new LineSegmentTree();
    }

    private readonly Document _document;
    private readonly LineSegmentTree _lineCollection;

    public int TotalNumberOfLines => _lineCollection.Count;

    #region ====Events====

    public event Action<LineLengthChangeEventArgs>? LineLengthChanged;
    public event Action<LineCountChangeEventArgs>? LineCountChanged;
    public event Action<LineEventArgs>? LineDeleted;

    #endregion

    public int GetLineNumberForOffset(int offset)
        => GetLineSegmentForOffset(offset).LineNumber;

    public LineSegment GetLineSegmentForOffset(int offset)
        => _lineCollection.GetByOffset(offset);

    public LineSegment GetLineSegment(int lineNumber)
        => _lineCollection.GetNode(lineNumber).Value.LineSegment;

    public int GetFirstLogicalLine(int visibleLineNumber)
    {
        if (!_document.TextEditorOptions.EnableFolding)
            return visibleLineNumber;

        var v = 0;
        var foldEnd = 0;
        var foldings = _document.FoldingManager.GetTopLevelFoldedFoldings();
        foreach (var fm in foldings)
        {
            if (fm.StartLine >= foldEnd)
            {
                if (v + fm.StartLine - foldEnd >= visibleLineNumber)
                    break;

                v += fm.StartLine - foldEnd;
                foldEnd = fm.EndLine;
            }
        }

        foldings.Clear();

        return foldEnd + visibleLineNumber - v;
    }

    public int GetVisibleLine(int logicalLineNumber)
    {
        if (!_document.TextEditorOptions.EnableFolding)
            return logicalLineNumber;

        var visibleLine = 0;
        var foldEnd = 0;
        var foldings = _document.FoldingManager.GetTopLevelFoldedFoldings();
        foreach (var fm in foldings)
        {
            if (fm.StartLine >= logicalLineNumber)
                break;

            if (fm.StartLine >= foldEnd)
            {
                visibleLine += fm.StartLine - foldEnd;
                if (fm.EndLine > logicalLineNumber)
                    return visibleLine;

                foldEnd = fm.EndLine;
            }
        }

        foldings.Clear();

        // Debug.Assert(logicalLineNumber >= foldEnd);
        visibleLine += logicalLineNumber - foldEnd;
        return visibleLine;
    }

    public void SetContent(string text)
    {
        _lineCollection.Clear();
        if (!string.IsNullOrEmpty(text))
        {
            Replace(0, 0, text);
        }
    }

    public void Insert(int offset, string text) => Replace(offset, 0, text);

    public void Remove(int offset, int length) => Replace(offset, length, "");

    public void Replace(int offset, int length, string text)
    {
        var lineStart = GetLineNumberForOffset(offset);
        var oldNumberOfLines = TotalNumberOfLines;
        var deferredEventList = new DeferredEventList();
        RemoveInternal(deferredEventList, offset, length);
        // var numberOfLinesAfterRemoving = TotalNumberOfLines;
        if (!string.IsNullOrEmpty(text))
        {
            InsertInternal(offset, text);
        }

        //TODO:
        // Only fire events after RemoveInternal+InsertInternal finished completely:
        // Otherwise we would expose inconsistent state to the event handlers.
        // if (deferredEventList.removedLines != null) {
        //   foreach (LineSegment ls in deferredEventList.removedLines)
        //   OnLineDeleted(new LineEventArgs(document, ls));
        // }
        //deferredEventList.RaiseEvents();

        if (TotalNumberOfLines != oldNumberOfLines)
        {
            LineCountChanged?.Invoke(new LineCountChangeEventArgs(_document, lineStart,
                TotalNumberOfLines - oldNumberOfLines));
        }
    }

    private void InsertInternal(int offset, string text)
    {
        var segment = _lineCollection.GetByOffset(offset);
        var ds = NextDelimiter(text, 0);
        if (ds == null)
        {
            // no newline is being inserted, all text is inserted in a single line
            segment.InsertedLinePart(this, offset - segment.Offset, text.Length);
            SetSegmentLength(segment, segment.TotalLength + text.Length);
            return;
        }

        var firstLine = segment;
        firstLine.InsertedLinePart(this, offset - firstLine.Offset, ds.Value.Offset);
        var lastDelimiterEnd = 0;
        while (ds != null)
        {
            // split line segment at line delimiter
            var lineBreakOffset = offset + ds.Value.Offset + ds.Value.Length;
            var segmentOffset = segment.Offset;
            var lengthAfterInsertionPos =
                segmentOffset + segment.TotalLength - (offset + lastDelimiterEnd);
            _lineCollection.SetSegmentLength(segment, lineBreakOffset - segmentOffset);
            var newSegment =
                _lineCollection.InsertSegmentAfter(segment, lengthAfterInsertionPos);
            segment.DelimiterLength = ds.Value.Length;

            segment = newSegment;
            lastDelimiterEnd = ds.Value.Offset + ds.Value.Length;

            ds = NextDelimiter(text, lastDelimiterEnd);
        }

        firstLine.SplitTo(segment);
        // insert rest after last delimiter
        if (lastDelimiterEnd != text.Length)
        {
            segment.InsertedLinePart(this, 0, text.Length - lastDelimiterEnd);
            SetSegmentLength(
                segment, segment.TotalLength + text.Length - lastDelimiterEnd);
        }
    }

    private void RemoveInternal(DeferredEventList deferredEventList, int offset, int length)
    {
        // Debug.Assert(length >= 0);
        if (length == 0) return;

        var it = _lineCollection.GetEnumeratorForOffset(offset);
        LineSegment startSegment = it.Current;
        int startSegmentOffset = startSegment.Offset;
        if (offset + length < startSegmentOffset + startSegment.TotalLength)
        {
            // just removing a part of this line segment
            startSegment.RemovedLinePart(
                this, deferredEventList, offset - startSegmentOffset, length);
            SetSegmentLength(startSegment, startSegment.TotalLength - length);
            return;
        }

        // merge startSegment with another line segment because startSegment's delimiter was deleted
        // possibly remove lines in between if multiple delimiters were deleted
        int charactersRemovedInStartLine =
            startSegmentOffset + startSegment.TotalLength - offset;
        //Debug.Assert(charactersRemovedInStartLine > 0);
        startSegment.RemovedLinePart(this, deferredEventList,
            offset - startSegmentOffset, charactersRemovedInStartLine);

        LineSegment endSegment = _lineCollection.GetByOffset(offset + length);
        if (endSegment == startSegment)
        {
            // special case: we are removing a part of the last line up to the
            // end of the document
            SetSegmentLength(startSegment, startSegment.TotalLength - length);
            return;
        }

        var endSegmentOffset = endSegment.Offset;
        var charactersLeftInEndLine =
            endSegmentOffset + endSegment.TotalLength - (offset + length);
        endSegment.RemovedLinePart(this, deferredEventList, 0,
            endSegment.TotalLength - charactersLeftInEndLine);
        startSegment.MergedWith(endSegment, offset - startSegmentOffset);
        SetSegmentLength(
            startSegment,
            startSegment.TotalLength -
            charactersRemovedInStartLine +
            charactersLeftInEndLine);
        startSegment.DelimiterLength = endSegment.DelimiterLength;
        // remove all segments between startSegment (excl.) and endSegment (incl.)
        it.MoveNext();
        LineSegment segmentToRemove;
        do
        {
            segmentToRemove = it.Current;
            it.MoveNext();
            _lineCollection.RemoveSegment(segmentToRemove);
            segmentToRemove.Deleted(deferredEventList);
        } while (segmentToRemove != endSegment);
    }

    private void SetSegmentLength(LineSegment segment, int newTotalLength)
    {
        var delta = newTotalLength - segment.TotalLength;
        if (delta == 0) return;

        _lineCollection.SetSegmentLength(segment, newTotalLength);
        LineLengthChanged?.Invoke(new LineLengthChangeEventArgs(_document, segment, delta));
    }

    private static DelimiterSegment? NextDelimiter(string text, int offset)
    {
        for (var i = offset; i < text.Length; i++)
        {
            switch (text[i])
            {
                case '\r':
                    if (i + 1 < text.Length && text[i + 1] == '\n')
                        return new DelimiterSegment(i, 2);
                    else
                        return new DelimiterSegment(i, 1);
                case '\n':
                    return new DelimiterSegment(i, 1);
            }
        }

        return null;
    }
}

internal readonly struct DelimiterSegment
{
    internal readonly int Offset;
    internal readonly int Length;

    internal DelimiterSegment(int offset, int length)
    {
        Offset = offset;
        Length = length;
    }
}