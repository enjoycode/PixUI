#if SK_GRAPHITE
using System.Runtime.Versioning;

namespace PixUI.Platform.Blazor;

[SupportedOSPlatform("browser")]
public sealed class WebGPUWindow : WebWindow
{
    public WebGPUWindow(Widget child, RunInfo runInfo) : base(child, runInfo.RoutePath)
    {
        _webGpuContext = Render.Backend.MakeWebGpuContext(
            runInfo.GpuInstanceId, runInfo.GpuDeviceId, runInfo.GpuQueueId);
        if (_webGpuContext == null) throw new Exception("Can't create WebGpuContext");

        _gpuRecorder = _webGpuContext.CreateRecorder();

        // CreateOnScreenSurface(runInfo.GpuOnScreenTextureId); 不需要,Invalidate时再创建
        CreateOffScreenSurface(runInfo.GpuOffScreenTextureId, runInfo.Width, runInfo.Height, runInfo.PixelRatio);
    }

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

    protected internal override void CreateOnScreenSurface(int onScreenTextureId)
    {
        if (_onScreenSurface != null)
        {
            _onScreenSurface.Dispose();
            _onScreenBackendTexture?.Dispose();
        }

        _onScreenTextureId = onScreenTextureId;
        _onScreenBackendTexture = Render.Backend.MakeWebGpuBackendTexture(_onScreenTextureId);
        _onScreenSurface = Surface.Create(_gpuRecorder!, _onScreenBackendTexture, ColorType.Rgba8888);
        _onScreenCanvas = _onScreenSurface.Canvas;
    }

    private void CreateOffScreenSurface(int offscreenTextureId, int width, int height, float ratio)
    {
        if (_offScreenSurface != null)
        {
            _offScreenSurface.Dispose();
            _offScreenBackendTexture?.Dispose();
        }

        _width = width;
        _height = height;
        _ratio = ratio;

        _offScreenTextureId = offscreenTextureId;
        _offScreenBackendTexture = Render.Backend.MakeWebGpuBackendTexture(_offScreenTextureId);
        _offScreenSurface = Surface.Create(_gpuRecorder!, _offScreenBackendTexture, ColorType.Rgba8888);
        _offScreenCanvas = _offScreenSurface.Canvas;
        _offScreenCanvas.Scale(ratio, ratio);
    }

    protected override ICanvas GetOnscreenCanvas() => _onScreenCanvas!;

    protected override ICanvas GetOffscreenCanvas() => _offScreenCanvas!;

    protected override void FlushOffScreen() =>
        Render.Backend.FlushSurface(_webGpuContext!, _gpuRecorder!);

    protected override void Present() =>
        Render.Backend.FlushSurface(_webGpuContext!, _gpuRecorder!);

    /// <summary>
    /// 窗体改变大小后重新创建画布并重新布局
    /// </summary>
    internal override void OnResize(int width, int height, float ratio, int onScreenTextureId, int offScreenTextureId)
    {
        CreateOnScreenSurface(onScreenTextureId);
        if (offScreenTextureId != 0)
            CreateOffScreenSurface(offScreenTextureId, width, height, ratio);

        RootWidget.Relayout();
    }
}

#endif