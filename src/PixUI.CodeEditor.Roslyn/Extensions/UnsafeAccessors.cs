using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace PixUI.CodeEditor.Roslyn;

internal static class UnsafeAccessors
{
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_ProviderName")]
    internal static extern string? CompletionItemGetProviderName(
        Microsoft.CodeAnalysis.Completion.CompletionItem completionItem);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_oldSpan")]
    internal static extern ref TextSpan SyntaxDifferGetOldSpan(Microsoft.CodeAnalysis.SyntaxDiffer differ);
}