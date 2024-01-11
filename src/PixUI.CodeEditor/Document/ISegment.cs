namespace CodeEditor;

/// <summary>
/// This interface is used to describe a span inside a text sequence
/// </summary>
public interface ISegment
{
    /// <value>
    /// The offset where the span begins
    /// </value>
    int Offset { get; set; }

    /// <value>
    /// The length of the span
    /// </value>
    int Length { get; set; }
}