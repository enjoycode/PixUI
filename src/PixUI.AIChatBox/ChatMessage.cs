namespace PixUI.AIChatBox;

public abstract class ChatMessage
{
    internal float Y { get; set; }

    internal float H { get; set; }

    internal abstract void OnLayout(float availableWidth);

    internal abstract void OnPaint(ICanvas canvas, ChatBoxController controller);
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
        foreach (var element in _elements)
            element.OnLayout(availableWidth);
        H = _elements.Sum(e => e.Height);
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        // draw border
        var border = RRect.FromRectAndRadius(Rect.FromLTWH(0, 0, controller.Width, H), 3, 3);
        canvas.DrawRRect(border, Paint.Shared(Colors.Red, PaintStyle.Stroke)); //TODO:

        //TODO:优化仅画可见区域
        var offsetY = 0f;
        foreach (var element in _elements)
        {
            canvas.Translate(0, offsetY);
            element.OnPaint(canvas, controller);
            offsetY += element.Height;
        }

        canvas.Translate(0, -offsetY);
    }
}

public sealed class UserMessage : ChatMessage
{
    internal override void OnLayout(float availableWidth) { }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller) { }
}