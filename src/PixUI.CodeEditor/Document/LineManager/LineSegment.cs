using System;
using System.Collections.Generic;
using PixUI;

namespace CodeEditor;

public sealed class LineSegment : ISegment
{
    internal LinesEnumerator TreeEntry = LinesEnumerator.Invalid;

    public bool IsDeleted => !TreeEntry.IsValid;

    public int LineNumber => TreeEntry.CurrentIndex;

    public int Offset
    {
        get => TreeEntry.CurrentOffset;
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// 不包含换行符的长度
    /// </summary>
    public int Length
    {
        get => TotalLength - DelimiterLength;
        set => throw new NotSupportedException();
    }

    public int TotalLength { get; internal set; }

    public int DelimiterLength { get; internal set; }

    private IList<CodeToken>? _lineTokens;
    private int _tokenColumnIndex; //仅用于Tokenize时缓存
    private Paragraph? _cachedParagraph;

    internal IList<CachedFoldInfo>? CachedFolds { get; private set; }

    #region ====Anchors====

    // Util.WeakCollection<TextAnchor> anchors;
    //
    // public TextAnchor CreateAnchor(int column)
    // {
    // 	if (column < 0 || column > Length)
    // 		throw new ArgumentOutOfRangeException("column");
    // 	TextAnchor anchor = new TextAnchor(this, column);
    // 	AddAnchor(anchor);
    // 	return anchor;
    // }
    //
    // void AddAnchor(TextAnchor anchor)
    // {
    // 	Debug.Assert(anchor.Line == this);
    // 	
    // 	if (anchors == null)
    // 		anchors = new Util.WeakCollection<TextAnchor>();
    // 	
    // 	anchors.Add(anchor);
    // }

    #endregion

    #region ====Edit Methods====

    /// <summary>
    /// Is called when a part of the line is inserted.
    /// </summary>
    internal void InsertedLinePart(LineManager manager, int startColumn, int length)
    {
        if (length == 0) return;

        ClearFoldedLineCache(manager);

        //TODO:
        //Console.WriteLine("InsertedLinePart " + startColumn + ", " + length);
        // if (anchors != null) {
        // 	foreach (TextAnchor a in anchors) {
        // 		if (a.MovementType == AnchorMovementType.BeforeInsertion
        // 		    ? a.ColumnNumber > startColumn
        // 		    : a.ColumnNumber >= startColumn)
        // 		{
        // 			a.ColumnNumber += length;
        // 		}
        // 	}
        // }
    }

    /// <summary>
    /// Is called when a part of the line is removed.
    /// </summary>
    internal void RemovedLinePart(LineManager manager, DeferredEventList deferredEventList,
        int startColumn, int length)
    {
        if (length == 0) return;

        ClearFoldedLineCache(manager);

        //Console.WriteLine("RemovedLinePart " + startColumn + ", " + length);
        //TODO: anchors
        // if (anchors != null) {
        // 	List<TextAnchor> deletedAnchors = null;
        // 	foreach (TextAnchor a in anchors) {
        // 		if (a.ColumnNumber > startColumn) {
        // 			if (a.ColumnNumber >= startColumn + length) {
        // 				a.ColumnNumber -= length;
        // 			} else {
        // 				if (deletedAnchors == null)
        // 					deletedAnchors = new List<TextAnchor>();
        // 				a.Delete(ref deferredEventList);
        // 				deletedAnchors.Add(a);
        // 			}
        // 		}
        // 	}
        // 	if (deletedAnchors != null) {
        // 		foreach (TextAnchor a in deletedAnchors) {
        // 			anchors.Remove(a);
        // 		}
        // 	}
        // }
    }

    /// <summary>
    /// Is called when the LineSegment is deleted.
    /// </summary>
    internal void Deleted(DeferredEventList deferredEventList)
    {
        TreeEntry = LinesEnumerator.Invalid;

        // TODO: anchors
        // if (anchors != null) {
        // 	foreach (TextAnchor a in anchors) {
        // 		a.Delete(ref deferredEventList);
        // 	}
        // 	anchors = null;
        // }
    }

    /// <summary>
    /// Is called after another line's content is appended to this line because the newline in between
    /// was deleted.
    /// The DefaultLineManager will call Deleted() on the deletedLine after the MergedWith call.
    /// 
    /// firstLineLength: the length of the line before the merge.
    /// </summary>
    internal void MergedWith(LineSegment deletedLine, int firstLineLength)
    {
        // TODO: anchors
        // if (deletedLine.anchors != null) {
        // 	foreach (TextAnchor a in deletedLine.anchors) {
        // 		a.Line = this;
        // 		AddAnchor(a);
        // 		a.ColumnNumber += firstLineLength;
        // 	}
        // 	deletedLine.anchors = null;
        // }
    }

    /// <summary>
    /// Is called after a newline was inserted into this line, splitting it into this and followingLine.
    /// </summary>
    internal void SplitTo(LineSegment followingLine)
    {
        // TODO: anchors
        // if (anchors != null) {
        // 	List<TextAnchor>? movedAnchors = null;
        // 	foreach (TextAnchor a in anchors) {
        // 		if (a.MovementType == AnchorMovementType.BeforeInsertion
        // 		    ? a.ColumnNumber > this.Length
        // 		    : a.ColumnNumber >= this.Length)
        // 		{
        // 			a.Line = followingLine;
        // 			followingLine.AddAnchor(a);
        // 			a.ColumnNumber -= this.Length;
        // 			
        // 			if (movedAnchors == null)
        // 				movedAnchors = new List<TextAnchor>();
        // 			movedAnchors.Add(a);
        // 		}
        // 	}
        // 	if (movedAnchors != null) {
        // 		foreach (TextAnchor a in movedAnchors) {
        // 			anchors.Remove(a);
        // 		}
        // 	}
        // }
    }

    #endregion

    #region ====Tokenize Methods====

    public void BeginTokenize()
    {
        ClearCachedParagraph();
        _lineTokens = new List<CodeToken>();
        _tokenColumnIndex = 0;
    }

    public void AddToken(TokenType type, int offset, int length)
    {
#if DEBUG
        if (offset < Offset)
            throw new IndexOutOfRangeException("offset is less than Line.Offset");
#endif

        var column = offset - Offset;
        //处理行首或间隙空格
        if (column > _tokenColumnIndex)
        {
            _lineTokens!.Add(new(TokenType.WhiteSpace, _tokenColumnIndex));
            _tokenColumnIndex = column;
        }

        _lineTokens!.Add(new(type, column));
        _tokenColumnIndex += length;
    }

    public void EndTokenize()
    {
        // 处理行尾空格
        if (_tokenColumnIndex < Length)
        {
            _lineTokens!.Add(new(TokenType.WhiteSpace, _tokenColumnIndex));
        }
    }

    public CodeToken? GetTokenAt(int column)
    {
        if (_lineTokens == null) return null;

        var tokenEndColumn = Length;
        for (var i = _lineTokens.Count - 1; i >= 0; i--)
        {
            var token = _lineTokens[i];
            var tokenStartColumn = token.StartColumn;
            if (tokenStartColumn < column && column <= tokenEndColumn)
            {
                return token;
            }

            tokenEndColumn = tokenStartColumn;
        }

        return null; //should never be here
    }

    public int GetLeadingWhiteSpaces()
    {
        if (_lineTokens == null || _lineTokens.Count == 0) return 0;
        var firstTokenType = _lineTokens[0].Type;
        if (firstTokenType != TokenType.WhiteSpace) return 0;
        return _lineTokens.Count > 1
            ? _lineTokens[1].StartColumn
            : Length;
    }

#if DEBUG
    internal void DumpTokens(Document document)
    {
        if (_lineTokens == null)
        {
            Console.WriteLine("No tokens.");
            return;
        }

        for (var i = 0; i < _lineTokens.Count; i++)
        {
            var token = _lineTokens[i];
            var tokenStartColumn = token.StartColumn;
            var tokenEndColumn = i == _lineTokens.Count - 1
                ? Length
                : _lineTokens[i + 1].StartColumn;
            var tokenOffset =
                document.PositionToOffset(new TextLocation(tokenStartColumn, LineNumber));
            var tokenText = document.GetText(tokenOffset, tokenEndColumn - tokenStartColumn);
            Console.WriteLine($"{token.Type} \"{tokenText}\"");
        }
    }
#endif

    #endregion

    #region ====Line Paragraph====

    internal Paragraph GetLineParagraph(TextEditor editor)
    {
        if (_cachedParagraph != null) return _cachedParagraph!;

        // ReSharper disable once UsingStatementResourceInitialization
        using var ps = new ParagraphStyle() { MaxLines = 1, Height = 1 };
        using var pb = new ParagraphBuilder(ps);

        if (_lineTokens == null || Length == 0)
        {
            var lineText = editor.Document.GetText(Offset, Length);
            pb.PushStyle(editor.Theme.TextStyle);
            pb.AddText(lineText);
        }
        else
        {
            if (editor.Document.TextEditorOptions.EnableFolding)
                BuildParagraphByFoldings(pb, editor);
            else
                BuildParagraphByTokens(pb, editor, 0, Length);
        }

        _cachedParagraph = pb.Build();
        _cachedParagraph!.Layout(float.PositiveInfinity);

        return _cachedParagraph!;
    }

    private void BuildParagraphByTokens(ParagraphBuilder pb, TextEditor editor, int startIndex, int endIndex)
    {
        for (var i = 0; i < _lineTokens!.Count; i++)
        {
            var token = _lineTokens[i];
            var tokenStartColumn = token.StartColumn;
            var tokenEndColumn = i == _lineTokens.Count - 1
                ? Length
                : _lineTokens[i + 1].StartColumn;

            if (startIndex >= tokenEndColumn) continue;

            // get token text, TODO: 优化避免生成字符串
            var tokenOffset = editor.Document.PositionToOffset(new TextLocation(tokenStartColumn, LineNumber));
            var tokenText = editor.Document.GetText(tokenOffset, tokenEndColumn - tokenStartColumn);

            // add to paragraph
            pb.PushStyle(editor.Theme.GetTokenStyle(token.Type));
            pb.AddText(tokenText);
            pb.Pop();

            if (tokenEndColumn >= endIndex) break;
        }
    }

    private void BuildParagraphByFoldings(ParagraphBuilder pb, TextEditor editor)
    {
        // there can't be a folding witch starts in an above line and ends here,
        // because the line is a new one, there must be a return before this line.

        var line = LineNumber;
        var column = -1;
        var lineChars = 0; //used for calc fold offset in line
        FoldMarker? preFold = null;

        while (true)
        {
            var starts = editor.Document.FoldingManager.GetFoldedFoldingsWithStartAfterColumn(line, column);
            if (starts.Count <= 0)
            {
                if (line == LineNumber)
                {
                    //current line has no fold
                    BuildParagraphByTokens(pb, editor, 0, TextLocation.MaxColumn);
                }
                else
                {
                    //has no fold follow
                    var endLine = editor.Document.GetLineSegment(preFold!.EndLine);
                    endLine.BuildParagraphByTokens(pb, editor, preFold!.EndColumn, TextLocation.MaxColumn);
                }

                break;
            }

            //search the first starting folding
            var firstFolding = starts[0];
            foreach (var fm in starts)
            {
                if (fm.StartColumn < firstFolding.StartColumn)
                    firstFolding = fm;
            }

            starts.Clear();

            if (line == LineNumber)
            {
                if (firstFolding.StartColumn > 0)
                {
                    //eg: if (xxx) {
                    //             -> fold start here
                    BuildParagraphByTokens(pb, editor, 0, firstFolding.StartColumn);
                    lineChars += firstFolding.StartColumn;
                }
            }
            else
            {
                //eg: if (xxx) {...} else {...}
                //             <---> preFold here
                var endLine = editor.Document.GetLineSegment(preFold!.EndLine);
                endLine.BuildParagraphByTokens(pb, editor, preFold!.EndColumn, firstFolding.StartColumn);
                lineChars += firstFolding.StartColumn - preFold.EndColumn;
            }

            // add folded text
            pb.PushStyle(editor.Theme.FoldedTextStyle);
            pb.AddText(firstFolding.FoldText);
            pb.Pop();
            CachedFolds ??= new List<CachedFoldInfo>();
            CachedFolds.Add(new CachedFoldInfo(lineChars, firstFolding));
            lineChars += firstFolding.FoldText.Length;

            // goto next iterator
            column = firstFolding.EndColumn;
            line = firstFolding.EndLine;
            preFold = firstFolding;
            if (line >= editor.Document.TotalNumberOfLines)
            {
                break;
            }
        } //end while
    }

    internal float GetXPos(TextEditor editor, int line, int column)
    {
        var para = GetLineParagraph(editor);

        // target line equals this line
        if (line == LineNumber)
        {
            if (column == 0) return 0;

            var columnStart = column - 1;
            if (column > 1 && TextUtils.IsMultiCodeUnit(editor.Document.GetCharAt(Offset + column - 2)))
            {
                columnStart -= 1;
            }

            var box1 = para.GetRectForPosition(columnStart, BoxHeightStyle.Tight, BoxWidthStyle.Tight);
            return box1.Rect.Right;
        }

        // target line is folded in this line, eg: if (XXX) {...} else {...}
        var offsetInLine = -1;
        foreach (var fold in CachedFolds!)
        {
            if (line == fold.FoldMarker.EndLine)
            {
                offsetInLine = fold.LineEnd + column - fold.FoldMarker.EndColumn;
                break;
            }
        }

        //TODO: find column start for multi code unit
        var box2 = para.GetRectForPosition(offsetInLine - 1, BoxHeightStyle.Tight, BoxWidthStyle.Tight);
        return box2.Rect.Right;
    }

    #endregion

    #region ====Clear Caches====

    internal void ClearCachedParagraph()
    {
        _cachedParagraph?.Dispose();
        _cachedParagraph = null;
        CachedFolds = null;
    }

    private void ClearFoldedLineCache(LineManager manager)
    {
        var thisLine = LineNumber;
        var visibleLine = manager.GetVisibleLine(thisLine);
        var logicalLine = manager.GetFirstLogicalLine(visibleLine);
        if (logicalLine != thisLine)
        {
            manager.GetLineSegment(logicalLine).ClearCachedParagraph();
        }
    }

    #endregion

    public override string ToString()
    {
        if (IsDeleted)
            return "[LineSegment: (deleted) Length = " + Length + ", TotalLength = " +
                   TotalLength + ", DelimiterLength = " + DelimiterLength + "]";
        return "[LineSegment: LineNumber=" + LineNumber + ", Offset = " + Offset +
               ", Length = " + Length + ", TotalLength = " + TotalLength +
               ", DelimiterLength = " + DelimiterLength + "]";
    }
}

internal readonly struct CachedFoldInfo
{
    public CachedFoldInfo(int lineStart, FoldMarker foldMarker)
    {
        LineStart = lineStart;
        FoldMarker = foldMarker;
    }

    internal readonly int LineStart;
    internal readonly FoldMarker FoldMarker;

    internal int LineEnd => LineStart + FoldMarker.FoldText.Length;
}