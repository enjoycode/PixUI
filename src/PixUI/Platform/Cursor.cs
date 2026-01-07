namespace PixUI;

public abstract class Cursor
{
    private static Cursor? _current;

    public static Cursor Current
    {
        get => _current ?? UIApplication.Current.CursorsProvider.Arrow;
        set
        {
            _current = value;
            UIApplication.Current.CursorsProvider.SetCursor(value);
        }
    }
}

public static class Cursors
{
    public static Cursor Arrow => UIApplication.Current.CursorsProvider.Arrow;

    public static Cursor Hand => UIApplication.Current.CursorsProvider.Hand;

    public static Cursor IBeam => UIApplication.Current.CursorsProvider.IBeam;

    public static Cursor ResizeLR => UIApplication.Current.CursorsProvider.ResizeLR;

    public static Cursor ResizeUD => UIApplication.Current.CursorsProvider.ResizeUD;
}

public interface IPlatformCursors
{
    public Cursor Arrow { get; }

    public Cursor Hand { get; }

    public Cursor IBeam { get; }

    public Cursor ResizeLR { get; }

    public Cursor ResizeUD { get; }

    public void SetCursor(Cursor cursor);
}