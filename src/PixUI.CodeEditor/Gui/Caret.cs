using System;
using PixUI;

namespace CodeEditor
{
    public enum CaretMode
    {
        InsertMode,
        OverwriteMode
    }

    public sealed class Caret
    {
        internal Caret(TextEditor editor)
        {
            _textEditor = editor;
        }

        private readonly TextEditor _textEditor;
        private int _line = 0;
        private int _column = 0;

        private float _caretPosX = 0;
        private float _caretPosY = 0;
        
        internal event Action? PositionChanged;

        // private TextLocation _currentPos = new TextLocation(-1, -1);

        // /// <summary>
        // /// The 'prefered' xPos in which the caret moves, when it is moved
        // /// up/down. Measured in pixels, not in characters!
        // /// </summary>
        // private float _desiredXPos = 0;
        
        public int Line
        {
            get => _line;
            // internal set
            // {
            //     if (_line != value)
            //     {
            //         _line = value;
            //         ValidateCaretPos();
            //         UpdateCaretPosition();
            //         OnPositionChanged();
            //     }
            // }
        }

        public int Column
        {
            get => _column;
            // internal set
            // {
            //     if (_column != value)
            //     {
            //         _column = value;
            //         ValidateCaretPos();
            //         UpdateCaretPosition();
            //         OnPositionChanged();
            //     }
            // }
        }
        
        internal TextLocation Position
        {
            get => new TextLocation(_column, _line);
            set
            {
                if (_line != value.Line || _column != value.Column)
                {
                    _line = value.Line;
                    _column = value.Column;
                    UpdateCaretPosition();
                    OnPositionChanged();
                }
            }
        }

        internal int Offset => _textEditor.Document.PositionToOffset(Position);
        
        internal CaretMode Mode { get; set; } = CaretMode.InsertMode;

        private float CaretWidth => 2;
        private float CaretHeight => _textEditor.TextView.FontHeight;

        /// <summary>
        /// 相对于编辑器(非TextView)的位置(像素)
        /// </summary>
        internal float CanvasPosX =>
            _textEditor.TextView.Bounds.Left + _caretPosX - _textEditor.VirtualTop.X - 0.5f /*offset*/;

        /// <summary>
        /// 相对于编辑器(非TextView)的位置(像素)
        /// </summary>
        internal float CanvasPosY => _textEditor.TextView.Bounds.Top + _caretPosY - _textEditor.VirtualTop.Y;

        /// <summary>
        /// 光标在TextView内是否可见
        /// </summary>
        internal bool IsVisibleInCanvas
        {
            get
            {
                var textViewBounds = _textEditor.TextView.Bounds;
                var cx = CanvasPosX;
                var cy = CanvasPosY;
                if (cx + CaretWidth <= textViewBounds.Left || cx >= textViewBounds.Right) return false;
                if (cy + CaretHeight <= textViewBounds.Top || cy >= textViewBounds.Bottom) return false;
                return true;
            }
        }

        internal TextLocation ValidatePosition(TextLocation pos)
        {
            var line = Math.Max(0, Math.Min(_textEditor.Document.TotalNumberOfLines - 1, pos.Line));
            var column = Math.Max(0, pos.Column);

            if (column >= TextLocation.MaxColumn ||
                !_textEditor.Document.TextEditorOptions.AllowCaretBeyondEOL)
            {
                var lineSegment = _textEditor.Document.GetLineSegment(line);
                column = Math.Min(column, lineSegment.Length);
            }

            return new TextLocation(column, line);
        }

        private void ValidateCaretPos()
        {
            _line = Math.Max(0, Math.Min(_textEditor.Document.TotalNumberOfLines - 1, _line));
            _column = Math.Max(0, _column);

            if (_column >= TextLocation.MaxColumn ||
                !_textEditor.Document.TextEditorOptions.AllowCaretBeyondEOL)
            {
                var lineSegment = _textEditor.Document.GetLineSegment(_line);
                _column = Math.Min(_column, lineSegment.Length);
            }
        }

        /// <summary>
        /// 更新光标在画布上的绘制坐标
        /// </summary>
        internal void UpdateCaretPosition()
        {
            // _oldLine = _line;

            ValidateCaretPos();

            _caretPosX = _textEditor.TextView.GetDrawingXPos(_line, _column) + _textEditor.VirtualTop.X;
            _caretPosY = _textEditor.Document.GetVisibleLine(_line) * _textEditor.TextView.FontHeight;

            // Console.WriteLine(
            //     "UpdateCaretPosition: line=$_line col=$_column offset=$Offset x=$_caretPosX y=$_caretPosY");
        }

        private void OnPositionChanged()
        {
            PositionChanged?.Invoke();
            //如果光标超出画布可见范围则滚动至可见
            if (!IsVisibleInCanvas)
                _textEditor.ScrollToCaret();
        }

        internal void Paint(Canvas canvas)
        {
            var textViewBounds = _textEditor.TextView.Bounds;

            // draw caret
            var cx = CanvasPosX;
            var cy = CanvasPosY;
            if (IsVisibleInCanvas)
            {
                var paint = PaintUtils.Shared(_textEditor.Theme.CaretColor);
                canvas.DrawRect(Rect.FromLTWH(cx, cy, CaretWidth, CaretHeight), paint);
            }

            // draw highlight background
            var bgPaint = PaintUtils.Shared(_textEditor.Theme.LineHighlightColor);
            canvas.DrawRect(Rect.FromLTWH(textViewBounds.Left, cy, textViewBounds.Width, CaretHeight), bgPaint);
        }
    }
}