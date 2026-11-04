using Markdig;

namespace PixUI.AIChatBox;

public sealed class ChatBoxController
{
    public ChatBoxController()
    {
        ScrollController = new ScrollController(ScrollDirection.Vertical);
        MarkdownPipeline = new MarkdownPipelineBuilder()
            .UseEmphasisExtras()
            .UsePipeTables()
            .Build();
        Theme = new Theme();

        NormalTextStyle = CreateTextStyle(Theme, FontWeight.Normal, FontSlant.Upright);
        BoldTextStyle = CreateTextStyle(Theme, FontWeight.Bold, FontSlant.Upright);
        ItalicTextStyle = CreateTextStyle(Theme, FontWeight.Normal, FontSlant.Italic);
        BoldAndItalicTextStyle = CreateTextStyle(Theme, FontWeight.Bold, FontSlant.Italic);
        StrikeTextStyle = CreateTextStyle(Theme, FontWeight.Bold, FontSlant.Italic); //TODO: fix
    }

    internal readonly ScrollController ScrollController;
    internal readonly List<ChatMessage> Messages = [];
    internal float Width { get; set; }
    internal readonly MarkdownPipeline MarkdownPipeline;
    internal Action? RepaintAction { get; set; }
    public Theme Theme { get; set; }

    public void AppendAIThinkContent(string text)
    {
        if (Messages.Count == 0 || Messages[^1] is not AIMessage lastAIMessage)
        {
            lastAIMessage = new AIMessage();
            if (Messages.Count > 1)
                lastAIMessage.Y = Messages[^1].Y + Messages[^1].H;
            Messages.Add(lastAIMessage);
        }

        lastAIMessage.AppendThinkContent(text, this);
        RepaintAction?.Invoke(); //TODO: check is visible
    }

    public ITextStyle NormalTextStyle { get; private set; }
    public ITextStyle BoldAndItalicTextStyle { get; private set; }
    public ITextStyle BoldTextStyle { get; private set; }
    public ITextStyle ItalicTextStyle { get; private set; }
    public ITextStyle StrikeTextStyle { get; private set; }

    private static ITextStyle CreateTextStyle(Theme theme, FontWeight weight, FontSlant slant)
    {
        var ts = TextStyle.Create();
        ts.FontSize = theme.FontSize;
        ts.Color = theme.TextColor;
        ts.FontStyle = new FontStyle(weight, slant);
        return ts;
    }

    internal ITextStyle GetTextStyle(InlineStyle style)
    {
        //TODO: Code and Link first
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
}