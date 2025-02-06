using System;
using System.Threading.Tasks;

namespace CodeEditor;

public interface ISyntaxParser : IDisposable
{
    bool HasSyntaxError { get; }

    Document Document { get; set; }

    void BeginEdit(int offset, int length, int textLength);

    void EndEdit(int offset, int length, int textLength);

    char? GetAutoClosingPairs(char ch);

    bool IsBlockStartBracket(char ch);

    bool IsBlockEndBracket(char ch);

    ValueTask<ValueTuple<int,int>> ParseAndTokenize();
}