using System;
using LiveCharts.Drawing;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;

namespace LiveCharts;

public class BaseGaugeItem<TSeries>
    where TSeries : IPieSeries<SkiaDrawingContext>, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GaugeItem"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="builder">The builder.</param>
    public BaseGaugeItem(
        ObservableValue value, Action<TSeries>? builder = null)
    {
        Value = value;
        Builder = builder;
        if (value.Value == Background) IsFillSeriesBuilder = true;
    }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public ObservableValue Value { get; set; }

    /// <summary>
    /// Gets or sets the series builder.
    /// </summary>
    public Action<TSeries>? Builder { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance is specific to the fill series.
    /// </summary>
    public bool IsFillSeriesBuilder { get; internal set; }

    /// <summary>
    /// Gets a constant value that represents the background series.
    /// </summary>
    public static double Background { get; } = double.MaxValue;
}