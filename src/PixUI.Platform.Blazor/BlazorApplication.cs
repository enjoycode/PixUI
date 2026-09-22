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
}

[SupportedOSPlatform("browser")]
public sealed class BlazorApplication : UIApplication
{
    private BlazorApplication(bool isMacOS)
    {
        _isMacOS = isMacOS;
    }

    internal static IJSRuntime JSRuntime = null!;
    internal static HttpClient HttpClient = null!;
    internal static BlazorWindow Window { get; private set; } = null!;

    #region ====Platform Providers====

    public override IPlatformCursors CursorsProvider { get; } = new BlazorCursors();
    public override IPlatformClipboard ClipboardProvider { get; } = new BlazorClipboard();
    public override IPlatformFileDialog FileDialogProvider { get; } = new BlazorFileDialog();

    #endregion

    private readonly bool _isMacOS;

    public override bool IsMacOS() => _isMacOS;

    protected override void PushWebHistory(string fullPath, int index)
        => WebBrowser.PushWebHistory(fullPath, index);

    protected override void ReplaceWebHistory(string fullPath, int index)
        => WebBrowser.ReplaceWebHistory(fullPath, index);

    public static async void Run(Func<Widget> rootBuilder, RunInfo runInfo)
    {
        var app = new BlazorApplication(runInfo.IsMacOS);
        Current = app;

        //创建WebWindow
        Window = new BlazorWindow(rootBuilder(), runInfo);
        app.MainWindow = Window;
        //开始构建WidgetTree并首秀
        Window.FirstShow();
    }

    public override void PostInvalidateEvent() => WebBrowser.PostInvalidateEvent();

    internal void RunInvalidateRequest(int onScreenTextureId)
    {
        //重新创建OnScreenSurface
        Window.CreateOnScreenSurface(onScreenTextureId);
        OnInvalidateRequest();
    }

    public override void BeginInvoke(Action action)
    {
        //TODO: fix if thread supported
        action();
    }
}