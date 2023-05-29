using System;
using System.Collections.Generic;

namespace CodeEditor
{
    internal sealed class CSharpLanguage : ICodeLanguage
    {
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

        private static readonly Dictionary<string, TokenType> TokenMap = new()
        {
            { ";", TokenType.PunctuationDelimiter },
            { ".", TokenType.PunctuationDelimiter },
            { ",", TokenType.PunctuationDelimiter },

            { "--", TokenType.Operator },
            { "-", TokenType.Operator },
            { "-=", TokenType.Operator },
            { "&", TokenType.Operator },
            { "&&", TokenType.Operator },
            { "+", TokenType.Operator },
            { "++", TokenType.Operator },
            { "+=", TokenType.Operator },
            { "<", TokenType.Operator },
            { "<<", TokenType.Operator },
            { "=", TokenType.Operator },
            { "==", TokenType.Operator },
            { "!", TokenType.Operator },
            { "!=", TokenType.Operator },
            { "=>", TokenType.Operator },
            { ">", TokenType.Operator },
            { ">>", TokenType.Operator },
            { "|", TokenType.Operator },
            { "||", TokenType.Operator },
            { "?", TokenType.Operator },
            { "??", TokenType.Operator },
            { "^", TokenType.Operator },
            { "~", TokenType.Operator },
            { "*", TokenType.Operator },
            { "/", TokenType.Operator },
            { "%", TokenType.Operator },
            { ":", TokenType.Operator },

            { "(", TokenType.PunctuationBracket },
            { ")", TokenType.PunctuationBracket },
            { "[", TokenType.PunctuationBracket },
            { "]", TokenType.PunctuationBracket },
            { "{", TokenType.PunctuationBracket },
            { "}", TokenType.PunctuationBracket },

            { "as", TokenType.Keyword },
            { "base", TokenType.Keyword },
            { "break", TokenType.Keyword },
            { "case", TokenType.Keyword },
            { "catch", TokenType.Keyword },
            { "checked", TokenType.Keyword },
            { "class", TokenType.Keyword },
            { "continue", TokenType.Keyword },
            { "default", TokenType.Keyword },
            { "delegate", TokenType.Keyword },
            { "do", TokenType.Keyword },
            { "else", TokenType.Keyword },
            { "enum", TokenType.Keyword },
            { "event", TokenType.Keyword },
            { "explicit", TokenType.Keyword },
            { "finally", TokenType.Keyword },
            { "for", TokenType.Keyword },
            { "foreach", TokenType.Keyword },
            { "goto", TokenType.Keyword },
            { "if", TokenType.Keyword },
            { "implicit", TokenType.Keyword },
            { "interface", TokenType.Keyword },
            { "is", TokenType.Keyword },
            { "lock", TokenType.Keyword },
            { "namespace", TokenType.Keyword },
            { "operator", TokenType.Keyword },
            { "params", TokenType.Keyword },
            { "return", TokenType.Keyword },
            { "sizeof", TokenType.Keyword },
            { "stackalloc", TokenType.Keyword },
            { "struct", TokenType.Keyword },
            { "switch", TokenType.Keyword },
            { "throw", TokenType.Keyword },
            { "try", TokenType.Keyword },
            { "typeof", TokenType.Keyword },
            { "unchecked", TokenType.Keyword },
            { "using", TokenType.Keyword },
            { "while", TokenType.Keyword },
            { "new", TokenType.Keyword },
            { "await", TokenType.Keyword },
            { "in", TokenType.Keyword },
            { "yield", TokenType.Keyword },
            { "get", TokenType.Keyword },
            { "set", TokenType.Keyword },
            { "when", TokenType.Keyword },
            { "out", TokenType.Keyword },
            { "ref", TokenType.Keyword },
            { "from", TokenType.Keyword },
            { "where", TokenType.Keyword },
            { "select", TokenType.Keyword },
            { "record", TokenType.Keyword },
            { "init", TokenType.Keyword },
            { "with", TokenType.Keyword },
            { "let", TokenType.Keyword },
            { "var", TokenType.Keyword },
            { "this", TokenType.Keyword },
        };

        public bool IsLeafNode(TSSyntaxNode node)
        {
            var type = node.Type;
            return type == "modifier" || type == "string_literal" || type == "character_literal";
        }

        public TokenType GetTokenType(TSSyntaxNode node)
        {
            var type = node.Type;
            if (type == "Error")
                return TokenType.Unknown;

            if (!node.IsNamed())
            {
                if (TokenMap.TryGetValue(type, out var res))
                    return res;
                return TokenType.Unknown;
            }

            // is named node
            switch (type)
            {
                case "identifier":
                    return GetIdentifierTokenType(node);

                case "implicit_type":
                case "pointer_type":
                case "function_pointer_type":
                case "predefined_type":
                    return TokenType.BuiltinType;

                case "real_literal":
                case "integer_literal":
                    return TokenType.LiteralNumber;

                case "string_literal":
                case "character_literal":
                    return TokenType.LiteralString;

                case "null_literal":
                case "boolean_literal":
                    return TokenType.Constant;

                case "modifier":
                case "void_keyword":
                    return TokenType.Keyword;

                case "comment":
                    return TokenType.Comment;
                default:
                    return TokenType.Unknown;
                //throw new NotImplementedException(node.Type);
            }
        }

        private static TokenType GetIdentifierTokenType(TSSyntaxNode node)
        {
            var parentType = node.Parent!.Type;
            if (parentType == "Error")
                return TokenType.Unknown;

            switch (parentType)
            {
                case "namespace_declaration":
                case "using_directive":
                    return TokenType.Module;

                case "class_declaration":
                case "interface_declaration":
                case "enum_declaration":
                case "struct_declaration":
                case "record_declaration":
                case "object_creation_expression":
                case "constructor_declaration":
                case "generic_name":
                case "array_type":
                case "base_list":
                    return TokenType.Type;

                case "argument":
                case "variable_declarator":
                case "property_declaration":
                    return TokenType.Variable;

                case "method_declaration":
                    return TokenType.Function;

                case "qualified_name":
                    return GetIdentifierTypeFromQualifiedName(node);
                case "member_access_expression":
                    return GetIdentifierTypeFromMemberAccess(node);

                default:
                    return TokenType.Unknown;
            }
        }

        private static TokenType GetIdentifierTypeFromQualifiedName(TSSyntaxNode node)
        {
            if (node.Parent!.Parent?.Type == "qualified_name")
                return TokenType.Module;
            if (node.Parent!.Parent?.Type == "assignment_expression")
                return TokenType.Variable; //TODO:是否静态类型的成员

            return node.NextNamedSibling == null ? TokenType.Type : TokenType.Module;
        }

        /// <summary>
        /// Get identifier token type from MemberAccess
        /// </summary>
        /// <param name="node">MemberAccessNode, eg: "some.identifier"</param>
        private static TokenType GetIdentifierTypeFromMemberAccess(TSSyntaxNode node)
        {
            if (node.Parent!.Parent!.Type == "invocation_expression")
                return TokenType.Function;
            //TODO:查找上下文变量列表
            return node.NextNamedSibling == null ? TokenType.Variable : TokenType.Type;
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

        private TSQuery? _foldQuery;

        public List<FoldMarker>? GenerateFoldMarkers(Document document)
        {
            var syntaxParser = document.SyntaxParser;
            var rootNode = syntaxParser.RootNode;
            if (rootNode == null) return null;

            _foldQuery ??= syntaxParser.CreateQuery(FoldQuery);
            var captures = _foldQuery.Captures(rootNode);
#if __WEB__
            var lastNodeId = 0;
#else
            var lastNodeId = IntPtr.Zero;
#endif
            var result = new List<FoldMarker>(captures.Length);
            foreach (var capture in captures)
            {
                if (lastNodeId == capture.node.id) continue;
                lastNodeId = capture.node.id;

#if __WEB__
                var node = capture.node;
#else
                var node = TSSyntaxNode.Create(capture.node)!;
#endif

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
}