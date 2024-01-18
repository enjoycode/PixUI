using System;
using AppKit;

namespace PixUI.Platform.Mac;

internal sealed class MacCursor : Cursor
{
    internal readonly NSCursor NSCursor;

    public MacCursor(NSCursor nsCursor)
    {
        NSCursor = nsCursor;
    }
}

internal sealed class MacCursors : IPlatformCursors
{
    private Cursor? _arrow;
    private Cursor? _hand;
    private Cursor? _ibeam;
    private Cursor? _resizeLR;
    private Cursor? _resizeUD;

    public Cursor Arrow
    {
        get
        {
            _arrow ??= new MacCursor(NSCursor.ArrowCursor);
            return _arrow;
        }
    }

    public Cursor Hand
    {
        get
        {
            _hand ??= new MacCursor(NSCursor.PointingHandCursor);
            return _hand;
        }
    }

    public Cursor IBeam
    {
        get
        {
            _ibeam ??= new MacCursor(NSCursor.IBeamCursor);
            return _ibeam;
        }
    }

    public Cursor ResizeLR
    {
        get
        {
            _resizeLR ??= new MacCursor(NSCursor.ResizeLeftRightCursor);
            return _resizeLR;
        }
    }

    public Cursor ResizeUD
    {
        get
        {
            _resizeUD ??= new MacCursor(NSCursor.ResizeUpDownCursor);
            return _resizeUD;
        }
    }

    public void SetCursor(Cursor cursor)
    {
        ((MacCursor)cursor).NSCursor.Set();
    }
}