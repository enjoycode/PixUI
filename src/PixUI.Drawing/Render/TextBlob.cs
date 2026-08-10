namespace PixUI;

public interface ITextBlob : IDisposable
{
    Rect Bounds { get; }
}

public static class TextBlob
{
    public static ITextBlob? Create(ReadOnlySpan<char> text, IFont font, out float width, Point origin = default)
        => Render.Backend.MakeTextBlob(text, font, out width, origin);
}