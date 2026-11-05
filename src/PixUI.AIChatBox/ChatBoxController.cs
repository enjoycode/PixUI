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

    internal readonly ScrollController ScrollController;
    internal readonly List<ChatMessage> Messages = [];
    internal float Width { get; set; }
    internal readonly MarkdownPipeline MarkdownPipeline;
    internal Action? RepaintAction { get; set; }

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
}