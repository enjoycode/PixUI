using System;
using LiveChartsCore.Defaults;

namespace LiveCharts;

/// <summary>
/// Defines a gauge item.
/// </summary>
public class GaugeItem : BaseGaugeItem<PieSeries<ObservableValue>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GaugeItem"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="builder">The builder.</param>
    public GaugeItem(
        double value, Action<PieSeries<ObservableValue>>? builder = null)
        : base(new ObservableValue(value), builder) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GaugeItem"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="builder">The builder.</param>
    public GaugeItem(
        ObservableValue value,
        Action<PieSeries<ObservableValue>>? builder = null)
        : base(value, builder) { }
}