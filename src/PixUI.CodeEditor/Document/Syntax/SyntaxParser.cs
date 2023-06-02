using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using PixUI;

namespace CodeEditor
{
    public sealed class SyntaxParser : IDisposable
    {
#if __WEB__
        internal const int ParserEncoding = 1;
#else
        internal const int ParserEncoding = 2;
#endif

        public SyntaxParser(Document document)
        {
            _document = document;
            var language = TSCSharpLanguage.Get();
            // @ts-ignore for new TSParser();
            _parser = new TSParser(); //Don't use Initializer
            _parser.Language = language;
            Language = new CSharpLanguage();
        }

        private readonly Document _document;

        private readonly TSParser _parser;
        internal ICodeLanguage Language { get; }

        private TSTree? _oldTree;
        private TSEdit _edit = new TSEdit();
        internal TSSyntaxNode? RootNode => _oldTree?.Root;

        //重新Parse后受影响的行范围(需要重新绘制)
        private int _startLineOfChanged;
        private int _endLineOfChanged;

        #region ====Edit Methods====

        internal void BeginInsert(int offset, int length)
        {
            var startLocation = _document.OffsetToPosition(offset);
            _edit.startIndex = (uint)offset * ParserEncoding;
            _edit.oldEndIndex = _edit.startIndex;
            _edit.newEndIndex = _edit.startIndex + (uint)length * ParserEncoding;
            _edit.startPosition = TSPoint.FromLocation(startLocation);
            _edit.oldEndPosition = _edit.startPosition;
        }

        internal void EndInsert(int offset, int length)
        {
            var endLocation = _document.OffsetToPosition(offset + length);
            _edit.newEndPosition = TSPoint.FromLocation(endLocation);

#if __WEB__
            _oldTree!.Edit(_edit);
#else
            _oldTree!.Edit(ref _edit);
#endif

            Parse(false);
            Tokenize(_startLineOfChanged, _endLineOfChanged);
        }

        internal void BeginRemove(int offset, int length)
        {
            var startLocation = _document.OffsetToPosition(offset);
            var endLocation = _document.OffsetToPosition(offset + length);
            _edit.startIndex = (uint)offset * ParserEncoding;
            _edit.oldEndIndex = _edit.startIndex + (uint)length * ParserEncoding;
            _edit.newEndIndex = _edit.startIndex;
            _edit.startPosition = TSPoint.FromLocation(startLocation);
            _edit.oldEndPosition = TSPoint.FromLocation(endLocation);
            _edit.newEndPosition = _edit.startPosition;
        }

        internal void EndRemove()
        {
#if __WEB__
            _oldTree!.Edit(_edit);
#else
            _oldTree!.Edit(ref _edit);
#endif
            Parse(false);
            Tokenize(_startLineOfChanged, _endLineOfChanged);
        }

        internal void BeginReplace(int offset, int length, int textLenght)
        {
            var startLocation = _document.OffsetToPosition(offset);
            var endLocation = _document.OffsetToPosition(offset + length);
            _edit.startIndex = (uint)offset * ParserEncoding;
            _edit.oldEndIndex = _edit.startIndex + (uint)length * ParserEncoding;
            _edit.newEndIndex = _edit.startIndex + (uint)((textLenght - length) * ParserEncoding);
            _edit.startPosition = TSPoint.FromLocation(startLocation);
            _edit.oldEndPosition = TSPoint.FromLocation(endLocation);
        }

        internal void EndReplace(int offset, int length, int textLength)
        {
            var endLocation = _document.OffsetToPosition(offset + (textLength - length));
            _edit.newEndPosition = TSPoint.FromLocation(endLocation);

#if __WEB__
            _oldTree!.Edit(_edit);
#else
            _oldTree!.Edit(ref _edit);
#endif
            Parse(false);
            Tokenize(_startLineOfChanged, _endLineOfChanged);
        }

        #endregion

        #region ====Parse & Tokenize====

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
        public unsafe void Parse(bool reset)
        {
#if !__WEB__
            using var input = new ParserInput(_document.TextBuffer);
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
            var newTree = _parser.Parse(tsInput, reset ? null : _oldTree);
            gcHandle.Free();
#if DEBUG
            Console.WriteLine($"SyntaxParser.Parse: 耗时{Stopwatch.GetElapsedTime(ts).TotalMilliseconds}ms");
#endif

            //获取变动范围
            if (_oldTree != null && !reset)
            {
                uint rangeCount = 0;
                var rangesPtr = TreeSitterApi.ts_tree_get_changed_ranges(_oldTree.Handle,
                    newTree.Handle, ref rangeCount);

                _oldTree!.Dispose();

                _startLineOfChanged = (int)_edit.startPosition.row; //设为当前行
                _endLineOfChanged = _startLineOfChanged + 1;
                for (var i = 0; i < rangeCount; i++)
                {
                    var startLine = (int)rangesPtr[i].StartPosition.row;
                    var endLine = (int)rangesPtr[i].EndPosition.row;
                    _startLineOfChanged = Math.Min(_startLineOfChanged, startLine);
                    _endLineOfChanged = Math.Max(_endLineOfChanged, endLine);
                    // Console.WriteLine(
                    //     $"{rangesPtr[i]} {rangesPtr[i].StartIndex}-{rangesPtr[i].EndIndex}");
                }

                TSLanguage.ts_util_free(new IntPtr(rangesPtr));
            }

            _oldTree = newTree;

            //生成FoldMarkers
            var foldMarkers = Language.GenerateFoldMarkers(_document);
            _document.FoldingManager.UpdateFoldings(foldMarkers);
#endif
        }

        internal void Tokenize(int startLine, int endLine)
        {
#if DEBUG
            var ts = Stopwatch.GetTimestamp();
#endif
            for (var i = startLine; i < endLine; i++)
            {
                TokenizeLine(i);
            }

#if DEBUG
            Console.WriteLine(
                $"SyntaxParser.Tokenize [{startLine}-{endLine}] 耗时: {Stopwatch.GetElapsedTime(ts).TotalMilliseconds}ms");
#endif
        }

        internal void TokenizeLine(int line)
        {
            var lineSegment = _document.GetLineSegment(line);
            var lineLength = lineSegment.Length;
            if (lineLength == 0) return;

            var lineStartPoint = new TSPoint(line, 0);
            var lineEndPoint = new TSPoint(line, lineLength * ParserEncoding);
            var lineNode = _oldTree!.Root.NamedDescendantForPosition(lineStartPoint, lineEndPoint);
            // Console.WriteLine(lineNode);

            lineSegment.BeginTokenize();

            if (ContainsFullLine(lineNode!, lineSegment))
            {
                VisitNode(lineNode!, lineSegment);
            }
            else
            {
                //TODO:
                lineSegment.AddToken(TokenType.Unknown, lineSegment.Offset, lineSegment.Length);
            }

            lineSegment.EndTokenize();

            //CodeToken.DumpLineTokens(lineSegment, _document);
        }

        private void VisitChildren(TSSyntaxNode node, int count, LineSegment lineSegment)
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

        private void VisitNode(TSSyntaxNode node, LineSegment lineSegment)
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

        private static bool ContainsFullLine(TSSyntaxNode node, LineSegment lineSegment)
        {
            var nodeStartOffset = node.StartIndex / ParserEncoding;
            var nodeEndOffset = node.EndIndex / ParserEncoding;

            return nodeStartOffset <= lineSegment.Offset &&
                   (lineSegment.Offset + lineSegment.Length) <= nodeEndOffset;
        }

        private static bool BeforeLine(TSSyntaxNode node, LineSegment lineSegment)
        {
            var nodeEndOffset = node.EndIndex / ParserEncoding;
            return nodeEndOffset < lineSegment.Offset;
        }

        private static bool AfterLine(TSSyntaxNode node, LineSegment lineSegment)
        {
            var nodeStartOffset = node.StartIndex / ParserEncoding;
            return nodeStartOffset > (lineSegment.Offset + lineSegment.Length);
        }

        #endregion

        internal TSQuery CreateQuery(string scm) => _parser.Language.Query(scm)!;

        internal DirtyLines GetDirtyLines(CodeEditorController controller)
        {
            return new DirtyLines(controller)
            {
                StartLine = _startLineOfChanged,
                EndLine = _endLineOfChanged
            };
        }

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
}