using System.Collections.Generic;

namespace CodeEditor;

/// <summary>
/// A list of events that are fired after the line manager has finished working.
/// </summary>
internal struct DeferredEventList
{
    private List<LineSegment>? _removedLines;
    private List<TextAnchor>? _textAnchors;

    public void AddRemovedLine(LineSegment line)
    {
        _removedLines ??= new List<LineSegment>();
        _removedLines.Add(line);
    }

    public void AddDeletedAnchor(TextAnchor anchor)
    {
        _textAnchors ??= new List<TextAnchor>();
        _textAnchors.Add(anchor);
    }

    public void RaiseEvents(LineManager lineManager)
    {
        if (_removedLines != null)
        {
            foreach (var line in _removedLines)
                lineManager.OnLineDeleted(line);
        }

        if (_textAnchors != null)
        {
            foreach (var anchor in _textAnchors)
            {
                anchor.RaiseDeleted();
            }
        }
    }
}