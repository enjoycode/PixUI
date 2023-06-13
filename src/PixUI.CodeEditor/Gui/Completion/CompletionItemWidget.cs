using PixUI;

namespace CodeEditor
{
    internal sealed class CompletionItemWidget : Widget
    {
        public CompletionItemWidget(ICompletionItem item, State<bool> isSelected)
        {
            _item = item;
            _isSelected = isSelected;
            _iconPainter = new IconPainter(() => Invalidate(InvalidAction.Repaint));
        }

        private readonly ICompletionItem _item;
        private readonly State<bool> _isSelected;
        private readonly IconPainter _iconPainter;
        private Paragraph? _paragraph; //TODO: use TextPainter

        public override void Layout(float availableWidth, float availableHeight)
        {
            SetSize(availableWidth, availableHeight);
        }

        public override void Paint(Canvas canvas, IDirtyArea? area = null)
        {
            const float fontSize = 13f;
            const float x = 2f;
            const float y = 3f;
            _iconPainter.Paint(canvas, fontSize, Colors.Gray, GetIcon(_item.Kind), x, y);
            _paragraph ??= TextPainter.BuildParagraph(_item.Label, float.PositiveInfinity,
                fontSize, Colors.Black, null, 1, true);

            canvas.DrawParagraph(_paragraph!, x + 20, y);
        }

        private static IconData GetIcon(CompletionItemKind kind)
        {
            switch (kind)
            {
                case CompletionItemKind.Function:
                case CompletionItemKind.Method: return MaterialIcons.Functions;
                case CompletionItemKind.Event: return MaterialIcons.Bolt;
                default: return MaterialIcons.Title;
            }
        }
    }
}