using System;
using PixUI;

namespace CodeEditor;

/// <summary>
/// 代码编辑器的总管，管理各个可视区域、光标、选择等
/// </summary>
public sealed class TextEditor
{
    internal TextEditor(CodeEditorController controller)
    {
        Controller = controller;
        Controller.Document.UndoStack.TextEditor = this;

        Caret = new Caret(this);
        SelectionManager = new SelectionManager(this);

        TextView = new TextView(this);
        LeftAreas = new EditorArea[] { new GutterArea(this), new FoldArea(this) };

        //TODO: Caret position changed to matching bracket
    }

    internal readonly CodeEditorController Controller;
    public readonly Caret Caret;
    internal readonly SelectionManager SelectionManager;

    internal TextEditorTheme Theme => Controller.Theme;
    public Document Document => Controller.Document;

    internal readonly TextView TextView;
    internal readonly EditorArea[] LeftAreas;

    private Point _virtualTop = Point.Empty;
    internal Point PointerPos = Point.Empty; //缓存位置

    #region ====Scroll Operations====

    internal Point VirtualTop
    {
        get => _virtualTop;
        set
        {
            var newVirtualTop = new Point(Math.Max(0, value.X), Math.Min(MaxVScrollValue, Math.Max(0, value.Y)));
            if (_virtualTop != newVirtualTop)
                _virtualTop = newVirtualTop;

            //TODO: updateCaretPosition
        }
    }

    internal float MaxVScrollValue =>
        (Document.GetVisibleLine(Document.TotalNumberOfLines - 1) + 1 +
         TextView.VisibleLineCount * 2f / 3) * TextView.FontHeight; //TODO:减少范围

    internal float MaxHScrollValue
    {
        get
        {
            var max = 0f;
            var firstLine = TextView.FirstVisibleLine;
            var lastLine = Document.GetFirstLogicalLine(TextView.FirstPhysicalLine + TextView.VisibleLineCount);
            if (lastLine >= Document.TotalNumberOfLines)
                lastLine = Document.TotalNumberOfLines - 1;

            for (var i = firstLine; i <= lastLine; i++)
            {
                if (!Document.FoldingManager.IsLineVisible(i)) continue;

                var lineSegment = Document.GetLineSegment(i);
                if (lineSegment.Length == 0) continue;

                var para = lineSegment.GetLineParagraph(this);
                max = Math.Max(max, para.MaxIntrinsicWidth);
            }

            return Math.Max(0, max - TextView.Bounds.Width); //TODO: 加上光标宽度或指定量
        }
    }

    public void ScrollToCaret() => ScrollTo(Caret.Line, Caret.Column); //TODO:可以简化，因Caret已缓存画布位置

    public void ScrollTo(int line, int column)
    {
        //滚动至指定行
        ScrollTo(line);

        //滚动至指定列
        var xPos = TextView.GetDrawingXPos(line, column);
        //Log.Debug($"指定列{column}的XPos={xPos} Bounds=[{TextView.Bounds.Width}]");
        var offsetX = 0f;
        if (xPos < 0)
            offsetX = xPos;
        else if (xPos > TextView.Bounds.Width)
            offsetX = xPos - TextView.Bounds.Width; //TODO:细调偏移量包括光标宽度
        if (offsetX != 0)
            Controller.OnScroll(offsetX, 0);
    }

    public void ScrollTo(int line)
    {
        line = Math.Max(0, Math.Min(Document.TotalNumberOfLines - 1, line));
        line = Document.GetVisibleLine(line);
        var curLineMin = TextView.FirstPhysicalLine;
        if (TextView.LineHeightRemainder > 0)
            curLineMin++;

        var maxVScrollValue = MaxVScrollValue;
        if (line < curLineMin)
        {
            var offsetY = Math.Max(0, Math.Min(maxVScrollValue, line * TextView.FontHeight));
            Controller.OnScroll(0, offsetY - _virtualTop.Y);
        }
        else
        {
            var curLineMax = curLineMin + TextView.VisibleLineCount;
            if (line + 2 > curLineMax)
            {
                var offsetY = TextView.VisibleLineCount == 1
                    ? Math.Max(0, Math.Min(maxVScrollValue, (line - 4) * TextView.FontHeight))
                    : Math.Min(maxVScrollValue, (line - TextView.VisibleLineCount + 2) * TextView.FontHeight);
                Controller.OnScroll(0, offsetY - _virtualTop.Y);
            }
        }
    }

    #endregion

    #region ====Text Operations====

    /// <summary>
    /// Inserts or replace text at the caret position
    /// </summary>
    internal void InsertOrReplaceString(string text, int replaceOffset = 0)
    {
        Document.UndoStack.StartUndoGroup();

        if (Document.TextEditorOptions.DocumentSelectionMode == DocumentSelectionMode.Normal &&
            SelectionManager.HasSomethingSelected)
        {
            Caret.Position = SelectionManager.SelectionCollection[0].StartPosition;
            SelectionManager.RemoveSelectedText();
        }

        var caretLine = Document.GetLineSegment(Caret.Line);
        if (caretLine.Length < Caret.Column)
        {
            var whiteSpaceLength = Caret.Column - caretLine.Length;
            text = new string(' ', whiteSpaceLength) + text;
        }

        if (replaceOffset == 0)
        {
            Document.Insert(Caret.Offset, text);
            Caret.Position = Document.OffsetToPosition(Caret.Offset + text.Length);
        }
        else
        {
            Document.Replace(Caret.Offset - replaceOffset, replaceOffset, text);
            if (replaceOffset == text.Length)
            {
                Caret.UpdateCaretPosition(); //替换后位置没有变化，需要更新光标的绘制坐标
            }
            else
            {
                Caret.Position = new TextLocation(
                    Caret.Position.Column - replaceOffset + text.Length,
                    Caret.Position.Line);
            }
        }

        Document.UndoStack.EndUndoGroup();
    }

    /// <summary>
    /// Delete selection text
    /// </summary>
    internal void DeleteSelection()
    {
        if (SelectionManager.SelectionIsReadonly) return;

        Caret.Position = SelectionManager.SelectionCollection[0].StartPosition;
        SelectionManager.RemoveSelectedText();
        //textArea.scrollToCaret();
    }

    #endregion

    #region ====Paint all Area====

    internal void Paint(Canvas canvas, Size size, IDirtyArea? dirtyArea)
    {
        //TODO: check dirtyArea
        var currentXPos = 0f;
        var currentYPos = 0f;
        //var adjustScrollBars = false;

        // paint left areas
        foreach (var area in LeftAreas)
        {
            if (!area.IsVisible) continue;

            var areaRect = Rect.FromLTWH(currentXPos, currentYPos, area.Size.Width, size.Height - currentYPos);
            if (areaRect != area.Bounds)
            {
                //adjustScrollBars = true;
                area.Bounds = areaRect;
            }

            currentXPos += area.Bounds.Width;
            area.Paint(canvas, areaRect);
        }

        // paint text area
        var textRect = Rect.FromLTWH(currentXPos, currentYPos, size.Width - currentXPos, size.Height - currentYPos);
        if (textRect != TextView.Bounds)
        {
            //adjustScrollBars = true;
            TextView.Bounds = textRect;
            //TODO: updateCaretPosition
        }

        TextView.Paint(canvas, textRect);
    }

    #endregion
}