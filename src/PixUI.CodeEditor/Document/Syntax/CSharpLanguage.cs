using System;
using System.Collections.Generic;

namespace CodeEditor;

internal sealed class CSharpLanguage : ICodeLanguage
{
    private static readonly Lazy<TSQuery> _highlightsQuery = new(() =>
    {
        const string scm = "Resources.CSharpHighlights.scm";
        return TSCSharpLanguage.Instance.Value.Query(PixUI.CodeEditor.ResourceLoad.LoadString(scm))!;
    });

    private static readonly Dictionary<uint, string> _highlightsMap = new();

    private static readonly Lazy<TSQuery> _foldQuery = new(() => TSCSharpLanguage.Instance.Value.Query(FoldQuery)!);

    public char? GetAutoColsingPairs(char ch)
    {
        switch (ch)
        {
            case '{': return '}';
            case '[': return ']';
            case '(': return ')';
            case '"': return '"';
            default: return null;
        }
    }

    #region ====ITokensProvider====

    public void Tokenize(Document document, int startLine, int endLine)
    {
        //TODO:暂简单实现，待参考tree-sitter highlight

        var syntaxParser = document.SyntaxParser;
        var rootNode = syntaxParser.RootNode;
        if (rootNode == null) return;

        const string scm = "Resources.CSharpHighlights.scm";
        var captures = _highlightsQuery.Value.Captures(rootNode,
            new(startLine, 0), new(endLine, 0));
        var line = startLine;
        var lineSegment = document.GetLineSegment(line);
        lineSegment.BeginTokenize();
        foreach (var capture in captures)
        {
            var nodeStartPos = capture.node.StartPosition;
            if (nodeStartPos.row < line) continue;
            if (nodeStartPos.row >= endLine) break;
            if (nodeStartPos.row > line)
            {
                lineSegment.EndTokenize();
                line = (int)nodeStartPos.row;
                lineSegment = document.GetLineSegment(line);
                lineSegment.BeginTokenize();
            }

            var nodeEndPos = capture.node.EndPosition;
            var tokenType = GetTokenType(capture.index);
            var tokenOffset = nodeStartPos.column / SyntaxParser.ParserEncoding;
            var tokenLength = (nodeEndPos.column - nodeStartPos.column) / SyntaxParser.ParserEncoding;
            lineSegment.AddToken(tokenType, (int)tokenOffset, (int)tokenLength);
        }

        lineSegment.EndTokenize();
    }

    private TokenType GetTokenType(uint captureIndex)
    {
        if (!_highlightsMap.TryGetValue(captureIndex, out var captureName))
        {
            captureName = _highlightsQuery.Value.CaptureNameForId(captureIndex);
            _highlightsMap[captureIndex] = captureName;
        }

        return captureName switch
        {
            "module" => TokenType.Module,
            "type" => TokenType.Type,
            "type.builtin" => TokenType.BuiltinType,
            "function" => TokenType.Function,
            "constructor" => TokenType.Type,
            "property.definition" => TokenType.Variable,
            "number" => TokenType.LiteralNumber,
            "string" => TokenType.LiteralString,
            "constant.builtin" => TokenType.Constant,
            "comment" => TokenType.Comment,
            "punctuation.delimiter" => TokenType.PunctuationDelimiter,
            "punctuation.bracket" => TokenType.PunctuationBracket,
            "operator" => TokenType.Operator,
            "keyword" => TokenType.Keyword,
            "variable" => TokenType.Variable,
            _ => TokenType.Unknown
        };
    }

    #endregion

    #region ====IFoldingProvider====

    //参考: https://github.com/nvim-treesitter/nvim-treesitter/blob/master/queries/c_sharp/folds.scm
    private const string FoldQuery = @"
body: [
  (declaration_list)
  (switch_body)
  (enum_member_declaration_list)
] @fold

accessors: [
  (accessor_list)
] @fold

initializer: [
  (initializer_expression)
] @fold

(block) @fold
";

    public List<FoldMarker>? GenerateFoldMarkers(Document document)
    {
        var syntaxParser = document.SyntaxParser;
        var rootNode = syntaxParser.RootNode;
        if (rootNode == null) return null;

        var captures = _foldQuery.Value.Captures(rootNode);

        var lastNodeId = IntPtr.Zero;
        var result = new List<FoldMarker>();
        foreach (var capture in captures)
        {
            if (lastNodeId == capture.node.id) continue;
            lastNodeId = capture.node.id;

            var node = TSSyntaxNode.Create(capture.node)!; //TODO: don't create TSSyntaxNode
            //暂跳过同一行的
            if (node.StartPosition.row == node.EndPosition.row) continue;

            var startIndex = node.StartIndex / SyntaxParser.ParserEncoding;
            var endIndex = node.EndIndex / SyntaxParser.ParserEncoding;

            var mark = new FoldMarker(document, 0, 0, 0, 0, FoldType.TypeBody, "{...}");
            mark.Offset = startIndex;
            mark.Length = endIndex - startIndex;
            result.Add(mark);
        }

        return result;
    }

    #endregion
}