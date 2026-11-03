using Markdig;

namespace PixUI.AIChatBox;

public sealed class ChatBoxController
{
    public ChatBoxController()
    {
        ScrollController = new ScrollController(ScrollDirection.Vertical);
        MarkdownPipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
        Theme = new Theme();
        LiteralTextStyle = TextStyle.Create();
        LiteralTextStyle.FontSize = Theme.FontSize;
    }

    internal readonly ScrollController ScrollController;
    internal readonly List<ChatMessage> Messages = [];
    internal float Width { get; set; }
    internal readonly MarkdownPipeline MarkdownPipeline;

    public Theme Theme { get; set; }

    public void AppendAIThinkContent(string text)
    {
        if (Messages.Count == 0 || Messages[^1] is not AIMessage lastAIMessage)
        {
            lastAIMessage = new AIMessage();
            Messages.Add(lastAIMessage);
        }

        lastAIMessage.AppendThinkContent(text, this);
        //TODO: repaint
    }

    public ITextStyle LiteralTextStyle { get; private set; }
}