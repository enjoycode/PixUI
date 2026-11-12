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
    }

    private const float SPACING = 10f;
    internal readonly ScrollController ScrollController;
    internal readonly List<ChatMessage> Messages = [];
    internal float Width { get; set; }
    internal readonly MarkdownPipeline MarkdownPipeline;
    internal Action? RepaintAction { get; set; }

    public void AppendAIContent(string text, bool thinking = false)
    {
        if (Messages.Count == 0 || Messages[^1] is not AIMessage lastAIMessage)
        {
            lastAIMessage = new AIMessage();
            if (Messages.Count > 0)
                lastAIMessage.Y = Messages[^1].Y + Messages[^1].H + SPACING;
            Messages.Add(lastAIMessage);
        }

        lastAIMessage.AppendContent(text, this, thinking);
        RepaintAction?.Invoke(); //TODO: check is visible
    }

    public void SendUserMessage(string text /*TODO: 附件*/)
    {
        var msg = new UserMessage(text, this);
        msg.Y = Messages.Count > 0 ? Messages[^1].Y + Messages[^1].H + SPACING : 0;
        Messages.Add(msg);
        RepaintAction?.Invoke(); //TODO: check is visible
    }
}