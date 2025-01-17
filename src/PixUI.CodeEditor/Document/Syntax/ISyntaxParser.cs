using System;

namespace CodeEditor;

public interface ISyntaxParser : IDisposable
{
    bool HasSyntaxError { get; }

    Document Document { get; set; }

    void BeginInsert(int offset, int length);

    void EndInsert(int offset, int length);

    void BeginRemove(int offset, int length);

    void EndRemove();

    void BeginReplace(int offset, int length, int textLength);

    void EndReplace(int offset, int length, int textLength);

    char? GetAutoClosingPairs(char ch);

    bool IsBlockStartBracket(char ch);

    bool IsBlockEndBracket(char ch);

    void Parse(bool reset);

    /// <summary>
    /// Tokenize lines range [startLine, endLine)
    /// </summary>
    void Tokenize(int startLine, int endLine);
}