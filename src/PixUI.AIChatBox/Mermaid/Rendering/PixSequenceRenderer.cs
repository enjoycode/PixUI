using Mermaider.Models;
using Mermaider.Rendering;
using Mermaider.Text;

namespace PixUI.Mermaid;

internal static class PixSequenceRenderer
{
    private const float DestroySize = 6;

    /// <summary>
    /// Space between a message line and the centre of its caption.
    /// </summary>
    private const float LabelLift = 14;

    /// <summary>
    /// Width the person glyph and its gap take in front of an actor's name (also reserved by the layout).
    /// </summary>
    internal const float GlyphWidth = 22;

    public static IPicture Render(PositionedSequenceDiagram diagram)
    {
        using var picRecorder = PictureRecorder.Create();
        using var canvas = picRecorder.BeginRecording(Rect.FromLTWH(0, 0, (float)diagram.Width, (float)diagram.Height));

        //TODO: styles
        // var clusterCount = diagram.Actors
        //     .Select(a => palette.Has(a.Id) ? palette.Family(a.Id).Key : a.Id)
        //     .Distinct()
        //     .Count();

        for (var i = 0; i < diagram.Boxes.Count; i++)
            AppendBox(diagram.Boxes[i], canvas /*palette.GroupFamily("box" + i), i, strict*/);

        foreach (var lifeline in diagram.Lifelines)
            AppendLifeline(lifeline, canvas);

        foreach (var message in diagram.Messages)
            AppendMessage(message, diagram.DestroyMarkers, canvas);

        foreach (var actor in diagram.Actors)
            AppendActor(actor, canvas /*, palette.Has(actor.Id) ? palette.Family(actor.Id) : ds.Neutral*/);

        if (diagram.Actors.Count > 0)
        {
            var actorH = diagram.Actors[0].Height;
            var bottomActorY = diagram.Height - actorH - 30;
            foreach (var actor in diagram.Actors)
            {
                // a destroyed participant ends at its cross; mermaid draws no closing box for it
                if (diagram.DestroyMarkers.Any(dm => Math.Abs(dm.X - actor.X) < 0.5))
                    continue;
                AppendActor(actor with { Y = bottomActorY },
                    canvas, /*palette.Has(actor.Id) ? palette.Family(actor.Id) : ds.Neutral,*/ mirror: true);
            }
        }

        return picRecorder.EndRecording();
    }

    private static void AppendBox(PositionedSequenceBox box, ICanvas canvas) { }

    private static void AppendActor(PositionedSequenceActor actor, /*TODO: ColorFamily*/ ICanvas canvas,
        bool mirror = false)
    {
        PixDesignSystem.AppendBox((float)(actor.X - actor.Width / 2), (float)actor.Y, (float)actor.Width,
            (float)actor.Height, canvas /*family*/);

        var cy = (float)(actor.Y + actor.Height / 2);
        if (actor.Type == SequenceActorType.Actor)
        {
            // an actor is the same chip with a person glyph in the family ink in front of its name
            var textW = TextMetrics.MeasureMultiline(actor.Label.AsSpan(), DesignSystem.Px(TypeRole.Label),
                PixDesignSystem.Weight(TypeRole.Label)).Width;
            var left = (float)(actor.X - (GlyphWidth + textW) / 2);
            PixDesignSystem.AppendGlyph(DesignSystem.Glyph.Person, left + 8, cy, Colors.Black, canvas);
            PixDesignSystem.AppendText(actor.Label, left + GlyphWidth, cy, TypeRole.Label, canvas /*anchor: "start"*/);
        }
        else
        {
            PixDesignSystem.AppendText(actor.Label, (float)actor.X, cy, TypeRole.Label, canvas);
        }
    }

    private static void AppendLifeline(Lifeline lifeline, ICanvas canvas)
    {
        var strokeColor = Colors.Gray; //TODO: _line-soft
        var stroke = Paint.Shared(strokeColor, PaintStyle.Stroke, 1.25f);
        stroke.IsAntialias = true;
        using var dashEffect = PathEffect.CreateDash([4, 4], 10); //TODO: cache on DesignSystem
        stroke.PathEffect = dashEffect;

        canvas.DrawLine((float)lifeline.X, (float)lifeline.TopY, (float)lifeline.X, (float)lifeline.BottomY, stroke);
    }

    private static void AppendMessage(PositionedSequenceMessage msg, IReadOnlyList<PositionedDestroyMarker> destroys,
        ICanvas canvas)
    {
        // a message that destroys its target stops short of the cross instead of piercing it
        if (!msg.IsSelf && destroys.Any(d => Math.Abs(d.X - msg.X2) < 0.5 && Math.Abs(d.Y - msg.Y) < 0.5))
            msg = msg with { X2 = msg.X2 + (msg.X2 > msg.X1 ? -(DestroySize + 4) : DestroySize + 4) };

        var dashed = msg.LineStyle == SequenceLineStyle.Dashed;
        var dashEffect = dashed ? PixDesignSystem.DashArray : null;
        var shape = msg.ArrowHead == SequenceArrowHead.Filled ? MarkerShape.Arrow : MarkerShape.Open;
        var edgeColor = PixDesignSystem.EdgeColor;
        var edgeWidth = (float)PixDesignSystem.Spec.EdgeWidth;

        var stroke = Paint.Shared(edgeColor, PaintStyle.Stroke, edgeWidth);
        stroke.IsAntialias = true;
        stroke.PathEffect = dashEffect;

        var label = msg.Label;
        if (msg.IsSelf)
        {
            const double loopW = 30;
            const double loopH = 20;
            const double labelPadding = 8;

            using var pb = PathBuilder.Create();
            pb.MoveTo((float)msg.X1, (float)msg.Y);
            pb.LineTo((float)(msg.X1 + loopW), (float)msg.Y);
            pb.LineTo((float)(msg.X1 + loopW), (float)(msg.Y + loopH));
            pb.LineTo((float)msg.X2, (float)(msg.Y + loopH));
            using var path = pb.Detach();

            stroke.StrokeJoin = StrokeJoin.Round;
            canvas.DrawPath(path, stroke);

            AppendAutoNumberBadge(ref label, (float)msg.X1, (float)msg.Y, canvas);
            if (label.Length > 0)
            {
                var labelW = DesignSystem.LabelBoxWidth(TextMetrics
                    .MeasureMultiline(label.AsSpan(), DesignSystem.Px(TypeRole.Caption), 500).Width);
                PixDesignSystem.AppendEdgeLabel((float)(msg.X1 + loopW + labelPadding + labelW / 2),
                    (float)(msg.Y + loopH / 2), label, canvas);
            }
        }
        else
        {
            canvas.DrawLine((float)msg.X1, (float)msg.Y, (float)msg.X2, (float)msg.Y, stroke);

            var marker = PixDesignSystem.Marker(shape);
            marker.PaintTo(canvas, new((float)msg.X1, (float)msg.Y), new((float)msg.X2, (float)msg.Y));

            AppendAutoNumberBadge(ref label, (float)msg.X1, (float)msg.Y, canvas);
            if (label.Length > 0)
            {
                // the same label pill every diagram uses, sitting just above its line
                PixDesignSystem.AppendEdgeLabel((float)(msg.X1 + msg.X2) / 2, (float)msg.Y - LabelLift, label, canvas);
            }
        }
    }

    private static void AppendAutoNumberBadge(ref string label, float x, float y, ICanvas canvas)
    {
        var dotIdx = label.IndexOf(". ", StringComparison.Ordinal);
        if (dotIdx is <= 0 or > 4)
            return;

        var numStr = label[..dotIdx];
        foreach (var c in numStr)
        {
            if (!char.IsDigit(c))
                return;
        }

        label = label[(dotIdx + 2)..];

        // a round accent marker on the message's start, number knocked out in the page colour
        const float r = 9;
        var strokeColor = Colors.Black; // TODO: --bg
        var accentBaseColor = Colors.Magenta; //TODO:ColorFamily.AccentBase
        var fill = Paint.Shared(accentBaseColor);
        canvas.DrawCircle(x, y, r, fill);
        var stroke = Paint.Shared(strokeColor, PaintStyle.Stroke, 1.5f);
        canvas.DrawCircle(x, y, r, stroke);
        PixDesignSystem.AppendText(numStr, x, y, TypeRole.Tag, canvas, strokeColor);
    }
}