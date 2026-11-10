using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal abstract class MarkdownBlock
{
    internal abstract float Height { get; }

    /// <summary>
    /// 将Markdig的Block转换为相应的MarkdownBlock并进行布局
    /// </summary>
    internal abstract void Build(Block node, float availableWidth);

    /// <summary>
    /// 在视图宽度改变时重新布局
    /// </summary>
    internal abstract void Relayout(float availableWidth);

    internal abstract void OnPaint(ICanvas canvas, ChatBoxController controller);
    internal virtual void Dispose() { }
}