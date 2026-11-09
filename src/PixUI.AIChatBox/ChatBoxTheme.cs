namespace PixUI.AIChatBox;

public static class ChatBoxTheme
{
    static ChatBoxTheme()
    {
        NormalTextStyle = CreateTextStyle(FontWeight.Normal, FontSlant.Upright);
        BoldTextStyle = CreateTextStyle(FontWeight.Bold, FontSlant.Upright);
        ItalicTextStyle = CreateTextStyle(FontWeight.Normal, FontSlant.Italic);
        BoldAndItalicTextStyle = CreateTextStyle(FontWeight.Bold, FontSlant.Italic);
        StrikeTextStyle = CreateTextStyle(FontWeight.Bold, FontSlant.Italic); //TODO: fix
        InlineCodeTextStyle = CreateTextStyle(FontSize, Colors.Magenta, FontWeight.Bold, FontSlant.Upright);

        ParagraphStyle = PixUI.ParagraphStyle.Create();
        ParagraphStyle.TextStyle = NormalTextStyle;
    }

    public static float FontSize { get; set; } = 13f;

    public static Color TextColor { get; set; } = Colors.Black;

    public static float TableCellPadding { get; set; } = 2f;

    public static Color QuoteColor { get; set; } = Colors.Gray;

    public static Color QuoteBackgroundColor { get; set; } = Colors.LightGray;

    public static Color HorizontalRuleColor { get; set; } = Colors.LightGray;

    // public float LineSpacing { get; set; } = 2f;

    #region ====TextStyles====

    public static ITextStyle NormalTextStyle { get; private set; }
    public static ITextStyle BoldAndItalicTextStyle { get; private set; }
    public static ITextStyle BoldTextStyle { get; private set; }
    public static ITextStyle ItalicTextStyle { get; private set; }
    public static ITextStyle StrikeTextStyle { get; private set; }
    public static ITextStyle InlineCodeTextStyle { get; private set; }

    public static IParagraphStyle ParagraphStyle { get; private set; }

    private static ITextStyle CreateTextStyle(FontWeight weight, FontSlant slant) =>
        CreateTextStyle(FontSize, TextColor, weight, slant);

    private static ITextStyle CreateTextStyle(float fontSize, Color color, FontWeight weight, FontSlant slant)
    {
        var ts = TextStyle.Create();
        ts.FontSize = fontSize;
        ts.Color = color;
        ts.FontStyle = new FontStyle(weight, slant);
        return ts;
    }

    internal static ITextStyle GetTextStyle(InlineStyle style, int headingLevel)
    {
        ITextStyle ts;
        if (style.HasFlag(InlineStyle.Bold) && style.HasFlag(InlineStyle.Italic))
            ts = BoldAndItalicTextStyle;
        else if (style.HasFlag(InlineStyle.Bold))
            ts = BoldTextStyle;
        else if (style.HasFlag(InlineStyle.Italic))
            ts = ItalicTextStyle;
        else if (style.HasFlag(InlineStyle.Strike))
            ts = StrikeTextStyle;
        else
            ts = NormalTextStyle;

        ts.FontSize = GetFontSize(headingLevel);

        return ts;
    }

    internal static ITextStyle GetInlineCodeTextStyle(int headingLevel)
    {
        var ts = InlineCodeTextStyle;
        ts.FontSize = GetFontSize(headingLevel);
        return ts;
    }

    private static float GetFontSize(int headingLevel) => headingLevel switch
    {
        1 => FontSize + 18,
        2 => FontSize + 15,
        3 => FontSize + 12,
        4 => FontSize + 9,
        5 => FontSize + 6,
        6 => FontSize + 3,
        _ => FontSize
    };

    #endregion
}