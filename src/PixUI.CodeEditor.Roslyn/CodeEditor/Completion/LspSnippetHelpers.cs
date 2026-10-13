using System.Text.RegularExpressions;

namespace PixUI.CodeEditor.Roslyn;

internal static partial class LspSnippetHelpers
{
    [GeneratedRegex(@"([\\\$}])", RegexOptions.Compiled)]
    private static partial Regex GenRegex();
    
    private static readonly Regex EscapeRegex = GenRegex();

    /// <summary>
    /// Escape the given string for use as an LSP snippet. This escapes '\', '$', and '}'.
    /// </summary>
    public static string? Escape(string? snippet) => snippet == null
        ? null
        : EscapeRegex.Replace(snippet, @"\$1");
    
}