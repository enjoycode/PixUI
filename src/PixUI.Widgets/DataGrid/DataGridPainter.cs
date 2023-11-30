namespace PixUI;

public static class DataGridPainter
{
    public static Paragraph BuildCellParagraph(Rect rect, CellStyle style, string text, int maxLines)
    {
        using var ts = new TextStyle();
        ts.Color = style.Color ?? Colors.Black;
        ts.FontSize = style.FontSize;
        ts.FontStyle = new FontStyle(style.FontWeight, FontSlant.Upright);
        ts.Height = 1;

        var textAlign = style.HorizontalAlignment switch
        {
            HorizontalAlignment.Right => TextAlign.Right,
            HorizontalAlignment.Center => TextAlign.Center,
            _ => TextAlign.Left
        };

        using var ps = new ParagraphStyle();
        ps.MaxLines = (uint)maxLines;
        ps.TextStyle = ts;
        ps.Height = 1;
        ps.TextAlign = textAlign;
        using var pb = new ParagraphBuilder(ps);

        pb.PushStyle(ts);
        pb.AddText(text);
        pb.Pop();
        var ph = pb.Build();
        ph.Layout(rect.Width - CellStyle.CellPadding * 2);
        return ph;
    }

    /// <summary>
    /// 根据上下对齐方式画文本, 左右对齐已在Paragraph内处理
    /// </summary>
    public static void PaintCellParagraph(Canvas canvas, Rect rect, CellStyle style, Paragraph paragraph)
    {
        if (style.VerticalAlignment == VerticalAlignment.Middle)
        {
            var x = rect.Left;
            var y = rect.Top + (rect.Height - paragraph.Height) / 2;
            canvas.DrawParagraph(paragraph, x + CellStyle.CellPadding, y);
        }
        else if (style.VerticalAlignment == VerticalAlignment.Bottom)
        {
            var x = rect.Left;
            var y = rect.Bottom;
            canvas.DrawParagraph(paragraph, x + CellStyle.CellPadding, y - CellStyle.CellPadding - paragraph.Height);
        }
        else
        {
            canvas.DrawParagraph(paragraph, rect.Left + CellStyle.CellPadding, rect.Top + CellStyle.CellPadding);
        }
    }

    public static void PaintCellBorder(Canvas canvas, in Rect cellRect, Color borderColor /*TODO: use CellStyle*/)
    {
        var paint = PaintUtils.Shared(borderColor, PaintStyle.Stroke, 1);
        canvas.DrawRect(cellRect, paint);
    }
}