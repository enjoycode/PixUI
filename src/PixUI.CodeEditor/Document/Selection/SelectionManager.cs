using System;
using System.Collections.Generic;
using System.Text;

namespace CodeEditor
{
    public sealed class SelectionManager
    {
        internal SelectionManager(TextEditor editor)
        {
            _textEditor = editor;
            SelectionCollection = new List<Selection>();
            SelectFrom = new SelectFrom();
            SelectionStart = TextLocation.Empty;
        }

        private readonly TextEditor _textEditor;
        internal readonly IList<Selection> SelectionCollection;

        internal SelectFrom SelectFrom;
        internal TextLocation SelectionStart;

        public event Action? SelectionChanged;

        internal bool HasSomethingSelected => SelectionCollection.Count > 0;

        internal bool SelectionIsReadonly => false; //TODO:

        internal string SelectedText
        {
            get
            {
                if (!HasSomethingSelected) return string.Empty;
                if (SelectionCollection.Count == 1)
                    return SelectionCollection[0].SelectedText;

#if __WEB__
                var res = "";
                foreach (var selection in SelectionCollection)
                {
                    res += selection.SelectedText;
                }

                return res;
#else
                var sb = new StringBuilder();
                foreach (var selection in SelectionCollection)
                {
                    sb.Append(selection.SelectedText);
                }

                return sb.ToString();
#endif
            }
        }

        /// <summary>
        /// Clears the selection and sets a new selection
        /// </summary>
        internal void SetSelection(TextLocation startPosition, TextLocation endPosition)
        {
            if (SelectionCollection.Count == 1 &&
                SelectionCollection[0].StartPosition == startPosition &&
                SelectionCollection[0].EndPosition == endPosition)
                return;

            SelectionCollection.Clear(); //clearWithoutUpdate();
            SelectionCollection.Add(new Selection(_textEditor.Document, startPosition,
                endPosition));
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        internal void ClearSelection()
        {
            var mousePos = _textEditor.PointerPos;

            // this is the most logical place to reset selection starting
            // positions because it is always called before a new selection
            SelectFrom.First = SelectFrom.Where;
            var newSelectionStart = _textEditor.TextView.GetLogicalPosition(
                mousePos.X - _textEditor.TextView.Bounds.Left,
                mousePos.Y - _textEditor.TextView.Bounds.Top);

            if (SelectFrom.Where == WhereFrom.Gutter)
            {
                newSelectionStart.Column = 0;
                //selectionStart.Y = -1;
            }

            if (newSelectionStart.Line >= _textEditor.Document.TotalNumberOfLines)
            {
                newSelectionStart.Line = _textEditor.Document.TotalNumberOfLines - 1;
                newSelectionStart.Column =
                    _textEditor.Document.GetLineSegment(_textEditor.Document.TotalNumberOfLines - 1)
                        .Length;
            }

            SelectionStart = newSelectionStart;
            SelectionCollection.Clear(); //clearWithoutUpdate();
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Removes the selected text from the buffer and clears the selection.
        /// </summary>
        internal void RemoveSelectedText()
        {
            if (SelectionIsReadonly)
            {
                ClearSelection();
                return;
            }

            // var lines = new List<int>();
            var oneLine = true;
            foreach (var s in SelectionCollection)
            {
                if (oneLine)
                {
                    var lineBegin = s.StartPosition.Line;
                    if (lineBegin != s.EndPosition.Line)
                        oneLine = false;
                    // else
                    //     lines.Add(lineBegin);
                }

                var offset = s.Offset;
                _textEditor.Document.Remove(offset, s.Length);
            }

            ClearSelection();
            // if (offset >= 0)
            // {
            //     //textArea.caret.offset = offset;
            // }
        }

        internal void ExtendSelection(TextLocation oldPosition, TextLocation newPosition)
        {
            // where old position is where the cursor was,
            // and new position is where it has ended up from a click (both zero based)
            if (oldPosition == newPosition) return;

            TextLocation min;
            TextLocation max;
            var oldnewX = newPosition.Column;
            var oldIsGreater = GreaterEqPos(oldPosition, newPosition);
            if (oldIsGreater)
            {
                min = newPosition;
                max = oldPosition;
            }
            else
            {
                min = oldPosition;
                max = newPosition;
            }

            if (min == max) return;

            if (!HasSomethingSelected)
            {
                SetSelection(min, max);
                // initialise selectFrom for a cursor selection
                if (SelectFrom.Where == WhereFrom.None)
                    SelectionStart = oldPosition; //textArea.Caret.Position;
                return;
            }

            var selection = SelectionCollection[0];
            // changed selection via gutter
            if (SelectFrom.Where == WhereFrom.Gutter)
            {
                // selection new position is always at the left edge for gutter selections
                newPosition.Column = 0;
            }

            if (GreaterEqPos(newPosition, SelectionStart))
            {
                // selecting forward
                selection.StartPosition = SelectionStart;
                // this handles last line selection
                if (SelectFrom.Where == WhereFrom.Gutter /*&& newPosition.Y != oldPosition.Y*/)
                {
                    selection.EndPosition =
                        new TextLocation(_textEditor.Caret.Column, _textEditor.Caret.Line);
                }
                else
                {
                    newPosition.Column = oldnewX;
                    selection.EndPosition = newPosition;
                }
            }
            else
            {
                // selecting back
                if (SelectFrom.Where == WhereFrom.Gutter && SelectFrom.First == WhereFrom.Gutter)
                {
                    // gutter selection
                    selection.EndPosition = NextValidPosition(SelectionStart.Line);
                }
                else
                {
                    // internal text selection
                    selection.EndPosition = SelectionStart; //selection.StartPosition;
                }

                selection.StartPosition = newPosition;
            }

            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// retrieve the next available line
        /// - checks that there are more lines available after the current one
        /// - if there are then the next line is returned
        /// - if there are NOT then the last position on the given line is returned
        /// </summary>
        internal TextLocation NextValidPosition(int line)
        {
            if (line < _textEditor.Document.TotalNumberOfLines - 1)
                return new TextLocation(0, line + 1);
            return new TextLocation(
                _textEditor.Document.GetLineSegment(_textEditor.Document.TotalNumberOfLines - 1)
                    .Length + 1,
                line);
        }

        internal static bool GreaterEqPos(TextLocation p1, TextLocation p2)
            => p1.Line > p2.Line || p1.Line == p2.Line && p1.Column >= p2.Column;
    }

    /// <summary>
    /// selection initiated from type
    /// </summary>
    internal enum WhereFrom
    {
        None,
        Gutter,
        TextArea
    }

    /// <summary>
    /// selection initiated from
    /// </summary>
    internal sealed class SelectFrom
    {
        internal WhereFrom Where = WhereFrom.None;
        internal WhereFrom First = WhereFrom.None;
    }
}