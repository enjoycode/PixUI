namespace PixUI;

public interface IPath : IDisposable
{
    Point[] Points { get; }

    Rect Bounds { get; }

    bool TryGetLastPoint(out Point point);

    void MoveTo(float x, float y);

    void LineTo(float x, float y);

    void CubicTo(float x0, float y0, float x1, float y1, float x2, float y2);

    void AddRect(Rect rect, bool isCCW = false);

    void AddOval(Rect rect, PathDirection direction = PathDirection.Clockwise);

    void AddRRect(RRect rect, bool isCCW = false);

    void Close();
}

public static class Path
{
    public static IPath Create() => Render.Provider.MakePath();

    public static IPath ParseSvgPathData(string svgPath) => Render.Provider.PathFromSvgData(svgPath);
}