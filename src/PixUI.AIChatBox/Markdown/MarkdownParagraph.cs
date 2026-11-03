using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace PixUI.AIChatBox;

internal sealed class MarkdownParagraph : MarkdownBlock
{
    private IParagraph? _paragraph;

    internal override float Height => _paragraph?.Height ?? 0;

    internal override void Layout(Block node, string textBuffer, ChatBoxController controller)
    {
        _paragraph?.Dispose();
        var paraNode = (ParagraphBlock)node;
        var inline = paraNode.Inline;
        if (inline == null) return;

        using var ps = ParagraphStyle.Create(); //TODO: move to controller
        using var pb = ParagraphBuilder.Create(ps);

        var child = inline.FirstChild;
        while (child != null)
        {
            switch (child)
            {
                case LiteralInline literal:
                {
                    pb.PushStyle(controller.LiteralTextStyle);
                    pb.AddText(textBuffer.Substring(literal.Span.Start, literal.Span.Length));
                    pb.Pop();
                    break;
                }
                case EmphasisInline emphasis:
                {
                    //TODO:
                    break;
                }
                default: throw new NotImplementedException();
            }

            child = child.NextSibling;
        }

        _paragraph = pb.Build();
        _paragraph.Layout(controller.Width);
    }
}