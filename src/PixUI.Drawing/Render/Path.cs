namespace PixUI;

public interface IPath : IDisposable
{
    PathFillType FillType { get; set; }

    Point[] Points { get; }

    Rect Bounds { get; }

    bool IsRect { get; }

    bool IsEmpty();

    bool IsClosed();

    Rect GetRect();

    bool GetTightBounds(out Rect result);

    bool TryGetLastPoint(out Point point);

    bool Contains(float x, float y);

    void MoveTo(float x, float y);

    void LineTo(float x, float y);

    void ArcTo(Rect oval, float startAngle, float sweepAngle, bool forceMoveTo);

    void ArcTo(float rx, float ry, float xAxisRotate, bool useSmallArc, bool isCCW, float x, float y);

    void CubicTo(float x0, float y0, float x1, float y1, float x2, float y2);

    void QuadTo(float x0, float y0, float x1, float y1);

    void AddRect(Rect rect, bool isCCW = false);

    void AddOval(Rect rect, PathDirection direction = PathDirection.Clockwise);

    void AddRRect(RRect rect, bool isCCW = false);

    void AddArc(Rect oval, float startAngle, float sweepAngle);

    void AddPath(IPath other, PathAddMode mode = PathAddMode.Append);

    bool Op(IPath other, PathOp op);

    void Transform(Matrix3 matrix);

    void Close();
}

public static class Path
{
    public static IPath Create() => Render.Backend.MakePath();

    public static IPath ParseSvgPathData(string svgPath) => Render.Backend.MakePathFromSvgData(svgPath);
}