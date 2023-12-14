using Microsoft.JSInterop;

namespace PixUI.Platform.Blazor;

public sealed class BlazorApplication : UIApplication
{
    private BlazorApplication(bool isMacOS)
    {
        _isMacOS = isMacOS;
    }
    
    internal static IJSRuntime JSRuntime = null!;
    internal static HttpClient HttpClient = null!;
    internal static BlazorWindow Window { get; private set; } = null!;

    private readonly bool _isMacOS;

    public override bool IsMacOS() => _isMacOS;

    protected override void PushWebHistory(string fullPath, int index)
        => ((IJSInProcessRuntime)JSRuntime).InvokeVoid("PushWebHistory", fullPath, index);

    protected override void ReplaceWebHistory(string fullPath, int index)
        => ((IJSInProcessRuntime)JSRuntime).InvokeVoid("ReplaceWebHistory", fullPath, index);

    public static void Run(Func<Widget> rootBuilder, int glHandle, 
        int width, int height, float ratio,
        string? routePath, bool isMacOS)
    {
        //TODO: 其他平台支持
        Cursor.PlatformCursors = new BlazorCursors();
        Clipboard.Init(new BlazorClipboard());

        var app = new BlazorApplication(isMacOS);
        Current = app;

        //创建WebWindow
        Window = new BlazorWindow(rootBuilder(), glHandle, width, height, ratio, routePath);
        app.MainWindow = Window;
        //开始构建WidgetTree并首秀
        Window.FirstShow();
    }

    public override void PostInvalidateEvent()
        => ((IJSInProcessRuntime)JSRuntime).InvokeVoid("PostInvalidateEvent");

    internal void RunInvaldateRequest() => OnInvalidateRequest();

    public override void BeginInvoke(Action action)
    {
        //TODO: fix if thread supported
        action();
    }
}