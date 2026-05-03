namespace PixUI;

public interface ICanvas : IDisposable
{
    #region ====Draw Methods====

    void DrawLine(float x0, float y0, float x1, float y1, IPaint paint);
    void DrawLine(Point x, Point y, IPaint paint);
    void DrawRect(Rect rect, IPaint paint);
    void DrawRect(float x, float y, float w, float h, IPaint paint);
    void DrawRect(RectI rect, IPaint paint);
    void DrawRRect(RRect rect, IPaint paint);
    void DrawDRRect(RRect outer, RRect inner, IPaint paint);
    void DrawOval(float cx, float cy, float rx, float ry, IPaint paint);
    void DrawOval(Rect rect, IPaint paint);
    void DrawCircle(float cx, float cy, float radius, IPaint paint);
    void DrawArc(Rect oval, float startAngle, float sweepAngle, bool useCenter, IPaint paint);
    void DrawPath(IPath path, IPaint paint);
    void DrawString(string text, float x, float y, IFont font, Color color);
    void DrawParagraph(IParagraph paragraph, float x, float y);
    void DrawGlyph(ushort glyphId, float posX, float posY, float originX, float originY, IFont font, IPaint paint);
    void DrawImage(IImage image, float x, float y, IPaint? paint = null);
    void DrawImage(IImage image, Rect dest, IPaint? paint = null);
    void DrawImage(IImage image, Rect source, Rect dest, IPaint? paint = null);
    void DrawShadow(IPath path, Color color, float elevation, bool transparentOccluder, float devicePixelRatio);

    #endregion

    #region ====Clip====

    bool IsClipEmpty { get; }
    Rect ClipBounds { get; }
    void ClipRect(Rect rect, ClipOp op = ClipOp.Intersect, bool antialias = false);
    void ClipRRect(RRect rRect, ClipOp op = ClipOp.Intersect, bool antialias = false);
    void ClipPath(IPath path, ClipOp op, bool antialias);

    #endregion

    #region ====Matrix====

    void Translate(float dx, float dy);
    void Scale(float sx, float sy);
    void Skew(float sx, float sy);
    void RotateDegrees(float degrees);
    void RotateDegrees(float degrees, float px, float py);
    void Concat( /*ref*/ Matrix4 matrix);
    void SetMatrix(Matrix4 matrix);
    void ResetMatrix();
    Matrix3 GetTotalMatrix();

    #endregion

    #region ====Save & Restore====

    int SaveCount { get; }
    int Save();
    int SaveLayer(IPaint? paint = null, Rect? bounds = null);
    void Restore();
    void RestoreToCount(int count);

    #endregion

    #region ====Clear & Flush====

    void Flush();
    void Clear();
    void Clear(Color color);

    #endregion
}