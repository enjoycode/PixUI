using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.PooledObjects;

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

        var completions = await completionService.GetCompletionsAsync(doc, offset, trigger);
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
        string typedText = sourceText.GetSubText(typedSpan).ToString();

        //暂简单过滤排序
        var filteredAndOrdered = completions.ItemsList.ToImmutableArray();
        if (!string.IsNullOrEmpty(typedText)) filteredAndOrdered = FilterCompletionList(completions, typedText);

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
            filteredAndOrdered,
            typedSpan,
            expectingImportedItems,
            isSuggestionMode);

        //var isIncomplete = !seenUnimportedCompletions && expectingImportedItems;
        return completionsList;
    }

    private static ImmutableArray<Microsoft.CodeAnalysis.Completion.CompletionItem> FilterCompletionList(
        CompletionList completionList, string filterText)
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        using var helper = new PatternMatchHelper(filterText);
        using var _ = ArrayBuilder<MatchResult>.GetInstance(out var matchResultsBuilder);
        foreach (var item in completionList.ItemsList)
        {
            var result = helper.GetMatchResult(item, false, cultureInfo);
            if (result.ShouldBeConsideredMatchingFilterText)
                matchResultsBuilder.Add(result);
        }

        matchResultsBuilder.Sort(MatchResult.SortingComparer);
        return matchResultsBuilder.SelectAsArray(matchResult => matchResult.CompletionItem);
    }
}