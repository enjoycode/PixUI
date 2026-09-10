namespace PixUI;

public sealed class NoDrawCanvas : ICanvas
{
    public static NoDrawCanvas Instance { get; } = new NoDrawCanvas();

    public void Dispose() { }
    public ISurface? Surface => null;
    public void DrawLine(float x0, float y0, float x1, float y1, IPaint paint) { }
    public void DrawLine(Point x, Point y, IPaint paint) { }
    public void DrawRect(Rect rect, IPaint paint) { }
    public void DrawRect(float x, float y, float w, float h, IPaint paint) { }
    public void DrawRect(RectI rect, IPaint paint) { }
    public void DrawRRect(RRect rect, IPaint paint) { }
    public void DrawDRRect(RRect outer, RRect inner, IPaint paint) { }
    public void DrawOval(float cx, float cy, float rx, float ry, IPaint paint) { }
    public void DrawOval(Rect rect, IPaint paint) { }
    public void DrawCircle(float cx, float cy, float radius, IPaint paint) { }
    public void DrawArc(Rect oval, float startAngle, float sweepAngle, bool useCenter, IPaint paint) { }
    public void DrawPath(IPath path, IPaint paint) { }
    public void DrawString(string text, float x, float y, IFont font, Color color) { }
    public void DrawTextBlob(ITextBlob textBlob, float x, float y, IPaint paint) { }
    public void DrawParagraph(IParagraph paragraph, float x, float y) { }

    public void DrawGlyph(ushort glyphId, float posX, float posY, float originX, float originY, IFont font,
        IPaint paint) { }

    public void DrawImage(IImage image, float x, float y, IPaint? paint = null) { }
    public void DrawImage(IImage image, Rect dest, IPaint? paint = null) { }
    public void DrawImage(IImage image, Rect source, Rect dest, IPaint? paint = null) { }
    public void DrawPicture(IPicture picture, float x, float y, IPaint? paint = null) { }
    public void DrawPicture(IPicture picture, in Matrix3 matrix, IPaint? paint = null) { }

    public void DrawShadow(IPath path, Color color, float elevation, bool transparentOccluder,
        float devicePixelRatio) { }

    public bool IsClipEmpty => true;
    public Rect ClipBounds => Rect.Empty;
    public void ClipRect(Rect rect, ClipOp op = ClipOp.Intersect, bool antialias = false) { }
    public void ClipRRect(RRect rRect, ClipOp op = ClipOp.Intersect, bool antialias = false) { }
    public void ClipPath(IPath path, ClipOp op, bool antialias) { }
    public void Translate(float dx, float dy) { }
    public void Scale(float sx, float sy) { }
    public void Skew(float sx, float sy) { }
    public void RotateDegrees(float degrees) { }
    public void RotateDegrees(float degrees, float px, float py) { }
    public void Concat(Matrix4 matrix) { }
    public void SetMatrix(Matrix4 matrix) { }
    public void ResetMatrix() { }
    public Matrix3 GetTotalMatrix() => Matrix3.Identity;
    public int SaveCount => 0;
    public int Save() => 0;
    public int SaveLayer(IPaint? paint = null, Rect? bounds = null) => 0;
    public void Restore() { }
    public void RestoreToCount(int count) { }
    public void Flush() { }
    public void Clear() { }
    public void Clear(Color color) { }
}