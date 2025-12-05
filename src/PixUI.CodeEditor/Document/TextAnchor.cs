using System;

namespace CodeEditor;

public enum AnchorMovementType
{
    /// <summary>
    /// When text is inserted at the anchor position, the type of the insertion
    /// determines where the caret moves to. For normal insertions, the anchor will move
    /// after the inserted text.
    /// </summary>
    Default,

    /// <summary>
    /// Behaves like a start marker - when text is inserted at the anchor position, the anchor will stay
    /// before the inserted text.
    /// </summary>
    BeforeInsertion,

    /// <summary>
    /// Behave like an end marker - when text is inserted at the anchor position, the anchor will move
    /// after the inserted text.
    /// </summary>
    AfterInsertion
}

/// <summary>
/// An anchor that can be put into a document and moves around when the document is changed.
/// </summary>
public sealed class TextAnchor
{
    private static Exception AnchorDeletedError() =>
        new InvalidOperationException("The text containing the anchor was deleted");

    internal TextAnchor(LineSegment lineSegment, int columnNumber)
    {
        _lineSegment = lineSegment;
        _columnNumber = columnNumber;
    }

    private LineSegment? _lineSegment;
    private int _columnNumber;

    public LineSegment Line
    {
        get => _lineSegment ?? throw AnchorDeletedError();
        internal set => _lineSegment = value;
    }

    public bool IsDeleted => _lineSegment == null;

    public int LineNumber => Line.LineNumber;

    public int ColumnNumber
    {
        get => _lineSegment == null ? throw AnchorDeletedError() : _columnNumber;
        internal set => _columnNumber = value;
    }

    public TextLocation Location => new(ColumnNumber, LineNumber);

    public int Offset => Line.Offset + _columnNumber;

    /// <summary>
    /// Controls how the anchor moves.
    /// </summary>
    public AnchorMovementType MovementType { get; set; }

    public event Action? Deleted;

    internal void Delete(ref DeferredEventList deferredEventList)
    {
        // we cannot fire an event here because this method is called while the LineManager adjusts the
        // lineCollection, so an event handler could see inconsistent state
        _lineSegment = null;
        deferredEventList.AddDeletedAnchor(this);
    }

    internal void RaiseDeleted() => Deleted?.Invoke();

    public override string ToString()
        => IsDeleted ? "[TextAnchor (deleted)]" : $"[TextAnchor {Location}]";
}