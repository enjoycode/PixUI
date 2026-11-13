namespace PixUI.AIChatBox;

internal sealed class FunctionCallElement : MessageElement
{
    public FunctionCallElement(string name, IDictionary<string, object?>? args)
    {
        using var pb = ParagraphBuilder.Create(ChatBoxTheme.ParagraphStyle);
        pb.PushStyle(ChatBoxTheme.NormalTextStyle);
        pb.AddText($"Function Call: {name}(");

        if (args != null && args.Count > 0)
        {
            var isFirst = true;
            foreach (var arg in args)
            {
                if (!isFirst) pb.AddText(", ");
                else isFirst = false;
                pb.AddText($"{arg.Key}: {arg.Value}");
            }
        }

        pb.AddText(")");
        _paragraph = pb.Build();
    }

    private readonly IParagraph? _paragraph;
    private const float PADDING = 5;

    internal override float Height => _paragraph?.Height + PADDING * 2 ?? 16 + PADDING * 2;

    internal override void OnLayout(float availableWidth)
    {
        _paragraph?.Layout(availableWidth - PADDING * 2);
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        var rect = Rect.FromLTWH(0, 0, controller.Width, Height);
        canvas.DrawRect(rect, Paint.Shared(Colors.Magenta, PaintStyle.Stroke));

        if (_paragraph != null)
            canvas.DrawParagraph(_paragraph, PADDING, PADDING);
    }
}