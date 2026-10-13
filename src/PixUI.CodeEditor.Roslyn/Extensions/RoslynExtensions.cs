using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace PixUI.CodeEditor.Roslyn;

internal static class RoslynExtensions
{
    internal static object GetUnderlyingNode(this ref readonly SyntaxTrivia syntaxTrivia)
        => UnsafeAccessors.SyntaxTriviaGetUnderlyingNode(in syntaxTrivia);

    internal static object GetUnderlyingNode(this ref readonly SyntaxNodeOrToken nodeOrToken)
        => UnsafeAccessors.SyntaxNodeOrTokenGetUnderlyingNode(in nodeOrToken);

    internal static int GetPosition(this SyntaxNodeOrToken nodeOrToken)
        => UnsafeAccessors.SyntaxNodeOrTokenGetPosition(in nodeOrToken);

    internal static int GetEndPosition(this ref readonly SyntaxNodeOrToken nodeOrToken)
        => UnsafeAccessors.SyntaxNodeOrTokenGetEndPosition(in nodeOrToken);

    internal static string? GetProviderName(this Microsoft.CodeAnalysis.Completion.CompletionItem completionItem)
        => UnsafeAccessors.CompletionItemGetProviderName(completionItem);

    /// <summary>
    /// Converts a zero-based position to TextLocation.
    /// </summary>
    public static TextLocation GetPointFromPosition(this SourceText text, int position)
    {
        var line = text.Lines.GetLineFromPosition(position);
        return new TextLocation(position - line.Start, line.LineNumber);
    }
    
    /// <summary>
    /// Converts a line number and offset to a zero-based position within a <see cref="SourceText"/>.
    /// </summary>
    public static int GetPositionFromLineAndOffset(this SourceText text, int lineNumber, int offset)
        => text.Lines[lineNumber].Start + offset;
}