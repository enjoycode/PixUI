namespace PixUI;

public interface IPath : IDisposable
{
    PathFillType FillType { get; set; }

    Point[] Points { get; }

    Rect Bounds { get; }

    bool IsEmpty();

    bool GetTightBounds(out Rect result);

    bool TryGetLastPoint(out Point point);

    void MoveTo(float x, float y);

    void LineTo(float x, float y);

    void ArcTo(Rect oval, float startAngle, float sweepAngle, bool forceMoveTo);

    void ArcTo(float rx, float ry, float xAxisRotate, bool useSmallArc, bool isCCW, float x, float y);

    void CubicTo(float x0, float y0, float x1, float y1, float x2, float y2);

    void QuadTo(float x0, float y0, float x1, float y1);

    void AddRect(Rect rect, bool isCCW = false);

    void AddOval(Rect rect, PathDirection direction = PathDirection.Clockwise);

    void AddRRect(RRect rect, bool isCCW = false);

    void AddPath(IPath other, PathAddMode mode = PathAddMode.Append);

    void Close();
}

public static class Path
{
    public static IPath Create() => Render.Provider.MakePath();

    public static IPath ParseSvgPathData(string svgPath) => Render.Provider.PathFromSvgData(svgPath);
}