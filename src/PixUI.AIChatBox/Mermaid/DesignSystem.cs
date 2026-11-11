using Mermaider.Rendering;
using Mermaider.Theming;

namespace PixUI.Mermaid;

public static class DesignSystem
{
    internal static StyleSpec Spec { get; } = StyleSpec.Quiet;

    /// <summary>
    /// A start (filled) or end (ring + core) terminal in the accent: dot, square or disc per preset.
    /// </summary>
    internal static void AppendTerminal(float cx, float cy, float r, bool end, ICanvas canvas)
    {
        var accentColor = Colors.Magenta; //TODO: ColorFamily.AccentBase;
        if (Spec.Terminal == TerminalKind.Square)
        {
            var s = r * 1.6f;
            if (end)
            {
                var rect = Rect.FromLTWH(cx - s / 2, cy - s / 2, s, s);
                var stroke = Paint.Shared(accentColor, PaintStyle.Stroke, 1.5f);
                canvas.DrawRect(rect, stroke);

                var inner = s * 0.5f;
                rect = Rect.FromLTWH(cx - inner / 2, cy - inner / 2, inner, inner);
                var fill = Paint.Shared(accentColor);
                canvas.DrawRect(rect, fill);
            }
            else
            {
                var rect = Rect.FromLTWH(cx - s / 2, cy - s / 2, s, s);
                var fill = Paint.Shared(accentColor);
                canvas.DrawRect(rect, fill);
            }

            return;
        }

        var rr = Spec.Terminal == TerminalKind.Disc ? Math.Min(r, 8) : r;
        if (end)
        {
            var stroke = Paint.Shared(accentColor, PaintStyle.Stroke, 2f);
            canvas.DrawCircle(cx, cy, rr, stroke);
            var fill = Paint.Shared(accentColor);
            canvas.DrawCircle(cx, cy, Math.Max(2, rr - 4), fill);
        }
        else
        {
            var fill = Paint.Shared(accentColor);
            canvas.DrawCircle(cx, cy, rr, fill);
        }
    }

    internal static void AppendText(string text, float cx, float cy, TypeRole role, Color color, ICanvas canvas)
    {
        var fontSize = (float)Mermaider.Rendering.DesignSystem.Px(role);
        using var ph = TextPainter.BuildParagraph(text, float.MaxValue, fontSize, color);
        AppendParagraph(ph, cx, cy, canvas);
    }

    private static void AppendParagraph(IParagraph ph, float cx, float cy, ICanvas canvas)
    {
        var x = cx - ph.LongestLine / 2;
        var y = cy - ph.Height / 2;
        canvas.DrawParagraph(ph, x, y);
    }

    internal static void AppendEdgeLabel(float cx, float cy, string text, ICanvas canvas, Color? colorOverride = null)
    {
        using var ps = ParagraphStyle.Create();
        using var ts = TextStyle.Create();
        ts.FontSize = (float)Mermaider.Rendering.DesignSystem.Px(TypeRole.Caption);
        ts.FontStyle = new FontStyle(FontWeight.Medium, FontSlant.Upright);
        ts.Color = Colors.Black; //TODO:
        using var pb = ParagraphBuilder.Create(ps);
        pb.PushStyle(ts);
        pb.AddText(text);
        pb.Pop();
        using var ph = pb.Build();
        ph.Layout(float.MaxValue);

        var w = (float)Mermaider.Rendering.DesignSystem.LabelBoxWidth(ph.LongestLine);
        var h = Math.Max((float)Mermaider.Rendering.DesignSystem.PillHeight, ph.Height + 6);
        var r = Spec.Label == LabelKind.Chip ? Math.Min(6f, h / 2) : h / 2;

        var edgeColor = Colors.Black; //TODO: var(--_line)
        var fillColor = Colors.LightGray; //TODO: var(--bg)
        var fillBadgeColor = Colors.LightGray; //TODO: var(--_key-badge)
        switch (Spec.Label)
        {
            case LabelKind.Pill:
            {
                var rect = RRect.FromRectAndRadius(Rect.FromLTWH(cx - (w / 2), cy - (h / 2), w, h), r, r);
                var fill = Paint.Shared(fillColor);
                fill.IsAntialias = true;
                canvas.DrawRRect(rect, fill);
                var stroke = Paint.Shared(edgeColor, PaintStyle.Stroke, (float)Spec.EdgeWidth);
                stroke.IsAntialias = true;
                canvas.DrawRRect(rect, stroke);
                break;
            }
            case LabelKind.Halo:
            {
                // a borderless page-coloured knock-out: the halo alone lets the line show through the gaps between words
                var rect = Rect.FromLTWH(cx - (ph.LongestLine / 2) - 3, cy - (ph.Height / 2) - 1, ph.LongestLine + 6,
                    ph.Height + 2);
                var fill = Paint.Shared(fillColor);
                canvas.DrawRect(rect, fill);
                break;
            }
            case LabelKind.Chip:
            {
                var rect = RRect.FromRectAndRadius(Rect.FromLTWH(cx - (w / 2), cy - (h / 2), w, h), r, r);
                var fill = Paint.Shared(fillBadgeColor);
                fill.IsAntialias = true;
                canvas.DrawRRect(rect, fill);
                break;
            }
        }

        AppendParagraph(ph, cx, cy, canvas);
    }
}