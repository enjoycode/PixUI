using PixUI;

namespace PixUI.CodeEditor;

internal sealed class CompletionItemWidget : Widget
{
    public CompletionItemWidget(ICompletionItem item /*, State<bool> isSelected*/)
    {
        _item = item;
        // _isSelected = isSelected;
        _iconPainter = new IconPainter(() => Repaint());
    }

    private readonly ICompletionItem _item;

    // private readonly State<bool> _isSelected;
    private readonly IconPainter _iconPainter;
    private IParagraph? _paragraph; //TODO: use TextPainter

    protected override void OnLayout(Size maxSize)
    {
        SetLayoutSize(AvailableSize);
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        const float fontSize = 13f;
        const float x = 2f;
        const float y = 3f;
        _iconPainter.Paint(canvas, fontSize, Colors.Gray, GetIcon(_item.Kind), x, y);
        _paragraph ??= TextPainter.BuildParagraph(_item.Label, float.PositiveInfinity,
            fontSize, Colors.Black, null, 1, true);

        canvas.DrawParagraph(_paragraph, x + 20, y);
    }

    private static IconData GetIcon(CompletionItemKind kind)
    {
        switch (kind)
        {
            case CompletionItemKind.Keyword: return MaterialIcons.Key;
            case CompletionItemKind.Class: return MaterialIcons.AccountTree;
            case CompletionItemKind.Function:
            case CompletionItemKind.Method: return MaterialIcons.Functions;
            case CompletionItemKind.Property:
            case CompletionItemKind.Field: return MaterialIcons.LocalOffer;
            case CompletionItemKind.Interface: return MaterialIcons.Hub;
            case CompletionItemKind.Enum: return MaterialIcons.Storage;
            case CompletionItemKind.Event: return MaterialIcons.Bolt;
            case CompletionItemKind.Module: return MaterialIcons.DataObject;
            default: return MaterialIcons.Title;
        }
    }
}