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

public readonly struct CodeToken
{
    private readonly int _value;

    public CodeToken(TokenType type, int startColumn)
    {
        _value = ((int)type << 24) | startColumn;
    }

    public TokenType Type => (TokenType)(_value >> 24);
    public int StartColumn => _value & 0xFFFFFF;

    public override string ToString() => $"[{Type}, {StartColumn}]";
}