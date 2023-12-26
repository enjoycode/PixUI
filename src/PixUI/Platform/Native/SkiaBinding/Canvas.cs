#if !__WEB__
using System;
using System.Diagnostics.CodeAnalysis;

namespace PixUI
{
    public sealed unsafe class Canvas : SKObject
    {
        // private const int PatchCornerCount = 4;
        // private const int PatchCubicsCount = 12;
        // private const double RadiansCircle = 2.0 * Math.PI;
        private const double DegreesCircle = 360.0;

        public readonly SKSurface? Surface;

        private Canvas(SKSurface surface, IntPtr handle, bool owns) : base(handle, owns)
        {
            Surface = surface;
        }

        private Canvas(IntPtr handle, bool owns) : base(handle, owns) { }

        protected override void DisposeNative() => SkiaApi.sk_canvas_destroy(Handle);

        internal static Canvas? GetObject(SKSurface surface, IntPtr handle, bool owns = true,
            bool unrefExisting = true) =>
            GetOrAddObject(handle, owns, unrefExisting, (h, o) => new Canvas(surface, h, o));

        internal static Canvas? GetObject(IntPtr handle, bool owns = true, bool unrefExisting = true) =>
            GetOrAddObject(handle, owns, unrefExisting, (h, o) => new Canvas(h, o));


        #region ====Draw Methods====

        public void DrawLine(float x0, float y0, float x1, float y1, Paint paint) =>
            SkiaApi.sk_canvas_draw_line(Handle, x0, y0, x1, y1, paint.Handle);

        public void DrawRect(Rect rect, Paint paint) => SkiaApi.sk_canvas_draw_rect(Handle, &rect, paint.Handle);

        public void DrawRect(float x, float y, float w, float h, Paint paint) =>
            DrawRect(Rect.FromLTWH(x, y, w, h), paint);

        public void DrawRect(RectI rect, Paint paint) =>
            DrawRect(rect.Left, rect.Top, rect.Width, rect.Height, paint);

        public void DrawRRect(RRect rect, Paint paint)
        {
            if (rect == null)
                throw new ArgumentNullException(nameof(rect));
            if (paint == null)
                throw new ArgumentNullException(nameof(paint));
            SkiaApi.sk_canvas_draw_rrect(Handle, rect.Handle, paint.Handle);
        }

        // ReSharper disable once InconsistentNaming
        public void DrawDRRect(RRect outer, RRect inner, Paint paint)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (paint == null)
                throw new ArgumentNullException(nameof(paint));

            SkiaApi.sk_canvas_draw_drrect(Handle, outer.Handle, inner.Handle, paint.Handle);
        }

        public void DrawOval(float cx, float cy, float rx, float ry, Paint paint)
        {
            DrawOval(new Rect(cx - rx, cy - ry, cx + rx, cy + ry), paint);
        }

        public void DrawOval(Rect rect, Paint paint)
        {
            if (paint == null)
                throw new ArgumentNullException(nameof(paint));
            SkiaApi.sk_canvas_draw_oval(Handle, &rect, paint.Handle);
        }

        public void DrawCircle(float cx, float cy, float radius, Paint paint)
        {
            if (paint == null)
                throw new ArgumentNullException(nameof(paint));
            SkiaApi.sk_canvas_draw_circle(Handle, cx, cy, radius, paint.Handle);
        }

        public void DrawArc(Rect oval, float startAngle, float sweepAngle, bool useCenter,
            Paint paint)
        {
            if (paint == null)
                throw new ArgumentNullException(nameof(paint));

            const float toDegrees = (float)(180 / Math.PI);
            SkiaApi.sk_canvas_draw_arc(Handle, &oval, startAngle * toDegrees,
                sweepAngle * toDegrees, useCenter, paint.Handle);
        }

        public void DrawPath(Path path, Paint paint)
        {
            if (paint == null)
                throw new ArgumentNullException(nameof(paint));
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            SkiaApi.sk_canvas_draw_path(Handle, path.Handle, paint.Handle);
        }

        public void DrawString(string text, float x, float y, Font font, Color color)
        {
            fixed (char* ptr = text)
            {
                SkiaApi.sk_canvas_draw_simple_text(Handle, ptr, new IntPtr(text.Length * 2), SKTextEncoding.Utf16,
                    x, y, font.Handle, PixUI.Paint.Shared(color).Handle);
            }
        }

        public void DrawParagraph(Paragraph paragraph, float x, float y)
        {
            paragraph.Paint(this, x, y);
        }

        public void DrawGlyph(ushort glyphId, float posX, float posY, float originX, float originY,
            Font font, Paint paint)
        {
            var pos = new Point(posX, posY);
            var origin = new Point(originX, originY);
            SkiaApi.sk_canvas_draw_glyph(Handle, glyphId, &pos, &origin,
                font.Handle, paint.Handle);
        }

        public void DrawImage(Image image, float x, float y, Paint? paint = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            SkiaApi.sk_canvas_draw_image(Handle, image.Handle, x, y,
                paint?.Handle ?? IntPtr.Zero);
        }

        public void DrawImage(Image image, Rect dest, Paint? paint = null) =>
            DrawImage(image, Rect.FromLTWH(0, 0, image.Width, image.Height), dest, paint);


        public void DrawImage(Image image, Rect source, Rect dest, Paint? paint = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            SkiaApi.sk_canvas_draw_image_rect(Handle, image.Handle, &source, &dest,
                paint?.Handle ?? IntPtr.Zero);
        }

        public void DrawShadow(Path path, Color color, float elevation, bool transparentOccluder,
            float devicePixelRatio)
            => SkiaApi.sk_canvas_draw_shadow(Handle, path.Handle, (uint)color, elevation,
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

        public void ClipRect(Rect rect, ClipOp op, bool antialias) =>
            SkiaApi.sk_canvas_clip_rect_with_operation(Handle, &rect, op, antialias);

        public void ClipPath(Path path, ClipOp op, bool antialias)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            SkiaApi.sk_canvas_clip_path_with_operation(Handle, path.Handle, op, antialias);
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

        public int SaveLayer(Paint? paint = null, Rect? bounds = null)
        {
            if (bounds == null)
                return SkiaApi.sk_canvas_save_layer(Handle, null, paint?.Handle ?? IntPtr.Zero);

            var rect = bounds.Value;
            return SkiaApi.sk_canvas_save_layer(Handle, &rect, paint?.Handle ?? IntPtr.Zero);
        }

        public void Restore() => SkiaApi.sk_canvas_restore(Handle);

        public void RestoreToCount(int count) => SkiaApi.sk_canvas_restore_to_count(Handle, count);

        #endregion

        #region ====Clear====

        public void Clear() => Clear(Color.Empty);

        public void Clear(Color color) => SkiaApi.sk_canvas_clear(Handle, (uint)color);

        #endregion
    }
}
#endif