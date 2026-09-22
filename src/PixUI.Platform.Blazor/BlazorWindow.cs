using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace PixUI.Platform.Blazor;

[SupportedOSPlatform("browser")]
public sealed class BlazorWindow : UIWindow
{
    public BlazorWindow(Widget child, JSObject device, int width, int height, float ratio,
        string? initRoutePath = null) : base(child, initRoutePath)
    {
        _device = device;
        CreateContext(device);
        CreateOffScreenSurface(width, height, ratio);
    }

    private readonly JSObject _device;
    private IGpuContext? _webGpuContext;
    private IGpuRecorder? _gpuRecorder;

    private int _onScreenTextureId;
    private IGpuBackendTexture? _onScreenBackendTexture;
    private ISurface? _onScreenSurface;
    private ICanvas? _onScreenCanvas;

    private ISurface? _offScreenSurface;
    private int _offScreenTextureId;
    private IGpuBackendTexture? _offScreenBackendTexture;
    private ICanvas? _offScreenCanvas;
    private int _width;
    private int _height;
    private float _ratio;

    public override float ScaleFactor => _ratio;
    public override float Width => _width;
    public override float Height => _height;

    private void CreateContext(JSObject device)
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

        _gpuRecorder = _webGpuContext.CreateRecorder();
    }

    private void CreateOffScreenSurface(int width, int height, float ratio)
    {
        if (_offScreenSurface != null)
        {
            _offScreenSurface.Dispose();
            _offScreenBackendTexture?.Dispose();
            WebGPU.ReleaseTexture(_offScreenTextureId);
        }

        _width = width;
        _height = height;
        _ratio = ratio;

        var pixWidth = (int)(width * ratio);
        var pixHeigh = (int)(height * ratio);

        //create offscreen surface
        var offscreenTexture = WebGPU.CreateTexture(_device, pixWidth, pixHeigh);
        _offScreenTextureId = WebGPU.RegisterTexture(offscreenTexture);
        _offScreenBackendTexture = Render.Backend.MakeWebGpuBackendTexture(_offScreenTextureId);
        _offScreenSurface = Surface.Create(_gpuRecorder!, _offScreenBackendTexture, ColorType.Rgba8888);
        _offScreenCanvas = _offScreenSurface.Canvas;
        _offScreenCanvas.Scale(ratio, ratio);
    }

    protected override ICanvas GetOnscreenCanvas()
    {
        if (_onScreenSurface != null)
        {
            _onScreenSurface.Dispose();
            _onScreenBackendTexture?.Dispose();
            //Check should WebGPU.ReleaseTexture(_onScreenTextureId)
        }

        var onscreenTexture = WebGPU.GetCanvasTexture(_device);
        _onScreenTextureId = WebGPU.RegisterTexture(onscreenTexture);
        _onScreenBackendTexture = Render.Backend.MakeWebGpuBackendTexture(_onScreenTextureId);
        _onScreenSurface = Surface.Create(_gpuRecorder!, _onScreenBackendTexture, ColorType.Rgba8888);
        _onScreenCanvas = _onScreenSurface.Canvas;
        return _onScreenCanvas;
    }

    protected override ICanvas GetOffscreenCanvas() => _offScreenCanvas!;

    protected override void FlushOffScreen() =>
        Render.Backend.FlushSurface(_webGpuContext!, _gpuRecorder!);

    protected override void Present() =>
        Render.Backend.FlushSurface(_webGpuContext!, _gpuRecorder!);

    internal void FirstShow()
    {
        RootWidget.PerformLayout(new(Width, Height));
        Overlay.PerformLayout(new(Width, Height));

        var widgetsCanvas = GetOffscreenCanvas();
        RootWidget.OnPaint(widgetsCanvas);
        // FlushOffScreen();

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
        // _offScreenSurface?.Dispose();
        // _onScreenSurface?.Dispose();
        //
        // CreateSurface(width, height, ratio);
        // RootWidget.Relayout();
    }

    public override void StartTextInput() => WebBrowser.StartTextInput();

    public override void StopTextInput() => WebBrowser.StopTextInput();

    public override void SetTextInputRect(Rect rect) =>
        WebBrowser.SetInputRect(rect.X, rect.Y, rect.Width, rect.Height);

    internal void RouteGoto(int historyId) => RouteHistoryManager.Goto(historyId);

    internal void RoutePush(string path) => RouteHistoryManager.Push(path);

    internal int NewRouteId() => RouteHistoryManager.NewIdForPush();
}