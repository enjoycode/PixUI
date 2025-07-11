namespace PixUI.Diagram;

/// <summary>
/// Settings for Directional Router.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
public class DirectionalRoutingSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DirectionalRoutingSettings"/> class.
    /// </summary>
    public DirectionalRoutingSettings(string source, string target, short horizontalIndicator, short directionParameter)
    {
        SourceConnectorName = source;
        TargetConnectorName = target;
        HorizontalIndicator = horizontalIndicator;
        DirectionParameter = directionParameter;
    }

    /// <summary>
    /// Gets or sets the name of the source connector.
    /// </summary>
    public string SourceConnectorName { get; set; }

    /// <summary>
    /// Gets or sets the name of the target connector.
    /// </summary>
    public string TargetConnectorName { get; set; }

    /// <summary>
    /// Gets or sets the horizontal indicator.
    /// </summary>
    public short HorizontalIndicator { get; private set; }

    /// <summary>
    /// Gets or sets the direction parameter.
    /// </summary>
    public short DirectionParameter { get; private set; }
}