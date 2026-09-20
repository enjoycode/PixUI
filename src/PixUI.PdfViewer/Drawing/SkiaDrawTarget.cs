using System.Numerics;
using Melville.Pdf.Model.Renderers;
using Melville.Pdf.Model.Renderers.GraphicsStates;

namespace PixUI.PdfViewer.Drawing;

internal class SkiaDrawTarget : IDrawTarget
{
    private readonly ICanvas _target;
    private readonly GraphicsStateStack<SkiaGraphicsState> _state;
    private IPath _compositePath = Path.Create();
    private IPathBuilder? _pathBuilder;

    public SkiaDrawTarget(ICanvas target, GraphicsStateStack<SkiaGraphicsState> state)
    {
        _target = target;
        _state = state;
    }

    public void Dispose() => _pathBuilder?.Dispose();

    private void TryAddCurrent()
    {
        if (_pathBuilder == null /*|| _pathBuilder.IsEmpty()*/) return;
        //var matrix = currentMatrix.Transform();
        using var compositePathBuilder = PathBuilder.Create();
        compositePathBuilder.AddPath(_pathBuilder.Detach() /*, ref matrix*/);
        _compositePath = compositePathBuilder.Detach();
        _pathBuilder = PathBuilder.Create();
    }

    public void MoveTo(Vector2 startPoint) => GetOrCreatePath()
        .MoveTo(startPoint.X, startPoint.Y);

    //The Adobe Pdf interpreter ignores drawing operations before the first MoveTo operation.
    //If path == null then we have not yet gotten a moveto command and we just ignore all the drawing operations
    private IPathBuilder GetOrCreatePath() => _pathBuilder ??= PathBuilder.Create();

    public void LineTo(Vector2 endPoint) => _pathBuilder?.LineTo(
        endPoint.X, endPoint.Y);

    public void ClosePath()
    {
        _pathBuilder?.Close();
    }

    public void CurveTo(Vector2 control, Vector2 endPoint) =>
        _pathBuilder?.QuadTo(control.X, control.Y, endPoint.X, endPoint.Y);

    public void CurveTo(Vector2 control1, Vector2 control2, Vector2 endPoint) =>
        _pathBuilder?.CubicTo(control1.X, control1.Y, control2.X, control2.Y, endPoint.X, endPoint.Y);

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
        _compositePath.FillType = evenOddFillRule ? PathFillType.EvenOdd : PathFillType.Winding;


    public void ClipToPath(bool evenOddRule)
    {
        TryAddCurrent();
        SetCurrentFillRule(evenOddRule);
        _target.ClipPath(_compositePath, ClipOp.Intersect, true);
    }
}