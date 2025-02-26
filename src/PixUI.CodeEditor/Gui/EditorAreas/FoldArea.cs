using System;
using System.Collections.Generic;
using PixUI;

namespace CodeEditor;

internal sealed class FoldArea : EditorArea
{
    public FoldArea(TextEditor textEditor) : base(textEditor) { }

    // private int _selectedFoldLine = -1;

    private static Paint GetNormalPaint()
    {
        return PixUI.Paint.Shared(new Color(200, 200, 200, 255), PaintStyle.Stroke, 1f);
    }

    private static Paint GetSelectedPaint()
    {
        return PixUI.Paint.Shared(new Color(200, 200, 200, 255), PaintStyle.Stroke, 1.5f);
    }

    private bool SelectedFoldingFrom(IList<FoldingSegment> list)
    {
        // foreach (var fm in list)
        // {
        //     if (_selectedFoldLine == fm.StartLine) return true;
        // }

        return false;
    }

    internal override Size Size => new Size(TextEditor.TextView.FontHeight, -1);

    internal override bool IsVisible => TextEditor.Document.TextEditorOptions.EnableFolding;

    internal override void HandlePointerDown(float x, float y, PointerButtons buttons)
    {
        var physicalLine = (int)((y + TextEditor.VirtualTop.Y) / TextEditor.TextView.FontHeight);
        var realLine = Document.GetFirstLogicalLine(physicalLine);
        if (realLine < 0 || realLine + 1 >= Document.TotalNumberOfLines)
            return;

        var line = Document.GetLineSegment(realLine);
        var changed = false;
        // find foldings start at this line
        //var foldings = Document.FoldingManager.GetFoldingsWithStart(realLine);
        var fs = Document.FoldingManager.FindFirstWithStartAfter(line.Offset);
        while (fs != null)
        {
            if (fs.StartOffset >= line.Offset + line.TotalLength)
                break;
            fs.IsFolded = !fs.IsFolded;
            changed = true;
            fs = Document.FoldingManager.GetNextFolding(fs);
        }

        if (!changed)
            return;

        // clear line cached paragraph
        line.ClearCachedParagraph();
        // update the caret position
        TextEditor.Caret.UpdateCaretPosition();
        // notify folding changed
        Document.FoldingManager.RaiseFoldingsChanged();
        // TODO:重绘范围
        TextEditor.Controller.Widget.RequestInvalidate(true, null);
    }

    internal override void Paint(Canvas canvas, Rect rect)
    {
        if (rect.Width <= 0 || rect.Height <= 0) return;

        //background
        var paint = PixUI.Paint.Shared(TextEditor.Theme.TextBgColor);
        canvas.DrawRect(rect, paint);

        var firstLineNum = TextEditor.TextView.FirstVisibleLine;
        var lastLineNum = Document.GetFirstLogicalLine(firstLineNum + TextEditor.TextView.VisibleLineCount);
        lastLineNum = Math.Min(lastLineNum, Document.TotalNumberOfLines - 1);
        var firstLine = Document.GetLineSegment(firstLineNum);
        var lastLine = Document.GetLineSegment(lastLineNum);
        var viewStartOffset = firstLine.Offset;
        var viewEndOffset = lastLine.Offset + lastLine.Length;

        var foldings = Document.FoldingManager.FindOverlapping(viewStartOffset, viewEndOffset - viewStartOffset);
        if (foldings.Count == 0)
            return;
        
        

        Console.WriteLine($"视图范围: {firstLineNum} -- {lastLineNum} ");

        // var fontHeight = TextEditor.TextView.FontHeight;
        // var visibleLineRemainder = TextEditor.TextView.VisibleLineDrawingRemainder;
        // var maxHeight = (int)((Bounds.Height + visibleLineRemainder) / fontHeight + 1);
        // for (var y = 0; y < maxHeight; ++y)
        // {
        //     var markerRect = Rect.FromLTWH(
        //         Bounds.Left,
        //         Bounds.Top + y * fontHeight - visibleLineRemainder,
        //         Bounds.Width,
        //         fontHeight);
        //     if (rect.IntersectsWith(markerRect.Left, markerRect.Top, markerRect.Width, markerRect.Height))
        //     {
        //         //TODO: paint separator line?
        //         // canvas.drawLine(ui.Offset(drawingPosition.left, markerRect.top),
        //         //     ui.Offset(drawingPosition.left, markerRect.bottom), normalPaint);
        //
        //         var currentLine = Document.GetFirstLogicalLine(TextEditor.TextView.FirstPhysicalLine + y);
        //         if (currentLine < Document.TotalNumberOfLines)
        //         {
        //             // PaintFoldMarker(canvas, currentLine, markerRect);
        //         }
        //     }
        // }
    }

    // private void PaintFoldMarker(Canvas canvas, int lineNumber, Rect rect)
    // {
    //     var foldingManager = Document.FoldingManager;
    //     //TODO: 优化一次循环
    //     var foldingsWithStart = foldingManager.GetFoldingsWithStart(lineNumber);
    //     var foldingsBetween = foldingManager.GetFoldingsContainsLineNumber(lineNumber);
    //     var foldingsWithEnd = foldingManager.GetFoldingsWithEnd(lineNumber);
    //
    //     var isFoldStart = foldingsWithStart.Count > 0;
    //     var isBetween = foldingsBetween.Count > 0;
    //     var isFoldEnd = foldingsWithEnd.Count > 0;
    //
    //     var isStartSelected = SelectedFoldingFrom(foldingsWithStart);
    //     var isBetweenSelected = SelectedFoldingFrom(foldingsBetween);
    //     var isEndSelected = SelectedFoldingFrom(foldingsWithEnd);
    //
    //     var foldMarkerSize = TextEditor.TextView.FontHeight * 0.57f;
    //     foldMarkerSize -= foldMarkerSize % 2;
    //     var foldMarkerYPos = rect.Top + (rect.Height - foldMarkerSize) / 2;
    //     var xPos = rect.Left + (rect.Width - foldMarkerSize) / 2 + foldMarkerSize / 2;
    //
    //     if (isFoldStart)
    //     {
    //         var isVisible = true;
    //         var moreLinedOpenFold = false;
    //         foreach (var fm in foldingsWithStart)
    //         {
    //             if (fm.IsFolded)
    //                 isVisible = false;
    //             else
    //                 moreLinedOpenFold = fm.EndLine > fm.StartLine;
    //         }
    //
    //         var isFoldEndFromUpperFold = false;
    //         foreach (var fm in foldingsWithEnd)
    //         {
    //             if (fm.EndLine > fm.StartLine && !fm.IsFolded)
    //                 isFoldEndFromUpperFold = true;
    //         }
    //
    //         PaintMarker(canvas,
    //             Rect.FromLTWH(rect.Left + (rect.Width - foldMarkerSize) / 2f,
    //                 foldMarkerYPos, foldMarkerSize, foldMarkerSize),
    //             isVisible, isStartSelected);
    //
    //         // paint line above fold marker
    //         if (isBetween || isFoldEndFromUpperFold)
    //         {
    //             canvas.DrawLine(xPos, rect.Top, xPos, foldMarkerYPos - 1,
    //                 isBetweenSelected ? GetSelectedPaint() : GetNormalPaint());
    //         }
    //
    //         // paint line below fold marker
    //         if (isBetween || moreLinedOpenFold)
    //         {
    //             canvas.DrawLine(
    //                 xPos, foldMarkerYPos + foldMarkerSize + 1,
    //                 xPos, rect.Bottom,
    //                 isEndSelected || (isStartSelected && isVisible) || isBetweenSelected
    //                     ? GetSelectedPaint()
    //                     : GetNormalPaint());
    //         }
    //     }
    //     else
    //     {
    //         if (isFoldEnd)
    //         {
    //             var midY = rect.Top + rect.Height / 2;
    //
    //             // paint fold end marker
    //             canvas.DrawLine(
    //                 xPos, midY,
    //                 xPos + foldMarkerSize / 2, midY,
    //                 isEndSelected ? GetSelectedPaint() : GetNormalPaint());
    //
    //             // paint line above fold end marker
    //             // must be drawn after fold marker because it might have a different color than the fold marker
    //             canvas.DrawLine(xPos, rect.Top, xPos, midY,
    //                 isBetweenSelected || isEndSelected ? GetSelectedPaint() : GetNormalPaint());
    //
    //             // paint line below fold end marker
    //             if (isBetween)
    //             {
    //                 canvas.DrawLine(xPos, midY + 1, xPos, rect.Bottom,
    //                     isBetweenSelected ? GetSelectedPaint() : GetNormalPaint());
    //             }
    //         }
    //         else if (isBetween)
    //         {
    //             // just paint the line
    //             canvas.DrawLine(xPos, rect.Top, xPos, rect.Bottom,
    //                 isBetweenSelected ? GetSelectedPaint() : GetNormalPaint());
    //         }
    //     }
    // }

    private static void PaintMarker(Canvas canvas, Rect rect, bool isOpened, bool isSelected)
    {
        canvas.DrawRect(Rect.FromLTWH(rect.Left, rect.Top, rect.Width, rect.Height),
            isSelected ? GetSelectedPaint() : GetNormalPaint());

        var space = rect.Height / 8 + 1;
        var mid = rect.Height / 2 + rect.Height % 2;

        // draw minus
        canvas.DrawLine(rect.Left + space, rect.Top + mid, rect.Left + rect.Width - space,
            rect.Top + mid, GetNormalPaint());

        // draw plus
        if (!isOpened)
        {
            canvas.DrawLine(rect.Left + mid, rect.Top + space, rect.Left + mid,
                rect.Top + rect.Height - space, GetNormalPaint());
        }
    }
}