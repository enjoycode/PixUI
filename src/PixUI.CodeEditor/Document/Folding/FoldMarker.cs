using System;

namespace CodeEditor
{
    public enum FoldType
    {
        Unspecified,
        MemberBody,
        Region,
        TypeBody
    }

    public sealed class FoldMarker : ISegment, IComparable<FoldMarker>
    {
        public FoldMarker(Document document,
            int startLine, int startColumn,
            int endLine, int endColumn, FoldType foldType,
            string? foldText = null, bool isFolded = false)
        {
            _document = document;
            IsFolded = isFolded;
            _foldType = foldType;
            FoldText = string.IsNullOrEmpty(foldText) ? "..." : foldText;

            startLine = Math.Min(_document.TotalNumberOfLines - 1, Math.Max(startLine, 0));
            var startLineSegment = _document.GetLineSegment(startLine);

            endLine = Math.Min(document.TotalNumberOfLines - 1, Math.Max(endLine, 0));
            var endLineSegment = _document.GetLineSegment(endLine);

            _offset = startLineSegment.Offset + Math.Min(startColumn, startLineSegment.Length);
            _length = endLineSegment.Offset + Math.Min(endColumn, endLineSegment.Length) - _offset;
        }

        private readonly Document _document;
        internal bool IsFolded;
        private FoldType _foldType;
        internal string FoldText { get; }

        private int _startLine = -1;
        private int _startColumn = 0;
        private int _endLine = -1;
        private int _endColumn = 0;

        private int _offset;
        private int _length;

        public int Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                _startLine = _endLine = -1;
            }
        }

        public int Length
        {
            get => _length;
            set
            {
                _length = value;
                _endLine = -1;
            }
        }

        public int StartLine
        {
            get
            {
                if (_startLine < 0) GetStartPointForOffset(Offset);
                return _startLine;
            }
        }

        public int StartColumn
        {
            get
            {
                if (_startLine < 0) GetStartPointForOffset(Offset);
                return _startColumn;
            }
        }

        public int EndLine
        {
            get
            {
                if (_endLine < 0) GetEndPointForOffset(Offset + Length);
                return _endLine;
            }
        }

        public int EndColumn
        {
            get
            {
                if (_endLine < 0) GetEndPointForOffset(Offset + Length);
                return _endColumn;
            }
        }

        private void GetStartPointForOffset(int offset)
        {
            if (offset > _document.TextLength)
            {
                _startLine = _document.TotalNumberOfLines + 1;
                _startColumn = 1;
            }
            else if (offset < 0)
            {
                _startLine = _startColumn = -1;
            }
            else
            {
                _startLine = _document.GetLineNumberForOffset(offset);
                _startColumn = offset - _document.GetLineSegment(_startLine).Offset;
            }
        }

        private void GetEndPointForOffset(int offset)
        {
            if (offset > _document.TextLength)
            {
                _endLine = _document.TotalNumberOfLines + 1;
                _endColumn = 1;
            }
            else if (offset < 0)
            {
                _endLine = _endColumn = -1;
            }
            else
            {
                _endLine = _document.GetLineNumberForOffset(offset);
                _endColumn = offset - _document.GetLineSegment(_endLine).Offset;
            }
        }

        public int CompareTo(FoldMarker other)
        {
            return Offset != other.Offset
                ? Offset.CompareTo(other.Offset)
                : Length.CompareTo(other.Length);
        }
    }
}