using System.Numerics;
using Melville.Pdf.LowLevel.Model.ContentStreams;
using Melville.Pdf.Model.Renderers.Colors;
using Melville.Pdf.Model.Renderers.GraphicsStates;
using PixUI;
using PixUI.PdfViewer.Drawing;

internal static class SkiaStateInterpreter
{
    internal static Paint Brush(this SkiaGraphicsState state)
    {
        var ret = state.NonstrokeBrush.TryGetNativeBrush<ISkiaBrushCreator>().CreateBrush(state);
        ret.Style = PaintStyle.Fill;
        return ret;
    }

    internal static Paint Pen(this SkiaGraphicsState state)
    {
        var paint = state.StrokeBrush.TryGetNativeBrush<ISkiaBrushCreator>().CreateBrush(state);
        paint.Style = PaintStyle.Stroke;
        paint.StrokeWidth = (float)state.EffectiveLineWidth();
        paint.StrokeCap = StrokeCap(state.LineCap);
        paint.StrokeJoin = CreateStrokeJoin(state.LineJoinStyle);
        paint.StrokeMiter = (float)state.MiterLimit;

        SetDashState(state, paint);
        return paint;
    }

    public static Color AsSkColor(in this DeviceColor dc) => new(dc.RedByte, dc.GreenByte, dc.BlueByte, dc.Alpha);

    //By coincidence these two enums are equivilent, so a simple cast works.
    private static StrokeJoin CreateStrokeJoin(LineJoinStyle joinStyle) => (StrokeJoin)joinStyle;

    private static void SetDashState(GraphicsState state, Paint paint)
    {
        paint.PathEffect = state.IsDashedStroke() ? CreatePathEffect(state.DashArray, state.DashPhase) : null;
    }

    private static PathEffect CreatePathEffect(IReadOnlyList<double> dashes, double phase) =>
        PathEffect.CreateDash(CreateDashArray(dashes), (float)phase)!;

    private static float[] CreateDashArray(IReadOnlyList<double> dashes)
    {
        return dashes.Count % 2 == 0
            ? dashes.Select(i => (float)i).ToArray()
            : dashes.Concat(dashes).Select(i => (float)i).ToArray();
    }

    // by coincidence PDF and skia define these with the same values so we
    // can just cast the enum right across
    private static StrokeCap StrokeCap(LineCap cap) => (StrokeCap)cap;

    public static Matrix3 Transform(this GraphicsState gs) => Transform(gs.TransformMatrix);

    public static Matrix3 Transform(this Matrix3x2 tm) => new(
        tm.M11, tm.M21, tm.M31, tm.M12, tm.M22, tm.M32, 0, 0, 1);
}