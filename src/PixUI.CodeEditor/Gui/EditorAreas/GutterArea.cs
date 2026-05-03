using PixUI;

namespace CodeEditor;

internal sealed class GutterArea : EditorArea
{
    public GutterArea(TextEditor textEditor) : base(textEditor)
    {
        _numberCache = GenerateNumberCache();
        _numberWidth = _numberCache[7].LongestLine;
    }

    private readonly IParagraph[] _numberCache;
    private readonly float _numberWidth;

    private IParagraph[] GenerateNumberCache()
    {
        var cache = new IParagraph[10];
        using var ts = TextStyle.Create();
        ts.Color = Theme.LineNumberColor;
        for (var i = 0; i < 10; i++)
        {
            using var ps = ParagraphStyle.Create();
            ps.MaxLines = 1;
            using var pb = ParagraphBuilder.Create(ps);
            pb.PushStyle(ts);
            pb.AddText(i.ToString());
            var ph = pb.Build();
            ph.Layout(float.MaxValue);
            cache[i] = ph;
        }

        return cache;
    }

    internal override Size Size => new Size(_numberWidth * 5, -1);

    internal override void Paint(ICanvas canvas, int[] viewLines)
    {
        if (Bounds.Width <= 0 || Bounds.Height <= 0) return;

        // background
        var paint = PixUI.Paint.Shared(Theme.LineBgColor);
        canvas.DrawRect(Bounds, paint);

        // line numbers
        var lineHeight = TextEditor.TextView.FontHeight;
        var visibleLineRemainder = TextEditor.TextView.VisibleLineDrawingRemainder;
        for (var i = 0; i < viewLines.Length; i++)
        {
            var yPos = Bounds.Top + lineHeight * i - visibleLineRemainder + Theme.LineSpace;
            DrawLineNumber(canvas, viewLines[i] + 1, yPos);
        }
    }

    private void DrawLineNumber(ICanvas canvas, int lineNumber, float yPos)
    {
        var xPos = Bounds.Left + 2;

        //TODO:暂计算至千位
        var unitPlace = lineNumber % 10;
        var tenPlace = lineNumber / 10 % 10;
        var hundredPlace = lineNumber / 100 % 10;
        var thousandPlace = lineNumber / 1000 % 10;

        canvas.DrawParagraph(_numberCache[unitPlace], xPos + _numberWidth * 3, yPos);
        if (lineNumber >= 10)
            canvas.DrawParagraph(_numberCache[tenPlace], xPos + _numberWidth * 2, yPos);
        if (lineNumber >= 100)
            canvas.DrawParagraph(_numberCache[hundredPlace], xPos + _numberWidth, yPos);
        if (lineNumber >= 1000)
            canvas.DrawParagraph(_numberCache[thousandPlace], xPos, yPos);
    }
}