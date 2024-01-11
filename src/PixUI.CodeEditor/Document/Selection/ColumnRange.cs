using System;

namespace CodeEditor;

public readonly struct ColumnRange : IEquatable<ColumnRange>
{
    public ColumnRange(int startColumn, int endColumn)
    {
        StartColumn = startColumn;
        EndColumn = endColumn;
    }

    public readonly int StartColumn;
    public readonly int EndColumn;

    public bool Equals(ColumnRange other)
    {
        return StartColumn == other.StartColumn && EndColumn == other.EndColumn;
    }

    public override bool Equals(object? obj)
    {
        return obj is ColumnRange other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartColumn, EndColumn);
    }

    public ColumnRange Clone() => new ColumnRange(StartColumn, EndColumn);
}