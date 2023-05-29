namespace CodeEditor
{
    public interface ITokensProvider
    {
        bool IsLeafNode(TSSyntaxNode node);

        TokenType GetTokenType(TSSyntaxNode node);
    }
}