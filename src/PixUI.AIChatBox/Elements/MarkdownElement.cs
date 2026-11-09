using Markdig.Extensions.Tables;
using Markdig.Parsers;
using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownElement : MessageElement
{
    private readonly List<MarkdownBlock> _blocks = [];
    private string _textBuffer = string.Empty; //TODO: 待Markdig支持流式缓存

    internal override float Height => _blocks.Sum(b => b.Height);

    internal float Append(string markdown, ChatBoxController controller)
    {
        float heightDiff = 0;
        var isNew = string.IsNullOrEmpty(_textBuffer);
        _textBuffer += markdown;
        var newDoc = MarkdownParser.Parse(_textBuffer, controller.MarkdownPipeline);
        Block? lastNode = null;
        foreach (var node in newDoc)
        {
            var block = CreateBlock(node);
            block.Layout(node, controller.Width);

            if (isNew)
            {
                heightDiff += block.Height;
                _blocks.Add(block);
            }
            else
            {
                heightDiff += (block.Height - _blocks[^1].Height);
                _blocks[^1].Dispose();
                _blocks[^1] = block;
            }

            isNew = true;
            lastNode = node;
        }

        if (lastNode != null)
        {
            var lastNodeIsOpen = lastNode.IsOpen;
            if (lastNode is HeadingBlock)
            {
                //特殊处理HeadingBlock，IsOpen始终为false, 只能根据是否换行来判断
                if (_textBuffer.Length <= lastNode.Span.Start + lastNode.Span.Length)
                    lastNodeIsOpen = true;
            }

            _textBuffer = lastNodeIsOpen ? _textBuffer[lastNode.Span.Start..] : string.Empty;
        }

        return heightDiff;
    }

    internal override void OnLayout(float availableWidth)
    {
        foreach (var block in _blocks)
        {
            block.Relayout(availableWidth);
        }
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        //TODO: 仅画可见区域
        var offsetY = 0f;
        foreach (var block in _blocks)
        {
            block.OnPaint(canvas, controller);
            canvas.Translate(0, block.Height);

            offsetY += block.Height;
        }

        canvas.Translate(0, -offsetY);
    }

    internal static MarkdownBlock CreateBlock(Block node) => node switch
    {
        HeadingBlock => new MarkdownHeading(),
        ParagraphBlock => new MarkdownParagraph(),
        Table => new MarkdownTable(),
        FencedCodeBlock => new MarkdownCodeBlock(),
        QuoteBlock => new MarkdownQuote(),
        ThematicBreakBlock => new MarkdownThematicBreak(),
        _ => new MarkdownUnsupported()
    };
}