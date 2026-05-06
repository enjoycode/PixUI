namespace PixUI.Platform.Win;

internal class WinD3D12WindowContext : NativeWindowContext
{
    public WinD3D12WindowContext(NativeWindow nativeWindow, DisplayParams displayParams)
        : base(nativeWindow, displayParams)
    {
        InitContext();
    }

    private IDirect3DBackendContext _direct3DBackendContext;
    private IDirect3DSwapChain _swapChain;
    private ISurface? _onscreenSurface1;
    private ISurface? _onscreenSurface2;
    private ISurface? _offscreenSurface;
    private ICanvas? _onscreenCanvas1;
    private ICanvas? _onscreenCanvas2;
    private ICanvas? _offscreenCanvas;
    private int _backBufferIndex = 0;

    private void InitContext()
    {
        var win = (WinWindow)NativeWindow;

        //set size to window client rectangle
        WinApi.Win32GetClientRect(win.HWND, out var clientRect); //GetWindowRect ?
        Width = clientRect.right - clientRect.left;
        Height = clientRect.bottom - clientRect.top;

        GrContext = Render.Backend.MakeGRContextDirect3D(out _direct3DBackendContext);

        // make the swap chain
        _swapChain = Render.Backend.MakeDirect3DSwapChain(win.HWND, _direct3DBackendContext, (uint)Width, (uint)Height);
        // create surfaces
        _onscreenSurface1 = CreateOnscreenSurface(0);
        _onscreenSurface2 = CreateOnscreenSurface(1);
        _onscreenCanvas1 = _onscreenSurface1!.Canvas;
        _onscreenCanvas2 = _onscreenSurface2!.Canvas;
        CreateOffscreenSurface();
    }

    private ISurface CreateOnscreenSurface(int index) => Render.Backend.MakeSurfaceForDirect3DWindow(GrContext!,
        _swapChain, index, Width, Height, DisplayParams.ColorSpace, DisplayParams.SurfaceProps)!;

    private void CreateOffscreenSurface()
    {
        var imageInfo = new ImageInfo { Width = Width, Height = Height, ColorType = ColorType.Rgba8888 };
        _offscreenSurface = Surface.Create(GrContext!, true, imageInfo);
        _offscreenCanvas = _offscreenSurface!.Canvas;
        //直接缩放一次OffscreenCanvas，后续就不用处理了
        _offscreenCanvas.Scale(NativeWindow.ScaleFactor, NativeWindow.ScaleFactor);
    }

    public override ICanvas GetOffScreenCanvas() => _offscreenCanvas!;

    public override ICanvas GetOnScreenCanvas()
    {
        _backBufferIndex = _swapChain.CurrentBufferIndex;
        return _backBufferIndex == 0 ? _onscreenCanvas1! : _onscreenCanvas2!;
    }

    public override void Resize(int width, int height)
    {
        // Clean up any outstanding resources in command lists
        GrContext!.Flush(true, true);

        // release the previous surface and back buffer resources
        _onscreenCanvas1?.Dispose();
        _onscreenCanvas2?.Dispose();
        _offscreenCanvas?.Dispose();
        _offscreenSurface?.Dispose();
        _onscreenSurface1?.Dispose();
        _onscreenSurface2?.Dispose();
        _swapChain.ReleaseBuffers(2);

        GrContext.PurgeResources(); //TODO:?

        Width = width;
        Height = height;
        // resize swap chain buffers and recreate surfaces
        _swapChain.ResizeBuffers((uint)width, (uint)height);
        _onscreenSurface1 = CreateOnscreenSurface(0);
        _onscreenSurface2 = CreateOnscreenSurface(1);
        _onscreenCanvas1 = _onscreenSurface1!.Canvas;
        _onscreenCanvas2 = _onscreenSurface2!.Canvas;
        CreateOffscreenSurface();
    }

    public override void SwapBuffers()
    {
        //Console.WriteLine($"SwapBuffer to: {_backBufferIndex}");
        var surface = _backBufferIndex == 1 ? _onscreenSurface2 : _onscreenSurface1;
        _swapChain.SwapBuffer(GrContext!, _direct3DBackendContext, surface!);
    }
}