namespace PixUI.Mermaid;

/// <summary>
/// 模拟svg的marker
/// </summary>
public sealed class Marker : IDisposable
{
    /// <summary>Shape in the marker's own coordinate system.</summary>
    public IPath Path { get; set; } = null!;

    /// <summary>Rendered width in user-space units.</summary>
    public float Width { get; set; }

    /// <summary>Rendered height in user-space units.</summary>
    public float Height { get; set; }

    /// <summary>
    /// Anchor point (in the marker coordinate system) that is pinned to the
    /// line endpoint.  For a right-pointing triangle tip at (10, 3.5):
    /// <c>RefX = 10, RefY = 3.5</c>.
    /// </summary>
    public float RefX { get; set; }

    public float RefY { get; set; }

    public Color? StrokeColor { get; set; }
    public Color? FillColor { get; set; }
    public float StrokeWidth { get; set; } = 1f;
    public StrokeJoin StrokeJoin { get; set; } = StrokeJoin.Miter;
    public StrokeCap StrokeCap { get; set; } = StrokeCap.Butt;

    /// <summary>
    /// Draws anchored at <paramref name="end"/>, rotated to follow the  start → end  direction.
    /// </summary>
    public void PaintTo(ICanvas canvas, Point start, Point end)
    {
        // ── 0. degenerate guard ──
        float dx = end.X - start.X;
        float dy = end.Y - start.Y;
        if (dx == 0f && dy == 0f)
            return; // zero-length segment → nothing to orient

        // ── 1. direction angle (always "auto") ──
        float angleDeg = MathF.Atan2(dy, dx) * 180f / MathF.PI;

        // ── 2. transform chain ──
        //
        //   A point P in marker.Path is transformed as:
        //
        //     P → ① translate(-refX, -refY)   pull reference point to origin
        //       → ② rotate(θ)                align to segment direction
        //       → ③ translate(endX, endY)    move to the endpoint
        //
        //   Skia calls post-multiply into the CTM, so the *last* call is
        //   applied *first* to the geometry:

        canvas.Save();

        canvas.Translate(end.X, end.Y); // ③
        canvas.RotateDegrees(angleDeg); // ②
        canvas.Translate(-RefX, -RefY); // ①

        // ── 3. optional viewport clip (SVG default: overflow="hidden") ──
        // if (Width > 0f && Height > 0f) canvas.ClipRect(Rect.FromLTWH(0f, 0f, Width, Height));

        // ── 4. draw ──
        if (FillColor.HasValue)
        {
            var paint = Paint.Shared(FillColor.Value);
            paint.IsAntialias = true;
            canvas.DrawPath(Path, paint);
        }

        if (StrokeColor.HasValue)
        {
            var paint = Paint.Shared(StrokeColor.Value, PaintStyle.Stroke, StrokeWidth);
            paint.IsAntialias = true;
            paint.StrokeJoin = StrokeJoin;
            paint.StrokeCap = StrokeCap;
            canvas.DrawPath(Path, paint);
        }

        canvas.Restore();
    }

    public void Dispose() => Path?.Dispose();
}