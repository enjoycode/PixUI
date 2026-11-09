using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownQuote : MarkdownBlock
{
    private readonly List<MarkdownBlock> _blocks = [];
    private const float _quoteWidth = 5f;

    internal override float Height => _blocks.Sum(b => b.Height);

    internal override void Layout(Block node, float availableWidth)
    {
        var quoteNode = (QuoteBlock)node;

        foreach (var child in quoteNode)
        {
            var block = MarkdownElement.CreateBlock(child);
            block.Layout(child, availableWidth - _quoteWidth);
            _blocks.Add(block);
        }
    }

    internal override void Relayout(float availableWidth)
    {
        foreach (var block in _blocks)
        {
            block.Relayout(availableWidth - _quoteWidth);
        }
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        //draw quote
        var rect = Rect.FromLTWH(0, 0, controller.Width, Height);
        canvas.DrawRect(rect, Paint.Shared(ChatBoxTheme.QuoteBackgroundColor));
        rect = Rect.FromLTWH(0, 0, _quoteWidth, Height);
        canvas.DrawRect(rect, Paint.Shared(ChatBoxTheme.QuoteColor));

        //draw children
        canvas.Translate(_quoteWidth, 0);
        foreach (var block in _blocks)
            block.OnPaint(canvas, controller);
        canvas.Translate(-_quoteWidth, 0);
    }

    internal override void Dispose()
    {
        foreach (var block in _blocks)
            block.Dispose();
    }
}