using System;
using PixUI;

namespace CodeEditor
{
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

        internal Point VirtualTop
        {
            get => _virtualTop;
            set
            {
                var newVirtualTop = new Point(Math.Max(0, value.X),
                    Math.Min(MaxVScrollValue, Math.Max(0, value.Y)));
                if (_virtualTop != newVirtualTop)
                    _virtualTop = newVirtualTop;

                //TODO: updateCaretPosition
            }
        }

        internal float MaxVScrollValue =>
            (Document.GetVisibleLine(Document.TotalNumberOfLines - 1) + 1 +
             TextView.VisibleLineCount * 2 / 3) * TextView.FontHeight;

        internal Point PointerPos = Point.Empty; //缓存位置


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

                var areaRect = Rect.FromLTWH(currentXPos, currentYPos, area.Size.Width,
                    size.Height - currentYPos);
                if (areaRect != area.Bounds)
                {
                    //adjustScrollBars = true;
                    area.Bounds = areaRect;
                }

                currentXPos += area.Bounds.Width;
                area.Paint(canvas, areaRect);
            }

            // paint text area
            var textRect = Rect.FromLTWH(currentXPos, currentYPos,
                size.Width - currentXPos, size.Height - currentYPos);
            if (textRect != TextView.Bounds)
            {
                //adjustScrollBars = true;
                TextView.Bounds = textRect;
                //TODO: updateCaretPosition
            }

            TextView.Paint(canvas, textRect);
        }
    }
}