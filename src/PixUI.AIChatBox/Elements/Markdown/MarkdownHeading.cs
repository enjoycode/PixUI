using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownHeading : MarkdownBlock
{
    private IParagraph? _paragraph;

    internal override float Height => _paragraph?.Height ?? 0;

    private float Width => _paragraph?.LongestLine ?? 0;

    internal override void Build(Block node, float availableWidth)
    {
        var headingNode = (HeadingBlock)node;
        if (headingNode.Inline == null) return;

        using var pb = ParagraphBuilder.Create(ChatBoxTheme.ParagraphStyle);
        var styles = new Stack<InlineStyle>();
        styles.Push(InlineStyle.Bold);

        ContainerInlineBuilder.Build(pb, headingNode.Inline, styles, headingNode.Level);

        styles.Clear();
        _paragraph = pb.Build();
        _paragraph.Layout(availableWidth);
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