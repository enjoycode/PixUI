using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownCodeBlock : MarkdownBlock
{
    internal override float Height => throw new NotImplementedException();

    internal override void Layout(Block node, string textBuffer, ChatBoxController controller)
    {
        var codeNode = (FencedCodeBlock)node;
        throw new NotImplementedException();
    }
}