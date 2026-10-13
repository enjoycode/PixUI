using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Text;

namespace PixUI.CodeEditor.Roslyn;

public sealed class RoslynCompletionProvider : ICompletionProvider
{
    public RoslynCompletionProvider(RoslynWorkspace workspace)
    {
        _workspace = workspace;
    }

    private readonly RoslynWorkspace _workspace;
    public IEnumerable<char> TriggerCharacters { get; } = ['.'];

    public async Task<IList<ICompletionItem>?> ProvideCompletionItems(Document document, int offset,
        string? completionWord)
    {
        var docId = (DocumentId)document.Tag!;
        var doc = _workspace.CurrentSolution.GetDocument(docId);
        if (doc is null)
            return Array.Empty<ICompletionItem>();

        var sourceText = await doc.GetTextAsync();

        var completionService = CompletionService.GetService(doc);
        Debug.Assert(completionService != null, "Can't get CompletionService");

        // Check should trigger
        var trigger = !string.IsNullOrEmpty(completionWord)
            ? CompletionTrigger.CreateInsertionTrigger(completionWord[0])
            : CompletionTrigger.Invoke;
        //TODO: CompletionOptions {
        // ShowItemsFromUnimportedNamespaces = EnableImportCompletion
        // ForceExpandedCompletionIndexCreation = forceExpandedCompletionIndexCreation
        // }
        if (trigger.Kind ==
            CompletionTriggerKind.Insertion /*request.CompletionTrigger == CompletionTriggerKind.TriggerCharacter*/)
        {
            if (!completionService.ShouldTriggerCompletion(sourceText, offset, trigger, roles: null, null /*TODO*/))
                return Array.Empty<ICompletionItem>();
        }

        var completions = await completionService.GetCompletionsAsync(doc, offset);
        if (completions.ItemsList.Count == 0)
            return Array.Empty<ICompletionItem>();

        // if (request.TriggerCharacter == ' ' && !completions.ItemsList.Any(c =>
        //     {
        //         var providerName = c.GetProviderName();
        //         return providerName is CompletionListBuilder.OverrideCompletionProvider or
        //             CompletionListBuilder.PartialMethodCompletionProvider or
        //             CompletionListBuilder.ObjectCreationCompletionProvider;
        //     }))
        // {
        //     // Only trigger on space if there is an object creation completion
        //     return new CompletionResponse { Items = ImmutableArray<CompletionItem>.Empty };
        // }

        var typedSpan = completionService.GetDefaultCompletionListSpan(sourceText, offset);
        // string typedText = sourceText.GetSubText(typedSpan).ToString();

        // If we don't encounter any unimported types, and the completion context thinks that some would be available, then
        // that completion provider is still creating the cache. We'll mark this completion list as not completed, and the
        // editor will ask again when the user types more. By then, hopefully the cache will have populated and we can mark
        // the completion as done.
        bool expectingImportedItems = false; //options.ShowItemsFromUnimportedNamespaces;
        var isSuggestionMode = completions.SuggestionModeItem is not null;

        var (completionsList, seenUnimportedCompletions) = await CompletionListBuilder.BuildCompletionItemsAsync(
            doc,
            sourceText,
            // cacheId,
            offset,
            completionService,
            completions,
            typedSpan,
            expectingImportedItems,
            isSuggestionMode);

        //var isIncomplete = !seenUnimportedCompletions && expectingImportedItems;
        return completionsList;
    }
    
    // private static CompletionList FilterCompletionList(CompletionList completionList, TextSpan filterSpan, 
    //     CompletionTrigger completionTrigger, CompletionFilterReason filterReason, SourceText sourceText)
    // {
    //     var filterText = sourceText.GetSubText(filterSpan).ToString();
    //     Microsoft.CodeAnalysis.PooledObjects.ArrayBuilder;
    //
    //     // Use pattern matching to determine which items are most relevant out of the calculated items.
    //     using var _ = ArrayBuilder<MatchResult>.GetInstance(out var matchResultsBuilder);
    //     var index = 0;
    //     using var helper = new PatternMatchHelper(filterText);
    //     foreach (var item in completionList.ItemsList)
    //     {
    //         if (helper.TryCreateMatchResult(
    //                 item,
    //                 completionTrigger.Kind,
    //                 filterReason,
    //                 recentItemIndex: -1,
    //                 includeMatchSpans: true,
    //                 index,
    //                 out var matchResult))
    //         {
    //             matchResultsBuilder.Add(matchResult);
    //             index++;
    //         }
    //     }
    //
    //     // Next, we sort the list based on the pattern matching result.
    //     matchResultsBuilder.Sort(MatchResult.SortingComparer);
    //
    //     var filteredList = matchResultsBuilder.SelectAsArray(matchResult =>
    //         new SharpIdeCompletionItem(matchResult.CompletionItem, matchResult.PatternMatch?.MatchedSpans));
    //
    //     return filteredList;
    // }
}