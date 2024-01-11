using System.Collections.Generic;

namespace CodeEditor;

/// <summary>
/// A list of events that are fired after the line manager has finished working.
/// </summary>
internal sealed class DeferredEventList
{
    internal List<LineSegment> removedLines;
    internal List<TextAnchor>? textAnchor;

    public void AddRemovedLine(LineSegment line)
    {
        removedLines ??= new List<LineSegment>();
        removedLines.Add(line);
    }

    public void AddDeletedAnchor(TextAnchor anchor)
    {
        textAnchor ??= new List<TextAnchor>();
        textAnchor.Add(anchor);
    }

    public void RaiseEvents()
    {
        // removedLines is raised by the LineManager
        if (textAnchor == null) return;
        foreach (var a in textAnchor)
        {
            a.RaiseDeleted();
        }
    }
}