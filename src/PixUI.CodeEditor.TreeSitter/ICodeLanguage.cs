namespace CodeEditor;

public interface ICodeLanguage : ITokensProvider, IFoldingProvider
{
    char? GetAutoClosingPairs(char ch);

    bool IsBlockStartBracket(char ch);
    bool IsBlockEndBracket(char ch);
}