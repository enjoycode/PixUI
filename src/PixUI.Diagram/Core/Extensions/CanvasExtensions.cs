namespace PixUI.Diagram;

public static class CanvasExtensions
{
    public static void DrawLine(this Canvas canvas, Color color, float width, float x0, float y0, float x1, float y1)
    {
        var paint = Paint.Shared(color, PaintStyle.Stroke, width);
        if (Math.Abs(x0 - x1) > 0.001 || Math.Abs(y0 - y1) > 0.001)
            paint.AntiAlias = true;
        canvas.DrawLine(x0, y0, x1, y1, paint);
    }

    public static void DrawRectangle(this Canvas canvas, Color color, float width, Rect paintRect)
    {
        var paint = Paint.Shared(color, PaintStyle.Stroke, width);
        canvas.DrawRect(paintRect, paint);
    }

    public static void FillRectangle(this Canvas canvas, Color color, Rect rect)
    {
        var paint = Paint.Shared(color);
        canvas.DrawRect(rect, paint);
    }

    public static void FillRoundRectangle(this Canvas canvas, Color color, Rect rect, float rx, float ry)
    {
        var paint = Paint.Shared(color);
        paint.AntiAlias = true;
        canvas.DrawRRect(RRect.FromRectAndRadius(rect, rx, ry), paint);
    }

    public static void DrawPath(this Canvas canvas, Color color, float width, Path path)
    {
        var paint = Paint.Shared(color, PaintStyle.Stroke, width);
        paint.AntiAlias = true;
        canvas.DrawPath(path, paint);
    }

    public static void DrawPathDashed(this Canvas canvas, Color color, float width, float[] dash, Path path)
    {
        using var dashEffect = PathEffect.CreateDash(dash, 10);
        var paint = Paint.Shared(color, PaintStyle.Stroke, width);
        paint.AntiAlias = true;
        paint.PathEffect = dashEffect;
        canvas.DrawPath(path, paint);
        paint.Reset();
    }

    public static void FillPath(this Canvas canvas, Color color, Path path)
    {
        var paint = Paint.Shared(color);
        paint.AntiAlias = true;
        canvas.DrawPath(path, paint);
    }

    public static void DrawEllipse(this Canvas canvas, Color color, float penWidth, Rect rect)
    {
        var paint = Paint.Shared(color, PaintStyle.Stroke, penWidth);
        paint.AntiAlias = true;
        canvas.DrawOval(rect, paint);
    }

    public static void FillEllipse(this Canvas canvas, Color color, Rect rect)
    {
        var paint = Paint.Shared(color);
        paint.AntiAlias = true;
        canvas.DrawOval(rect, paint);
    }
}