using Mermaider.Models;
using Mermaider.Rendering;

namespace PixUI.Mermaid;

internal static class PixRenderer
{
    public static IPicture Render(PositionedGraph graph, DiagramType diagramType)
    {
        using var picRecorder = PictureRecorder.Create();
        using var canvas = picRecorder.BeginRecording(Rect.FromLTWH(0, 0, (float)graph.Width, (float)graph.Height));
        var edgeRadius = (float)PixDesignSystem.Spec.EdgeBendRadius;

        ClusterPalette? palette = null;
        // var palette = diagramType is DiagramType.Flowchart or DiagramType.State
        //     ? ClusterPalette.Build(graph, context.Styles.Colors).WithTint(ds.TintStrength)
        //     : null;

        //TODO: group body
        foreach (var group in graph.Groups) { }

        foreach (var edge in graph.Edges)
        {
            if (edge.Style == EdgeStyle.Invisible) continue;
            AppendEdge(edge, edgeRadius, canvas);
        }

        foreach (var edge in graph.Edges)
        {
            if (edge.Style != EdgeStyle.Invisible && !string.IsNullOrEmpty(edge.Label))
                AppendEdgeLabel(edge, canvas);
        }

        foreach (var node in graph.Nodes)
            AppendNode(node, canvas, palette);

        //TODO: group header
        foreach (var group in graph.Groups) { }

        return picRecorder.EndRecording();
    }

    #region ====Edge====

    private static void AppendEdge(PositionedEdge edge, float cornerRadius, ICanvas canvas)
    {
        if (edge.Points.Count < 2) return;

        //TODO: styles
        var strokeColor = Colors.Black;
        var strokeWidth = (float)PixDesignSystem.Spec.EdgeWidth;

        using var path = SvgRenderer.IsOrthogonal(edge.Points)
            ? BuildOrthogonalPath(edge.Points, cornerRadius)
            : BuildRoundedPath(edge.Points, cornerRadius);
        var paint = Paint.Shared(strokeColor, PaintStyle.Stroke, strokeWidth);
        paint.IsAntialias = true;
        canvas.DrawPath(path, paint);

        //start and end arrow
        if (edge.HasArrowStart)
        {
            var sourcePoint = new Point((float)edge.Points[0].X, (float)edge.Points[0].Y);
            var sourceSecondPoint = new Point((float)edge.Points[1].X, (float)edge.Points[1].Y);
            var marker = PixDesignSystem.Marker(MarkerShape.Arrow);
            marker.PaintTo(canvas, sourceSecondPoint,  sourcePoint);
        }

        if (edge.HasArrowEnd)
        {
            var targetPoint = new Point((float)edge.Points[^1].X, (float)edge.Points[^1].Y);
            var targetSecondPoint = new Point((float)edge.Points[^2].X, (float)edge.Points[^2].Y);
            var marker = PixDesignSystem.Marker(MarkerShape.Arrow);
            marker.PaintTo(canvas, targetSecondPoint, targetPoint);
        }
    }

    private static void AppendEdgeLabel(PositionedEdge edge, ICanvas canvas)
    {
        var mid = edge.LabelPosition ?? VisualLanguage.PathMidpoint(edge.Points);
        PixDesignSystem.AppendEdgeLabel((float)mid.X, (float)mid.Y, edge.Label!, canvas /*TODO: overrideColor*/);
    }

    /// <summary>
    /// Axis-aligned polyline with uniformly rounded corners; no bezier S-curves,
    /// so every edge shares one visual style.
    /// </summary>
    private static IPath BuildOrthogonalPath(IReadOnlyList<Mermaider.Models.Point> points, float radius)
    {
        using var path = PathBuilder.Create();

        // ── start ──
        path.MoveTo((float)points[0].X, (float)points[0].Y);

        // ── interior corners ──
        for (var i = 1; i < points.Count - 1; i++)
        {
            var prev = points[i - 1];
            var cur = points[i];
            var next = points[i + 1];

            // unit direction of incoming / outgoing edge (orthogonal ⇒ 0 or ±1)
            var inDx = Math.Sign(cur.X - prev.X);
            var inDy = Math.Sign(cur.Y - prev.Y);
            var outDx = Math.Sign(next.X - cur.X);
            var outDy = Math.Sign(next.Y - cur.Y);

            // collinear → skip
            if (inDx == outDx && inDy == outDy)
                continue;

            // Manhattan length of each leg (valid because edges are axis-aligned)
            var inLen = Math.Abs(cur.X - prev.X) + Math.Abs(cur.Y - prev.Y);
            var outLen = Math.Abs(next.X - cur.X) + Math.Abs(next.Y - cur.Y);

            var r = Math.Min(radius, Math.Min(inLen, outLen) * 0.5f);

            // radius too small → fall back to a sharp corner
            if (r < 0.5f)
            {
                path.LineTo((float)cur.X, (float)cur.Y);
                continue;
            }

            // 1) walk the incoming edge up to r before the corner
            var lineX = (float)(cur.X - inDx * r);
            var lineY = (float)(cur.Y - inDy * r);
            path.LineTo(lineX, lineY);

            // 2) quadratic Bézier: control = corner, end = r past the corner on outgoing edge
            var endX = (float)(cur.X + outDx * r);
            var endY = (float)(cur.Y + outDy * r);
            path.QuadTo((float)cur.X, (float)cur.Y, endX, endY);
        }

        // ── last point ──
        var last = points[^1];
        path.LineTo((float)last.X, (float)last.Y);

        return path.Detach();
    }

    /// <summary>
    /// Rounded-corner polyline.  4-point Z/S-bends are rendered as a single cubic
    /// Bézier so crossing edges (e.g. E→H and F→G) appear as curves that cross
    /// cleanly in the middle, matching mermaid.js visual style.
    /// </summary>
    private static IPath BuildRoundedPath(IReadOnlyList<Mermaider.Models.Point> points, float radius)
    {
        using var path = PathBuilder.Create();

        // ── 4-point Z/S-bend: render as one smooth cubic Bézier ──
        if (points.Count == 4)
        {
            var x0 = (float)points[0].X;
            var y0 = (float)points[0].Y;
            var x1 = (float)points[1].X;
            var y1 = (float)points[1].Y;
            var x2 = (float)points[2].X;
            var y2 = (float)points[2].Y;
            var x3 = (float)points[3].X;
            var y3 = (float)points[3].Y;

            // Vertical Z-bend:  x0≈x1,  y1≈y2,  x2≈x3,  x0≠x2
            var isVerticalZBend = Math.Abs(x0 - x1) < 3.0f
                                  && Math.Abs(y1 - y2) < 3.0f
                                  && Math.Abs(x2 - x3) < 3.0f
                                  && Math.Abs(x0 - x2) > 3.0f;
            if (isVerticalZBend)
            {
                var ym = (y0 + y3) * 0.5f;
                path.MoveTo(x0, y0);
                path.CubicTo(x0, ym, x3, ym, x3, y3);
                return path.Detach();
            }

            // Horizontal Z-bend (downward):  y0≈y1,  x1≈x2,  y2≈y3,  y2>y0,  x0≠x3
            var isHorizontalZBend = Math.Abs(y0 - y1) < 1.0f
                                    && Math.Abs(x1 - x2) < 1.0f
                                    && Math.Abs(y2 - y3) < 1.0f
                                    && y2 > y0 + 1.0f
                                    && Math.Abs(x0 - x3) > 1.0f;
            if (isHorizontalZBend)
            {
                var xm = (x0 + x3) * 0.5f;
                path.MoveTo(x0, y0);
                path.CubicTo(xm, y0, xm, y3, x3, y3);
                return path.Detach();
            }
        }

        // ── general rounded-corner polyline ──
        path.MoveTo((float)points[0].X, (float)points[0].Y);

        if (points.Count == 2)
        {
            path.LineTo((float)points[1].X, (float)points[1].Y);
            return path.Detach();
        }

        for (var i = 1; i < points.Count - 1; i++)
        {
            var prev = points[i - 1];
            var curr = points[i];
            var next = points[i + 1];

            var dx1 = (float)curr.X - (float)prev.X;
            var dy1 = (float)curr.Y - (float)prev.Y;
            var len1 = MathF.Sqrt(dx1 * dx1 + dy1 * dy1);

            var dx2 = (float)next.X - (float)curr.X;
            var dy2 = (float)next.Y - (float)curr.Y;
            var len2 = MathF.Sqrt(dx2 * dx2 + dy2 * dy2);

            // degenerate leg → sharp corner
            if (len1 < 0.1f || len2 < 0.1f)
            {
                path.LineTo((float)curr.X, (float)curr.Y);
                continue;
            }

            // collinear (cross product ≈ 0) → skip, avoids spurious Q on virtual-node waypoints
            var cross = dx1 * dy2 - dy1 * dx2;
            if (MathF.Abs(cross) < 0.1f)
                continue;

            var r = MathF.Min(radius, MathF.Min(len1 * 0.5f, len2 * 0.5f));

            if (r < 0.1f)
            {
                path.LineTo((float)curr.X, (float)curr.Y);
                continue;
            }

            // walk in-edge to r before corner, then quadratic Bézier through the corner
            var ux1 = dx1 / len1;
            var uy1 = dy1 / len1;
            var ux2 = dx2 / len2;
            var uy2 = dy2 / len2;

            var startX = (float)curr.X - ux1 * r;
            var startY = (float)curr.Y - uy1 * r;
            var endX = (float)curr.X + ux2 * r;
            var endY = (float)curr.Y + uy2 * r;

            path.LineTo(startX, startY);
            path.QuadTo((float)curr.X, (float)curr.Y, endX, endY);
        }

        // last point
        var last = points[^1];
        path.LineTo((float)last.X, (float)last.Y);

        return path.Detach();
    }

    #endregion

    #region ====Node====

    private static void AppendNode(PositionedNode node, ICanvas canvas, ClusterPalette? palette = null)
    {
        AppendNodeShape(node, canvas, palette);
        AppendNodeLabel(node, canvas);
    }

    private static void AppendNodeShape(PositionedNode node, ICanvas canvas, ClusterPalette? palette = null)
    {
        var (x, y, w, h) = ((float)node.X, (float)node.Y, (float)node.Width, (float)node.Height);
        //TODO: styles
        IPaint? stroke = Paint.Create(Colors.Black, PaintStyle.Stroke);
        stroke.StrokeWidth = (float)PixDesignSystem.Spec.OutlineWidth;
        stroke.IsAntialias = true;
        IPaint? fill = Paint.Create(Colors.Light(Colors.LightGray));
        fill.IsAntialias = true;
        var radius = (float)PixDesignSystem.Spec.NodeRadius;

        switch (node.Shape)
        {
            case NodeShape.Rectangle:
                AppendRect(x, y, w, h, radius, stroke, fill, canvas);
                break;
            case NodeShape.Rounded:
                AppendRect(x, y, w, h, Math.Min(radius + 6, h / 2), stroke, fill, canvas);
                break;
            case NodeShape.Stadium:
                AppendRect(x, y, w, h, h / 2, stroke, fill, canvas);
                break;
            case NodeShape.Diamond:
                AppendDiamond(x, y, w, h, stroke, fill, canvas);
                break;
            case NodeShape.Circle:
                AppendCircle(x, y, w, h, stroke, fill, canvas);
                break;
            case NodeShape.DoubleCircle:
                AppendDoubleCircle(x, y, w, h, stroke, fill, canvas);
                break;
            case NodeShape.Subroutine:
                AppendSubroutine(x, y, w, h, stroke, fill, canvas);
                break;
            case NodeShape.Hexagon:
                AppendHexagon(x, y, w, h, stroke, fill, canvas);
                break;
            case NodeShape.Cylinder:
                AppendCylinder(x, y, w, h, stroke, fill, canvas);
                break;
            case NodeShape.Asymmetric:
                AppendAsymmetric(x, y, w, h, stroke, fill, canvas);
                break;
            case NodeShape.Trapezoid:
                AppendTrapezoid(x, y, w, h, stroke, fill, canvas);
                break;
            case NodeShape.TrapezoidAlt:
                AppendTrapezoidAlt(x, y, w, h, stroke, fill, canvas);
                break;
            case NodeShape.Parallelogram:
                AppendParallelogram(x, y, w, h, true, stroke, fill, canvas);
                break;
            case NodeShape.ParallelogramAlt:
                AppendParallelogram(x, y, w, h, false, stroke, fill, canvas);
                break;
            case NodeShape.StateStart:
                PixDesignSystem.AppendTerminal(x + w / 2, y + h / 2, Math.Min(w, h) / 2 - 2f, false, canvas);
                break;
            case NodeShape.StateEnd:
                PixDesignSystem.AppendTerminal(x + w / 2, y + h / 2, Math.Min(w, h) / 2 - 2, true, canvas);
                break;
            case NodeShape.ForkJoin:
                AppendForkJoin(x, y, w, h, canvas);
                break;
        }

        stroke?.Dispose();
        fill?.Dispose();
    }

    private static void AppendRect(float x, float y, float w, float h, float radius,
        IPaint? stroke, IPaint? fill, ICanvas canvas)
    {
        var rect = Rect.FromLTWH(x, y, w, h);
        if (radius > 0)
        {
            var roundRect = RRect.FromRectAndRadius(rect, radius, radius);
            if (fill != null)
                canvas.DrawRRect(roundRect, fill);
            if (stroke != null)
                canvas.DrawRRect(roundRect, stroke);
        }
        else
        {
            if (fill != null)
                canvas.DrawRect(rect, fill);
            if (stroke != null)
                canvas.DrawRect(rect, stroke);
        }
    }

    private static void AppendDiamond(float x, float y, float w, float h, IPaint? stroke, IPaint? fill, ICanvas canvas)
    {
        var cx = x + w / 2;
        var cy = y + h / 2;
        var hw = w / 2;
        var hh = h / 2;

        using var pb = PathBuilder.Create();
        pb.MoveTo(cx, cy - hh);
        pb.LineTo(cx + hw, cy);
        pb.LineTo(cx, cy + hh);
        pb.LineTo(cx - hw, cy);
        pb.Close();
        using var path = pb.Detach();

        if (fill != null)
            canvas.DrawPath(path, fill);
        if (stroke != null)
            canvas.DrawPath(path, stroke);
    }

    private static void AppendCircle(float x, float y, float w, float h, IPaint? stroke, IPaint? fill, ICanvas canvas)
    {
        var cx = x + w / 2;
        var cy = y + h / 2;
        var r = Math.Min(w, h) / 2;

        if (fill != null)
            canvas.DrawCircle(cx, cy, r, fill);
        if (stroke != null)
            canvas.DrawCircle(cx, cy, r, stroke);
    }

    private static void AppendDoubleCircle(float x, float y, float w, float h, IPaint? stroke, IPaint? fill,
        ICanvas canvas)
    {
        var cx = x + w / 2;
        var cy = y + h / 2;
        var outerR = Math.Min(w, h) / 2;
        var innerR = outerR - 5;

        if (fill != null)
        {
            canvas.DrawCircle(cx, cy, outerR, fill);
            // canvas.DrawCircle(cx, cy, innerR, fill);
        }

        if (stroke != null)
        {
            canvas.DrawCircle(cx, cy, outerR, stroke);
            canvas.DrawCircle(cx, cy, innerR, stroke);
        }
    }

    private static void AppendSubroutine(float x, float y, float w, float h, IPaint? stroke, IPaint? fill,
        ICanvas canvas)
    {
        const int inset = 8;
        AppendRect(x, y, w, h, RenderConstants.Radii.Rectangle, stroke, fill, canvas);

        if (stroke != null)
        {
            canvas.DrawLine(x + inset, y, x + inset, y + h, stroke);
            canvas.DrawLine(x + w - inset, y, x + w - inset, y + h, stroke);
        }
    }

    private static void AppendHexagon(float x, float y, float w, float h, IPaint? stroke, IPaint? fill, ICanvas canvas)
    {
        var inset = h / 4;
        using var pb = PathBuilder.Create();
        pb.MoveTo(x + inset, y);
        pb.LineTo(x + w - inset, y);
        pb.LineTo(x + w, y + h / 2);
        pb.LineTo(x + w - inset, y + h);
        pb.LineTo(x + inset, y + h);
        pb.LineTo(x, y + h / 2);
        pb.Close();
        using var path = pb.Detach();
        if (fill != null)
            canvas.DrawPath(path, fill);
        if (stroke != null)
            canvas.DrawPath(path, stroke);
    }

    private static void AppendCylinder(float x, float y, float w, float h, IPaint? stroke, IPaint? fill, ICanvas canvas)
    {
        const int ry = 7;
        var cx = x + w / 2;
        var bodyTop = y + ry;
        var bodyH = h - 2 * ry;

        if (fill != null)
            canvas.DrawRect(Rect.FromLTWH(x, bodyTop, w, bodyH), fill);

        if (stroke != null)
        {
            canvas.DrawLine(x, bodyTop, x, bodyTop + bodyH, stroke);
            canvas.DrawLine(x + w, bodyTop, x + w, bodyTop + bodyH, stroke);
        }

        if (fill != null)
        {
            canvas.DrawOval(cx, y + h - ry, w / 2, ry, fill);
            canvas.DrawOval(cx, bodyTop, w / 2, ry, fill);
        }

        if (stroke != null)
        {
            canvas.DrawOval(cx, y + h - ry, w / 2, ry, stroke);
            canvas.DrawOval(cx, bodyTop, w / 2, ry, stroke);
        }
    }

    private static void AppendAsymmetric(float x, float y, float w, float h, IPaint? stroke, IPaint? fill,
        ICanvas canvas)
    {
        const int indent = 12;
        using var pb = PathBuilder.Create();
        pb.MoveTo(x + indent, y);
        pb.LineTo(x + w, y);
        pb.LineTo(x + w, y + h);
        pb.LineTo(x + indent, y + h);
        pb.LineTo(x, y + h / 2);
        pb.Close();
        using var path = pb.Detach();
        if (fill != null)
            canvas.DrawPath(path, fill);
        if (stroke != null)
            canvas.DrawPath(path, stroke);
    }

    private static void AppendTrapezoid(float x, float y, float w, float h, IPaint? stroke, IPaint? fill,
        ICanvas canvas)
    {
        var inset = w * 0.15f;
        using var pb = PathBuilder.Create();
        pb.MoveTo(x + inset, y);
        pb.LineTo(x + w - inset, y);
        pb.LineTo(x + w, y + h);
        pb.LineTo(x, y + h);
        pb.Close();
        using var path = pb.Detach();
        if (fill != null)
            canvas.DrawPath(path, fill);
        if (stroke != null)
            canvas.DrawPath(path, stroke);
    }

    private static void AppendTrapezoidAlt(float x, float y, float w, float h, IPaint? stroke, IPaint? fill,
        ICanvas canvas)
    {
        var inset = w * 0.15f;
        using var pb = PathBuilder.Create();
        pb.MoveTo(x, y);
        pb.LineTo(x + w, y);
        pb.LineTo(x + w - inset, y + h);
        pb.LineTo(x + inset, y + h);
        pb.Close();
        using var path = pb.Detach();
        if (fill != null)
            canvas.DrawPath(path, fill);
        if (stroke != null)
            canvas.DrawPath(path, stroke);
    }

    private static void AppendParallelogram(float x, float y, float w, float h, bool leansRight,
        IPaint? stroke, IPaint? fill, ICanvas canvas)
    {
        var lean = w * 0.15f;
        var (topLeft, topRight, bottomRight, bottomLeft) = leansRight
            ? (x + lean, x + w, x + w - lean, x)
            : (x, x + w - lean, x + w, x + lean);

        using var pb = PathBuilder.Create();
        pb.MoveTo(topLeft, y);
        pb.LineTo(topRight, y);
        pb.LineTo(bottomRight, y + h);
        pb.LineTo(bottomLeft, y + h);
        pb.Close();
        using var path = pb.Detach();
        if (fill != null)
            canvas.DrawPath(path, fill);
        if (stroke != null)
            canvas.DrawPath(path, stroke);
    }

    private static void AppendForkJoin(float x, float y, float w, float h, ICanvas canvas)
    {
        var fillColor = Colors.Black; //TODO: var(--_line-strong)
        var fill = Paint.Shared(fillColor);
        var rect = Rect.FromLTWH(x, y, w, h);
        var rounded = RRect.FromRectAndRadius(rect, 2, 2);
        canvas.DrawRRect(rounded, fill);
    }

    private static void AppendNodeLabel(PositionedNode node, ICanvas canvas)
    {
        if (node.Shape is NodeShape.StateStart or NodeShape.StateEnd or NodeShape.ForkJoin &&
            string.IsNullOrEmpty(node.Label))
            return;

        var cx = (float)(node.X + node.Width / 2);
        var cy = (float)(node.Y + node.Height / 2);
        //TODO: styles
        var textColor = Colors.Black;

        //TODO: markdown
        //if (node.IsMarkdown)
        PixDesignSystem.AppendText(node.Label, cx, cy, TypeRole.Label, canvas, textColor);
    }

    #endregion
}