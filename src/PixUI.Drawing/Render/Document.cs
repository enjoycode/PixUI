namespace PixUI;

public interface IDocument : IDisposable
{
    ICanvas BeginPage(float width, float height);

    void EndPage();

    void Close();
}

public static class Document
{
    public static IDocument CreatePdf(Stream stream, float dpi) => Render.Backend.MakeDocumentPdf(stream, dpi);
}