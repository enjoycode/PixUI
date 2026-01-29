namespace PixUI;

public static class TextPainter
{
    public static Paragraph BuildParagraph(string text, float width, float fontSize, in Color color,
        FontStyle? fontStyle = null, int maxLines = 1, bool forceHeight = false)
    {
        using var ts = new TextStyle { Color = color, FontSize = fontSize };
        if (fontStyle != null)
            ts.FontStyle = fontStyle.Value;

        using var ps = new ParagraphStyle { MaxLines = (uint)maxLines, TextStyle = ts };
        if (forceHeight)
        {
            ts.Height = 1;
            ps.Height = 1;
        }

        using var pb = new ParagraphBuilder(ps);

        pb.PushStyle(ts);
        pb.AddText(text);
        pb.Pop();
        var ph = pb.Build();
        ph.Layout(width);
        return ph;
    }
}