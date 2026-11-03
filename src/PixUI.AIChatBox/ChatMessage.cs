namespace PixUI.AIChatBox;

public abstract class ChatMessage
{
    internal float Y { get; set; }

    internal float H { get; set; }

    internal abstract void OnLayout(float availableWidth);

    internal abstract void OnPaint(ICanvas canvas);
}

public sealed class AIMessage : ChatMessage
{
    private readonly List<MessageElement> _elements = [];

    internal void AppendThinkContent(string content, ChatBoxController controller)
    {
        if (_elements.Count == 0 || _elements[^1] is not MarkdownElement markdownElement)
        {
            markdownElement = new MarkdownElement();
            _elements.Add(markdownElement);
        }

        var heightDiff = markdownElement.Append(content, controller);
        H += heightDiff;
    }

    internal override void OnLayout(float availableWidth)
    {
        throw new NotImplementedException();
    }

    internal override void OnPaint(ICanvas canvas)
    {
        throw new NotImplementedException();
    }
}

public sealed class UserMessage : ChatMessage
{
    internal override void OnLayout(float availableWidth)
    {
        throw new NotImplementedException();
    }

    internal override void OnPaint(ICanvas canvas)
    {
        throw new NotImplementedException();
    }
}