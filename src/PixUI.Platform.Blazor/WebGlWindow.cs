using System.Runtime.Versioning;

namespace PixUI.Platform.Blazor;

[SupportedOSPlatform("browser")]
public sealed class WebGlWindow : BlazorWindow
{
    public WebGlWindow(Widget child, RunInfo runInfo) : base(child, runInfo.RoutePath)
    {
        CreateContext();
        CreateSurface(runInfo.Width, runInfo.Height, runInfo.PixelRatio);
    }

    private IGRContext? _grContext;
    private ISurface? _onScreenSurface;
    private ICanvas? _onScreenCanvas;
    private ISurface? _offScreenSurface;
    private ICanvas? _offScreenCanvas;
    private int _width;
    private int _height;
    private float _ratio;

    public override float ScaleFactor => _ratio;
    public override float Width => _width;
    public override float Height => _height;

    private void CreateContext()
    {
        _grContext = Render.Backend.MakeGRContextWebGL();
        if (_grContext == null) throw new Exception("Can't create WebGL GRContext");
    }

    private void CreateSurface(int width, int height, float ratio)
    {
        _width = width;
        _height = height;
        _ratio = ratio;

        var pixWidth = (int)(width * ratio);
        var pixHeigh = (int)(height * ratio);
        _offScreenSurface = Surface.Create(_grContext!, true, new ImageInfo
            {
                Width = pixWidth, Height = pixHeigh,
                AlphaType = AlphaType.Premul, ColorType = ColorType.Rgba8888,
                ColorSpace = Render.Backend.ColorSpaceSRGB
            },
            0, SurfaceOrigin.BottomLeft);
        _offScreenCanvas = _offScreenSurface!.Canvas;
        _offScreenCanvas.Scale(ratio, ratio);
        _onScreenSurface = Surface.CreateForWebGL(_grContext!, pixWidth, pixHeigh);
        _onScreenCanvas = _onScreenSurface!.Canvas;
    }

    protected override ICanvas GetOnscreenCanvas() => _onScreenCanvas!;

    protected override ICanvas GetOffscreenCanvas() => _offScreenCanvas!;

    protected override void Present() => _grContext?.Flush(true);

    protected internal override void CreateOnScreenSurface(int onScreenTextureId)
    {
        throw new NotImplementedException();
    }

    protected override void FlushOffScreen()
    {
        //TODO: _offScreenSurface.Flush();
    }
}