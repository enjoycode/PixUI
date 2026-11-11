using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownList : MarkdownBlock
{
    private readonly List<ListItemInfo> _items = [];
    private int _level;
    private const float INDENT = 5f;
    private static IParagraph? _dotBullet;

    internal override float Height => _items.Sum(i => i.Height);

    internal override void Build(Block node, float availableWidth)
    {
        _items.ForEach(i => i.Dispose());
        _items.Clear();

        var listNode = (ListBlock)node;
        _level = GetListBlockLevel(listNode);
        availableWidth -= _level * INDENT;

        foreach (var child in listNode)
        {
            var itemNode = (ListItemBlock)child;
            var bullet = BuildBulletParagraph(listNode, itemNode);
            var blocks = new List<MarkdownBlock>();
            foreach (var item in itemNode)
            {
                var block = MarkdownElement.CreateBlock(item);
                block.Build(item, availableWidth - bullet.MaxIntrinsicWidth);
                blocks.Add(block);
            }

            _items.Add(new ListItemInfo() { BulletParagraph = bullet, Blocks = blocks });
        }
    }

    internal override void Relayout(float availableWidth)
    {
        availableWidth -= _level * INDENT;
        foreach (var item in _items)
            item.Relayout(availableWidth - item.BulletParagraph.MaxIntrinsicWidth);
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        var offsetX = _level * INDENT;
        var offsetY = 0f;
        foreach (var item in _items)
        {
            canvas.Translate(offsetX, offsetY);
            item.OnPaint(canvas, controller);
            canvas.Translate(-offsetX, -offsetY);
            offsetY += item.Height;
        }
    }

    private static int GetListBlockLevel(ListBlock listBlock)
    {
        int level = 0;
        ContainerBlock? temp = listBlock;
        while (temp != null)
        {
            if (temp.Parent is not ListItemBlock)
                break;

            temp = temp.Parent.Parent;
            level++;
        }

        return level;
    }

    private static IParagraph BuildBulletParagraph(ListBlock listBlock, ListItemBlock itemBlock)
    {
        //TODO:暂简单忽略有序及层级
        if (_dotBullet != null)
            return _dotBullet;

        using var pb = ParagraphBuilder.Create(ChatBoxTheme.ParagraphStyle);
        pb.PushStyle(ChatBoxTheme.NormalTextStyle);
        pb.AddText(" ● ");
        pb.Pop();
        var ph = pb.Build();
        ph.Layout(float.MaxValue);
        _dotBullet = ph;
        return _dotBullet;
    }

    private readonly struct ListItemInfo
    {
        public IParagraph BulletParagraph { get; init; }
        public List<MarkdownBlock> Blocks { get; init; }

        public float Height
        {
            get
            {
                if (Blocks.Count > 0 && Blocks[0] is MarkdownParagraph)
                    return Blocks.Sum(b => b.Height);
                return Blocks.Sum(b => b.Height) + BulletParagraph.Height;
            }
        }

        public void Relayout(float availableWidth)
        {
            foreach (var item in Blocks)
                item.Relayout(availableWidth);
        }

        public void OnPaint(ICanvas canvas, ChatBoxController controller)
        {
            canvas.DrawParagraph(BulletParagraph, 0, 0);
            if (Blocks.Count <= 0) return;

            var offsetX = BulletParagraph.MaxIntrinsicWidth;
            var offsetY = 0f;
            if (Blocks[0] is not MarkdownParagraph)
                offsetY += BulletParagraph.Height;

            foreach (var block in Blocks)
            {
                canvas.Translate(offsetX, offsetY);
                block.OnPaint(canvas, controller);
                canvas.Translate(-offsetX, -offsetY);
                offsetY += block.Height;
            }
        }

        public void Dispose()
        {
            if (!ReferenceEquals(BulletParagraph, _dotBullet))
                BulletParagraph.Dispose();
            Blocks.ForEach(block => block.Dispose());
            Blocks.Clear();
        }
    }
}