namespace PixUI;

public static class PaintUtils
{
    private static Paint? _shared; //Do not create instance, web has not init CanvasKit

    public static Paint Shared(in Color? color = null, PaintStyle style = PaintStyle.Fill,
        float strokeWidth = 1)
    {
        if (_shared == null) _shared = new Paint();
        else _shared.Reset();
        _shared.Style = style;
        if (color != null)
            _shared.Color = color.Value;
        _shared.StrokeWidth = strokeWidth;
        return _shared;
    }
}