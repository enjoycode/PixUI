namespace CodeEditor;

public interface ICodeLanguage : ITokensProvider, IFoldingProvider
{
    char? GetAutoColsingPairs(char ch);
}