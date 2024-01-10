using System;

namespace CodeEditor;

public enum TokenType
{
    Unknown,
    WhiteSpace,
    Error,

    Module,
    Type,
    BuiltinType,

    LiteralNumber,
    LiteralString,
    Constant,

    Keyword,
    Comment,
    PunctuationDelimiter,
    PunctuationBracket,
    Operator,
    Variable,
    Function,
}

internal static class CodeToken
{
    internal static int Make(TokenType type, int startColumn)
    {
        return ((int)type << 24) | startColumn;
    }

    internal static int GetTokenStartColumn(int token) => token & 0xFFFFFF;

    internal static TokenType GetTokenType(int token) => (TokenType)(token >> 24);
        
}