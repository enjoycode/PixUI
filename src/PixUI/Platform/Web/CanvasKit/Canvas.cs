#if __WEB__
namespace PixUI
{
    [TSType("CanvasKit.Canvas")]
    public sealed class Canvas
    {
        private Canvas() { }

        [TSRename("save")]
        public void Save() { }

        [TSRename("saveLayer")]
        public int SaveLayer(Paint? paint = null, in Rect? bounds = null) => 0;

        [TSRename("restore")]
        public void Restore() { }

        [TSRename("clear")]
        public void Clear(in Color color) { }

        [TSRename("translate")]
        public void Translate(float x, float y) { }

        [TSRename("scale")]
        public void Scale(float sx, float sy) { }

        [TSRename("skew")]
        public void Skew(float sx, float sy) {}

        [TSTemplate("{0}.rotate({1}, 0, 0)")]
        public void RotateDegrees (float degrees) { }
        
        [TSRename("rotate")]
        public void RotateDegrees (float degrees, float px, float py) {}

        [TSTemplate("{0}.concat({1}.TransponseTo())")]
        public void Concat(in Matrix4 matrix) { }

        [TSRename("clipRect")]
        public void ClipRect(in Rect rect, ClipOp op, bool antialias) { }

        [TSRename("clipPath")]
        public void ClipPath(Path path, ClipOp op, bool antialias) { }

        [TSRename("drawLine")]
        public void DrawLine(float x0, float y0, float x1, float y1, Paint paint) { }

        [TSRename("drawRect")]
        public void DrawRect(in Rect rect, Paint paint) { }

        [TSTemplate("{0}.drawRect(PixUI.Rect.FromLTWH({1},{2},{3},{4}), {5})")]
        public void DrawRect(float x, float y, float w, float h, Paint paint) { }

        [TSRename("drawRRect")]
        public void DrawRRect(RRect rect, Paint paint) { }

        [TSRename("drawDRRect")]
        public void DrawDRRect(RRect outer, RRect inner, Paint paint) { }

        [TSRename("drawOval")]
        public void DrawOval(in Rect rect, Paint paint) {}

        [TSTemplate("{0}.drawOval(new PixUI.Rect({1},{2},{3},{4}), {5})")]
        public void DrawOval(float cx, float cy, float rx, float ry, Paint paint) {}
        
        [TSRename("drawCircle")]
        public void DrawCircle(float cx, float cy, float radius, Paint paint) {}

        [TSTemplate("{0}.drawArc({1}, {2} * 180 / Math.PI, {3} * 180 / Math.PI, {4}, {5})")]
        public void DrawArc(in Rect oval, float startAngle, float sweepAngle, bool useCenter, Paint paint) {}

        [TSRename("drawPath")]
        public void DrawPath(Path path, Paint paint) { }

        [TSRename("drawParagraph")]
        public void DrawParagraph(Paragraph paragraph, float x, float y) { }

        [TSRename("drawImage")]
        public void DrawImage(Image image, float x, float y, Paint? paint = null) { }

        [TSRename("drawImageRect")]
        public void DrawImage(Image image, in Rect source, in Rect dest, Paint? paint = null) { }

        [TSTemplate("{0}.drawGlyphs([{1}], [{2}, {3}], {4}, {5}, {6}, {7})")]
        public void DrawGlyph(ushort glyphId, float posX, float posY, float originX, float originY,
            Font font, Paint paint) { }

        [TSTemplate("PixUI.DrawShadow({0}, {1}, {2}, {3}, {4}, {5})")]
        public void DrawShadow(Path path, in Color color, float elevation,
            bool transparentOccluder,
            float devicePixelRatio) { }
    }
}
#endif