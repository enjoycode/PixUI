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

using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Geometries;
using LiveChartsCore;

namespace LiveCharts;

/// <summary>
/// Defines a stacked column series in the user interface.
/// </summary>
/// <typeparam name="TModel">
/// The type of the points, you can use any type, the library already knows how to handle the most common numeric types,
/// to use a custom type, you must register the type globally 
/// (<see cref="LiveChartsSettings.HasMap{TModel}(System.Action{TModel, ChartPoint})"/>)
/// or at the series level 
/// (<see cref="Series{TModel,TVisual,TLabel,TDrawingContext}.Mapping"/>).
/// </typeparam>
public class StackedColumnSeries<TModel> : StackedColumnSeries<TModel, RoundedRectangleGeometry, LabelGeometry> { }

/// <summary>
/// Defines a stacked column series in the user interface.
/// </summary>
/// <typeparam name="TModel">
/// The type of the points, you can use any type, the library already knows how to handle the most common numeric types,
/// to use a custom type, you must register the type globally 
/// (<see cref="LiveChartsSettings.HasMap{TModel}(System.Action{TModel, ChartPoint})"/>)
/// or at the series level 
/// (<see cref="Series{TModel, TVisual, TLabel, TDrawingContext}.Mapping"/>).
/// </typeparam>
/// <typeparam name="TVisual">
/// The type of the geometry of every point of the series.
/// </typeparam>
public class StackedColumnSeries<TModel, TVisual> : StackedColumnSeries<TModel, TVisual, LabelGeometry>
    where TVisual : class, ISizedGeometry<SkiaDrawingContext>, new() { }

/// <summary>
/// Defines a stacked column series in the user interface.
/// </summary>
/// <typeparam name="TModel">
/// The type of the points, you can use any type, the library already knows how to handle the most common numeric types,
/// to use a custom type, you must register the type globally 
/// (<see cref="LiveChartsSettings.HasMap{TModel}(System.Action{TModel, ChartPoint})"/>)
/// or at the series level 
/// (<see cref="Series{TModel, TVisual, TLabel, TDrawingContext}.Mapping"/>).
/// </typeparam>
/// <typeparam name="TVisual">
/// The type of the geometry of every point of the series.
/// </typeparam>
/// <typeparam name="TLabel">
/// The type of the data label of every point.
/// </typeparam>
public class StackedColumnSeries<TModel, TVisual, TLabel>
    : StackedColumnSeries<TModel, TVisual, TLabel, SkiaDrawingContext, LineGeometry>
    where TVisual : class, ISizedGeometry<SkiaDrawingContext>, new()
    where TLabel : class, ILabelGeometry<SkiaDrawingContext>, new() { }