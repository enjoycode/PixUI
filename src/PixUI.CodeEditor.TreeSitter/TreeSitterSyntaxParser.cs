using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using PixUI;

namespace CodeEditor;

public sealed class TreeSitterSyntaxParser : ISyntaxParser
{
#if __WEB__
    internal const int ParserEncoding = 1;
#else
    public const int ParserEncoding = 2;
#endif

    public TreeSitterSyntaxParser()
    {
        // @ts-ignore for new TSParser();
        _parser = new TSParser(); //Don't use Initializer
        _parser.Language = TSCSharpLanguage.Instance;
        Language = new CSharpLanguage();
    }

    public Document Document { get; set; } = null!;

    private readonly TSParser _parser;
    internal ICodeLanguage Language { get; }

    private TSTree? _oldTree;
    private TSEdit _edit;
    internal TSNode? RootNode => _oldTree?.Root;

    //重新Parse后受影响的行范围(需要重新绘制)
    private int _startLineOfChanged;
    private int _endLineOfChanged;

    #region ====Language Methods====

    public bool HasSyntaxError => RootNode?.HasError() ?? false;

    public char? GetAutoClosingPairs(char ch) => Language.GetAutoClosingPairs(ch);

    public bool IsBlockStartBracket(char ch) => Language.IsBlockStartBracket(ch);

    public bool IsBlockEndBracket(char ch) => Language.IsBlockEndBracket(ch);

    #endregion

    #region ====Edit Methods====

    public void BeginEdit(int offset, int length, int textLength)
    {
        var startLocation = Document.OffsetToPosition(offset);
        var endLocation = length == 0 ? startLocation : Document.OffsetToPosition(offset + length);
        _edit.startIndex = (uint)(offset * ParserEncoding);
        _edit.oldEndIndex = _edit.startIndex + (uint)(length * ParserEncoding);
        _edit.newEndIndex = _edit.startIndex + (uint)(textLength * ParserEncoding);
        _edit.startPosition = TSPoint.FromLocation(startLocation);
        _edit.oldEndPosition = TSPoint.FromLocation(endLocation);
    }

    public void EndEdit(int offset, int length, int textLength)
    {
        _edit.newEndPosition = length > 0 && textLength == 0
            ? _edit.startPosition
            : TSPoint.FromLocation(Document.OffsetToPosition(offset + textLength));

#if __WEB__
        _oldTree?.Edit(_edit);
#else
        _oldTree?.Edit(ref _edit);
#endif
        Parse();
        Tokenize(_startLineOfChanged, _endLineOfChanged);
    }

    #endregion

    #region ====Parse & Tokenize====

    public ValueTask<ValueTuple<int, int>> ParseAndTokenize() => new((_startLineOfChanged, _endLineOfChanged));

    [TSRawScript(@"
        public Parse(reset: boolean) {
            let input = new CodeEditor.ParserInput(this._document.TextBuffer);
            // @ts-ignore
            let newTree = this._parser.parse(input.Read.bind(input), reset === true ? null : this._oldTree);

            //获取变动范围
            if (this._oldTree && !reset) {
                let changes = newTree.getChangedRanges(this._oldTree);

                this._oldTree.delete();

                this._startLineOfChanged = this._edit.startPosition.row;
                this._endLineOfChanged = this._startLineOfChanged + 1;
                for (const range of changes) {
                    this._startLineOfChanged = Math.min(this._startLineOfChanged, range.startPosition.row);
                    this._endLineOfChanged = Math.max(this._endLineOfChanged, range.endPosition.row);
                }
            }
            this._oldTree = newTree;

            //生成FoldMarkers
            let foldMarkers = this.Language.GenerateFoldMarkers(this._document);
            this._document.FoldingManager.UpdateFoldings(foldMarkers);
        }
")]
    private unsafe void Parse()
    {
#if !__WEB__
        using var input = new ParserInput(Document.TextBuffer);
        var gcHandle = GCHandle.Alloc(input);
        var tsInput = new TSInput
        {
            payload = GCHandle.ToIntPtr(gcHandle),
            read = (IntPtr)(delegate* unmanaged<void*, uint, TSPoint*, uint*, void*>)&ParserInput.Read,
            encoding = TsInputEncoding.Utf16
        };

#if DEBUG
        var ts = Stopwatch.GetTimestamp();
#endif
        var newTree = _parser.Parse(tsInput, _oldTree);
        gcHandle.Free();

#if DEBUG
        Log.Debug($"SyntaxParser.Parse: 耗时{Stopwatch.GetElapsedTime(ts).TotalMilliseconds}ms");
#endif

        //获取变动范围
        if (_oldTree != null)
        {
            uint rangeCount = 0;
            var rangesPtr = TreeSitterApi.ts_tree_get_changed_ranges(_oldTree.Handle,
                newTree.Handle, ref rangeCount);

            _oldTree!.Dispose();

            //rangeCount可能等于0, 每个range的startLine可能等于endLine

            _startLineOfChanged = (int)_edit.startPosition.row; //设为当前行
            _endLineOfChanged = _startLineOfChanged + 1;
            for (var i = 0; i < rangeCount; i++)
            {
                var startLine = (int)rangesPtr[i].StartPosition.row;
                var endLine = (int)rangesPtr[i].EndPosition.row;
                _startLineOfChanged = Math.Min(_startLineOfChanged, startLine);
                _endLineOfChanged = Math.Max(_endLineOfChanged, endLine + 1);
                // Console.WriteLine(
                //     $"{rangesPtr[i]} {rangesPtr[i].StartIndex}-{rangesPtr[i].EndIndex}");
            }

            NativeMemory.Free(rangesPtr); //TreeSitterApi.ts_util_free(new IntPtr(rangesPtr));
        }
        else
        {
            _startLineOfChanged = 0;
            _endLineOfChanged = Document.TotalNumberOfLines;
        }

        _oldTree = newTree;

        //生成FoldMarkers
        var foldMarkers = Language.GenerateFoldMarkers(Document);
        Document.FoldingManager.UpdateFoldings(foldMarkers);
#endif
    }

    /// <summary>
    /// Tokenize lines range [startLine, endLine)
    /// </summary>
    private void Tokenize(int startLine, int endLine)
    {
#if DEBUG
        var ts = Stopwatch.GetTimestamp();
#endif

        for (var i = startLine; i < endLine; i++)
        {
            TokenizeLine(i);
        }

#if DEBUG
        Log.Debug($"Tokenize[{startLine}-{endLine}]耗时: {Stopwatch.GetElapsedTime(ts).TotalMilliseconds}ms");
#endif
    }

    private void TokenizeLine(int line)
    {
        var lineSegment = Document.GetLineSegment(line);
        var lineLength = lineSegment.Length;
        if (lineLength == 0) return;

        var lineStartPoint = new TSPoint(line, 0);
        var lineEndPoint = new TSPoint(line, lineLength * ParserEncoding);
        var lineNode = _oldTree!.Root.NamedDescendantForPosition(lineStartPoint, lineEndPoint);
        if (lineNode == null) return;

        lineSegment.BeginTokenize();

        if (ContainsFullLine(lineNode.Value, lineSegment))
        {
            VisitNode(lineNode.Value, lineSegment);
        }
        else
        {
            //TODO:
            lineSegment.AddToken(TokenType.Unknown, lineSegment.Offset, lineSegment.Length);
        }

        lineSegment.EndTokenize();

        //CodeToken.DumpLineTokens(lineSegment, _document);
    }

    private void VisitChildren(in TSNode node, int count, LineSegment lineSegment)
    {
        var cursor = new TSTreeCursor(node);
        cursor.GotoFirstChild();
        for (var i = 0; i < count; i++)
        {
            var child = cursor.Current;
            if (BeforeLine(child, lineSegment))
            {
                cursor.GotoNextSibling();
                continue;
            }

            if (AfterLine(child, lineSegment)) break;
            VisitNode(child, lineSegment);
            cursor.GotoNextSibling();
        }

        cursor.Dispose();
    }

    private void VisitNode(in TSNode node, LineSegment lineSegment)
    {
        var childrenCount = node.ChildCount;
        if (childrenCount > 0 && !Language.IsLeafNode(node))
        {
            VisitChildren(node, childrenCount, lineSegment);
            return;
        }

        // leaf node now
        // 注意: 1.可能跨行的Comment; 2.如下特例(" this._")会产生长度为0的MISSING节点
        // member_access_expression [4, 0] - [4, 6]
        //     expression: this_expression [4, 0] - [4, 4]
        //     name: identifier [4, 5] - [4, 6]
        // MISSING ; [4, 6] - [4, 6]
        if (node.EndIndex <= node.StartIndex) return;

        var tokenType = Language.GetTokenType(node);
        var startOffset = Math.Max(node.StartIndex / ParserEncoding, lineSegment.Offset);
        var length = Math.Min((node.EndIndex - node.StartIndex) / ParserEncoding, lineSegment.Length);
        lineSegment.AddToken(tokenType, startOffset, length);
    }

    private static bool ContainsFullLine(in TSNode node, LineSegment lineSegment)
    {
        var nodeStartOffset = node.StartIndex / ParserEncoding;
        var nodeEndOffset = node.EndIndex / ParserEncoding;

        return nodeStartOffset <= lineSegment.Offset &&
               (lineSegment.Offset + lineSegment.Length) <= nodeEndOffset;
    }

    private static bool BeforeLine(in TSNode node, LineSegment lineSegment)
    {
        var nodeEndOffset = node.EndIndex / ParserEncoding;
        return nodeEndOffset < lineSegment.Offset;
    }

    private static bool AfterLine(in TSNode node, LineSegment lineSegment)
    {
        var nodeStartOffset = node.StartIndex / ParserEncoding;
        return nodeStartOffset > (lineSegment.Offset + lineSegment.Length);
    }

    #endregion

    internal void DumpTree()
    {
        if (_oldTree == null)
            Console.WriteLine("No parsed tree.");
        Console.WriteLine(_oldTree!.Root);
    }

    public void Dispose()
    {
        _oldTree?.Dispose();
        _parser.Dispose();
    }
}