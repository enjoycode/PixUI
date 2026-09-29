using System.Runtime.Versioning;
using Microsoft.JSInterop;

namespace PixUI.Platform.Blazor;

public struct RunInfo
{
    public int GpuInstanceId { get; set; }
    public int GpuDeviceId { get; set; }
    public int GpuQueueId { get; set; }
    public int GpuOnScreenTextureId { get; set; }
    public int GpuOffScreenTextureId { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
    public float PixelRatio { get; set; }

    /// <summary>
    /// 启动时的路由
    /// </summary>
    public string? RoutePath { get; set; }

    /// <summary>
    /// 是否MacOS,主要用于设置一些快捷键
    /// </summary>
    public bool IsMacOS { get; set; }

    /// <summary>
    /// WebSocket Url
    /// </summary>
    public string WsUrl { get; set; }
}

[SupportedOSPlatform("browser")]
public sealed class WebApplication : UIApplication
{
    private WebApplication(bool isMacOS)
    {
        _isMacOS = isMacOS;
    }

    internal static IJSRuntime JSRuntime = null!;
    internal static HttpClient HttpClient = null!;
    internal static WebWindow Window { get; private set; } = null!;

    #region ====Platform Providers====

    public override IPlatformCursors CursorsProvider { get; } = new WebCursors();
    public override IPlatformClipboard ClipboardProvider { get; } = new WebClipboard();
    public override IPlatformFileDialog FileDialogProvider { get; } = new WebFileDialog();

    #endregion

    private readonly bool _isMacOS;

    public override bool IsMacOS() => _isMacOS;

    protected override void PushWebHistory(string fullPath, int index)
        => WebBrowser.PushWebHistory(fullPath, index);

    protected override void ReplaceWebHistory(string fullPath, int index)
        => WebBrowser.ReplaceWebHistory(fullPath, index);

    public static void Run(Func<Widget> rootBuilder, RunInfo runInfo)
    {
        var app = new WebApplication(runInfo.IsMacOS);
        Current = app;

        //创建WebWindow
#if SK_GRAPHITE
        Window = new WebGPUWindow(rootBuilder(), runInfo);
#else
        Window = new WebGLWindow(rootBuilder(), runInfo);
#endif
        app.MainWindow = Window;
        //开始构建WidgetTree并首秀
        Window.FirstShow();
    }

    public override void PostInvalidateEvent() => WebBrowser.PostInvalidateEvent();

#if SK_GRAPHITE
    internal void RunInvalidateRequest(int onScreenTextureId)
    {
        Window.CreateOnScreenSurface(onScreenTextureId); //重新创建OnScreenSurface
        OnInvalidateRequest();
    }
#else
    internal void RunInvalidateRequest() => OnInvalidateRequest();
#endif

    public override void BeginInvoke(Action action)
    {
        //TODO: fix if thread supported
        action();
    }
}