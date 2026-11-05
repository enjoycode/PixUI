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

    // public float LineSpacing { get; set; } = 2f;
    
    #region ====TextStyles====

    public static ITextStyle NormalTextStyle { get; private set; }
    public static ITextStyle BoldAndItalicTextStyle { get; private set; }
    public static ITextStyle BoldTextStyle { get; private set; }
    public static ITextStyle ItalicTextStyle { get; private set; }
    public static ITextStyle StrikeTextStyle { get; private set; }
    public static  ITextStyle InlineCodeTextStyle { get; private set; }

    public static  IParagraphStyle ParagraphStyle { get; private set; }

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

    internal static ITextStyle GetTextStyle(InlineStyle style)
    {
        //TODO: Link first
        if (style.HasFlag(InlineStyle.Bold) && style.HasFlag(InlineStyle.Italic))
            return BoldAndItalicTextStyle;
        if (style.HasFlag(InlineStyle.Bold))
            return BoldTextStyle;
        if (style.HasFlag(InlineStyle.Italic))
            return ItalicTextStyle;
        if (style.HasFlag(InlineStyle.Strike))
            return StrikeTextStyle;

        return NormalTextStyle;
    }

    #endregion
}