namespace CodeEditor;

public interface ICodeLanguage : ITokensProvider, IFoldingProvider
{
    char? GetAutoColsingPairs(char ch);

    bool IsBlockStartBracket(char ch);
    bool IsBlockEndBracket(char ch);
}