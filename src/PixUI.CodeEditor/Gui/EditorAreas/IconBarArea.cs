using System.Collections.Generic;
using PixUI;

namespace CodeEditor;

internal sealed class IconBarArea : EditorArea
{
    public IconBarArea(TextEditor textEditor) : base(textEditor) { }

    private const int IconBarWidth = 18;

    internal override Size Size => new(IconBarWidth, -1);

    internal override bool IsVisible => Document.TextEditorOptions.EnableDebugging;

    internal override void HandlePointerDown(float x, float y, PointerButtons buttons)
    {
        var lineHeight = TextEditor.TextView.FontHeight;
        var clickedVisibleLine = (y + TextEditor.VirtualTop.Y) / lineHeight;
        var lineNumber = Document.GetFirstLogicalLine((int)clickedVisibleLine);

        // if ((buttons & PointerButtons.Right) == PointerButtons.Right)
        // {
        //     if (TextEditor.Caret.Line != lineNumber)
        //         TextEditor.Caret.Line = lineNumber;
        // }

        var bookmarkManager = Document.BookmarkManager;
        var marks = bookmarkManager.Marks;
        if (marks.Count > 0)
        {
            var marksInLine = new List<Bookmark>();
            var oldCount = marks.Count;
            foreach (var mark in marks)
            {
                if (mark.LineNumber == lineNumber)
                    marksInLine.Add(mark);
            }

            for (var i = marksInLine.Count - 1; i >= 0; i--)
            {
                var mark = marksInLine[i];
                if (mark.Click(TextEditor, new Point(x, y), buttons))
                {
                    if (oldCount != marks.Count)
                    {
                        //TODO: only invalidate area
                        TextEditor.Controller.Widget.RequestInvalidate(true, null);
                    }

                    return;
                }
            }
        }

        // no marks exists
        var newMark = bookmarkManager.CreateMark(new TextLocation(0, lineNumber));
        bookmarkManager.AddMark(newMark);
        //TODO: only invalidate area
        TextEditor.Controller.Widget.RequestInvalidate(true, null);
    }

    internal override void Paint(Canvas canvas, int[] viewLines)
    {
        if (Bounds.Width <= 0 || Bounds.Height <= 0)
            return;

        //paint background
        var paint = PixUI.Paint.Shared(Theme.LineBgColor);
        canvas.DrawRect(Bounds, paint);

        //paint icons
        foreach (var mark in Document.BookmarkManager.Marks)
        {
            var lineNumber = Document.GetVisibleLine(mark.LineNumber);
            var lineHeight = TextEditor.TextView.FontHeight;
            var yPos = (lineNumber * lineHeight) - TextEditor.VirtualTop.Y;
            if (IsLineInsideRegion(yPos, yPos + lineHeight, Bounds.Y, Bounds.Bottom))
            {
                if (lineNumber == Document.GetVisibleLine(mark.LineNumber - 1))
                {
                    // marker is inside folded region, do not draw it
                    continue;
                }

                mark.Draw(canvas, Rect.FromLTWH(Bounds.Left, yPos, IconBarWidth, lineHeight));
            }
        }
    }

    private static bool IsLineInsideRegion(float top, float bottom, float regionTop, float regionBottom)
    {
        if (top >= regionTop && top <= regionBottom)
        {
            // Region overlaps the line's top edge.
            return true;
        }

        if (regionTop > top && regionTop < bottom)
        {
            // Region's top edge inside line.
            return true;
        }

        return false;
    }
}