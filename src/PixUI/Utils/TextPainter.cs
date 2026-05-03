namespace PixUI;

public static class TextPainter
{
    public static IParagraph BuildParagraph(string text, float width, float fontSize, in Color color,
        FontStyle? fontStyle = null, int maxLines = 1, bool forceHeight = false)
    {
        using var ts = TextStyle.Create();
        ts.Color = color;
        ts.FontSize = fontSize;
        if (fontStyle != null)
            ts.FontStyle = fontStyle.Value;

        using var ps = ParagraphStyle.Create();
        ps.MaxLines = (uint)maxLines;
        ps.TextStyle = ts;
        if (forceHeight)
        {
            ts.Height = 1;
            ps.Height = 1;
        }

        using var pb = ParagraphBuilder.Create(ps);

        pb.PushStyle(ts);
        pb.AddText(text);
        pb.Pop();
        var ph = pb.Build();
        ph.Layout(width);
        return ph;
    }
}