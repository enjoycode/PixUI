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

    internal void AppendContent(string content, ChatBoxController controller, bool thinking = false)
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
        // var border = RRect.FromRectAndRadius(Rect.FromLTWH(0, 0, controller.Width, H), 3, 3);
        // canvas.DrawRRect(border, Paint.Shared(Colors.Red, PaintStyle.Stroke)); //TODO:

        //TODO:优化仅画可见区域
        var offsetY = 0f;
        foreach (var element in _elements)
        {
            canvas.Translate(0, offsetY);
            element.OnPaint(canvas, controller);
            canvas.Translate(0, -offsetY);
            offsetY += element.Height;
        }
    }
}

public sealed class UserMessage : ChatMessage
{
    private const float LIFT = 100f;
    private const float PADDING = 10f;
    private readonly List<MessageElement> _elements = []; //可能需要包含附件等元素

    public UserMessage(string content, ChatBoxController controller)
    {
        var markdownElement = new MarkdownElement();
        _elements.Add(markdownElement);
        markdownElement.Append(content, controller);
        markdownElement.OnLayout(controller.Width - LIFT - PADDING * 2);
        H = markdownElement.Height + PADDING * 2;
    }

    internal override void OnLayout(float availableWidth)
    {
        availableWidth = availableWidth - LIFT - PADDING * 2;
        foreach (var element in _elements)
            element.OnLayout(availableWidth);
        H = _elements.Sum(e => e.Height) + PADDING * 2;
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        // draw border
        var border = RRect.FromRectAndRadius(Rect.FromLTWH(LIFT, 0, controller.Width - LIFT, H), 5, 5);
        var paint = Paint.Shared(Colors.LightGray);
        paint.IsAntialias = true;
        canvas.DrawRRect(border, paint);

        //TODO:优化仅画可见区域
        canvas.Translate(LIFT + PADDING, 0);
        var offsetY = PADDING;
        foreach (var element in _elements)
        {
            canvas.Translate(0, offsetY);
            element.OnPaint(canvas, controller);
            canvas.Translate(0, -offsetY);
            offsetY += element.Height;
        }

        canvas.Translate(-LIFT - PADDING, 0);
    }
}