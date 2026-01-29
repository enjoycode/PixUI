using System.Numerics;
using Melville.Pdf.Model.Renderers;
using Melville.Pdf.Model.Renderers.GraphicsStates;
using Path = PixUI.Path;

namespace PixUI.PdfViewer.Drawing;

internal class SkiaDrawTarget : IDrawTarget, IDisposable
{
    private readonly Canvas _target;
    private readonly GraphicsStateStack<SkiaGraphicsState> _state;
    private readonly Path _compositePath = new();
    private Path? _path = null;

    public SkiaDrawTarget(Canvas target, GraphicsStateStack<SkiaGraphicsState> state)
    {
        _target = target;
        _state = state;
    }

    public void Dispose() => _path?.Dispose();

    private void TryAddCurrent()
    {
        if (_path == null || _path.IsEmpty()) return;
        //var matrix = currentMatrix.Transform();
        _compositePath.AddPath(_path); //, ref matrix);
        _path = new Path();
    }

    public void MoveTo(Vector2 startPoint) => GetOrCreatePath()
        .MoveTo(startPoint.X, startPoint.Y);

    //The Adobe Pdf interpreter ignores drawing operations before the first MoveTo operation.
    //If path == null then we have not yet gotten a moveto command and we just ignore all the drawing operations
    private Path GetOrCreatePath() => _path ??= new Path();

    public void LineTo(Vector2 endPoint) => _path?.LineTo(
        endPoint.X, endPoint.Y);

    public void ClosePath()
    {
        _path?.Close();
    }

    public void CurveTo(Vector2 control, Vector2 endPoint) =>
        _path?.QuadTo(control.X, control.Y, endPoint.X, endPoint.Y);

    public void CurveTo(Vector2 control1, Vector2 control2, Vector2 endPoint) =>
        _path?.CubicTo(control1.X, control1.Y, control2.X, control2.Y, endPoint.X, endPoint.Y);

    public void EndGlyph() { }

    public void PaintPath(bool stroke, bool fill, bool evenOddFillRule)
    {
        TryAddCurrent();
        InnerPaintPath(stroke, fill, evenOddFillRule);
    }

    private void InnerPaintPath(bool stroke, bool fill, bool evenOddFillRule)
    {
        if (fill && _state.StronglyTypedCurrentState().Brush() is { } brush)
        {
            SetCurrentFillRule(evenOddFillRule);
            _target.DrawPath(_compositePath, brush);
        }

        if (stroke && _state.StronglyTypedCurrentState().Pen() is { } pen)
        {
            _target.DrawPath(_compositePath, pen);
        }
    }

    private void SetCurrentFillRule(bool evenOddFillRule) =>
        _compositePath.FillType = evenOddFillRule ? SKPathFillType.EvenOdd : SKPathFillType.Winding;


    public void ClipToPath(bool evenOddRule)
    {
        TryAddCurrent();
        SetCurrentFillRule(evenOddRule);
        _target.ClipPath(_compositePath, ClipOp.Intersect, true);
    }
}