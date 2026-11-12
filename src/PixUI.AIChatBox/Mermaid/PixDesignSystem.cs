using Mermaider.Rendering;
using Mermaider.Theming;

namespace PixUI.Mermaid;

public static class PixDesignSystem
{
    internal static StyleSpec Spec { get; } = StyleSpec.Quiet;

    internal static Color FgColor { get; } = Colors.Black;
    internal static Color BgColor { get; } = Colors.Light(Colors.LightGray);
    internal static Color AccentColor { get; } = 0xFF3B82F6;
    internal static Color EdgeColor { get; } = Colors.Black;
    private static Color MarkerColor => Spec.Marker == MarkerKind.Thin ? AccentColor : EdgeColor;

    internal static readonly IPathEffect? DashArray = PathEffect.CreateDash([5f, 4f], 10);

    private static readonly Dictionary<string, Marker> _markers = [];

    internal static int Weight(TypeRole role) => role switch
    {
        TypeRole.Title => 700,
        TypeRole.Heading => Spec.HeadingWeight,
        TypeRole.Subheading => 600,
        TypeRole.Label => Spec.LabelWeight,
        TypeRole.Body or TypeRole.Meta => 400,
        TypeRole.Caption => 500,
        _ => 600,
    };

    /// <summary>
    /// Draws <paramref name="glyph"/> centred at (<paramref name="cx"/>, <paramref name="cy"/>)
    /// in <paramref name="ink"/>, about 20px across × <paramref name="scale"/>.
    /// </summary>
    internal static void AppendGlyph(DesignSystem.Glyph glyph, float cx, float cy, Color ink, ICanvas canvas,
        float scale = 1.0f)
    {
        //TODO:
    }

    internal static void AppendBox(float x, float y, float w, float h, ICanvas canvas, /*TODO: ColorFamily*/
        float? radius = null)
    {
        //TODO: styles
        var strokeColor = EdgeColor; //NodeStroke(family)
        var fillColor = BgColor; //NodeFill(family)

        var r = (float)(radius ?? Spec.NodeRadius);
        var rect = RRect.FromRectAndRadius(Rect.FromLTWH(x, y, w, h), r, r);
        var fill = Paint.Shared(fillColor);
        fill.IsAntialias = true;
        canvas.DrawRRect(rect, fill);
        var stroke = Paint.Shared(strokeColor, PaintStyle.Stroke, (float)Spec.OutlineWidth);
        stroke.IsAntialias = true;
        canvas.DrawRRect(rect, stroke);
    }

    /// <summary>
    /// A start (filled) or end (ring + core) terminal in the accent: dot, square or disc per preset.
    /// </summary>
    internal static void AppendTerminal(float cx, float cy, float r, bool end, ICanvas canvas)
    {
        var accentColor = AccentColor; //TODO: ColorFamily.AccentBase;
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
            stroke.IsAntialias = true;
            canvas.DrawCircle(cx, cy, rr, stroke);
            var fill = Paint.Shared(accentColor);
            fill.IsAntialias = true;
            canvas.DrawCircle(cx, cy, Math.Max(2, rr - 4), fill);
        }
        else
        {
            var fill = Paint.Shared(accentColor);
            fill.IsAntialias = true;
            canvas.DrawCircle(cx, cy, rr, fill);
        }
    }

    internal static void AppendText(string text, float cx, float cy, TypeRole role, ICanvas canvas, Color? color = null)
    {
        var textColor = color ?? Colors.Black;
        var fontSize = (float)DesignSystem.Px(role);
        using var ph = TextPainter.BuildParagraph(text, float.MaxValue, fontSize, textColor);
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
        ts.FontSize = (float)DesignSystem.Px(TypeRole.Caption);
        ts.FontStyle = new FontStyle(FontWeight.Medium, FontSlant.Upright);
        ts.Color = Colors.Black; //TODO:
        using var pb = ParagraphBuilder.Create(ps);
        pb.PushStyle(ts);
        pb.AddText(text);
        pb.Pop();
        using var ph = pb.Build();
        ph.Layout(float.MaxValue);

        var w = (float)DesignSystem.LabelBoxWidth(ph.LongestLine);
        var h = Math.Max((float)DesignSystem.PillHeight, ph.Height + 6);
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

    /// <summary>Returns a shared marker, registering its definition.</summary>
    /// <param name="shape">The marker shape.</param>
    /// <param name="accent">Draw it in the accent (story lines, highlighted edges) instead of the preset's marker colour.</param>
    internal static Marker Marker(MarkerShape shape, bool accent = false)
    {
        var id = $"mk-{(int)shape}--{accent}";
        if (_markers.TryGetValue(id, out var marker))
            return marker;
        var m = BuildMarker(shape, accent ? AccentColor : MarkerColor);
        _markers.Add(id, m);
        return m;
    }

    private static Marker BuildMarker(MarkerShape shape, Color color)
    {
        // geometry in a 12 × 12 box pointing right; start markers are mirrored
        var (size, half) = Spec.Marker switch
        {
            MarkerKind.Thin => (10.0f, 3.5f),
            MarkerKind.Chunky => (11.0f, 5.5f),
            _ => (10.0f, 4.5f),
        };
        var join = Spec.Marker == MarkerKind.Chunky ? StrokeJoin.Round : StrokeJoin.Miter;
        var sw = Spec.Marker == MarkerKind.Chunky ? 1.5f : 1f;
        var tip = size;
        var back = 0;
        var cy = 6.0f;

        var marker = new Marker();
        // build marker path by shape type
        switch (shape)
        {
            case MarkerShape.Arrow:
            {
                if (Spec.Marker == MarkerKind.Thin)
                {
                    using var pb = PathBuilder.Create();
                    pb.MoveTo(back, cy - half);
                    pb.LineTo(tip, cy);
                    pb.LineTo(back, cy + half);
                    marker.Path = pb.Detach();
                    marker.StrokeColor = color;
                    marker.StrokeWidth = 1.25f;
                    marker.StrokeJoin = join;
                    marker.StrokeCap = StrokeCap.Round;
                }
                else
                {
                    using var pb = PathBuilder.Create();
                    pb.MoveTo(back, cy - half);
                    pb.LineTo(tip, cy);
                    pb.LineTo(back, cy + half);
                    pb.Close();
                    marker.Path = pb.Detach();
                    marker.FillColor = color;
                    marker.StrokeColor = color;
                    marker.StrokeWidth = sw;
                    marker.StrokeJoin = join;
                }

                break;
            }
            case MarkerShape.Open:
            {
                using var pb = PathBuilder.Create();
                pb.MoveTo(back, cy - half);
                pb.LineTo(tip, cy);
                pb.LineTo(back, cy + half);
                marker.Path = pb.Detach();
                marker.StrokeColor = color;
                marker.StrokeWidth = sw + 0.25f;
                marker.StrokeJoin = join;
                marker.StrokeCap = StrokeCap.Round;
                break;
            }
            case MarkerShape.Triangle:
            {
                using var pb = PathBuilder.Create();
                pb.MoveTo(back, cy - half - 1);
                pb.LineTo(tip, cy);
                pb.LineTo(back, cy + half + 1);
                pb.Close();
                marker.Path = pb.Detach();
                marker.FillColor = BgColor;
                marker.StrokeColor = color;
                marker.StrokeWidth = sw + 0.25f;
                marker.StrokeJoin = join;
                break;
            }
            case MarkerShape.Diamond:
            {
                using var pb = PathBuilder.Create();
                pb.MoveTo(0, cy);
                pb.LineTo(size / 2 + 1, cy - half);
                pb.LineTo(size + 2, cy);
                pb.LineTo(size / 2 + 1, cy + half);
                pb.Close();
                marker.Path = pb.Detach();
                marker.FillColor = color;
                marker.StrokeColor = color;
                marker.StrokeWidth = sw;
                marker.StrokeJoin = join;
                break;
            }
            case MarkerShape.DiamondHollow:
            {
                using var pb = PathBuilder.Create();
                pb.MoveTo(0, cy);
                pb.LineTo(size / 2 + 1, cy - half);
                pb.LineTo(size + 2, cy);
                pb.LineTo(size / 2 + 1, cy + half);
                pb.Close();
                marker.Path = pb.Detach();
                marker.FillColor = BgColor;
                marker.StrokeColor = color;
                marker.StrokeWidth = sw + 0.25f;
                marker.StrokeJoin = join;
                break;
            }
            default:
            {
                using var pb = PathBuilder.Create();
                pb.AddCircle(size / +1, cy, half);
                marker.Path = pb.Detach();
                marker.FillColor = BgColor;
                marker.StrokeColor = color;
                marker.StrokeWidth = sw = 0.25f;
                break;
            }
        }

        var w = shape is MarkerShape.Diamond or MarkerShape.DiamondHollow or MarkerShape.Circle ? size + 2 : size;
        var refX = shape is MarkerShape.Diamond or MarkerShape.DiamondHollow or MarkerShape.Circle ? w : tip;
        marker.Width = w + 1;
        marker.Height = 12f;
        marker.RefX = refX;
        marker.RefY = cy;
        return marker;
    }
}