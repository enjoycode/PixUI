using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixUI.Platform.Win;

internal class WinD3D12WindowContext : NativeWindowContext
{
    public WinD3D12WindowContext(NativeWindow nativeWindow, DisplayParams displayParams)
        : base(nativeWindow, displayParams)
    {
        InitContext();
    }

    private IntPtr _d3dBackendContext;
    private IntPtr _swapchain;
    private SKSurface? _onscreenSurface1;
    private SKSurface? _onscreenSurface2;
    private SKSurface? _offscreenSurface;
    private Canvas? _onscreenCanvas1;
    private Canvas? _onscreenCanvas2;
    private Canvas? _offscreenCanvas;
    private int _backbufferIndex = 0;

    private void InitContext()
    {
        var win = (WinWindow)NativeWindow;

        //set size to window client rectangle
        WinApi.Win32GetClientRect(win.HWND, out var clientRect); //GetWindowRect ?
        Width = clientRect.right - clientRect.left;
        Height = clientRect.bottom - clientRect.top;

        _d3dBackendContext = SkiaApi.gr_d3d_new_backend_context();
        GrContext = GRContext.CreateDirect3D(_d3dBackendContext);

        // make the swapchain
        _swapchain = SkiaApi.gr_d3d_new_swapchain(win.HWND, _d3dBackendContext, (uint)Width, (uint)Height);
        // create surfaces
        _onscreenSurface1 = CreateOnscreenSurface(0);
        _onscreenSurface2 = CreateOnscreenSurface(1);
        _onscreenCanvas1 = _onscreenSurface1!.Canvas;
        _onscreenCanvas2 = _onscreenSurface2!.Canvas;
        CreateOffscreenSurface();
    }

    private SKSurface CreateOnscreenSurface(int index)
    {
        var backBuffer = SkiaApi.gr_d3d_swapchain_get_buffer(_swapchain, index);
        var backendRt = GRBackendRenderTarget.CreateDirect3D(Width, Height, backBuffer);
        var surface = SKSurface.Create(GrContext!, backendRt, GRSurfaceOrigin.TopLeft,
            ColorType.Rgba8888, DisplayParams.ColorSpace, DisplayParams.SurfaceProps);
        backendRt.Dispose();
        return surface!;
    }

    private void CreateOffscreenSurface()
    {
        var imageInfo = new ImageInfo { Width = Width, Height = Height, ColorType = ColorType.Rgba8888 };
        _offscreenSurface = SKSurface.Create(GrContext!, true, imageInfo);
        _offscreenCanvas = _offscreenSurface!.Canvas;
        //直接缩放一次OffscreenCanvas，后续就不用处理了
        _offscreenCanvas.Scale(NativeWindow.ScaleFactor, NativeWindow.ScaleFactor);
    }

    public override Canvas GetOffScreenCanvas() => _offscreenCanvas!;

    public override Canvas GetOnScreenCanvas()
    {
        _backbufferIndex = SkiaApi.gr_d3d_swapchain_get_current_buffer_index(_swapchain);
        return _backbufferIndex == 0 ? _onscreenCanvas1! : _onscreenCanvas2!;
    }

    public override void Resize(int width, int height)
    {
        // Clean up any outstanding resources in command lists
        GrContext!.Flush(true, true);

        // release the previous surface and backbuffer resources
        _onscreenCanvas1?.Dispose();
        _onscreenCanvas2?.Dispose();
        _offscreenCanvas?.Dispose();
        _offscreenSurface?.Dispose();
        _onscreenSurface1?.Dispose();
        _onscreenSurface2?.Dispose();
        SkiaApi.gr_d3d_swapchain_release_buffers(_swapchain, 2);

        GrContext.PurgeResources(); //TODO:?

        Width = width;
        Height = height;
        // resize swapchain buffers and recreate surfaces
        SkiaApi.gr_d3d_swapchain_resize_buffers(_swapchain, (uint)width, (uint)height);
        _onscreenSurface1 = CreateOnscreenSurface(0);
        _onscreenSurface2 = CreateOnscreenSurface(1);
        _onscreenCanvas1 = _onscreenSurface1!.Canvas;
        _onscreenCanvas2 = _onscreenSurface2!.Canvas;
        CreateOffscreenSurface();
    }

    public override void SwapBuffers()
    {
        //Console.WriteLine($"SwapBuffer to: {_backbufferIndex}");
        var surface = _backbufferIndex == 1 ? _onscreenSurface2 : _onscreenSurface1;
        SkiaApi.gr_d3d_swapbuffer(_d3dBackendContext, GrContext!.Handle, surface!.Handle, _swapchain);
    }
}