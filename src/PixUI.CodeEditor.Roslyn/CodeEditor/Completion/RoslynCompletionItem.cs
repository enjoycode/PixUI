using Microsoft.CodeAnalysis.Completion;

namespace PixUI.CodeEditor.Roslyn;

public sealed class RoslynCompletionItem : ICompletionItem
{
    public required CompletionItem RawCompletionItem { get; init; }

    string ICompletionItem.InsertText => Label; //TODO: remove

    /// <summary>
    /// The label of this completion item. By default
    /// also the text that is inserted when selecting
    /// this completion.
    /// </summary>
    public string Label { get; set; } = null!;

    /// <summary>
    /// The kind of this completion item. Based of the kind
    /// an icon is chosen by the editor.The standardized set
    /// of available values is defined in <see cref="CompletionItemKind"/>
    /// </summary>
    public CompletionItemKind Kind { get; set; }

    /// <summary>
    /// Tags for this completion item
    /// </summary>
    public IReadOnlyList<CompletionItemTag>? Tags { get; set; }

    /// <summary>
    /// A human-readable string with additional information
    /// about this item, like type or symbol information
    /// </summary>
    public string? Detail { get; set; }

    /// <summary>
    /// A human-readable string that represents a doc-comment. This is
    /// formatted as markdown.
    /// </summary>
    public string? Documentation { get; set; }

    /// <summary>
    /// Select this item when showing.
    /// </summary>
    public bool Preselect { get; set; }

    /// <summary>
    /// A string that should be used when comparing this item
    /// with other items. When null or empty the label is used.
    /// </summary>
    public string? SortText { get; set; }

    /// <summary>
    /// A string that should be used when filtering a set of
    /// completion items. When null or empty the label is used.
    /// </summary>
    public string? FilterText { get; set; }

    /// <summary>
    /// The format of <see cref="InsertText"/>. This applies to both <see cref="InsertText"/> and
    /// <see cref="TextEdit"/>.<see cref="LinePositionSpanTextChange.NewText"/>.
    /// </summary>
    public InsertTextFormat InsertTextFormat { get; set; }

    /// <summary>
    /// An edit which is applied to a document when selecting this completion. When an edit is provided the value of
    /// <see cref="InsertText"/> is ignored.
    /// </summary>
    public LinePositionSpanTextChange? TextEdit { get; set; }

    /// <summary>
    /// An optional set of characters that when pressed while this completion is active will accept it first and
    /// then type that character.
    /// </summary>
    public IReadOnlyList<char>? CommitCharacters { get; set; }

    /// <summary>
    /// An optional array of additional text edits that are applied when
    /// selecting this completion.Edits must not overlap (including the same insert position)
    /// with the main edit nor with themselves.
    ///
    /// Additional text edits should be used to change text unrelated to the current cursor position
    /// (for example adding an import statement at the top of the file if the completion item will
    /// insert an unqualified type).
    /// </summary>
    public IReadOnlyList<LinePositionSpanTextChange>? AdditionalTextEdits { get; set; }

    /// <summary>
    /// True if there is a post-insert step for this completion item for asynchronous completion support.
    /// </summary>
    public bool HasAfterInsertStep { get; set; }

    public override string ToString()
    {
        return $"{{ {nameof(Label)} = {Label}, {nameof(CompletionItemKind)} = {Kind} }}";
    }
}