using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownThematicBreak : MarkdownBlock
{
    internal override float Height => 16;
    internal override void Build(Block node, float availableWidth) { }
    internal override void Relayout(float availableWidth) { }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        canvas.DrawLine(0, Height / 2, controller.Width, Height / 2,
            Paint.Shared(ChatBoxTheme.HorizontalRuleColor, PaintStyle.Stroke, 1));
    }
}