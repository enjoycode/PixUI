using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LiveChartsCore.Defaults;

namespace LiveCharts;

/// <summary>
/// The gauge generator class.
/// </summary>
public static class GaugeGenerator
{
    /// <summary>
    /// Builds a solid Gauge, it generates a series collectio of
    /// <see cref="PieSeries{ObservableValue, DoughnutGeometry, LabelGeometry}"/>, these series
    /// are ready to be plotted in a pie chart, and will render the gauge, this reuses all the power and
    /// functionality of the <see cref="PieChart{TDrawingContext}"/> class.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <returns>A series collection of pie seires.</returns>
    public static ObservableCollection<PieSeries<ObservableValue>> BuildSolidGauge(
        params GaugeItem[] items)
    {
        if (!items.Any(x => x.IsFillSeriesBuilder))
            items = items.Concat(new[] { new GaugeItem(GaugeItem.Background) }).ToArray();

        return Build(GaugeOptions.Solid, items);
    }

    /// <summary>
    /// Builds an angular Gauge, it generates a series collectio of
    /// <see cref="PieSeries{ObservableValue, DoughnutGeometry, LabelGeometry}"/>, these series
    /// are ready to be plotted in a pie chart, and will render the gauge, this reuses all the power and
    /// functionality of the <see cref="PieChart{TDrawingContext}"/> class.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <returns>A series collection of pie seires.</returns>
    public static ObservableCollection<PieSeries<ObservableValue>> BuildAngularGaugeSections(
        params GaugeItem[] items)
    {
        if (!items.Any(x => x.IsFillSeriesBuilder))
            items = items.Concat(new[] { new GaugeItem(GaugeItem.Background) }).ToArray();

        return Build(GaugeOptions.Angular, items);
    }

    private static ObservableCollection<PieSeries<ObservableValue>> Build(
        GaugeOptions options, params GaugeItem[] items)
    {
        List<GaugeItem> seriesRules = new();
        List<GaugeItem> backgroundRules = new();

        foreach (var item in items)
        {
            if (item.IsFillSeriesBuilder)
                backgroundRules.Add(item);
            else
                seriesRules.Add(item);
        }

        var count = seriesRules.Count;
        var i = 0;

        var series = seriesRules.Select(item =>
        {
            Action<ObservableValue, PieSeries<ObservableValue>>? l =
                item.Builder is null ? null : (m, s) => { item.Builder?.Invoke(s); };

            return PieChartExtensions.AsSeries(
                item.Value,
                l ?? ((x, x1) => { }),
                i++,
                count,
                options);
        });

        var fillSeriesValues = new List<ObservableValue>();
        while (fillSeriesValues.Count < items.Length - 1) fillSeriesValues.Add(new ObservableValue(0));

        var backgroundSeries = new PieSeries<ObservableValue>(true, true)
        {
            ZIndex = -1,
            IsFillSeries = true,
            IsVisibleAtLegend = false,
            Values = fillSeriesValues
        };

        if (options == GaugeOptions.Angular)
        {
            backgroundSeries.HoverPushout = 0;
            backgroundSeries.IsHoverable = false;
            backgroundSeries.HoverPushout = 0;
            backgroundSeries.DataLabelsPaint = null;
        }

        foreach (var rule in backgroundRules) rule.Builder?.Invoke(backgroundSeries);

        return new ObservableCollection<PieSeries<ObservableValue>>(series.Concat(new[] { backgroundSeries }));
    }
}