using Markdig.Syntax.Inlines;

namespace PixUI.AIChatBox;

/// <summary>
/// 用于将ContainerInline转换为相应的IParagraph
/// </summary>
internal static class ContainerInlineBuilder
{
    internal static void Build(IParagraphBuilder pb, ContainerInline node,
        Stack<InlineStyle> styles, int headingLevel)
    {
        var child = node.FirstChild;
        while (child != null)
        {
            switch (child)
            {
                case LiteralInline literal: BuildLiteral(pb, styles.Peek(), headingLevel, literal); break;
                case EmphasisInline emphasis: BuildEmphasis(pb, emphasis, styles, headingLevel); break;
                case CodeInline code: BuildCode(pb, headingLevel, code); break;
                case LineBreakInline lineBreak: BuildLineBreak(pb, lineBreak); break;
                default: throw new NotImplementedException(child.GetType().Name);
            }

            child = child.NextSibling;
        }
    }

    private static void BuildLiteral(IParagraphBuilder pb, InlineStyle style, int headingLevel, LiteralInline node)
    {
        pb.PushStyle(ChatBoxTheme.GetTextStyle(style, headingLevel));
        pb.AddText(node.Content.AsSpan());
        pb.Pop();
    }

    private static void BuildLineBreak(IParagraphBuilder pb, LineBreakInline node)
    {
        pb.PushStyle(ChatBoxTheme.NormalTextStyle);
        pb.AddText("\n");
        pb.Pop();
    }

    private static void BuildCode(IParagraphBuilder pb, int headingLevel, CodeInline node)
    {
        pb.PushStyle(ChatBoxTheme.GetInlineCodeTextStyle(headingLevel));
        pb.AddText(node.ContentSpan);
        pb.Pop();
    }

    private static void BuildEmphasis(IParagraphBuilder pb, EmphasisInline node,
        Stack<InlineStyle> styles, int headingLevel)
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
        Build(pb, node, styles, headingLevel);
        styles.Pop();
    }
}