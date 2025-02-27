using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeEditor;

public sealed class FoldingManager : IDisposable
{
    public FoldingManager(Document document)
    {
        _document = document;
        _document.DocumentChanged += OnDocumentChanged;
    }

    private readonly Document _document;
    private readonly TextSegmentTree<FoldingSegment> _foldings = new();
    private bool _isFirstUpdate = true;

    internal void RaiseFoldingsChanged()
    {
        // FoldingsChanged?.Invoke();
    }

    private void OnDocumentChanged(DocumentEventArgs e)
    {
        _foldings.UpdateOffsets(e);

        var newEndOffset = e.Offset + e.Text.Length;
        // extend end offset to the end of the line (including delimiter)
        var endLine = _document.GetLineSegmentByOffset(newEndOffset);
        newEndOffset = endLine.Offset + endLine.TotalLength;
        foreach (var affectedFolding in _foldings.FindOverlappingSegments(e.Offset, newEndOffset - e.Offset))
        {
            if (affectedFolding.Length == 0)
                RemoveFolding(affectedFolding);
            // else
            //     affectedFolding.ValidateCollapsedLineSections();
        }
    }

    #region ====Visible & Logical Lines====

    public IEnumerable<int> GetLogicalLines(int visibleStartLine, int count)
    {
        if (_document.TextEditorOptions.EnableFolding)
        {
            var curLogicalLine = -1;
            var foldings = GetTopLevelFoldedFoldings();
            var diff = 0;
            var preFoldedEndLine = 0;
            foreach (var fs in foldings)
            {
                var fsStartLine = _document.GetLineNumberByOffset(fs.StartOffset);
                var fsEndLine = _document.GetLineNumberByOffset(fs.EndOffset);

                while (fsStartLine > curLogicalLine)
                {
                    curLogicalLine = curLogicalLine == -1
                        ? preFoldedEndLine + visibleStartLine - diff
                        : curLogicalLine + 1;
                    if (curLogicalLine >= _document.TotalNumberOfLines)
                        yield break;
                    yield return curLogicalLine;
                    count--;
                    if (count <= 0)
                        yield break;
                }

                if (fsStartLine >= preFoldedEndLine)
                {
                    diff += fsStartLine - preFoldedEndLine;
                    preFoldedEndLine = fsEndLine;
                    curLogicalLine = Math.Max(fsEndLine, curLogicalLine);
                }
            }

            // after all folded or there's no any folded
            while (count > 0)
            {
                curLogicalLine = curLogicalLine == -1 ? visibleStartLine : curLogicalLine + 1;
                if (curLogicalLine >= _document.TotalNumberOfLines)
                    yield break;
                yield return curLogicalLine;
                count--;
            }
        }
        else
        {
            for (var i = 0; i < count; i++)
            {
                var logicalLineNum = visibleStartLine + i;
                if (logicalLineNum >= _document.TotalNumberOfLines)
                    break;
                yield return logicalLineNum;
            }
        }
    }

    public int GetFirstLogicalLine(int visibleLineNumber)
    {
        if (!_document.TextEditorOptions.EnableFolding)
            return visibleLineNumber;

        var diff = 0;
        var preFoldedEndLine = 0;
        var foldings = GetTopLevelFoldedFoldings();
        foreach (var fs in foldings)
        {
            var fsStartLine = _document.GetLineNumberByOffset(fs.StartOffset);
            if (fsStartLine >= preFoldedEndLine)
            {
                if (diff + fsStartLine - preFoldedEndLine >= visibleLineNumber)
                    break;

                diff += fsStartLine - preFoldedEndLine;
                var fsEndLine = _document.GetLineNumberByOffset(fs.EndOffset);
                preFoldedEndLine = fsEndLine;
            }
        }

        return preFoldedEndLine + visibleLineNumber - diff;
    }

    public int GetVisibleLine(int logicalLineNumber)
    {
        if (!_document.TextEditorOptions.EnableFolding)
            return logicalLineNumber;

        var visibleLine = 0;
        var foldEnd = 0;
        var foldings = GetTopLevelFoldedFoldings();
        foreach (var fs in foldings)
        {
            var fsStartLine = _document.GetLineNumberByOffset(fs.StartOffset);
            if (fsStartLine >= logicalLineNumber)
                break;

            if (fsStartLine >= foldEnd)
            {
                var fsEndLine = _document.GetLineNumberByOffset(fs.EndOffset);
                visibleLine += fsStartLine - foldEnd;
                if (fsEndLine > logicalLineNumber)
                    return visibleLine;

                foldEnd = fsEndLine;
            }
        }

        // Debug.Assert(logicalLineNumber >= foldEnd);
        visibleLine += logicalLineNumber - foldEnd;
        return visibleLine;
    }

    #endregion

    #region ====Old Api====

    internal bool IsLineVisible(int lineNumber)
    {
        var line = _document.GetLineSegment(lineNumber);
        var contains = _foldings.FindOverlappingSegments(line);
        foreach (var fs in contains)
        {
            if (!fs.IsFolded) continue;
            if (fs.StartOffset >= line.Offset) continue;
            if (fs.EndOffset <= line.Offset + line.TotalLength) continue;

            //check target line is between folding
            var foldStartLine = _document.GetLineNumberByOffset(fs.StartOffset);
            var foldEndLine = _document.GetLineNumberByOffset(fs.EndOffset);
            if (lineNumber > foldStartLine && lineNumber < foldEndLine)
                return false;
        }

        return true;
    }

    internal IEnumerable<FoldingSegment> GetTopLevelFoldedFoldings()
    {
        var fs = _foldings.FindFirstSegmentWithStartAfter(0);
        while (fs != null)
        {
            if (fs.IsFolded)
            {
                yield return fs;
                fs = _foldings.FindFirstSegmentWithStartAfter(fs.EndOffset);
            }
            else
            {
                fs = _foldings.GetNextSegment(fs);
            }
        }
    }

    #endregion

    #region ====Get...Folding====

    /// <summary>
    /// Gets all foldings in this manager.
    /// The foldings are returned sorted by start offset;
    /// for multiple foldings at the same offset the order is undefined.
    /// </summary>
    public IEnumerable<FoldingSegment> AllFoldings => _foldings;

    /// <summary>
    /// Gets the first offset greater or equal to <paramref name="startOffset"/> where a folded folding starts.
    /// Returns -1 if there are no foldings after <paramref name="startOffset"/>.
    /// </summary>
    public int GetNextFoldedFoldingStart(int startOffset)
    {
        var fs = _foldings.FindFirstSegmentWithStartAfter(startOffset);
        while (fs != null && !fs.IsFolded)
            fs = _foldings.GetNextSegment(fs);
        return fs?.StartOffset ?? -1;
    }

    // /// <summary>
    // /// Gets the first folding with a <see cref="TextSegment.StartOffset"/> greater or equal to
    // /// <paramref name="startOffset"/>.
    // /// Returns null if there are no foldings after <paramref name="startOffset"/>.
    // /// </summary>
    // public FoldingSegment? GetNextFolding(int startOffset)
    // {
    //     // TODO: returns the longest folding instead of any folding at the first position after startOffset
    //     return _foldings.FindFirstSegmentWithStartAfter(startOffset);
    // }

    public FoldingSegment? FindFirstWithStartAfter(int startOffset) =>
        _foldings.FindFirstSegmentWithStartAfter(startOffset);

    public FoldingSegment? GetNextFolding(FoldingSegment foldingSegment) =>
        _foldings.GetNextSegment(foldingSegment);

    /// <summary>
    /// Gets all foldings that start exactly at <paramref name="startOffset"/>.
    /// </summary>
    public ReadOnlyCollection<FoldingSegment> GetFoldingsAt(int startOffset)
    {
        var result = new List<FoldingSegment>();
        var fs = _foldings.FindFirstSegmentWithStartAfter(startOffset);
        while (fs != null && fs.StartOffset == startOffset)
        {
            result.Add(fs);
            fs = _foldings.GetNextSegment(fs);
        }

        return new ReadOnlyCollection<FoldingSegment>(result);
    }

    /// <summary>
    /// Finds all that overlap with the given segment (including touching segments).
    /// Folding are returned in the order given by GetNextSegment/GetPreviousSegment.
    /// </summary>
    public ReadOnlyCollection<FoldingSegment> FindOverlapping(int offset, int length = 0)
    {
        return _foldings.FindOverlappingSegments(offset, length);
    }

    #endregion

    #region ====Create / Remove / Clear====

    /// <summary>
    /// Creates a folding for the specified text section.
    /// </summary>
    private FoldingSegment CreateFolding(int offset, int length)
    {
        if (length <= 0)
            throw new ArgumentException("length must be greater than 0");
        if (offset < 0 || offset + length > _document.TextLength)
            throw new ArgumentException("Folding must be within document boundary");
        var fs = new FoldingSegment(offset, length);
        _foldings.Add(fs);
        // Redraw(fs);
        return fs;
    }

    /// <summary>
    /// Removes a folding section from this manager.
    /// </summary>
    private void RemoveFolding(FoldingSegment fs)
    {
        if (fs == null)
            throw new ArgumentNullException(nameof(fs));
        fs.IsFolded = false;
        _foldings.Remove(fs);
        //Redraw(fs);
    }

    #endregion

    #region ====UpdateFoldings====

    /// <summary>
    /// Updates the foldings in this <see cref="FoldingManager"/> using the given new foldings.
    /// This method will try to detect which new foldings correspond to which existing foldings; and will keep the state
    /// (<see cref="FoldingSegment.IsFolded"/>) for existing foldings.
    /// </summary>
    /// <param name="newFoldings">The new set of foldings. These must be sorted by starting offset.</param>
    /// <param name="firstErrorOffset">The first position of a parse error. Existing foldings starting after
    /// this offset will be kept even if they don't appear in <paramref name="newFoldings"/>.
    /// Use -1 for this parameter if there were no parse errors.</param>
    public void UpdateFoldings(IEnumerable<NewFolding> newFoldings, int firstErrorOffset)
    {
        ArgumentNullException.ThrowIfNull(newFoldings);

        if (firstErrorOffset < 0)
            firstErrorOffset = int.MaxValue;

        var oldFoldings = AllFoldings.ToArray();
        var oldFoldingIndex = 0;
        var previousStartOffset = 0;
        // merge new foldings into old foldings so that sections keep being collapsed
        // both oldFoldings and newFoldings are sorted by start offset
        foreach (var newFolding in newFoldings)
        {
            // ensure newFoldings are sorted correctly
            if (newFolding.Offset < previousStartOffset)
                throw new ArgumentException("newFoldings must be sorted by start offset");
            previousStartOffset = newFolding.Offset;

            if (newFolding.Length <= 0)
                continue; // ignore zero-length foldings

            // remove old foldings that were skipped
            while (oldFoldingIndex < oldFoldings.Length && newFolding.Offset > oldFoldings[oldFoldingIndex].StartOffset)
            {
                RemoveFolding(oldFoldings[oldFoldingIndex++]);
            }

            FoldingSegment section;
            // reuse current folding if it's matching:
            if (oldFoldingIndex < oldFoldings.Length && newFolding.Offset == oldFoldings[oldFoldingIndex].StartOffset)
            {
                section = oldFoldings[oldFoldingIndex++];
                section.Length = newFolding.Length;
            }
            else
            {
                // no matching current folding; create a new one:
                section = CreateFolding(newFolding.Offset, newFolding.Length);
                // auto-close #regions only when opening the document
                if (_isFirstUpdate)
                    section.IsFolded = newFolding.IsFolded;

                section.Tag = newFolding;
            }

            section.FoldedText = newFolding.FoldedText;
        }

        _isFirstUpdate = false;
        // remove all outstanding old foldings:
        while (oldFoldingIndex < oldFoldings.Length)
        {
            var oldSection = oldFoldings[oldFoldingIndex++];
            if (oldSection.StartOffset >= firstErrorOffset)
                break;
            RemoveFolding(oldSection);
        }
    }

    #endregion

    public void Dispose()
    {
        _document.DocumentChanged -= OnDocumentChanged;
    }
}