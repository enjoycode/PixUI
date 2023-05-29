using System;

namespace CodeEditor
{
    public sealed class Selection
    {
        public Selection(Document document, TextLocation startPosition, TextLocation endPosition)
        {
            if (startPosition > endPosition)
                throw new ArgumentOutOfRangeException();

            Document = document;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }

        public readonly Document Document;
        public TextLocation StartPosition { get; internal set; }
        public TextLocation EndPosition { get; internal set; }

        public int Offset => Document.PositionToOffset(StartPosition);

        public int EndOffset => Document.PositionToOffset(EndPosition);

        public int Length => EndOffset - Offset;

        public bool IsEmpty => StartPosition == EndPosition;

        public string SelectedText => Length <= 0 ? "" : Document.GetText(Offset, Length);

        public bool ContainsOffset(int offset) => Offset <= offset && offset <= EndOffset;
    }
}