using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace PixUI.CodeEditor.Roslyn;

internal static class UnsafeAccessors
{
    private const string GreenNodeType = "Microsoft.CodeAnalysis.GreenNode, Microsoft.CodeAnalysis";
    
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_UnderlyingNode")]
    [return: UnsafeAccessorType(GreenNodeType)]
    internal static extern object SyntaxTriviaGetUnderlyingNode(ref readonly SyntaxTrivia syntaxTrivia);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_UnderlyingNode")]
    [return: UnsafeAccessorType(GreenNodeType)]
    internal static extern object SyntaxNodeOrTokenGetUnderlyingNode(ref readonly SyntaxNodeOrToken nodeOrToken);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_Position")]
    internal static extern int SyntaxNodeOrTokenGetPosition(ref readonly SyntaxNodeOrToken nodeOrToken);
    
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_EndPosition")]
    internal static extern int SyntaxNodeOrTokenGetEndPosition(ref readonly SyntaxNodeOrToken nodeOrToken);
}