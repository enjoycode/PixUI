namespace PixUI;

public sealed class ScrollEvent
{
    private static readonly ScrollEvent Default = new ScrollEvent();

    public float X { get; private set; }
    public float Y { get; private set; }

    public float Dx { get; private set; }

    public float Dy { get; private set; }

    private ScrollEvent() { }

    public static ScrollEvent Make(float x, float y, float dx, float dy)
    {
        Default.X = x;
        Default.Y = y;
        Default.Dx = dx;
        Default.Dy = dy;
        return Default;
    }
}