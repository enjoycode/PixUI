using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace PixUI.AIChatBox;

internal sealed class MarkdownParagraph : MarkdownBlock
{
    private IParagraph? _paragraph;

    internal override float Height => _paragraph?.Height ?? 0;

    internal float Width => _paragraph?.LongestLine ?? 0;

    internal override void Layout(Block node, ChatBoxController controller)
    {
        _paragraph?.Dispose();
        var paraNode = (ParagraphBlock)node;
        var inline = paraNode.Inline;
        if (inline?.FirstChild == null) return;

        using var pb = ParagraphBuilder.Create(ChatBoxTheme.ParagraphStyle);
        var styles = new Stack<InlineStyle>();
        styles.Push(InlineStyle.None);

        ContainerInlineBuilder.Build(pb, inline, styles, 0, controller);

        styles.Clear();
        _paragraph = pb.Build();
        _paragraph.Layout(controller.Width);
    }

    internal override void Relayout(float availableWidth)
    {
        if (_paragraph == null || (_paragraph.Lines <= 1 && Width <= availableWidth)) return;
        _paragraph?.Layout(availableWidth);
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        if (_paragraph == null) return;

        canvas.DrawParagraph(_paragraph, 0, 0);
    }

    internal override void Dispose() => _paragraph?.Dispose();
}