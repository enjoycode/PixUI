using Markdig.Syntax;
using TextMateSharp.Grammars;
using TextMateSharp.Internal.Grammars;

namespace PixUI.AIChatBox;

internal sealed class MarkdownCodeBlock : MarkdownBlock
{
    private const float _padding = 2f;
    private const float _doublePadding = _padding * 2f;
    private const float _cmdBarHeight = 20;
    private readonly List<CodeLineInfo> _lines = [];
    private string? _language;
    private IParagraph? _languageParagraph;

    internal override float Height => _lines.Sum(l => l.Height) + _cmdBarHeight + _doublePadding;

    internal override void Layout(Block node, ChatBoxController controller)
    {
        foreach (var oldLine in _lines)
            oldLine.Paragraph?.Dispose();
        _lines.Clear();

        var codeNode = (FencedCodeBlock)node;
        _language = codeNode.Info;
        BuildLanguageParagraph();

        if (codeNode.Lines.Count == 0) return;

        //TODO: 因仅追加考虑只处理追加的行
        var gramma = CodeTokenizer.GetGramma(_language);
        if (gramma == null)
            BuildWithoutGramma(codeNode);
        else
            BuildWithGramma(codeNode, gramma);
    }

    private void BuildLanguageParagraph()
    {
        if (string.IsNullOrEmpty(_language)) return;

        using var pb = ParagraphBuilder.Create(ChatBoxTheme.ParagraphStyle);
        using var textStyle = TextStyle.Create();
        textStyle.Color = CodeTokenizer.EditorForeground;
        pb.PushStyle(textStyle);
        pb.AddText(_language);
        pb.Pop();
        _languageParagraph = pb.Build();
        _languageParagraph.Layout(float.MaxValue);
    }

    private void BuildWithoutGramma(FencedCodeBlock node)
    {
        using var pb = ParagraphBuilder.Create(ChatBoxTheme.ParagraphStyle);
        using var textStyle = TextStyle.Create();
        textStyle.Color = CodeTokenizer.EditorForeground;
        foreach (var line in node.Lines.Lines)
        {
            IParagraph? lineParagraph = null;
            if (!line.Slice.IsEmpty)
            {
                pb.PushStyle(textStyle);
                pb.AddText(line.Slice.AsSpan());
                pb.Pop();
                lineParagraph = pb.Build();
                lineParagraph.Layout(float.MaxValue);
            }

            _lines.Add(new CodeLineInfo() { Paragraph = lineParagraph });
        }
    }

    private void BuildWithGramma(FencedCodeBlock node, IGrammar grammar)
    {
        IStateStack? ruleStack = null;
        foreach (var line in node.Lines.Lines)
        {
            IParagraph? lineParagraph = null;
            var slice = line.Slice;
            if (slice is { Text: not null, IsEmpty: false })
            {
                using var pb = ParagraphBuilder.Create(ChatBoxTheme.ParagraphStyle);
                var span = slice.Text.AsMemory(slice.Start, slice.Length);
                var result = grammar.TokenizeLine2(span, ruleStack, TimeSpan.MaxValue);
                ruleStack = result.RuleStack;

                var tokens = result.Tokens;
                for (int i = 0; i < tokens.Length; i += 2)
                {
                    int startIndex = tokens[i];
                    int endIndex = i + 2 < tokens.Length ? tokens[i + 2] : slice.Length;
                    int metadata = tokens[i + 1];

                    int foregroundId = EncodedTokenAttributes.GetForeground(metadata);
                    // var fontStyle = EncodedTokenAttributes.GetFontStyle(metadata);
                    // int backgroundId = EncodedTokenAttributes.GetBackground(metadata);

                    var textColor = CodeTokenizer.GetColor(foregroundId);

                    var textStyle = TextStyle.Create();
                    textStyle.Color = textColor;
                    textStyle.FontSize = ChatBoxTheme.FontSize;
                    pb.PushStyle(textStyle);
                    pb.AddText(slice.AsSpan()[startIndex..endIndex]);
                    pb.Pop();
                }

                lineParagraph = pb.Build();
                lineParagraph.Layout(float.MaxValue);
            }

            _lines.Add(new CodeLineInfo() { Paragraph = lineParagraph });
        }
    }

    internal override void Relayout(float availableWidth) { }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        var background = CodeTokenizer.EditorBackground;

        //draw border and background
        var rect = Rect.FromLTWH(_padding, _padding, controller.Width - _doublePadding, Height - _doublePadding);
        var round = RRect.FromRectAndRadius(rect, 3, 3);
        canvas.DrawRRect(round, Paint.Shared(background));

        var offsetY = _padding;
        //draw command bar
        if (_languageParagraph != null)
            canvas.DrawParagraph(_languageParagraph, 5, offsetY);
        offsetY += _cmdBarHeight;
        canvas.DrawLine(rect.X, offsetY, rect.Right, offsetY,
            Paint.Shared(CodeTokenizer.EditorForeground, PaintStyle.Stroke));

        //draw lines
        foreach (var line in _lines)
        {
            canvas.Translate(0, offsetY);
            if (line.Paragraph != null)
                canvas.DrawParagraph(line.Paragraph, 5, 0);
            canvas.Translate(0, -offsetY);
            offsetY += line.Height;
        }
    }

    private readonly struct CodeLineInfo
    {
        public IParagraph? Paragraph { get; init; } //null = empty line
        public float Height => Paragraph?.Height ?? 16 /*TODO: empty line height*/;
    }
}