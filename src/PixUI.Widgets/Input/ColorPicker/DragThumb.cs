namespace PixUI;

internal static class DragThumb
{
    public const float THUMB_RADIUS = 8f;
    
    public static void Draw(Canvas canvas, float cx, float cy, Color color)
    {
        var paint = Paint.Shared(Colors.White);
        paint.AntiAlias = true;
        canvas.DrawCircle(cx, cy, THUMB_RADIUS, paint);

        paint = Paint.Shared(color, PaintStyle.Stroke, 1);
        paint.AntiAlias = true;
        canvas.DrawCircle(cx, cy, THUMB_RADIUS, paint);

        paint.Style = PaintStyle.Fill;
        canvas.DrawCircle(cx, cy, THUMB_RADIUS - 3, paint);
        paint.Reset();
    }
}