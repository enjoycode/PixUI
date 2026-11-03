using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal abstract class MarkdownBlock
{
    internal abstract float Height { get; }
    internal abstract void Layout(Block node, string textBuffer, ChatBoxController controller);
}