using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Tags;
using Microsoft.CodeAnalysis.Text;

namespace PixUI.CodeEditor.Roslyn;

internal static class CompletionListBuilder
{
    #region ====CompletionItemKind====

    private static readonly Dictionary<string, CompletionItemKind> s_roslynTagToCompletionItemKind = new()
    {
        { WellKnownTags.Public, CompletionItemKind.Keyword },
        { WellKnownTags.Protected, CompletionItemKind.Keyword },
        { WellKnownTags.Private, CompletionItemKind.Keyword },
        { WellKnownTags.Internal, CompletionItemKind.Keyword },
        { WellKnownTags.File, CompletionItemKind.File },
        { WellKnownTags.Project, CompletionItemKind.File },
        { WellKnownTags.Folder, CompletionItemKind.Folder },
        { WellKnownTags.Assembly, CompletionItemKind.File },
        { WellKnownTags.Class, CompletionItemKind.Class },
        { WellKnownTags.Constant, CompletionItemKind.Constant },
        { WellKnownTags.Delegate, CompletionItemKind.Function },
        { WellKnownTags.Enum, CompletionItemKind.Enum },
        { WellKnownTags.EnumMember, CompletionItemKind.EnumMember },
        { WellKnownTags.Event, CompletionItemKind.Event },
        { WellKnownTags.ExtensionMethod, CompletionItemKind.Method },
        { WellKnownTags.Field, CompletionItemKind.Field },
        { WellKnownTags.Interface, CompletionItemKind.Interface },
        { WellKnownTags.Intrinsic, CompletionItemKind.Text },
        { WellKnownTags.Keyword, CompletionItemKind.Keyword },
        { WellKnownTags.Label, CompletionItemKind.Text },
        { WellKnownTags.Local, CompletionItemKind.Variable },
        { WellKnownTags.Namespace, CompletionItemKind.Module },
        { WellKnownTags.Method, CompletionItemKind.Method },
        { WellKnownTags.Module, CompletionItemKind.Module },
        { WellKnownTags.Operator, CompletionItemKind.Operator },
        { WellKnownTags.Parameter, CompletionItemKind.Variable },
        { WellKnownTags.Property, CompletionItemKind.Property },
        { WellKnownTags.RangeVariable, CompletionItemKind.Variable },
        { WellKnownTags.Reference, CompletionItemKind.Reference },
        { WellKnownTags.Structure, CompletionItemKind.Struct },
        { WellKnownTags.TypeParameter, CompletionItemKind.TypeParameter },
        { WellKnownTags.Snippet, CompletionItemKind.Snippet },
        { WellKnownTags.Error, CompletionItemKind.Text },
        { WellKnownTags.Warning, CompletionItemKind.Text },
    };

    #endregion

    #region ====ProviderNames====

    internal const string AggregateEmbeddedLanguageCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.Providers.AggregateEmbeddedLanguageCompletionProvider";

    internal const string AwaitCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.Providers.AwaitCompletionProvider";

    internal const string ExtensionMemberImportCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.Providers.ExtensionMemberImportCompletionProvider";

    internal const string InternalsVisibleToCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.Providers.InternalsVisibleToCompletionProvider";

    internal const string ObjectCreationCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.Providers.ObjectCreationCompletionProvider";

    internal const string OverrideCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.Providers.OverrideCompletionProvider";

    internal const string PartialMethodCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.Providers.PartialMethodCompletionProvider";

    internal const string SnippetCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.CompletionProviders.Snippets.CSharpSnippetCompletionProvider";

    internal const string TypeImportCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.Providers.TypeImportCompletionProvider";

    internal const string XmlDocCommentCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.Providers.XmlDocCommentCompletionProvider";

    internal const string UnnamedSymbolCompletionProvider =
        "Microsoft.CodeAnalysis.CSharp.Completion.Providers.UnnamedSymbolCompletionProvider";

    #endregion

    internal static async Task<(IList<ICompletionItem>, bool)> BuildCompletionItemsAsync(
        Microsoft.CodeAnalysis.Document document,
        SourceText sourceText,
        int position,
        CompletionService completionService,
        ImmutableArray<CompletionItem> completions,
        TextSpan typedSpan,
        bool expectingImportedItems, bool isSuggestionMode)
    {
        var completionsBuilder = new List<ICompletionItem>(completions.Length);
        var seenUnimportedCompletions = false;
        var commitCharacterRuleCache =
            new Dictionary<ImmutableArray<CharacterSetModificationRule>, IReadOnlyList<char>>();
        var commitCharacterRuleBuilder = new HashSet<char>();

        foreach (var completion in completions)
        {
            string labelText = completion.DisplayTextPrefix + completion.DisplayText + completion.DisplayTextSuffix;
            string? insertText;
            string? filterText = null;
            List<LinePositionSpanTextChange>? additionalTextEdits = null;
            InsertTextFormat insertTextFormat = InsertTextFormat.PlainText;
            TextSpan changeSpan;
            string? sortText;
            bool hasAfterInsertStep = false;
            if (completion.IsComplexTextEdit)
            {
                var providerName = completion.GetProviderName();
                // TODO: Add support for snippet items and UnnamedSymbolCompletionProvider
                if (providerName == SnippetCompletionProvider || providerName == UnnamedSymbolCompletionProvider)
                    continue;

                // The completion is somehow expensive. Currently, this one of two categories:
                // import completion or override/partial completion.
                // Debug.Assert(providerName is OverrideCompletionProvider
                //     or PartialMethodCompletionProvider
                //     or TypeImportCompletionProvider or ExtensionMemberImportCompletionProvider
                //     or AwaitCompletionProvider);

                changeSpan = typedSpan;

                switch (providerName)
                {
                    case OverrideCompletionProvider or PartialMethodCompletionProvider or AwaitCompletionProvider:
                        // For override and partial completion, we don't want to use the DisplayText as the insert text
                        // because they contain characters that will affect our ability to asynchronously resolve the
                        // change later.
                        insertText = completion.FilterText;
                        sortText = GetSortText(completion, labelText, expectingImportedItems);
                        hasAfterInsertStep = true;
                        break;

                    default: // case TypeImportCompletionProvider or ExtensionMemberImportCompletionProvider:
                        insertText = completion.DisplayText;
                        sortText = '1' + completion.SortText;
                        seenUnimportedCompletions = true;
                        break;
                }
            }
            else
            {
                // For non-complex completions, just await the text edit. It's cheap enough that
                // it doesn't impact our ability to pop completions quickly

                // If the completion item is the misc project name, skip it.
                // if (completion.DisplayText == Configuration.MiscProjectName) continue;

                GetCompletionInfo(
                    sourceText,
                    position,
                    completion,
                    await completionService.GetChangeAsync(document, completion),
                    typedSpan,
                    labelText,
                    expectingImportedItems,
                    out insertText, out filterText, out sortText, out insertTextFormat, out changeSpan,
                    out additionalTextEdits);
            }

            var treatAsASuggestion = isSuggestionMode || ShouldTreatCompletionItemAsSuggestion(completion, typedSpan);
            var commitCharacters = BuildCommitCharacters(completion.Rules.CommitCharacterRules, treatAsASuggestion,
                commitCharacterRuleCache, commitCharacterRuleBuilder);

            completionsBuilder.Add(new RoslynCompletionItem
            {
                RawCompletionItem = completion,
                Label = labelText,
                TextEdit = GetChangeForTextAndSpan(insertText!, changeSpan, sourceText),
                InsertTextFormat = insertTextFormat,
                AdditionalTextEdits = additionalTextEdits,
                SortText = sortText,
                FilterText = filterText,
                Kind = GetCompletionItemKind(completion.Tags),
                Detail = completion.InlineDescription,
                //Data = (cacheId, i),
                Preselect = completion.Rules.MatchPriority == MatchPriority.Preselect,
                CommitCharacters = commitCharacters,
                HasAfterInsertStep = hasAfterInsertStep,
            });
        }

        return (completionsBuilder, seenUnimportedCompletions);
    }

    private static void GetCompletionInfo(
        SourceText sourceText,
        int position,
        Microsoft.CodeAnalysis.Completion.CompletionItem completion,
        CompletionChange change,
        TextSpan typedSpan,
        string labelText,
        bool expectingImportedItems,
        out string? insertText,
        out string? filterText,
        out string? sortText,
        out InsertTextFormat insertTextFormat,
        out TextSpan changeSpan,
        out List<LinePositionSpanTextChange>? additionalTextEdits)
    {
        insertTextFormat = InsertTextFormat.PlainText;
        changeSpan = typedSpan;
        insertText = null;
        filterText = null;
        sortText = null;
        additionalTextEdits = null;

        // Roslyn will give us the position to move the cursor after the completion is entered.
        // However, this is in the _new_ document, after changes have been applied. In order to
        // snippetize the insertion text, we need to calculate the offset as we move along the
        // edits, subtracting or adding the difference for edits that do not insersect the current
        // span.
        var adjustedNewPosition = change!.NewPosition;

        var cursorPoint = sourceText.GetPointFromPosition(position);
        var lineStartPosition = sourceText.GetPositionFromLineAndOffset(cursorPoint.Line, offset: 0);

        // There must be at least one change that affects the current location, or something is seriously wrong
        Debug.Assert(change.TextChanges.Any(change => change.Span.IntersectsWith(position)));

        foreach (var textChange in change.TextChanges)
        {
            if (!textChange.Span.IntersectsWith(position))
            {
                HandleNonInsertsectingEdit(sourceText, ref additionalTextEdits, ref adjustedNewPosition, textChange);
            }
            else
            {
                // Either there should be no new position, or it should be within the text that is being added
                // by this change.
                int changeSpanStart = textChange.Span.Start;
                Debug.Assert(adjustedNewPosition is null ||
                             (adjustedNewPosition.Value <= changeSpanStart + textChange.NewText!.Length) &&
                             (adjustedNewPosition.Value >= changeSpanStart));

                // Filtering needs a range that is a _single_ line. Consider a case like this (whitespace documented with escapes):
                //
                // 1: class C
                // 2: {\t\r\n
                // 3:    override $$
                //
                // Roslyn will see the trailing \t on line 2 and remove it when creating the _main_ text change. However, that will
                // break filtering because filtering expects a single line as part of the range. So what we want to do is break the
                // the text change up into two: one to cover the previous line, as an additional edit, and then one to cover the
                // rest of the change.

                var updatedChange = textChange;

                if (changeSpanStart < lineStartPosition)
                {
                    // We know we're in the special case. In order to correctly determine the amount of leading newlines to trim, we want
                    // to calculate the number of lines before the cursor we're editing
                    var editStartPoint = sourceText.GetPointFromPosition(changeSpanStart);
                    var numLinesEdited = cursorPoint.Line - editStartPoint.Line;

                    Debug.Assert(textChange.NewText != null);

                    // Now count that many newlines forward in the edited text
                    int cutoffPosition = 0;
                    for (int numNewlinesFound = 0; numNewlinesFound < numLinesEdited; cutoffPosition++)
                    {
                        if (textChange.NewText![cutoffPosition] == '\n')
                        {
                            numNewlinesFound++;
                        }
                    }

                    // Now that we've found the cuttoff, we can build our two subchanges
                    var prefixChange =
                        new TextChange(new TextSpan(changeSpanStart, length: lineStartPosition - changeSpanStart),
                            textChange.NewText!.Substring(0, cutoffPosition));
                    HandleNonInsertsectingEdit(sourceText, ref additionalTextEdits, ref adjustedNewPosition,
                        prefixChange);
                    updatedChange =
                        new TextChange(new TextSpan(lineStartPosition, length: textChange.Span.End - lineStartPosition),
                            textChange.NewText.Substring(cutoffPosition));
                }

                changeSpan = updatedChange.Span;
                // When inserting at the beginning or middle of a word, we want to only replace characters
                // up until the caret position, but not after.  For example when typing at the beginning of a word
                // we only want to insert the completion before the rest of the word.
                // However, Roslyn returns the entire word as the span to replace, so we have to adjust it.
                if (position < changeSpan.End)
                {
                    changeSpan = new(changeSpan.Start, length: position - changeSpan.Start);
                }

                (insertText, insertTextFormat) = GetPossiblySnippetizedInsertText(updatedChange, adjustedNewPosition);

                // If we're expecting there to be unimported types, put in an explicit sort text to put things already in scope first.
                // Otherwise, omit the sort text if it's the same as the label to save on space.
                sortText = GetSortText(completion, labelText, expectingImportedItems);

                // If the completion is replacing a bigger range than the previously-typed word, we need to have the filter
                // text compensate. Clients will use the range of the text edit to determine the thing that is being filtered
                // against. For example, override completion:
                //
                //    override $$
                //    |--------| Range that is being changed by the completion
                //
                // That means vscode will consider "override <additional user input>" when looking to see whether the item
                // still matches. To compensate, we add the start of the replacing range, up to the start of the current word,
                // to ensure the item isn't silently filtered out.

                if (changeSpan != typedSpan)
                {
                    if (typedSpan.Start < changeSpan.Start)
                    {
                        // This means that some part of the currently-typed text is an exact match for the start of the
                        // change, so chop off changeSpan.Start - typedSpan.Start from the filter text to get it to match
                        // up with the range
                        int prefixMatchElement = changeSpan.Start - typedSpan.Start;
                        Debug.Assert(completion.FilterText!.StartsWith(sourceText
                            .GetSubText(new TextSpan(typedSpan.Start, prefixMatchElement)).ToString()));
                        filterText = completion.FilterText.Substring(prefixMatchElement);
                    }
                    else
                    {
                        var prefix = sourceText.GetSubText(TextSpan.FromBounds(changeSpan.Start, typedSpan.Start))
                            .ToString();
                        filterText = prefix + completion.FilterText;
                    }
                }
                else
                {
                    filterText = labelText == completion.FilterText ? null : completion.FilterText;
                }
            }
        }

        return;

        static (string?, InsertTextFormat) GetPossiblySnippetizedInsertText(TextChange change, int? adjustedNewPosition)
        {
            if (adjustedNewPosition is not int newPosition || change.NewText is null ||
                newPosition == change.Span.Start + change.NewText.Length)
            {
                return (change.NewText, InsertTextFormat.PlainText);
            }

            // Roslyn wants to move the cursor somewhere inside the result. Substring from the
            // requested start to the new position, and from the new position to the end of the
            // string.
            int midpoint = newPosition - change.Span.Start;
            var beforeText = LspSnippetHelpers.Escape(change.NewText.Substring(0, midpoint));
            var afterText = LspSnippetHelpers.Escape(change.NewText.Substring(midpoint));

            return ($"{beforeText}$0{afterText}", InsertTextFormat.Snippet);
        }

        static void HandleNonInsertsectingEdit(SourceText sourceText,
            ref List<LinePositionSpanTextChange>? additionalTextEdits, ref int? adjustedNewPosition,
            TextChange textChange)
        {
            additionalTextEdits ??= new();
            additionalTextEdits.Add(TextChanges.Convert(sourceText, textChange));

            if (adjustedNewPosition is int newPosition)
            {
                // Find the diff between the original text length and the new text length.
                var diff = (textChange.NewText?.Length ?? 0) - textChange.Span.Length;

                // If the new text is longer than the replaced text, we want to subtract that
                // length from the current new position to find the adjusted position in the old
                // document. If the new text was shorter, diff will be negative, and subtracting
                // will result in increasing the adjusted position as expected
                adjustedNewPosition = newPosition - diff;
            }
        }
    }

    private static string? GetSortText(CompletionItem completion, string labelText,
        bool expectingImportedItems) => expectingImportedItems
        ? '0' + completion.SortText
        : labelText == completion.SortText
            ? null
            : completion.SortText;

    private static bool ShouldTreatCompletionItemAsSuggestion(CompletionItem item,
        TextSpan typedSpan)
    {
        // The user hasn't actually typed anything and completion provider does
        // not request the item be hard-selected.
        return item.Rules.MatchPriority != MatchPriority.Preselect &&
               typedSpan.Length == 0 &&
               item.Rules.SelectionBehavior != CompletionItemSelectionBehavior.HardSelection;
    }

    private static IReadOnlyList<char>? BuildCommitCharacters(
        ImmutableArray<CharacterSetModificationRule> characterRules, bool isSuggestionMode,
        Dictionary<ImmutableArray<CharacterSetModificationRule>, IReadOnlyList<char>> commitCharacterRulesCache,
        HashSet<char> commitCharactersBuilder)
    {
        if (isSuggestionMode)
        {
            // To emulate soft selection we should remove all trigger characters forcing the Editor to
            // fallback to only <tab> and <enter> for accepting the completions.
            return null;
        }

        if (characterRules.IsEmpty)
        {
            // Use defaults
            return CompletionRules.Default.DefaultCommitCharacters;
        }

        if (commitCharacterRulesCache.TryGetValue(characterRules, out var cachedRules))
        {
            return cachedRules;
        }

        addAllCharacters(CompletionRules.Default.DefaultCommitCharacters);

        foreach (var modifiedRule in characterRules)
        {
            switch (modifiedRule.Kind)
            {
                case CharacterSetModificationKind.Add:
                    commitCharactersBuilder.UnionWith(modifiedRule.Characters);
                    break;

                case CharacterSetModificationKind.Remove:
                    commitCharactersBuilder.ExceptWith(modifiedRule.Characters);
                    break;

                case CharacterSetModificationKind.Replace:
                    commitCharactersBuilder.Clear();
                    addAllCharacters(modifiedRule.Characters);
                    break;
            }
        }

        // VS has a more complex concept of a commit mode vs suggestion mode for intellisense.
        // LSP doesn't have this, so mock it as best we can by removing space ` ` from the list
        // of commit characters if we're in suggestion mode.
        if (isSuggestionMode)
        {
            commitCharactersBuilder.Remove(' ');
        }

        var finalCharacters = commitCharactersBuilder.ToList();
        commitCharactersBuilder.Clear();

        commitCharacterRulesCache.Add(characterRules, finalCharacters);

        return finalCharacters;

        void addAllCharacters(ImmutableArray<char> characters)
        {
            foreach (var @char in characters)
            {
                commitCharactersBuilder.Add(@char);
            }
        }
    }

    private static CompletionItemKind GetCompletionItemKind(ImmutableArray<string> tags)
    {
        foreach (var tag in tags)
        {
            if (s_roslynTagToCompletionItemKind.TryGetValue(tag, out var itemKind))
            {
                return itemKind;
            }
        }

        return CompletionItemKind.Text;
    }

    private static LinePositionSpanTextChange GetChangeForTextAndSpan(string? insertText, TextSpan changeSpan,
        SourceText sourceText)
    {
        var changeLinePositionSpan = sourceText.Lines.GetLinePositionSpan(changeSpan);
        return new()
        {
            NewText = insertText ?? "",
            StartLine = changeLinePositionSpan.Start.Line,
            StartColumn = changeLinePositionSpan.Start.Character,
            EndLine = changeLinePositionSpan.End.Line,
            EndColumn = changeLinePositionSpan.End.Character
        };
    }
}