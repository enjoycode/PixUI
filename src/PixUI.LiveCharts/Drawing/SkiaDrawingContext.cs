// The MIT License(MIT)
//
// Copyright(c) 2021 Alberto Rodriguez Orozco & LiveCharts Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using LiveChartsCore.Drawing;
using LiveChartsCore.Motion;
using LiveChartsCore.Painting;
using PixUI.LiveCharts.Painting;

namespace PixUI.LiveCharts.Drawing;

/// <summary>
/// Defines a skia sharp drawing context.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SkiaSharpDrawingContext"/> class.
/// </remarks>
/// <param name="motionCanvas">The motion canvas.</param>
/// <param name="canvas">The canvas.</param>
/// <param name="background">The background color.</param>
/// <param name="clearBlendMode">The blend mode to use when clearing the canvas. Default is <see cref="BlendMode.SrcIn"/>.</param>
/// <param name="clearCanvasOnNewFrame">
/// Whether the canvas should be cleared at the beginning of each frame.
/// Avalonia and Uno-Skia hosts clear the surface themselves before invoking
/// <see cref="DrawingContext.OnBeginDraw"/>, so they pass <c>false</c>. Default
/// <c>true</c> for hosts where SkiaSharp owns the surface and must clear it
/// explicitly (WPF, MAUI, WinForms, Blazor, in-memory).
/// </param>
public class SkiaSharpDrawingContext(
    CoreMotionCanvas motionCanvas,
    SKCanvas canvas,
    SKColor background,
    BlendMode clearBlendMode = BlendMode.SrcIn,
    bool clearCanvasOnNewFrame = true)
    : DrawingContext
{
    /// <summary>
    /// Gets a value indicating whether the canvas is cleared at the beginning of
    /// each frame. See the constructor parameter of the same name.
    /// </summary>
    public bool ClearCanvasOnNewFrame { get; } = clearCanvasOnNewFrame;

    /// <summary>
    /// Gets or sets the motion canvas.
    /// </summary>
    /// <value>
    /// The motion canvas.
    /// </value>
    public CoreMotionCanvas MotionCanvas { get; } = motionCanvas;

    /// <summary>
    /// Gets or sets the canvas.
    /// </summary>
    /// <value>
    /// The canvas.
    /// </value>
    public SKCanvas Canvas { get; } = canvas;

    /// <summary>
    /// Gets or sets the paint.
    /// </summary>
    /// <value>
    /// The paint.
    /// </value>
    public SKPaint ActiveSkiaPaint { get; internal set; } = null!;

    /// <summary>
    /// Gets or sets the background.
    /// </summary>
    public SKColor Background { get; } = background;

    /// <summary>
    /// Gets the blend mode used when clearing the canvas during rendering operations.
    /// </summary>
    public BlendMode ClearBlendMode { get; } = clearBlendMode;

    /// <inheritdoc cref="DrawingContext.LogOnCanvas(string)"/>
    public override void LogOnCanvas(string log)
    {
        const float textSize = 14;
        using var font = SkiaPaint.FallbackTypeface.MakeFont(textSize);

        using var backgroundPaint = Paint.Create(Colors.Black.WithAlpha(180), PaintStyle.Fill);

        var lines = log.Split('`');

        Canvas.DrawRect(SKRect.FromLTWH(10, 0, 400, (textSize + 4f) * lines.Length), backgroundPaint);

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;
            Canvas.DrawString(line, 10, 10 + 2 + (textSize + 4f) * i, font, Colors.White);
        }
    }

    internal override void OnBeginDraw()
    {
        if (!ClearCanvasOnNewFrame)
            return;

        // clear the canvas with the background color if it's fully opaque
        if (Background.Alpha == 255)
        {
            Canvas.Clear(Background);
            return;
        }

        // if tranparency, we need to clear the canvas with a paint that has the background color and the clear blend mode
        // each skiasharp view depending on the platform or if GPU is eneabled could behave differently, adjust
        // the ClearBlendMode if needed at the MotionCanvas class.

        using var backgroundPaint = Paint.Create(Background, PaintStyle.Fill);
        backgroundPaint.BlendMode = ClearBlendMode;


        var bounds = Canvas.ClipBounds; //Canvas.DeviceClipBounds;
        Canvas.DrawRect(bounds, backgroundPaint);
        //Canvas.DrawRect(SKRect.FromLTWH(0, 0, Width, Height), backgroundPaint);
    }

    internal override void OnEndDraw()
    {
        // No cleanup is needed at the end of the draw operation.
        // This method is intentionally left empty, following the pattern in SkiaPaint.OnPaintFinished.
    }

    internal override void OnBeginZone(CanvasZone zone)
    {
        if (zone.Clip == LvcRectangle.Empty) return;
        
        zone.StateId = Canvas.Save();
        Canvas.ClipRect(new(zone.Clip.X, zone.Clip.Y, zone.Clip.X + zone.Clip.Width, zone.Clip.Y + zone.Clip.Height));
    }

    internal override void OnEndZone(CanvasZone zone)
    {
        if (zone.Clip == LvcRectangle.Empty) return;
        
        Canvas.RestoreToCount(zone.StateId);
    }

    internal override void Draw(IDrawnElement drawable)
    {
        var opacity = ActiveOpacity;

        var element = (IDrawnElement<SkiaSharpDrawingContext>)drawable;

        var canvasState = 0;
        if (element.HasTransform)
        {
            canvasState = Canvas.Save();
            var transform = BuildTransform(element);
            Canvas.Concat(transform);
        }

        if (ActiveLvcPaint is null)
        {
            // if the active paint is null, we need to draw by the element paint

            var elementFill = element.Fill;
            var elementStroke = element.Stroke;
            var elementPaint = element.Paint;

            if (elementFill is not null)
                DrawByPaint(elementFill, element, opacity);

            if (elementStroke is not null)
                DrawByPaint(elementStroke, element, opacity);

            // One paint can arrive under two names: a label reports its own Paint as its Fill, so
            // that a paint set on a single point's label is seen when the series' DataLabelsPaint
            // task draws it -- that path reads Fill and Stroke and never Paint (#1902, see
            // BaseLabelGeometry). Here, where the element draws itself, the alias means both reads
            // return the same instance, and painting each in turn drew the label twice: at twice
            // the cost, visibly heavier, and compounding alpha on a translucent paint.
            //
            // Draw a paint once, whichever of the three it came in as.
            if (elementPaint is not null &&
                !ReferenceEquals(elementPaint, elementFill) &&
                !ReferenceEquals(elementPaint, elementStroke))
                DrawByPaint(elementPaint, element, opacity);
        }
        else
        {
            // we will draw using the active paint while the element paint is null

            if (ActiveLvcPaint.PaintStyle.HasFlag(LiveChartsCore.Painting.PaintStyle.Fill))
            {
                var elementFill = element.Fill;

                if (elementFill is null)
                    DrawElement(element, opacity);
                else
                    DrawByPaint(elementFill, element, opacity);
            }

            if (ActiveLvcPaint.PaintStyle.HasFlag(LiveChartsCore.Painting.PaintStyle.Stroke))
            {
                var elementStroke = element.Stroke;

                if (elementStroke is null)
                    DrawElement(element, opacity);
                else
                    DrawByPaint(elementStroke, element, opacity);
            }
        }

        if (element.HasTransform)
        {
            Canvas.RestoreToCount(canvasState);
        }
    }

    internal override void SelectPaint(LiveChartsCore.Painting.Paint paint)
    {
        ActiveLvcPaint = paint;
        //ActiveSkiaPaint = paint.SKPaint; set by paint.OnPaintStarted

        paint.OnPaintStarted(this, null);
    }

    internal override void ClearPaintSelection(LiveChartsCore.Painting.Paint paint)
    {
        paint.OnPaintFinished(this, null);

        ActiveLvcPaint = null!;
        ActiveSkiaPaint = null!;
    }

    private void DrawByPaint(LiveChartsCore.Painting.Paint paint, IDrawnElement<SkiaSharpDrawingContext> element,
        float opacity)
    {
        var originalPaint = ActiveSkiaPaint;
        var originalTask = ActiveLvcPaint;

        // hack for now...
        // ActiveLvcPaint must be null for this kind of draw method...
        // normally used to draw tooltips and legends
        // Improve this? maybe a cleaner way?
        if (paint != MeasureTask.Instance)
        {
            ActiveLvcPaint = paint;

            // A per-element paint (an override the geometry carries, not a registered paint task) is
            // visited only here, so the frame loop never resets its validity latch the way it does for
            // tasks and geometries. Reset it before reading; OnPaintStarted re-flags it false while the
            // paint is still animating, and leaves it true once the transition completes.
            paint.IsValid = true;
            paint.OnPaintStarted(this, element);
        }

        DrawElement(element, opacity);

        // Fold the per-element paint's validity into the element so an animating override keeps the
        // canvas redrawing until its transition completes (the frame loop only checks geometries and
        // registered tasks, never per-element paints). The measure pass does not paint, so skip it.
        if (paint != MeasureTask.Instance)
            element.IsValid = element.IsValid && paint.IsValid;

        paint.OnPaintFinished(this, element);

        ActiveSkiaPaint = originalPaint;
        ActiveLvcPaint = originalTask;

        // Honor RemoveOnCompleted for a per-element override paint: once its transition is done, detach
        // it from the element and release its native resources, so the element draws with its paint task
        // again (or not at all) on the next frame. The frame loop only acts on RemoveOnCompleted for
        // registered tasks and geometries — a per-element paint is neither, so without this an override
        // assigned per hover would never be cleaned up and would orphan its SKPaint until finalization.
        if (paint != MeasureTask.Instance && paint.RemoveOnCompleted && paint.IsValid)
        {
            if (ReferenceEquals(element.Fill, paint)) element.Fill = null;
            else if (ReferenceEquals(element.Stroke, paint)) element.Stroke = null;
            else if (ReferenceEquals(element.Paint, paint)) element.Paint = null;

            paint.DisposeTask();
        }
    }

    private void DrawElement(IDrawnElement<SkiaSharpDrawingContext> element, float opacity)
    {
        var hasGeometryOpacity =
            ActiveLvcPaint is not null &&
            opacity < 1;

        var hasShadow =
            ActiveLvcPaint is not null &&
            element.DropShadow is not null &&
            element.DropShadow != LvcDropShadow.Empty;

        SKImageFilter? originalFilter = null;

        if (hasGeometryOpacity)
        {
            ActiveLvcPaint!.ApplyOpacityMask(this, opacity, element);
        }

        if (hasShadow)
        {
            var shadow = element.DropShadow!;
            originalFilter = ActiveSkiaPaint.ImageFilter;

            // A per-element shadow that is animating away interpolates toward a transparent,
            // zero-radius, zero-offset shadow. If the paint itself carries a base drop shadow
            // (e.g. an always-on glow), floor the element shadow at that base — radius, color AND
            // offset — so it eases down to the base instead of fading/sliding to nothing and
            // popping back when the transition completes (it becomes null, and the paint's own
            // filter takes over). ActiveLvcPaint is the paint currently drawing this element, so
            // the base is automatically the correct fallback per draw pass (fill vs stroke).
            var dx = shadow.Dx;
            var dy = shadow.Dy;
            var sigmaX = shadow.SigmaX;
            var sigmaY = shadow.SigmaY;
            var color = new SKColor(shadow.Color.R, shadow.Color.G, shadow.Color.B, shadow.Color.A);
            if (ActiveLvcPaint is SkiaPaint { ImageFilter: DropShadow baseShadow })
            {
                // Grow the radius from the base; take the offset and color from the base. The
                // element's own color/offset are interpolating toward nothing, so blending them in
                // would still fade/slide out — the radius is what conveys the grow/shrink anyway.
                sigmaX = Math.Max(sigmaX, baseShadow.SigmaX);
                sigmaY = Math.Max(sigmaY, baseShadow.SigmaY);
                dx = baseShadow.Dx;
                dy = baseShadow.Dy;
                color = baseShadow.Color;
            }

            ActiveSkiaPaint.ImageFilter = ImageFilter.CreateDropShadow(dx, dy, sigmaX, sigmaY, color, null);
        }

        element.Draw(this);

        if (hasShadow)
        {
            ActiveSkiaPaint.ImageFilter!.Dispose();
            ActiveSkiaPaint.ImageFilter = originalFilter;
        }

        if (hasGeometryOpacity)
        {
            ActiveLvcPaint!.RestoreOpacityMask(this, opacity, element);
        }
    }

    private static Matrix3 BuildTransform(IDrawnElement<SkiaSharpDrawingContext> element)
    {
        var m = element.Measure();
        var o = element.TransformOrigin;
        var p = new SKPoint(element.X, element.Y);
        var xo = m.Width * o.X;
        var yo = m.Height * o.Y;

        var origin = new SKPoint(p.X + xo, p.Y + yo);
        var matrix = Matrix3.CreateIdentity();

        if (element.HasTranslate)
        {
            var t = element.TranslateTransform;
            //matrix = Matrix3.Concat(matrix, Matrix3.CreateTranslation(t.X, t.Y));
            matrix = Matrix3.CreateTranslation(t.X, t.Y);
        }

        if (element.HasRotation)
        {
            matrix = Matrix3.Concat(matrix, Matrix3.CreateRotationDegrees(
                element.RotateTransform, origin.X, origin.Y));
        }

        if (element.HasScale)
        {
            var s = element.ScaleTransform;
            matrix = Matrix3.Concat(matrix, Matrix3.CreateScale(s.X, s.Y, origin.X, origin.Y));
        }

        if (element.HasSkew)
        {
            var skew = element.SkewTransform;
            var skewMatrix = Matrix3.CreateSkew((float)Math.Tan(skew.X * Math.PI / 180),
                (float)Math.Tan(skew.Y * Math.PI / 180));
            
            var translateToOrigin = Matrix3.CreateTranslation(origin.X, origin.Y);
            var translateBack = Matrix3.CreateTranslation(-origin.X, -origin.Y);
            matrix = Matrix3.Concat(matrix, translateToOrigin);
            matrix = Matrix3.Concat(matrix, skewMatrix);
            matrix = Matrix3.Concat(matrix, translateBack);
        }

        return matrix;
    }
}