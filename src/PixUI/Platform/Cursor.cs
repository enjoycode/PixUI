using System;

namespace PixUI;

public abstract class Cursor
{
    public static IPlatformCursors PlatformCursors = null!;

    private static Cursor? _current;

    public static Cursor Current
    {
        get
        {
            if (_current == null) return PlatformCursors.Arrow;
            return _current;
        }
        set
        {
            _current = value;
            PlatformCursors.SetCursor(value);
        }
    }
}

public static class Cursors
{
    public static Cursor Arrow => Cursor.PlatformCursors.Arrow;

    public static Cursor Hand => Cursor.PlatformCursors.Hand;

    public static Cursor IBeam => Cursor.PlatformCursors.IBeam;

    public static Cursor ResizeLR => Cursor.PlatformCursors.ResizeLR;

    public static Cursor ResizeUD => Cursor.PlatformCursors.ResizeUD;
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