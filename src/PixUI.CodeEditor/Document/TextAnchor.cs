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
    static Exception AnchorDeletedError()
    {
        return new InvalidOperationException("The text containing the anchor was deleted");
    }

    internal TextAnchor(LineSegment lineSegment, int columnNumber)
    {
        this.lineSegment = lineSegment;
        this.columnNumber = columnNumber;
    }

    private LineSegment lineSegment;
    private int columnNumber;

    public LineSegment Line
    {
        get
        {
            if (lineSegment == null) throw AnchorDeletedError();
            return lineSegment;
        }
        internal set { lineSegment = value; }
    }

    public bool IsDeleted => lineSegment == null;

    public int LineNumber => Line.LineNumber;

    public int ColumnNumber
    {
        get
        {
            if (lineSegment == null) throw AnchorDeletedError();
            return columnNumber;
        }
        internal set { columnNumber = value; }
    }

    public TextLocation Location
    {
        get { return new TextLocation(this.ColumnNumber, this.LineNumber); }
    }

    public int Offset
    {
        get { return Line.Offset + columnNumber; }
    }

    /// <summary>
    /// Controls how the anchor moves.
    /// </summary>
    public AnchorMovementType MovementType { get; set; }

    public event Action? Deleted;

    internal void Delete(ref DeferredEventList deferredEventList)
    {
        // we cannot fire an event here because this method is called while the LineManager adjusts the
        // lineCollection, so an event handler could see inconsistent state
        lineSegment = null;
        deferredEventList.AddDeletedAnchor(this);
    }

    internal void RaiseDeleted() => Deleted?.Invoke();

    public override string ToString()
        => IsDeleted ? "[TextAnchor (deleted)]" : $"[TextAnchor {Location}]";
}