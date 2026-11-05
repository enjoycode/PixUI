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

        using var pb = ParagraphBuilder.Create(controller.ParagraphStyle);
        var styles = new Stack<InlineStyle>();
        styles.Push(InlineStyle.None);

        BuildContainerInline(pb, inline, styles, controller);

        styles.Clear();
        _paragraph = pb.Build();
        _paragraph.Layout(controller.Width);
    }

    internal override void Relayout(float availableWidth)
    {
        if (_paragraph == null || (_paragraph.Lines <= 1 && Width <= availableWidth)) return;
        _paragraph?.Layout(availableWidth);
    }

    private static void BuildContainerInline(IParagraphBuilder pb, ContainerInline node, Stack<InlineStyle> styles,
        ChatBoxController controller)
    {
        var child = node.FirstChild;
        while (child != null)
        {
            switch (child)
            {
                case LiteralInline literal: BuildLiteral(pb, styles.Peek(), literal, controller); break;
                case EmphasisInline emphasis: BuildEmphasis(pb, emphasis, styles, controller); break;
                case CodeInline code: BuildCode(pb, code, controller); break;
                case LineBreakInline lineBreak: BuildLineBreak(pb, lineBreak, controller); break;
                default: throw new NotImplementedException(child.GetType().Name);
            }

            child = child.NextSibling;
        }
    }

    private static void BuildLiteral(IParagraphBuilder pb, InlineStyle style, LiteralInline node,
        ChatBoxController controller)
    {
        pb.PushStyle(controller.GetTextStyle(style));
        pb.AddText(node.Content.AsSpan());
        pb.Pop();
    }

    private static void BuildLineBreak(IParagraphBuilder pb, LineBreakInline node, ChatBoxController controller)
    {
        pb.PushStyle(controller.NormalTextStyle);
        pb.AddText("\n");
        pb.Pop();
    }

    private static void BuildCode(IParagraphBuilder pb, CodeInline node, ChatBoxController controller)
    {
        pb.PushStyle(controller.InlineCodeTextStyle);
        pb.AddText(node.ContentSpan);
        pb.Pop();
    }

    private static void BuildEmphasis(IParagraphBuilder pb, EmphasisInline node, Stack<InlineStyle> styles,
        ChatBoxController controller)
    {
        if (node.FirstChild == null) return;

        var style = node.Parent is EmphasisInline ? styles.Peek() : InlineStyle.None;
        if (node.DelimiterChar is '*' or '_')
        {
            if (node.DelimiterCount == 1)
                style |= InlineStyle.Italic;
            else if (node.DelimiterCount == 2)
                style |= InlineStyle.Bold;
        }
        else if (node.DelimiterChar is '~' && node.DelimiterCount == 2)
            style |= InlineStyle.Strike;

        styles.Push(style);
        BuildContainerInline(pb, node, styles, controller);
        styles.Pop();
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        if (_paragraph == null) return;

        canvas.DrawParagraph(_paragraph, 0, 0);
    }

    internal override void Dispose() => _paragraph?.Dispose();
}