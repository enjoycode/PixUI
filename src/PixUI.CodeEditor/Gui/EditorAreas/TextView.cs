using System;
using System.Linq;
using PixUI;

namespace CodeEditor;

internal sealed class TextView : EditorArea //TODO: rename to TextArea
{
    public TextView(TextEditor textEditor) : base(textEditor)
    {
        //TODO: 从Theme取Font设置 and Web use FontCollection.DefaultFontFamily
        var typeface = OperatingSystem.IsBrowser()
            ? FontCollection.Instance.FindTypeface(FontCollection.DefaultFamilyName, false, false)
            : FontCollection.Instance.DefaultFallback('X', null, false, false);
        var defaultFont = new Font(typeface, Theme.FontSize);

        FontHeight = textEditor.Theme.FontSize + textEditor.Theme.LineSpace * 2;
        SpaceWidth = defaultFont.MeasureString(" ").Width;
        WideSpaceWidth = Math.Max(SpaceWidth, defaultFont.MeasureString("x").Width);
        //TODO: change theme font reset cached value
    }

    internal float SpaceWidth { get; private set; }

    internal float WideSpaceWidth { get; private set; }

    internal float FontHeight { get; } //TODO: rename to LineHeight

    internal int VisibleLineCount => 1 + (int)Math.Round(Bounds.Height / FontHeight);

    internal int VisibleColumnCount => (int)(Bounds.Width / WideSpaceWidth) - 1;

    internal int VisibleLineDrawingRemainder => (int)Math.Round(TextEditor.VirtualTop.Y % FontHeight);

    /// <summary>
    /// Gets the first visible <b>logical</b> line.
    /// </summary>
    internal int FirstVisibleLine
    {
        get => Document.GetFirstLogicalLine((int)(TextEditor.VirtualTop.Y / FontHeight));
        set
        {
            if (FirstVisibleLine != value)
            {
                TextEditor.VirtualTop = new Point(TextEditor.VirtualTop.X,
                    Document.GetVisibleLine(value) * FontHeight);
            }
        }
    }

    internal int FirstPhysicalLine => (int)(TextEditor.VirtualTop.Y / FontHeight);

    internal int LineHeightRemainder => (int)(TextEditor.VirtualTop.Y % FontHeight);

    internal TextLocation GetLogicalPosition(float visualPosX, float visualPosY)
        => GetLogicalColumn(GetLogicalLine(visualPosY), visualPosX).Location;

    internal LogicalColumnInfo GetLogicalColumn(int lineNumber, float visualPosX)
    {
        visualPosX += TextEditor.VirtualTop.X;
        if (lineNumber >= Document.TotalNumberOfLines)
        {
            return new LogicalColumnInfo(new TextLocation((int)(visualPosX / SpaceWidth), lineNumber), null);
        }

        if (visualPosX <= 0)
        {
            return new LogicalColumnInfo(new TextLocation(0, lineNumber), null);
        }

        var line = Document.GetLineSegment(lineNumber);
        FoldingSegment? inFoldMarker = null;
        var para = line.GetLineParagraph(TextEditor);
        var columnInLine = para.GetGlyphPositionAtCoordinate(visualPosX, 1).Position;
        var column = columnInLine;
        // if has folded, eg: if (xxx) {...} else {...}
        if (line.CachedFolds != null && column > line.CachedFolds[0].LineStart)
        {
            foreach (var fold in line.CachedFolds)
            {
                if (columnInLine < fold.LineStart) break;

                var fsEndLocation = fold.FoldingSegment.GetEndLocation(Document);
                if (columnInLine >= fold.LineStart && columnInLine < fold.LineEnd)
                {
                    //in fold, TODO: nearest left or right
                    inFoldMarker = fold.FoldingSegment;
                    lineNumber = fsEndLocation.Line;
                    column = fsEndLocation.Column;
                    break;
                }
                else if (columnInLine >= fold.LineEnd)
                {
                    lineNumber = fsEndLocation.Line;
                    column = fsEndLocation.Column + (columnInLine - fold.LineEnd);
                }
            }
        }

        return new LogicalColumnInfo(new TextLocation(column, lineNumber), inFoldMarker);
    }

    /// <summary>
    /// returns logical line number for a visual point
    /// </summary>
    internal int GetLogicalLine(float visualPosY)
    {
        var clickedVisualLine = Math.Max(0, (int)((visualPosY + TextEditor.VirtualTop.Y) / FontHeight));
        return Document.GetFirstLogicalLine(clickedVisualLine);
    }

    /// <summary>
    /// 获取指定行列相对于TextView的X位置(像素)
    /// </summary>
    /// <returns>负值或大于TextView.Width表示超出可见范围(包括滚动偏移量 )</returns>
    internal float GetDrawingXPos(int logicalLine, int logicalColumn)
    {
        var foldings = Document.FoldingManager.GetTopLevelFoldedFoldings().ToArray();

        // search the folding that's interesting
        var foldedLineNumber = -1;
        for (var i = foldings.Length - 1; i >= 0; i--)
        {
            var fs = foldings[i];
            var fsStartLine = Document.GetLineNumberByOffset(fs.StartOffset);
            var fsEndLine = Document.GetLineNumberByOffset(fs.EndOffset);
            if (foldedLineNumber >= 0)
            {
                // has found fold in pre iterator
                if (fsEndLine == foldedLineNumber)
                    foldedLineNumber = fsStartLine;
                else
                    break;
            }
            else if (fsStartLine == logicalLine || fsEndLine == logicalLine)
            {
                foldedLineNumber = fsStartLine;
            }
        }

        var visualLine = foldedLineNumber < 0
            ? Document.GetLineSegment(logicalLine)
            : Document.GetLineSegment(foldedLineNumber);
        var drawingPos = visualLine.GetXPos(TextEditor, logicalLine, logicalColumn);
        return drawingPos - TextEditor.VirtualTop.X;
    }

    internal override void HandlePointerDown(float x, float y, PointerButtons buttons)
    {
        var vx = x - Bounds.Left;
        var vy = y - Bounds.Top;
        if (buttons == PointerButtons.Left)
        {
            //左键按下清除选择并设置新的光标位置
            var logicalLine = GetLogicalLine(vy);
            var logicalColumn = GetLogicalColumn(logicalLine, vx);

            //Console.WriteLine($"Click at TextView: {logicalColumn.Location}");

            TextEditor.SelectionManager.ClearSelection();
            TextEditor.Caret.Position = logicalColumn.Location;
        }
        else if (buttons == PointerButtons.Right)
        {
            //右键按下开始显示ContextMenu
            var contextMenuBuilder = TextEditor.Controller.ContextMenuBuilder;
            if (contextMenuBuilder != null)
            {
                var contextMenus = contextMenuBuilder(TextEditor);
                if (contextMenus.Length > 0)
                    ContextMenu.Show(contextMenus);
            }
        }
    }

    internal override void Paint(Canvas canvas, Rect rect)
    {
        if (rect.Width <= 0 || rect.Height <= 0) return;

        var horizontalDelta = TextEditor.VirtualTop.X;
        if (horizontalDelta > 0)
        {
            canvas.Save();
            canvas.ClipRect(Bounds, ClipOp.Intersect, false);
        }

        // paint background
        var paint = PixUI.Paint.Shared(Theme.TextBgColor);
        canvas.DrawRect(rect, paint);

        // paint lines one by one
        var maxLines = (int)((Bounds.Height + VisibleLineDrawingRemainder) / FontHeight + 1);
        PaintLines(canvas, maxLines);

        if (horizontalDelta > 0)
            canvas.Restore();
    }

    private void PaintLines(Canvas canvas, int maxLines)
    {
        var horizontalDelta = TextEditor.VirtualTop.X;
        for (var y = 0; y < maxLines; y++)
        {
            var lineRect = Rect.FromLTWH(
                Bounds.Left - horizontalDelta,
                Bounds.Top + y * FontHeight - VisibleLineDrawingRemainder,
                Bounds.Width + horizontalDelta,
                FontHeight);
            //TODO: check lineRect overlaps with dirty area.

            var currentLine = Document.GetFirstLogicalLine(Document.GetVisibleLine(FirstVisibleLine) + y);
            if (currentLine >= Document.TotalNumberOfLines) return;
            var lineSegment = Document.GetLineSegment(currentLine);
            if (lineSegment.Length == 0) continue;

            var lineParagraph = lineSegment.GetLineParagraph(TextEditor);
            canvas.DrawParagraph(lineParagraph, lineRect.Left, lineRect.Top + Theme.LineSpace);
        }
    }
}

internal readonly struct LogicalColumnInfo
{
    internal readonly TextLocation Location;
    internal readonly FoldingSegment? InFoldMarker;

    internal LogicalColumnInfo(TextLocation location, FoldingSegment? inFoldMarker)
    {
        Location = location;
        InFoldMarker = inFoldMarker;
    }
}