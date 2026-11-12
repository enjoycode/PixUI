using Mermaider.Models;
using Mermaider.Rendering;

namespace PixUI.Mermaid;

internal static class PixSequenceRenderer
{
    private const float DestroySize = 6;

    /// <summary>
    /// Space between a message line and the centre of its caption.
    /// </summary>
    private const float LabelLift = 14;

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

        return picRecorder.EndRecording();
    }

    private static void AppendBox(PositionedSequenceBox box, ICanvas canvas) { }

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
        var shape = msg.ArrowHead == SequenceArrowHead.Filled ? MarkerShape.Arrow : MarkerShape.Open;
        IPathEffect? dashEffect = dashed ? PathEffect.CreateDash([5f, 4f], 10) : null; //TODO: cache on DesignSystem

        var label = msg.Label;
        if (msg.IsSelf)
        {
            const double loopW = 30;
            const double loopH = 20;
            const double labelPadding = 8;
            throw new NotImplementedException();
        }
        else
        {
            var edgeColor = PixDesignSystem.EdgeColor;
            var edgeWidth = (float)PixDesignSystem.Spec.EdgeWidth;

            var stroke = Paint.Shared(edgeColor, PaintStyle.Stroke, edgeWidth);
            stroke.IsAntialias = true;
            stroke.PathEffect = dashEffect;
            stroke.StrokeJoin = StrokeJoin.Round;
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

        dashEffect?.Dispose();
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
        PixDesignSystem.AppendText(numStr, x, y, TypeRole.Tag, strokeColor, canvas);
    }
}