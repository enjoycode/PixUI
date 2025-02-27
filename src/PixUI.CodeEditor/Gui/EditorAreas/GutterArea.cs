using PixUI;

namespace CodeEditor;

internal sealed class GutterArea : EditorArea
{
    public GutterArea(TextEditor textEditor) : base(textEditor)
    {
        _numberCache = GenerateNumberCache();
        _numberWidth = _numberCache[7].LongestLine;
    }

    private readonly Paragraph[] _numberCache;
    private readonly float _numberWidth;

    private Paragraph[] GenerateNumberCache()
    {
        var cache = new Paragraph[10];
        using var ts = new TextStyle();
        ts.Color = Theme.LineNumberColor;
        for (var i = 0; i < 10; i++)
        {
            using var ps = new ParagraphStyle();
            ps.MaxLines = 1;
            using var pb = new ParagraphBuilder(ps);
            pb.PushStyle(ts);
            pb.AddText(i.ToString());
            var ph = pb.Build();
            ph.Layout(float.MaxValue);
            cache[i] = ph;
        }

        return cache;
    }

    internal override Size Size => new Size(_numberWidth * 5, -1);

    internal override void Paint(Canvas canvas, Rect rect, int[] viewLines)
    {
        if (rect.Width <= 0 || rect.Height <= 0) return;

        // background
        var paint = PixUI.Paint.Shared(Theme.LineBgColor);
        canvas.DrawRect(rect, paint);

        // line numbers
        var lineHeight = TextEditor.TextView.FontHeight;
        var visibleLineRemainder = TextEditor.TextView.VisibleLineDrawingRemainder;
        for (var i = 0; i < viewLines.Length; i++)
        {
            var yPos = Bounds.Top + lineHeight * i - visibleLineRemainder + Theme.LineSpace;
            DrawLineNumber(canvas, viewLines[i] + 1, yPos);
        }
    }

    private void DrawLineNumber(Canvas canvas, int lineNumber, float yPos)
    {
        //TODO:暂计算至千位
        var unitPlace = lineNumber % 10;
        var tenPlace = lineNumber / 10 % 10;
        var hundredPlace = lineNumber / 100 % 10;
        var thousandPlace = lineNumber / 1000 % 10;

        canvas.DrawParagraph(_numberCache[unitPlace], 2 + _numberWidth * 3, yPos);
        if (lineNumber >= 10)
            canvas.DrawParagraph(_numberCache[tenPlace], 2 + _numberWidth * 2, yPos);
        if (lineNumber >= 100)
            canvas.DrawParagraph(_numberCache[hundredPlace], 2 + _numberWidth, yPos);
        if (lineNumber >= 1000)
            canvas.DrawParagraph(_numberCache[thousandPlace], 2, yPos);
    }
}