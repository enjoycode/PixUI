using System.Runtime.InteropServices;

namespace PixUI.Platform.Win;

internal sealed class WinRasterWindowContext : NativeWindowContext
{
    public WinRasterWindowContext(NativeWindow nativeWindow, DisplayParams displayParams)
        : base(nativeWindow, displayParams)
    {
        InitContext();
    }

    private IntPtr _onscreenSurfaceMem;
    private IntPtr _offscreenSufaceMem;
    private ISurface? _onscreenSurface;
    private ISurface? _offscreenSurface;
    private ICanvas? _onscreenCanvas;
    private ICanvas? _offscreenCanvas;

    public override bool IsGpuContext => false;

    private unsafe void InitContext()
    {
        //set size to window client rectangle
        WinApi.Win32GetClientRect(((WinWindow)NativeWindow).HWND, out var clientRect);
        Width = clientRect.right - clientRect.left;
        Height = clientRect.bottom - clientRect.top;

        AllocMemAndCreateSurface();
    }

    private unsafe void AllocMemAndCreateSurface()
    {
        // alloc surface memory
        var memSize = sizeof(BITMAPINFOHEADER) + Width * Height * sizeof(uint);
        _onscreenSurfaceMem = Marshal.AllocHGlobal(memSize);
        _offscreenSufaceMem = Marshal.AllocHGlobal(memSize);
        // create raster surface
        _onscreenSurface = CreateSurface(_onscreenSurfaceMem, Width, Height);
        _offscreenSurface = CreateSurface(_offscreenSufaceMem, Width, Height);
        _onscreenCanvas = _onscreenSurface.Canvas;
        _offscreenCanvas = _offscreenSurface.Canvas;

        //直接缩放一次OffscreenCanvas，后续就不用处理了
        _offscreenCanvas.Scale(NativeWindow.ScaleFactor, NativeWindow.ScaleFactor);
    }

    private unsafe ISurface CreateSurface(IntPtr memPtr, int w, int h)
    {
        var bmpHeaderInfo = (BITMAPINFOHEADER*)memPtr.ToPointer();
        bmpHeaderInfo->biSize = (uint)sizeof(BITMAPINFOHEADER);
        bmpHeaderInfo->biWidth = w;
        bmpHeaderInfo->biHeight = -h; // negative means top-down bitmap. Skia draws top-down.
        bmpHeaderInfo->biPlanes = 1;
        bmpHeaderInfo->biBitCount = 32;
        bmpHeaderInfo->biCompression = 0 /*BI_RGB*/;
        void* pixels = (memPtr + sizeof(BITMAPINFOHEADER)).ToPointer();

        var skImgInfo = new ImageInfo
            { Width = w, Height = h, ColorType = ColorType.Bgra8888, AlphaType = AlphaType.Premul };
        return Surface.Create(skImgInfo, new IntPtr(pixels), sizeof(uint) * w);
    }

    public override ICanvas GetOffScreenCanvas() => _offscreenCanvas!;

    public override ICanvas GetOnScreenCanvas() => _onscreenCanvas!;

    public override void Resize(int width, int height)
    {
        //TODO: 暂简单实现

        //free exists
        Marshal.FreeHGlobal(_onscreenSurfaceMem);
        Marshal.FreeHGlobal(_offscreenSufaceMem);
        _onscreenSurface?.Dispose();
        _offscreenSurface?.Dispose();

        //reset size and recreate
        Width = width;
        Height = height;
        AllocMemAndCreateSurface();
    }

    public unsafe override void SwapBuffers()
    {
        var hWnd = ((WinWindow)NativeWindow).HWND;
        void* bmiPtr = _onscreenSurfaceMem.ToPointer();
        void* bitsPtr = (_onscreenSurfaceMem + sizeof(BITMAPINFOHEADER)).ToPointer();
        var dc = WinApi.Win32GetDC(hWnd);
        //WinApi.Win32BitBlt(dc, 0, 0, Width, Height, new IntPtr(bitsPtr), 0, 0, 0x00CC0020/*SRCCOPY*/);
        WinApi.Win32StretchDIBits(dc, 0, 0, Width, Height, 0, 0, Width, Height,
            bitsPtr, bmiPtr, 0 /*DIB_RGB_COLORS*/, 0x00CC0020 /*SRCCOPY*/);
        WinApi.Win32ReleaseDC(hWnd, dc);
    }
}