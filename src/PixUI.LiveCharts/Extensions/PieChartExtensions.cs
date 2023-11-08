using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LiveCharts.Drawing;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;

namespace LiveCharts;

/// <summary>
/// Defines the pie chart esxtensions.
/// </summary>
public static class PieChartExtensions
{
    /// <summary>
    /// Converts an IEnumerable to an ObservableCollection of pie series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="source">The data source.</param>
    /// <param name="builder">An optional builder.</param>
    /// <param name="gaugeOptions">Defines whether the series are treaded as gauge.</param>
    /// <returns></returns>
    public static ObservableCollection<PieSeries<TModel>> AsPieSeries<TModel>(
        this IEnumerable<TModel> source,
        Action<TModel, PieSeries<TModel>>? builder = null,
        GaugeOptions gaugeOptions = GaugeOptions.None)
    {
        return AsPieSeries<TModel, PieSeries<TModel>>(source, builder, gaugeOptions);
    }

    /// <summary>
    /// Converts an IEnumerable to an ObservableCollection of pie series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TVisual">The type of the visual.</typeparam>
    /// <param name="source">The data source.</param>
    /// <param name="builder">An optional builder.</param>
    /// <param name="gaugeOptions">Defines whether the series are treaded as gauge.</param>
    /// <returns></returns>
    public static ObservableCollection<PieSeries<TModel, TVisual>> AsPieSeries<TModel, TVisual>(
        this IEnumerable<TModel> source,
        Action<TModel, PieSeries<TModel, TVisual>>? builder = null,
        GaugeOptions gaugeOptions = GaugeOptions.None)
        where TVisual : class, IDoughnutGeometry<SkiaDrawingContext>, new()
    {
        return AsPieSeries<TModel, PieSeries<TModel, TVisual>>(source, builder, gaugeOptions);
    }

    /// <summary>
    /// Converts an IEnumerable to an ObservableCollection of pie series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TVisual">The type of the visual.</typeparam>
    /// <typeparam name="TLabel">The type of the label.</typeparam>
    /// <param name="source">The data source.</param>
    /// <param name="builder">An optional builder.</param>
    /// <param name="gaugeOptions">Defines whether the series are treaded as gauge.</param>
    /// <returns></returns>
    public static ObservableCollection<PieSeries<TModel, TVisual, TLabel>> AsPieSeries<TModel, TVisual, TLabel>(
        this IEnumerable<TModel> source,
        Action<TModel, PieSeries<TModel, TVisual, TLabel>>? builder = null,
        GaugeOptions gaugeOptions = GaugeOptions.None)
        where TVisual : class, IDoughnutGeometry<SkiaDrawingContext>, new()
        where TLabel : class, ILabelGeometry<SkiaDrawingContext>, new()
    {
        return AsPieSeries<TModel, PieSeries<TModel, TVisual, TLabel>>(source, builder, gaugeOptions);
    }

    /// <summary>
    /// Converts an IEnumerable to an ObservableCollection of pie series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    /// <param name="source">The data source.</param>
    /// <param name="builder">An optional builder.</param>
    /// <param name="gaugeOptions">Defines whether the series are treaded as gauge.</param>
    /// <returns></returns>
    public static ObservableCollection<TSeries> AsPieSeries<TModel, TSeries>(
        this IEnumerable<TModel> source,
        Action<TModel, TSeries>? builder = null,
        GaugeOptions gaugeOptions = GaugeOptions.None)
        where TSeries : IPieSeries<SkiaDrawingContext>, new()
    {
        var count = source.Count();
        builder ??= (instance, series) => { };
        var i = 0;

        return new ObservableCollection<TSeries>(
            source.Select(instance => AsSeries(instance, builder, i++, count, gaugeOptions)));
    }

    internal static TSeries AsSeries<TModel, TSeries>(
        TModel instance,
        Action<TModel, TSeries> builder,
        int i,
        int count,
        GaugeOptions gaugeOptions)
        where TSeries : IPieSeries<SkiaDrawingContext>, new()
    {
        var isGauge = gaugeOptions > 0;
        var series = new TSeries();

        if (isGauge)
        {
            var baseSeries = (IInternalSeries)series;
            baseSeries.SeriesProperties |= SeriesProperties.Gauge;
        }

        ObservableCollection<TModel> values;
        if (gaugeOptions == GaugeOptions.Solid)
        {
            values = new ObservableCollection<TModel>();
            while (values.Count < count - 1)
                values.Add(default!);
            values.Insert(i, instance);
        }
        else
        {
            values = new ObservableCollection<TModel> { instance };
            if (gaugeOptions == GaugeOptions.Angular)
            {
                series.HoverPushout = 0;
                series.IsHoverable = false;
                series.HoverPushout = 0;
                series.DataLabelsPaint = null;
                series.AnimationsSpeed = TimeSpan.FromSeconds(0);
                series.IsRelativeToMinValue = true;
            }
        }

        series.Values = values;

        if (isGauge) series.HoverPushout = 0;
        builder(instance, series);

        return series;
    }
}