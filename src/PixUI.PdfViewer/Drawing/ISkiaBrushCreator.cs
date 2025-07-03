using System.Numerics;
using Melville.Pdf.Model.Renderers.Colors;
using Melville.Pdf.Model.Renderers.GraphicsStates;
using PixUI;

namespace PixUI.PdfViewer.Drawing;

internal interface ISkiaBrushCreator : IDisposable
{
    Paint CreateBrush(SkiaGraphicsState topState);
}

internal class SolidColorBrushCreator : ISkiaBrushCreator
{
    private readonly Paint _value;

    public SolidColorBrushCreator(DeviceColor color)
    {
        _value = new Paint() { Color = color.AsSkColor(), AntiAlias = true };
    }

    public void Dispose() => _value.Dispose();

    public Paint CreateBrush(SkiaGraphicsState topState) => _value;
}

internal abstract class IntermediateBrushHolder<T> : ISkiaBrushCreator where T : IDisposable
{
    protected readonly T Value;
    private readonly List<IDisposable> _items = new();

    protected IntermediateBrushHolder(T value)
    {
        this.Value = value;
    }

    public void Dispose()
    {
        foreach (var toDispose in _items)
        {
            toDispose.Dispose();
        }
    }

    protected TLocal RegisterForDispose<TLocal>(TLocal product) where TLocal : class, IDisposable
    {
        _items.Add(product);
        return product;
    }

    public abstract Paint CreateBrush(SkiaGraphicsState topState);
}

internal class SurfacePatternHolder : IntermediateBrushHolder<SKSurface>
{
    private readonly Matrix3x2 _patternTransform;

    public SurfacePatternHolder(SKSurface value, Matrix3x2 patternTransform) : base(value)
    {
        this._patternTransform = patternTransform;
    }

    public override Paint CreateBrush(SkiaGraphicsState topState)
    {
        throw new NotImplementedException();
        // return RegisterForDispose(new Paint()
        //     {
        //         Shader = Shader.CreateImage(value.Snapshot(), TileMode.Repeat, TileMode.Repeat)
        //             .WithLocalMatrix((patternTransform * topState.RevertToPixelsMatrix()).Transform())
        //     }
        // );
    }
}

// internal class ImagePatternHolder : IntermediateBrushHolder<SKBitmap>
// {
//     public ImagePatternHolder(SKBitmap value) : base(value) { }
//
//     public override Paint CreateBrush(SkiaGraphicsState topState)
//     {
//         return RegisterForDispose(new Paint()
//         {
//             Shader = Shader.CreateBitmap(value, SKShaderTileMode.Clamp, SKShaderTileMode.Clamp)
//                 .WithLocalMatrix(topState.RevertToPixelsMatrix().Transform())
//         });
//     }
// }