using Melville.Parsing.AwaitConfiguration;
using Melville.Pdf.LowLevel.Model.Conventions;
using Melville.Pdf.LowLevel.Model.Objects;
using Melville.Pdf.Model.Renderers.Colors;
using Melville.Pdf.Model.Renderers.DocumentRenderers;
using Melville.Pdf.Model.Renderers.GraphicsStates;
using Melville.Pdf.Model.Renderers.Patterns.ShaderPatterns;
using Melville.Pdf.Model.Renderers.Patterns.TilePatterns;

namespace PixUI.PdfViewer.Drawing;

internal class SkiaNativeBrush : INativeBrush
{
    private DeviceColor _color = new DeviceColor(0, 0, 0, 255);
    private double _alpha = 1;
    private ISkiaBrushCreator? _creator = null;

    /// <inheritdoc />
    public void SetSolidColor(DeviceColor color)
    {
        this._color = color;
        _creator = null;
    }

    /// <inheritdoc />
    public void SetAlpha(double alpha)
    {
        this._alpha = alpha;
        if (_creator is SolidColorBrushCreator)
            _creator = null;
    }

    /// <inheritdoc />
    public async ValueTask SetPatternAsync(PdfDictionary pattern, DocumentRenderer parentRenderer,
        GraphicsState prior)
    {
        _creator = await pattern.GetOrDefaultAsync(KnownNames.PatternType, 0).CA() switch
        {
            1 => await CreateTilePatternBrushAsync(pattern, parentRenderer, prior).CA(),
            2 => await CreateShaderBrushAsync(pattern, prior).CA(),
            _ => new SolidColorBrushCreator(DeviceColor.Invisible)
        };
    }

    private async Task<ISkiaBrushCreator> CreateTilePatternBrushAsync(
        PdfDictionary pattern, DocumentRenderer parentRenderer, GraphicsState prior)
    {
        var request = await TileBrushRequest.ParseAsync(pattern).CA();
        var tileItem = await RenderWithSkia.ToSurfaceAsync(parentRenderer.PatternRenderer(request, prior), 0).CA();
        return new SurfacePatternHolder(tileItem, request.PatternTransform);
    }

    private async Task<ISkiaBrushCreator> CreateShaderBrushAsync(PdfDictionary pattern, GraphicsState prior)
    {
        throw new NotImplementedException();
        // var shader = await ShaderParser.ParseShaderAsync(pattern).CA();
        // var bitmap = new SKBitmap(new SKImageInfo(
        //     (int)prior.PageWidth, (int)prior.PageHeight,
        //     SKColorType.Bgra8888, SKAlphaType.Premul, SKColorSpace.CreateSrgb()));
        // unsafe
        // {
        //     shader.RenderBits((uint*)bitmap.GetPixels().ToPointer(), bitmap.Width, bitmap.Height);
        // }
        //
        // return new ImagePatternHolder(bitmap);
    }

    /// <inheritdoc />
    public void WriteColorTo(INativeBrush target)
    {
        if (target is not SkiaNativeBrush ret) return;
        ret._color = _color;
        ret._alpha = _alpha;
        ret._creator = _creator;
    }

    /// <inheritdoc />
    public T TryGetNativeBrush<T>() => (T)(_creator ??= ComputeSolidBrush());

    private SolidColorBrushCreator ComputeSolidBrush() => new(_color.AsPreMultiplied().WithAlpha(_alpha));
}

internal class SkiaGraphicsState() : GraphicsState(new SkiaNativeBrush(), new SkiaNativeBrush()) { }