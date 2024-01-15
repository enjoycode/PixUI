namespace CodeEditor;

public interface ITokensProvider
{
    bool IsLeafNode(in TSNode node);

    TokenType GetTokenType(in TSNode node);
}