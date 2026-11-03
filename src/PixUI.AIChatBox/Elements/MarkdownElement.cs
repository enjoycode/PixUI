using Markdig.Parsers;
using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownElement : MessageElement
{
    private readonly List<MarkdownBlock> _blocks = [];
    private string _textBuffer = string.Empty; //TODO: 待Markdig支持流式缓存

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
            block.Layout(node, _textBuffer, controller);

            if (isNew)
            {
                _blocks.Add(block);
                heightDiff += block.Height;
            }
            else
            {
                _blocks[^1] = block;
                heightDiff += (block.Height - _blocks[^1].Height);
            }

            isNew = true;
            lastNode = node;
        }

        if (lastNode != null)
            _textBuffer = lastNode.IsOpen ? _textBuffer[lastNode.Span.Start..] : string.Empty;

        return heightDiff;
    }

    internal override void OnLayout(float availableWidth)
    {
        throw new NotImplementedException();
    }

    internal override void OnPaint(ICanvas canvas)
    {
        throw new NotImplementedException();
    }

    private static MarkdownBlock CreateBlock(Block node)
    {
        switch (node)
        {
            case ParagraphBlock:
                return new MarkdownParagraph();
            case FencedCodeBlock:
                return new MarkdownCodeBlock();
            default: throw new NotImplementedException();
        }
    }
}