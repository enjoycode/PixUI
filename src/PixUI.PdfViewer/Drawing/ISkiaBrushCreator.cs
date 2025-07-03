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
    private readonly Paint value;

    public SolidColorBrushCreator(DeviceColor color)
    {
        value = new Paint() { Color = color.AsSkColor() };
    }

    public void Dispose() => value.Dispose();

    public Paint CreateBrush(SkiaGraphicsState topState) => value;
}

internal abstract class IntermediateBrushHolder<T> : ISkiaBrushCreator where T : IDisposable
{
    protected readonly T value;
    private readonly List<IDisposable> items = new();

    protected IntermediateBrushHolder(T value)
    {
        this.value = value;
    }

    public void Dispose()
    {
        foreach (var toDispose in items)
        {
            toDispose.Dispose();
        }
    }

    protected TLocal RegisterForDispose<TLocal>(TLocal product) where TLocal : class, IDisposable
    {
        items.Add(product);
        return product;
    }

    public abstract Paint CreateBrush(SkiaGraphicsState topState);
}

internal class SurfacePatternHolder : IntermediateBrushHolder<SKSurface>
{
    private readonly Matrix3x2 patternTransform;

    public SurfacePatternHolder(SKSurface value, Matrix3x2 patternTransform) : base(value)
    {
        this.patternTransform = patternTransform;
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