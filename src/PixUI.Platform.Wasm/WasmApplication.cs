namespace PixUI.Platform.Wasm;

public sealed class WasmApplication : UIApplication
{
    private WasmApplication() { }

    internal static WasmWindow Window { get; private set; } = null!;

    private static AppArgs _initArgs;

    internal static void InitArgs(int glHandle, int width, int height, float pixelRatio)
    {
        _initArgs = new AppArgs() { GLHandle = glHandle, Width = width, Height = height, PixelRatio = pixelRatio };
    }

    public static void Run(Func<Widget> rootBuilder)
    {
        // TODO:
        // Cursor.PlatformCursors = new BlazorCursors();
        // Clipboard.Init(new BlazorClipboard());

        var app = new WasmApplication();
        Current = app;

        //创建WebWindow
        Window = new WasmWindow(rootBuilder(), _initArgs.GLHandle,
            _initArgs.Width, _initArgs.Height, _initArgs.PixelRatio);
        app.MainWindow = Window;
        //开始构建WidgetTree并首秀
        Window.FirstShow();
    }

    public override void PostInvalidateEvent() => JSImports.PostInvalidateEvent();

    public override void BeginInvoke(Action action)
    {
        //TODO: fix if thread supported
        action();
    }

    internal void RunInvalidateRequest() => OnInvalidateRequest();

    private readonly struct AppArgs
    {
        public int GLHandle { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
        public float PixelRatio { get; init; }
    }
}