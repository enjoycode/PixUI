using System.Diagnostics.CodeAnalysis;

namespace PixUI;

public sealed unsafe class SKCanvas : SKObject, ICanvas
{
    // private const int PatchCornerCount = 4;
    // private const int PatchCubicsCount = 12;
    // private const double RadiansCircle = 2.0 * Math.PI;
    private const double DegreesCircle = 360.0;

    public readonly SKSurface? Surface;

    private SKCanvas(SKSurface surface, IntPtr handle, bool owns) : base(handle, owns)
    {
        Surface = surface;
    }

    private SKCanvas(IntPtr handle, bool owns) : base(handle, owns) { }

    protected override void DisposeNative() => SkiaApi.sk_canvas_destroy(Handle);

    internal static SKCanvas? GetObject(SKSurface surface, IntPtr handle, bool owns = true,
        bool unrefExisting = true) =>
        GetOrAddObject(handle, owns, unrefExisting, (h, o) => new SKCanvas(surface, h, o));

    internal static SKCanvas? GetObject(IntPtr handle, bool owns = true, bool unrefExisting = true) =>
        GetOrAddObject(handle, owns, unrefExisting, (h, o) => new SKCanvas(h, o));

    #region ====Draw Methods====

    public void DrawLine(float x0, float y0, float x1, float y1, IPaint paint) =>
        SkiaApi.sk_canvas_draw_line(Handle, x0, y0, x1, y1, ((SKPaint)paint).Handle);

    public void DrawLine(Point x, Point y, IPaint paint) =>
        SkiaApi.sk_canvas_draw_line(Handle, x.X, x.Y, y.X, y.Y, ((SKPaint)paint).Handle);

    public void DrawRect(Rect rect, IPaint paint) =>
        SkiaApi.sk_canvas_draw_rect(Handle, &rect, ((SKPaint)paint).Handle);

    public void DrawRect(float x, float y, float w, float h, IPaint paint) =>
        DrawRect(Rect.FromLTWH(x, y, w, h), paint);

    public void DrawRect(RectI rect, IPaint paint) =>
        DrawRect(rect.Left, rect.Top, rect.Width, rect.Height, paint);

    public void DrawRRect(RRect rect, IPaint paint)
    {
        if (paint == null)
            throw new ArgumentNullException(nameof(paint));

        if (rect.IsEmpty())
            return;
        SkiaApi.sk_canvas_draw_rrect(Handle, new IntPtr(&rect), ((SKPaint)paint).Handle);
    }

    // ReSharper disable once InconsistentNaming
    public void DrawDRRect(RRect outer, RRect inner, IPaint paint)
    {
        if (paint == null)
            throw new ArgumentNullException(nameof(paint));

        SkiaApi.sk_canvas_draw_drrect(Handle, new IntPtr(&outer), new IntPtr(&inner), ((SKPaint)paint).Handle);
    }

    public void DrawOval(float cx, float cy, float rx, float ry, IPaint paint)
    {
        DrawOval(new Rect(cx - rx, cy - ry, cx + rx, cy + ry), paint);
    }

    public void DrawOval(Rect rect, IPaint paint)
    {
        if (paint == null)
            throw new ArgumentNullException(nameof(paint));
        SkiaApi.sk_canvas_draw_oval(Handle, &rect, ((SKPaint)paint).Handle);
    }

    public void DrawCircle(float cx, float cy, float radius, IPaint paint)
    {
        if (paint == null)
            throw new ArgumentNullException(nameof(paint));
        SkiaApi.sk_canvas_draw_circle(Handle, cx, cy, radius, ((SKPaint)paint).Handle);
    }

    public void DrawArc(Rect oval, float startAngle, float sweepAngle, bool useCenter, IPaint paint)
    {
        if (paint == null)
            throw new ArgumentNullException(nameof(paint));

        const float toDegrees = (float)(180 / Math.PI);
        SkiaApi.sk_canvas_draw_arc(Handle, &oval, startAngle * toDegrees,
            sweepAngle * toDegrees, useCenter, ((SKPaint)paint).Handle);
    }

    public void DrawPath(IPath path, IPaint paint)
    {
        if (paint == null)
            throw new ArgumentNullException(nameof(paint));
        if (path == null)
            throw new ArgumentNullException(nameof(path));
        SkiaApi.sk_canvas_draw_path(Handle, ((SKPath)path).Handle, ((SKPaint)paint).Handle);
    }

    public void DrawString(string text, float x, float y, IFont font, Color color)
    {
        fixed (char* ptr = text)
        {
            SkiaApi.sk_canvas_draw_simple_text(Handle, ptr,
                new IntPtr(text.Length * 2), SKTextEncoding.Utf16,
                x, y, ((SKFont)font).Handle, SKPaint.Shared(color).Handle);
        }
    }

    public void DrawParagraph(IParagraph paragraph, float x, float y)
    {
        ((SKParagraph)paragraph).Paint(this, x, y);
    }

    public void DrawGlyph(ushort glyphId, float posX, float posY, float originX, float originY,
        IFont font, IPaint paint)
    {
        var pos = new Point(posX, posY);
        var origin = new Point(originX, originY);
        SkiaApi.sk_canvas_draw_glyph(Handle, glyphId, &pos, &origin,
            ((SKFont)font).Handle, ((SKPaint)paint).Handle);
    }

    public void DrawImage(IImage image, float x, float y, IPaint? paint = null)
    {
        if (image == null)
            throw new ArgumentNullException(nameof(image));
        SkiaApi.sk_canvas_draw_image(Handle, ((SKImage)image).Handle, x, y,
            (paint as SKPaint)?.Handle ?? IntPtr.Zero);
    }

    public void DrawImage(IImage image, Rect dest, IPaint? paint = null) =>
        DrawImage(image, Rect.FromLTWH(0, 0, image.Width, image.Height), dest, paint);

    public void DrawImage(IImage image, Rect source, Rect dest, IPaint? paint = null)
    {
        if (image == null)
            throw new ArgumentNullException(nameof(image));
        SkiaApi.sk_canvas_draw_image_rect(Handle, ((SKImage)image).Handle, &source, &dest,
            (paint as SKPaint)?.Handle ?? IntPtr.Zero);
    }

    public void DrawShadow(IPath path, Color color, float elevation, bool transparentOccluder, float devicePixelRatio)
        => SkiaApi.sk_canvas_draw_shadow(Handle, ((SKPath)path).Handle, (uint)color, elevation,
            transparentOccluder, devicePixelRatio);

    #endregion

    #region ====Clip====

    public bool IsClipEmpty => SkiaApi.sk_canvas_is_clip_empty(Handle);

    public Rect ClipBounds
    {
        get
        {
            Rect rect;
            var ok = SkiaApi.sk_canvas_get_local_clip_bounds(Handle, &rect);
            return ok ? rect : Rect.FromLTWH(-4194304, -4194304, 8388608, 8388608);
        }
    }

    public void ClipRect(Rect rect, ClipOp op = ClipOp.Intersect, bool antialias = false) =>
        SkiaApi.sk_canvas_clip_rect_with_operation(Handle, &rect, op, antialias);

    public void ClipRRect(RRect rRect, ClipOp op = ClipOp.Intersect, bool antialias = false) =>
        SkiaApi.sk_canvas_clip_rrect_with_operation(Handle, new IntPtr(&rRect), op, antialias);

    public void ClipPath(IPath path, ClipOp op, bool antialias)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path));

        SkiaApi.sk_canvas_clip_path_with_operation(Handle, ((SKPath)path).Handle, op, antialias);
    }

    #endregion

    #region ====Matrix====

    public void Translate(float dx, float dy)
    {
        if (dx == 0 && dy == 0)
            return;

        SkiaApi.sk_canvas_translate(Handle, dx, dy);
    }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public void Scale(float sx, float sy)
    {
        if (sx == 1 && sy == 1)
            return;
        SkiaApi.sk_canvas_scale(Handle, sx, sy);
    }

    public void Skew(float sx, float sy)
    {
        if (sx == 0 && sy == 0)
            return;

        SkiaApi.sk_canvas_skew(Handle, sx, sy);
    }

    public void RotateDegrees(float degrees)
    {
        if (degrees % DegreesCircle == 0)
            return;

        SkiaApi.sk_canvas_rotate_degrees(Handle, degrees);
    }

    public void RotateDegrees(float degrees, float px, float py)
    {
        if (degrees % DegreesCircle == 0)
            return;

        Translate(px, py);
        RotateDegrees(degrees);
        Translate(-px, -py);
    }

    public void Concat( /*ref*/ Matrix4 matrix)
    {
        // fixed (Matrix4* ptr = &matrix) {
        SkiaApi.sk_canvas_concat(Handle, &matrix);
        // }
    }

    public void SetMatrix(Matrix4 matrix) => SkiaApi.sk_canvas_set_matrix(Handle, &matrix);

    public void ResetMatrix() => SkiaApi.sk_canvas_reset_matrix(Handle);

    // public void Transform(Matrix4 matrix)
    // {
    //     Concat(new Matrix4(matrix.M0, matrix.M4, matrix.M8, matrix.M12,
    //         matrix.M1, matrix.M5, matrix.M9, matrix.M13,
    //         matrix.M2, matrix.M6, matrix.M10, matrix.M14,
    //         matrix.M3, matrix.M7, matrix.M11, matrix.M15));
    // }

    public Matrix3 GetTotalMatrix()
    {
        Matrix3 matrix;
        SkiaApi.sk_canvas_get_total_matrix(Handle, &matrix);
        return matrix;
    }

    #endregion

    #region ====Save & Restore====

    public int SaveCount => SkiaApi.sk_canvas_get_save_count(Handle);

    public int Save() => SkiaApi.sk_canvas_save(Handle);

    public int SaveLayer(IPaint? paint = null, Rect? bounds = null)
    {
        if (bounds == null)
            return SkiaApi.sk_canvas_save_layer(Handle, null, (paint as SKPaint)?.Handle ?? IntPtr.Zero);

        var rect = bounds.Value;
        return SkiaApi.sk_canvas_save_layer(Handle, &rect, (paint as SKPaint)?.Handle ?? IntPtr.Zero);
    }

    public void Restore() => SkiaApi.sk_canvas_restore(Handle);

    public void RestoreToCount(int count) => SkiaApi.sk_canvas_restore_to_count(Handle, count);

    #endregion

    #region ====Clear & Flush====

    public void Flush() => SkiaApi.sk_canvas_flush(Handle);

    public void Clear() => Clear(Color.Empty);

    public void Clear(Color color) => SkiaApi.sk_canvas_clear(Handle, (uint)color);

    #endregion
}