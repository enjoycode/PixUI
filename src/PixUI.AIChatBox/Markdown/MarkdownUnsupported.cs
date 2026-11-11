using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownUnsupported : MarkdownBlock
{
    private IParagraph? _paragraph;
    internal override float Height => 20;

    internal override void Build(Block node, float availableWidth)
    {
        using var pb = ParagraphBuilder.Create(ChatBoxTheme.ParagraphStyle);
        pb.PushStyle(ChatBoxTheme.NormalTextStyle);
        pb.AddText("暂未实现:");
        pb.AddText(node.GetType().Name);
        pb.Pop();
        _paragraph = pb.Build();
        _paragraph.Layout(availableWidth);
    }

    internal override void Relayout(float availableWidth)
    {
        _paragraph?.Layout(availableWidth);
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        var rect = Rect.FromLTWH(0, 0, controller.Width, Height);
        canvas.DrawRect(rect, Paint.Shared(Colors.Yellow));

        if (_paragraph != null)
            canvas.DrawParagraph(_paragraph, 0, 0);
    }
}