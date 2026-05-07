namespace PixUI.Platform.Wasm;

internal sealed class WasmWindow : UIWindow
{
    public WasmWindow(Widget child, int glHandle, int width, int height, float ratio) : base(child)
    {
        CreateContext(glHandle);
        CreateSurface(width, height, ratio);
    }

    private IGRContext? _grContext;
    private ISurface? _onScreenSurface;
    private ISurface? _offScreenSurface;
    private int _width;
    private int _height;
    private float _ratio;

    public override float ScaleFactor => _ratio;
    public override float Width => _width;
    public override float Height => _height;

    private void CreateContext(int glHandle)
    {
        //创建Surface TODO:根据类型创建，目前仅支持WebGL
        _grContext = Render.Backend.MakeGRContextWebGL(glHandle);
        if (_grContext == null) throw new Exception("Can't create WebGL GRContext");
    }

    private void CreateSurface(int width, int height, float ratio)
    {
        _width = width;
        _height = height;
        _ratio = ratio;

        var pixWidth = (int)(width * ratio);
        var pixHeight = (int)(height * ratio);
        _offScreenSurface = Surface.Create(_grContext!, true, new ImageInfo
            {
                Width = pixWidth, Height = pixHeight,
                AlphaType = AlphaType.Premul, ColorType = ColorType.Rgba8888,
                ColorSpace = Render.Backend.ColorSpaceSRGB
            },
            0, SurfaceOrigin.BottomLeft);
        if (_offScreenSurface == null) throw new Exception("Can't create off screen surface");
        _offScreenSurface!.Canvas.Scale(ratio, ratio);

        _onScreenSurface = Surface.CreateForWebGL(_grContext!, pixWidth, pixHeight);
        if (_onScreenSurface == null) throw new Exception("Can't create on screen surface");
    }

    protected override ICanvas GetOnscreenCanvas() => _onScreenSurface!.Canvas;

    protected override ICanvas GetOffscreenCanvas() => _offScreenSurface!.Canvas;

    protected override void Present() => _grContext?.Flush(true);

    internal void FirstShow()
    {
        RootWidget.Layout(Width, Height);
        Overlay.Layout(Width, Height);

        var widgetsCanvas = GetOffscreenCanvas();
        RootWidget.OnPaint(widgetsCanvas);

        var overlayCanvas = GetOnscreenCanvas();
        widgetsCanvas?.Flush(); // _offScreenSurface?.Flush();
        _offScreenSurface?.Draw(overlayCanvas, 0, 0, null);

        // test draw onscreen bounds
        // var paint = Paint.Shared (Colors.Blue, PaintStyle.Stroke, 18f);
        // overlayCanvas.Scale(_ratio, _ratio);
        // overlayCanvas.DrawRect(0, 0, Width, Height, paint);
        // overlayCanvas.ResetMatrix();
        // //overlayCanvas.Flush();

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

    public override void StartTextInput() => throw new NotImplementedException();
    //((IJSInProcessRuntime)BlazorApplication.JSRuntime).InvokeVoid("PixUI.StartTextInput");

    public override void StopTextInput() => throw new NotImplementedException();
    //((IJSInProcessRuntime)BlazorApplication.JSRuntime).InvokeVoid("PixUI.StopTextInput");

    public override void SetTextInputRect(Rect rect) => throw new NotImplementedException();
    // ((IJSInProcessRuntime)BlazorApplication.JSRuntime).InvokeVoid("PixUI.SetInputRect",
    //     rect.X, rect.Y, rect.Width, rect.Height);

    internal void RouteGoto(int historyId) => RouteHistoryManager.Goto(historyId);

    internal void RoutePush(string path) => RouteHistoryManager.Push(path);

    internal int NewRouteId() => RouteHistoryManager.NewIdForPush();
}