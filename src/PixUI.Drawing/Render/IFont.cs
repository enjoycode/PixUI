namespace PixUI;

public interface IFont : IDisposable
{
    ushort GetGlyphId(int codepoint);

    Size MeasureString(string text);
}