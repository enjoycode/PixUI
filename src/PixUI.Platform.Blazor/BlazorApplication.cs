using System.Runtime.Versioning;
using Microsoft.JSInterop;

namespace PixUI.Platform.Blazor;

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

    public static async void Run(Func<Widget> rootBuilder, int width, int height, float ratio,
        string? routePath, bool isMacOS)
    {
        var app = new BlazorApplication(isMacOS);
        Current = app;

        //创建WebWindow
        var adapter = await WebGPU.RequestAdapter() ?? throw new InvalidOperationException(
            "navigator.gpu.requestAdapter returned null — WebGPU is unavailable in this browser.");
        var device = await WebGPU.RequestDevice(adapter) ?? throw new InvalidOperationException(
            "adapter.requestDevice returned null.");
        Window = new BlazorWindow(rootBuilder(), device, width, height, ratio, routePath);
        app.MainWindow = Window;
        //开始构建WidgetTree并首秀
        Window.FirstShow();
    }

    public override void PostInvalidateEvent() => WebBrowser.PostInvalidateEvent();

    internal void RunInvalidateRequest() => OnInvalidateRequest();

    public override void BeginInvoke(Action action)
    {
        //TODO: fix if thread supported
        action();
    }
}