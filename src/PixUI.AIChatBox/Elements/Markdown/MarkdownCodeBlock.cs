using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownCodeBlock : MarkdownBlock
{
    internal override float Height => throw new NotImplementedException();

    internal override void Layout(Block node, ChatBoxController controller)
    {
        var codeNode = (FencedCodeBlock)node;
        throw new NotImplementedException();
    }

    internal override void Relayout(float availableWidth)
    {
        throw new NotImplementedException();
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        throw new NotImplementedException();
    }
}