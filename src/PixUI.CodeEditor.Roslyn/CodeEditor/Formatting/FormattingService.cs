using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;

namespace PixUI.CodeEditor.Roslyn;

public static class FormattingService
{
    static FormattingService()
    {
        _options = new CSharpSyntaxFormattingOptions()
        {
            LineFormatting = new LineFormattingOptions()
            {
                NewLine = "\n",
                UseTabs = false,
                TabSize = 4,
                IndentationSize = 4,
            },
        };
    }

    private static readonly CSharpSyntaxFormattingOptions _options;

    public static async void FormatDocument(TextEditor editor, Microsoft.CodeAnalysis.Document document)
    {
        try
        {
            await FormatDocumentAsync(editor, document);
        }
        catch (Exception e)
        {
            Notification.Error("FormatDocument error: {e.Message}");
        }
    }
    
    public static async Task FormatDocumentAsync(TextEditor editor, Microsoft.CodeAnalysis.Document document)
    {
        var newDoc = await Formatter.FormatAsync(document, _options, CancellationToken.None).ConfigureAwait(false);
        var changes = (await newDoc.GetTextChangesAsync(document).ConfigureAwait(false))
            .OrderByDescending(c => c.Span);
        
        //TODO:暂逐个处理，另UpdateCaretPosition至有效位置
        editor.Document.StartUndoGroup();
        foreach (var change in changes)
        {
            editor.Document.Replace(change.Span.Start, change.Span.Length, change.NewText ?? string.Empty);
        }

        editor.Document.EndUndoGroup();
    }
}