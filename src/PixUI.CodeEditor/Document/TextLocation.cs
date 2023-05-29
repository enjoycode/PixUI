using System;

namespace CodeEditor
{
    /// <summary>
    /// A line/column position.
    /// Text editor lines/columns are counting from zero.
    /// </summary>
    public struct TextLocation : IComparable<TextLocation>, IEquatable<TextLocation>
    {
        internal const int MaxColumn = 0xFFFFFF; //int.MaxValue;

        /// <summary>
        /// Represents no text location (-1, -1).
        /// </summary>
        public static readonly TextLocation Empty = new TextLocation(-1, -1);

        public TextLocation(int column, int line)
        {
            Line = line;
            Column = column;
        }

        public int Line;
        public int Column;

        public bool IsEmpty => Column <= 0 && Line <= 0;

        public override string ToString()
        {
            return $"(Line {Line}, Col {Column})";
        }

        public override int GetHashCode()
        {
            return unchecked(87 * Column.GetHashCode() ^ Line.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj is TextLocation other)
            {
                return other == this;
            }

            return false;
        }

        public bool Equals(TextLocation other) => this == other;

        public static bool operator ==(TextLocation a, TextLocation b)
            => a.Column == b.Column && a.Line == b.Line;

        public static bool operator !=(TextLocation a, TextLocation b)
            => a.Column != b.Column || a.Line != b.Line;

        public static bool operator <(TextLocation a, TextLocation b)
        {
            if (a.Line < b.Line)
                return true;
            if (a.Line == b.Line)
                return a.Column < b.Column;
            return false;
        }

        public static bool operator >(TextLocation a, TextLocation b)
        {
            if (a.Line > b.Line)
                return true;
            if (a.Line == b.Line)
                return a.Column > b.Column;
            return false;
        }

        // public static bool operator <=(TextLocation a, TextLocation b)
        // {
        //     return !(a > b);
        // }
        //
        // public static bool operator >=(TextLocation a, TextLocation b)
        // {
        //     return !(a < b);
        // }

        public TextLocation Clone() => new TextLocation(Column, Line);

        public int CompareTo(TextLocation other)
        {
            if (this == other)
                return 0;
            if (this < other)
                return -1;
            return 1;
        }
    }
}