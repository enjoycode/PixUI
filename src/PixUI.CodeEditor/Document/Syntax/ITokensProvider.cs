namespace CodeEditor;

public interface ITokensProvider
{
    void Tokenize(Document document, int startLine, int endLine);
}