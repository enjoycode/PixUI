﻿// The MIT License(MIT)
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
using LiveChartsCore.Kernel;
using LiveChartsCore.Measure;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Geometries;
using LiveChartsCore.VisualElements;

namespace LiveCharts.VisualElements;

/// <summary>
/// Defines a visual element with stroke and fill properties.
/// </summary>
public class LabelVisual : VisualElement<SkiaDrawingContext>
{
    internal LabelGeometry? _labelGeometry;
    internal IPaint<SkiaDrawingContext>? _paint;
    internal bool _isVirtual = false;
    internal string _text = string.Empty;
    internal double _textSize = 12;
    internal Align _verticalAlignment = Align.Middle;
    internal Align _horizontalAlignment = Align.Middle;
    internal LvcColor _backgroundColor;
    internal Padding _padding = Padding.All(0);
    internal double _rotation;
    internal float _lineHeight = 1.75f;
    internal LvcPoint _translate = new();
    private LvcSize _actualSize = new();
    private LvcPoint _targetPosition = new();

    /// <summary>
    /// Gets or sets the fill paint.
    /// </summary>
    public IPaint<SkiaDrawingContext>? Paint
    {
        get => _paint;
        set => SetPaintProperty(ref _paint, value);
    }

    /// <summary>
    /// Gets or sets the label text.
    /// </summary>
    public string Text { get => _text; set { _text = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the font size.
    /// </summary>
    public double TextSize { get => _textSize; set { _textSize = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the rotation.
    /// </summary>
    public double Rotation { get => _rotation; set { _rotation = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the translate transform.
    /// </summary>
    public LvcPoint Translate { get => _translate; set { _translate = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the vertical alignment.
    /// </summary>
    public Align VerticalAlignment { get => _verticalAlignment; set { _verticalAlignment = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the horizontal alignment.
    /// </summary>
    public Align HorizontalAlignment { get => _horizontalAlignment; set { _horizontalAlignment = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the background color.
    /// </summary>
    public LvcColor BackgroundColor { get => _backgroundColor; set { _backgroundColor = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the padding.
    /// </summary>
    public Padding Padding { get => _padding; set { _padding = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the line height [times the text measured height].
    /// </summary>
    public float LineHeight { get => _lineHeight; set { _lineHeight = value; OnPropertyChanged(); } }
    
    protected override IPaint<SkiaDrawingContext>?[] GetPaintTasks()
    {
        return new[] { _paint };
    }
    
    protected override void AlignToTopLeftCorner()
    {
        VerticalAlignment = Align.Start;
        HorizontalAlignment = Align.Start;
    }

    /// <inheritdoc cref="VisualElement{TDrawingContext}.OnInvalidated(Chart{TDrawingContext}, Scaler, Scaler)"/>
    protected override void OnInvalidated(LiveChartsCore.Chart<SkiaDrawingContext> chart, Scaler? primaryScaler, Scaler? secondaryScaler)
    {
        var x = (float)X;
        var y = (float)Y;

        if (LocationUnit == MeasureUnit.ChartValues)
        {
            if (primaryScaler is null || secondaryScaler is null)
                throw new Exception($"You can not use {MeasureUnit.ChartValues} scale at this element.");

            x = secondaryScaler.ToPixels(x);
            y = primaryScaler.ToPixels(y);
        }

        _targetPosition = new((float)X + _xc, (float)Y + _yc);
        _ = Measure(chart, primaryScaler, secondaryScaler);

        if (_labelGeometry is null)
        {
            var cp = GetPositionRelativeToParent();

            _labelGeometry = new LabelGeometry
            {
                Text = Text,
                TextSize = (float)TextSize,
                X = cp.X,
                Y = cp.Y,
                RotateTransform = (float)Rotation,
                TranslateTransform = Translate,
                VerticalAlign = VerticalAlignment,
                HorizontalAlign = HorizontalAlignment,
                Background = BackgroundColor,
                Padding = Padding
            };

            _ = _labelGeometry
                .TransitionateProperties()
                .WithAnimation(chart)
                .CompleteCurrentTransitions();
        }

        _labelGeometry.Text = Text;
        _labelGeometry.TextSize = (float)TextSize;
        _labelGeometry.X = x + _xc;
        _labelGeometry.Y = y + _yc;
        _labelGeometry.RotateTransform = (float)Rotation;
        _labelGeometry.TranslateTransform = Translate;
        _labelGeometry.VerticalAlign = VerticalAlignment;
        _labelGeometry.HorizontalAlign = HorizontalAlignment;
        _labelGeometry.Background = BackgroundColor;
        _labelGeometry.Padding = Padding;
        _labelGeometry.LineHeight = LineHeight;

        var drawing = chart.Canvas.Draw();
        if (Paint is not null) _ = drawing.SelectPaint(Paint).Draw(_labelGeometry);
    }

    /// <inheritdoc cref="VisualElement{TDrawingContext}.Measure(Chart{TDrawingContext}, Scaler, Scaler)"/>
    public override LvcSize Measure(LiveChartsCore.Chart<SkiaDrawingContext> chart, Scaler? primaryScaler, Scaler? secondaryScaler)
    {
        var l = _labelGeometry ?? new LabelGeometry()
        {
            Text = Text,
            TextSize = (float)TextSize,
            RotateTransform = (float)Rotation,
            TranslateTransform = Translate,
            VerticalAlign = VerticalAlignment,
            HorizontalAlign = HorizontalAlignment,
            Background = BackgroundColor,
            Padding = Padding,
        };

        return _actualSize = _paint is null
            ? new LvcSize()
            : l.Measure(_paint);
    }

    /// <inheritdoc cref="VisualElement{TDrawingContext}.GetTargetSize"/>
    public override LvcSize GetTargetSize()
    {
        return _actualSize;
    }

    /// <inheritdoc cref="VisualElement{TDrawingContext}.GetTargetLocation"/>
    public override LvcPoint GetTargetLocation()
    {
        var x = _targetPosition.X;
        var y = _targetPosition.Y;

        x += Translate.X;
        y += Translate.Y;

        var size = GetTargetSize();
        if (HorizontalAlignment == Align.Middle) x -= size.Width * 0.5f;
        if (HorizontalAlignment == Align.End) x -= size.Width;

        if (VerticalAlignment == Align.Middle) y -= size.Height * 0.5f;
        if (VerticalAlignment == Align.End) y -= size.Height;

        return new(x, y);
    }
}
