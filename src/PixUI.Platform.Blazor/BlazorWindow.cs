using System.Runtime.InteropServices.JavaScript;
using Microsoft.JSInterop;

namespace PixUI.Platform.Blazor;

public sealed class BlazorWindow : UIWindow
{
    public BlazorWindow(Widget child, JSObject adapter, JSObject device, int width, int height, float ratio,
        string? initRoutePath = null) : base(child, initRoutePath)
    {
        _device = device;
        CreateContext(adapter, device);
        CreateSurface(width, height, ratio);
    }

    private readonly JSObject _device;
    private IGpuContext? _webGpuContext;

    private IGpuRecorder? _onScreenRecorder;
    private int _onScreenTextureId;
    private IGpuBackendTexture? _onScreenBackendTexture;
    private ISurface? _onScreenSurface;
    private ICanvas? _onScreenCanvas;

    private ISurface? _offScreenSurface;
    private int _offScreenTextureId;
    private IGpuBackendTexture? _offScreenBackendTexture;
    private IGpuRecorder? _offScreenRecorder;
    private ICanvas? _offScreenCanvas;
    private int _width;
    private int _height;
    private float _ratio;

    public override float ScaleFactor => _ratio;
    public override float Width => _width;
    public override float Height => _height;

    private void CreateContext(JSObject adapter, JSObject device)
    {
        var instanceId = WebGPU.CreateInstance();
        if (instanceId == 0)
            throw new InvalidOperationException(
                "Module._wgpuCreateInstance not exported — cannot obtain a real WGPUInstance.");

        var queue = WebGPU.GetDeviceQueue(device);
        var queueId = WebGPU.RegisterQueue(queue, instanceId);
        var deviceId = WebGPU.RegisterDevice(device, instanceId);

        _webGpuContext = Render.Backend.MakeWebGpuContext(instanceId, deviceId, queueId);
        if (_webGpuContext == null) throw new Exception("Can't create WebGpuContext");
    }

    private void CreateSurface(int width, int height, float ratio)
    {
        _width = width;
        _height = height;
        _ratio = ratio;

        var pixWidth = (int)(width * ratio);
        var pixHeigh = (int)(height * ratio);

        _onScreenRecorder = _webGpuContext!.CreateRecorder();
        _offScreenRecorder = _webGpuContext.CreateRecorder();

        //create onscreen surface
        var onscreenTexture = WebGPU.GetCanvasTexture(_device);
        _onScreenTextureId = WebGPU.RegisterTexture(onscreenTexture);
        _onScreenBackendTexture = Render.Backend.MakeWebGpuBackendTexture(_onScreenTextureId);
        _onScreenSurface = Surface.Create(_onScreenRecorder, _onScreenBackendTexture, ColorType.Rgba8888);
        _onScreenCanvas = _onScreenSurface.Canvas;

        //create offscreen surface
        var onScreenSize = _onScreenBackendTexture.Dimensions;
        var offscreenTexture = WebGPU.CreateTexture(_device, onScreenSize.Width, onScreenSize.Height);
        _offScreenTextureId = WebGPU.RegisterTexture(offscreenTexture);
        _offScreenBackendTexture = Render.Backend.MakeWebGpuBackendTexture(_offScreenTextureId);
        _offScreenSurface = Surface.Create(_offScreenRecorder, _offScreenBackendTexture, ColorType.Rgba8888);
        _offScreenCanvas = _offScreenSurface.Canvas;
        _offScreenCanvas.Scale(ratio, ratio);
    }

    protected override ICanvas GetOnscreenCanvas() => _onScreenCanvas!;

    protected override ICanvas GetOffscreenCanvas() => _offScreenCanvas!;

    protected override void FlushOffScreen() =>
        Render.Backend.FlushSurface(_webGpuContext!, _offScreenRecorder!);

    protected override void Present() =>
        Render.Backend.FlushSurface(_webGpuContext!, _onScreenRecorder!);

    internal void FirstShow()
    {
        RootWidget.PerformLayout(new(Width, Height));
        Overlay.PerformLayout(new(Width, Height));

        var widgetsCanvas = GetOffscreenCanvas();
        RootWidget.OnPaint(widgetsCanvas);
        FlushOffScreen();

        var overlayCanvas = GetOnscreenCanvas();
        _offScreenSurface?.Draw(overlayCanvas, 0, 0, null);

        Present();
    }

    /// <summary>
    /// 窗体改变大小后重新创建画布并重新布局
    /// </summary>
    internal void OnResize(int width, int height, float ratio)
    {
        //TODO: reuse surface if can
        _offScreenSurface?.Dispose();
        _onScreenSurface?.Dispose();

        CreateSurface(width, height, ratio);
        RootWidget.Relayout();
    }

    public override void StartTextInput() =>
        ((IJSInProcessRuntime)BlazorApplication.JSRuntime).InvokeVoid("PixUI.StartTextInput");

    public override void StopTextInput() =>
        ((IJSInProcessRuntime)BlazorApplication.JSRuntime).InvokeVoid("PixUI.StopTextInput");

    public override void SetTextInputRect(Rect rect) =>
        ((IJSInProcessRuntime)BlazorApplication.JSRuntime).InvokeVoid("PixUI.SetInputRect",
            rect.X, rect.Y, rect.Width, rect.Height);

    internal void RouteGoto(int historyId) => RouteHistoryManager.Goto(historyId);

    internal void RoutePush(string path) => RouteHistoryManager.Push(path);

    internal int NewRouteId() => RouteHistoryManager.NewIdForPush();
}