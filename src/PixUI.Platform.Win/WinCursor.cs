using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixUI.Platform.Win
{
    internal sealed class WinCursor : Cursor
    {
        internal readonly IntPtr Handle;
        internal WinCursor(IntPtr handle)
        {
            Handle = handle;
        }   
    }

    internal sealed class WinCursors : IPlatformCursors
    {
        private Cursor? _arrow;
        private Cursor? _hand;
        private Cursor? _ibeam;
        private Cursor? _resizeLR;
        private Cursor? _resizeUD;

        internal static Cursor? Current { get; private set; }

        public Cursor Arrow
        {
            get
            {
                _arrow ??= new WinCursor(WinApi.Win32LoadCursor(IntPtr.Zero, LoadCursorType.IDC_ARROW));
                return _arrow;
            }
        }

        public Cursor Hand
        {
            get
            {
                _hand ??= new WinCursor(WinApi.Win32LoadCursor(IntPtr.Zero, LoadCursorType.IDC_HAND));
                return _hand;
            }
        }

        public Cursor IBeam
        {
            get
            {
                _ibeam ??= new WinCursor(WinApi.Win32LoadCursor(IntPtr.Zero, LoadCursorType.IDC_IBEAM));
                return _ibeam;
            }
        }

        public Cursor ResizeLR
        {
            get
            {
                _resizeLR ??= new WinCursor(WinApi.Win32LoadCursor(IntPtr.Zero, LoadCursorType.IDC_SIZEWE));
                return _resizeLR;
            }
        }

        public Cursor ResizeUD
        {
            get
            {
                _resizeUD ??= new WinCursor(WinApi.Win32LoadCursor(IntPtr.Zero, LoadCursorType.IDC_SIZENS));
                return _resizeUD;
            }
        }

        public void SetCursor(Cursor cursor)
        {
            Current = cursor;
            WinApi.Win32SetCursor(((WinCursor)cursor).Handle);
        }
    }
}
