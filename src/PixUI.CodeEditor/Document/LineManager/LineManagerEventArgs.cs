namespace CodeEditor
{
    public readonly struct LineCountChangeEventArgs
    {
        public LineCountChangeEventArgs(Document document, int start, int moved)
        {
            Document = document;
            Start = start;
            Moved = moved;
        }

        public readonly Document Document;

        /// -1 if no offset was specified for this event
        public readonly int Start;

        /// -1 if no length was specified for this event
        public readonly int Moved;
    }

    public readonly struct LineEventArgs
    {
        public LineEventArgs(Document document, LineSegment lineSegment)
        {
            Document = document;
            LineSegment = lineSegment;
        }

        public readonly Document Document;
        public readonly LineSegment LineSegment;
    }

    public readonly struct LineLengthChangeEventArgs
    {
        public LineLengthChangeEventArgs(Document document, LineSegment lineSegment,
            int lengthDelta)
        {
            Document = document;
            LineSegment = lineSegment;
            LengthDelta = lengthDelta;
        }

        public readonly Document Document;
        public readonly LineSegment LineSegment;
        public readonly int LengthDelta;
    }
}