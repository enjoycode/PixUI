namespace CodeEditor;

public readonly struct DocumentEventArgs
{
    public DocumentEventArgs(Document document, int offset, int length, string text)
    {
        Document = document;
        Offset = offset;
        Length = length;
        Text = text;
    }

    public readonly Document Document;
    public readonly int Offset;
    public readonly int Length;
    public readonly string Text;
}