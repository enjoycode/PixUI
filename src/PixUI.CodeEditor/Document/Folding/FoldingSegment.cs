namespace CodeEditor;

public sealed class FoldingSegment : TextSegment
{
    public FoldingSegment(int startOffset, int length, string foldedText = "...", bool isFolded = false)
    {
        StartOffset = startOffset;
        Length = length;
        FoldedText = foldedText;
        IsFolded = isFolded;
    }

    /// <summary>
    /// Gets/sets if the section is folded.
    /// </summary>
    public bool IsFolded { get; internal set; }

    /// <summary>
    /// Gets/Sets the text used to display the collapsed version of the folding section.
    /// </summary>
    public string FoldedText { get; internal set; }

    /// <summary>
    /// Gets/Sets an additional object associated with this folding section.
    /// </summary>
    public object? Tag { get; set; }

    internal TextLocation GetStartLocation(Document document)
    {
        var line = document.GetLineSegmentByOffset(StartOffset);
        var column = StartOffset - line.Offset;
        return new TextLocation(column, line.LineNumber);
    }

    internal TextLocation GetEndLocation(Document document)
    {
        var line = document.GetLineSegmentByOffset(EndOffset);
        var column = EndOffset - line.Offset;
        return new TextLocation(column, line.LineNumber);
    }
}