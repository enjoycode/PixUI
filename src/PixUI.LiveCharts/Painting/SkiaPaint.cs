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
using PixUI.LiveCharts.Drawing.Geometries;
using LiveChartsCore.Drawing;
using LiveChartsCore.Generators;
using PixUI.LiveCharts.Drawing;

namespace PixUI.LiveCharts.Painting;

/// <summary>
/// Initializes a new instance of the <see cref="SkiaPaint"/> class.
/// </summary>
/// <param name="strokeThickness">The stroke thickness.</param>
/// <param name="strokeMiter">The stroke miter.</param>
public abstract partial class SkiaPaint(float strokeThickness = 1f, float strokeMiter = 0f)
    : LiveChartsCore.Painting.Paint(strokeThickness, strokeMiter)
{
    /// <summary>
    /// Represents a method that builds a <see cref="SKFont"/> from a <see cref="SKPaint"/>,
    /// a <see cref="SKTypeface"/>, and a size.
    /// </summary>
    /// <param name="paint">The paint instance that skia will use to draw the text.</param>
    /// <param name="typeface">The typefaced requested by the <see cref="SkiaPaint"/> instance or the
    /// <see cref="LabelGeometry"/>.</param>
    /// <param name="size">The text size requested by the <see cref="LabelGeometry"/>.</param>
    /// <returns>A <see cref="SKFont"/> instance that will be used to draw and shape the label.</returns>
    public delegate IFont FontBuilderDelegate(SKPaint paint, SKTypeface typeface, float size);

    internal FontBuilderDelegate _fontBuilder = LiveChartsSkiaSharp.DefaultTextSettings.FontBuilder;
    internal SKPaint? _skiaPaint;

    // The paint owns the native effect/filter: it builds one from the current (possibly interpolated)
    // value, caches it, and rebuilds + disposes it when the source value changes. The PathEffect /
    // ImageFilter objects themselves hold no native resource — they are lightweight parameter values.
    private SKPathEffect? _skPathEffect;
    private PathEffect? _pathEffectSource;
    private SKImageFilter? _skImageFilter;
    private ImageFilter? _imageFilterSource;

    /// <summary>
    /// Gets or sets the SKTypeface.
    /// </summary>
    public SKTypeface? SKTypeface { get; set; }

    /// <summary>
    /// Gets or sets the stroke cap.
    /// </summary>
    /// <value>
    /// The stroke cap.
    /// </value>
    public SKStrokeCap StrokeCap { get; set; }

    /// <summary>
    /// Gets or sets the stroke join.
    /// </summary>
    /// <value>
    /// The stroke join.
    /// </value>
    public SKStrokeJoin StrokeJoin { get; set; }

    /// <summary>
    /// Gets or sets the path effect. Backed by a motion property: the effect is interpolated by the
    /// paint's motion rail, so it animates when the owning paint has a transition for this property
    /// (the paint is the animatable; the effect is just the value).
    /// </summary>
    /// <value>
    /// The path effect.
    /// </value>
    [MotionProperty]
    public partial PathEffect? PathEffect { get; set; }

    /// <summary>
    /// Gets or sets the image filter. Backed by a motion property (like <see cref="PathEffect"/>):
    /// the filter is interpolated by the paint's motion rail, so it animates when the owning paint
    /// has a transition for this property (the paint is the animatable; the filter is just the value).
    /// </summary>
    /// <value>
    /// The image filter.
    /// </value>
    [MotionProperty]
    public partial ImageFilter? ImageFilter { get; set; }

    /// <summary>
    /// Configures the SkiaSharp font manually.
    /// </summary>
    /// <param name="fontBuilder"></param>
    public SkiaPaint ConfigureSkiaSharpFont(FontBuilderDelegate fontBuilder)
    {
        _fontBuilder = fontBuilder;
        return this;
    }

    internal static SKTypeface FallbackTypeface =>
        field ??= (
            //    LiveChartsSkiaSharp.DefaultTextSettings.DefaultTypeface
            // ?? SKTypeface.Default           // let SkiaSharp decide
            TryCreateFont(FontCollection.DefaultFamilyName)
            ?? TryCreateFont("Arial") // common fallback
            ?? TryCreateFont("Helvetica") // macOS/iOS
            ?? TryCreateFont("Roboto") // Android
            ?? TryCreateFont("DejaVu Sans") // Linux
            ?? throw new InvalidOperationException(
                "LiveCharts could not find a default typeface, please set the DefaultTypeface property in the TextSettings. " +
                "LiveCharts could not find a default typeface. Please set the DefaultTypeface property using HasTextSettings, e.g.: " +
                "LiveCharts.Configure(config => config.HasTextSettings(new TextSettings { DefaultTypeface = SKTypeface.FromFamilyName(\"Arial\") }));")
        );

    internal bool IsGlobalSKTypeface =>
        GetSKTypeface() == FallbackTypeface;

    internal static void Map(SkiaPaint from, SkiaPaint to, float progress = 1)
    {
        to.PaintStyle = from.PaintStyle;
        to.IsAntialias = from.IsAntialias;
        to.StrokeCap = from.StrokeCap;
        to.StrokeJoin = from.StrokeJoin;
        to.SKTypeface = from.SKTypeface;

        to.StrokeThickness = from.StrokeThickness + progress * (to.StrokeThickness - from.StrokeThickness);
        to.StrokeMiter = from.StrokeMiter + progress * (to.StrokeMiter - from.StrokeMiter);
        to.PathEffect = PathEffect.Transitionate(from.PathEffect, to.PathEffect, progress);
        to.ImageFilter = ImageFilter.Transitionate(from.ImageFilter, to.ImageFilter, progress);
    }

    internal SKPaint UpdateSkiaPaint(SkiaSharpDrawingContext? context, IDrawnElement? drawnElement)
    {
        SKPaint paint;

        if (_skiaPaint is null)
        {
            paint = Paint.Create();
            _skiaPaint = paint;

            paint.Style = PaintStyle.HasFlag(LiveChartsCore.Painting.PaintStyle.Stroke)
                ? SKPaintStyle.Stroke
                : SKPaintStyle.Fill;
        }
        else
        {
            paint = _skiaPaint;
        }

        paint.IsAntialias = IsAntialias;
        paint.StrokeCap = StrokeCap;
        paint.StrokeJoin = StrokeJoin;
        paint.StrokeMiter = StrokeMiter;
        paint.StrokeWidth = StrokeThickness;

        // Read the effect ONCE: when animating, the motion returns a fresh interpolated effect per
        // call. Rebuild the native only when the source value actually changes (reference identity):
        // a static effect returns the same instance every frame → reuse the cached native; an
        // animating effect returns a new instance per frame → rebuild and dispose the previous one.
        var pathEffect = PathEffect;
        if (pathEffect is null)
        {
            _skPathEffect?.Dispose();
            _skPathEffect = null;
            _pathEffectSource = null;
        }
        else if (!ReferenceEquals(pathEffect, _pathEffectSource))
        {
            _skPathEffect?.Dispose();
            _skPathEffect = pathEffect.CreateNative();
            _pathEffectSource = pathEffect;
        }

        paint.PathEffect = _skPathEffect;

        var imageFilter = ImageFilter;
        if (imageFilter is null)
        {
            _skImageFilter?.Dispose();
            _skImageFilter = null;
            _imageFilterSource = null;
        }
        else if (!ReferenceEquals(imageFilter, _imageFilterSource))
        {
            _skImageFilter?.Dispose();
            _skImageFilter = imageFilter.CreateNative();
            _imageFilterSource = imageFilter;
        }

        paint.ImageFilter = _skImageFilter;

        if (drawnElement is not null)
            paint.StrokeWidth = drawnElement.StrokeThickness;

        // special case for text paints.
        // when  the label is mesured, we do not have a context yet.
        if (context is null)
            return paint;

        context.ActiveSkiaPaint = paint;

        return paint;
    }

    internal SKTypeface GetSKTypeface() => SKTypeface ?? FallbackTypeface;

    internal override void OnPaintFinished(DrawingContext context, IDrawnElement? drawnElement)
    {
        // This method is intentionally left empty.
        // No additional actions are required after painting is finished in this derived class.
    }

    internal override void DisposeTask()
    {
        // if (_skiaPaint is not null && !IsGlobalSKTypeface)
        //     _skiaPaint.Typeface?.Dispose();

        // the paint owns the native effect/filter it built — release them deterministically. Also
        // clear the source references so the next UpdateSkiaPaint rebuilds the native instead of
        // reusing the (now disposed) one when the same effect/filter instance is still assigned.
        _skPathEffect?.Dispose();
        _skPathEffect = null;
        _pathEffectSource = null;
        _skImageFilter?.Dispose();
        _skImageFilter = null;
        _imageFilterSource = null;

        _skiaPaint?.Dispose();
        _skiaPaint = null;
    }

    private static SKTypeface? TryCreateFont(string family)
    {
        var tf = FontCollection.FindTypeface(family, false, false);
        return tf?.FamilyName == family ? tf : null;
    }
}