using System;
using PixUI;

namespace CodeEditor
{
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
            using var ts = new TextStyle() { Color = Theme.LineNumberColor };
            for (var i = 0; i < 10; i++)
            {
                using var ps = new ParagraphStyle() { MaxLines = 1 };
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

        internal override void Paint(Canvas canvas, Rect rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0) return;

            // background
            var paint = PaintUtils.Shared(Theme.LineBgColor);
            canvas.DrawRect(rect, paint);

            // line numbers
            var lineHeight = TextEditor.TextView.FontHeight;
            var visibleLineRemainder = TextEditor.TextView.VisibleLineDrawingRemainder;
            var maxHeight = (int)((Bounds.Height + visibleLineRemainder) / lineHeight) + 1;
            for (var y = 0; y < maxHeight; y++)
            {
                var yPos = Bounds.Top + lineHeight * y - visibleLineRemainder +
                           Theme.LineSpace;
                if (rect.IntersectsWith(Bounds.Left, yPos, Bounds.Width, lineHeight))
                {
                    var curLine = Document.GetFirstLogicalLine(
                        Document.GetVisibleLine(TextEditor.TextView.FirstVisibleLine) + y);
                    if (curLine < Document.TotalNumberOfLines)
                        DrawLineNumber(canvas, curLine + 1, yPos);
                }
            }
        }

        private void DrawLineNumber(Canvas canvas, int lineNumber, float yPos)
        {
            //TODO:暂计算至千位
            var unitPlace = lineNumber % 10;

#if __WEB__
            var tenPlace = (int)(lineNumber / 10) % 10;
            var hundredPlace = (int)(lineNumber / 100f) % 10;
            var thousandPlace = (int)(lineNumber / 1000f) % 10;
#else
            var tenPlace = lineNumber / 10 % 10;
            var hundredPlace = lineNumber / 100 % 10;
            var thousandPlace = lineNumber / 1000 % 10;
#endif

            canvas.DrawParagraph(_numberCache[unitPlace], 2 + _numberWidth * 3, yPos);
            if (lineNumber >= 10)
                canvas.DrawParagraph(_numberCache[tenPlace], 2 + _numberWidth * 2, yPos);
            if (lineNumber >= 100)
                canvas.DrawParagraph(_numberCache[hundredPlace], 2 + _numberWidth, yPos);
            if (lineNumber >= 1000)
                canvas.DrawParagraph(_numberCache[thousandPlace], 2, yPos);
        }
    }
}