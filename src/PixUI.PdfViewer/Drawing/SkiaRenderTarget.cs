using System.Numerics;
using Melville.Parsing.AwaitConfiguration;
using Melville.Pdf.LowLevel.Model.Wrappers;
using Melville.Pdf.Model.Renderers;
using Melville.Pdf.Model.Renderers.Bitmaps;

namespace PixUI.PdfViewer.Drawing;

internal partial class SkiaRenderTarget : RenderTargetBase<Canvas, SkiaGraphicsState>
{
    public SkiaRenderTarget(Canvas target) :
        base(target)
    {
        State.ContextPushed += (_, __) => Target.Save();
        State.BeforeContextPopped += (_, __) => Target.Restore();
        State.TransformPushed += (_, e) => Target.SetMatrix(new Matrix4(e.CumulativeMatrix.Transform()));
    }

    public override void SetBackgroundRect(in PdfRect rect, double width, double height, in Matrix3x2 transform)
    {
        Target.Clear(new Color(0xFFFFFFFF) /*White*/);
    }

    public override IDrawTarget CreateDrawTarget() => new SkiaDrawTarget(Target, State);


    public override async ValueTask RenderBitmapAsync(IPdfBitmap bitmap)
    {
        throw new NotImplementedException();
        // using var skBitmap = await bitmap.ToSkBitmapAsync().CA();
        // var oldMatrix = Target.TotalMatrix;
        // Target.SetMatrix(oldMatrix.PreConcat(new SKMatrix(1, 0, 0, 0, -1, 1, 0, 0, 1)));
        // Target.DrawImage(SKImage.FromBitmap(skBitmap),
        //     new SKRect(0, 0, bitmap.Width, bitmap.Height), new SKRect(0, 0, 1, 1),
        //     Quality(bitmap), fillPaint);
        // Target.SetMatrix(oldMatrix);
    }

    // private SKSamplingOptions Quality(IPdfBitmap bitmap) =>
    //     bitmap.ShouldInterpolate(State.StronglyTypedCurrentState().TransformMatrix)
    //         ? new SKSamplingOptions(SKFilterMode.Linear)
    //         : new SKSamplingOptions(SKFilterMode.Nearest);

    private readonly Paint _fillPaint = new();

    public override void Dispose()

    {
        _fillPaint.Dispose();
        base.Dispose();
    }
}