using Microsoft.CodeAnalysis;

namespace PixUI.CodeEditor.Roslyn;

internal static class SyntaxExtensions
{
    internal static object GetUnderlyingNode(this ref readonly SyntaxTrivia syntaxTrivia)
        => UnsafeAccessors.SyntaxTriviaGetUnderlyingNode(in syntaxTrivia);

    internal static object GetUnderlyingNode(this ref readonly SyntaxNodeOrToken nodeOrToken)
        => UnsafeAccessors.SyntaxNodeOrTokenGetUnderlyingNode(in nodeOrToken);

    internal static int GetPosition(this SyntaxNodeOrToken nodeOrToken)
        => UnsafeAccessors.SyntaxNodeOrTokenGetPosition(in nodeOrToken);

    internal static int GetEndPosition(this ref readonly SyntaxNodeOrToken nodeOrToken)
        => UnsafeAccessors.SyntaxNodeOrTokenGetEndPosition(in nodeOrToken);
}