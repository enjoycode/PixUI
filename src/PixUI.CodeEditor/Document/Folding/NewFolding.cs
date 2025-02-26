namespace CodeEditor;

public readonly struct NewFolding
{
    public NewFolding(int offset, int length, string foldedText = "", bool isFolded = false)
    {
        Offset = offset;
        Length = length;
        FoldedText = foldedText;
        IsFolded = isFolded;
    }

    public readonly int Offset;
    public readonly int Length;
    public readonly string FoldedText;
    public readonly bool IsFolded;
}