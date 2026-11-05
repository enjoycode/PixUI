using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal abstract class MarkdownBlock
{
    internal abstract float Height { get; }
    internal abstract void Layout(Block node, ChatBoxController controller);
    internal abstract void Relayout(float availableWidth);
    internal abstract void OnPaint(ICanvas canvas, ChatBoxController controller);
    internal virtual void Dispose() { }
}