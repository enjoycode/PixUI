namespace PixUI.Diagram;

/// <summary>
/// Predefined positions of a connector in a shape.
/// </summary>
/// <seealso cref="IConnector"/>
internal static class ConnectorPosition
{
    /// <summary>
    /// The connection's connector is calculated.
    /// </summary>
    public const string Auto = "Auto";

    /// <summary>
    /// The connection is bound to the left of the shape.
    /// </summary>
    public const string Left = "Left";

    /// <summary>
    /// The connection is bound to the top of the shape.
    /// </summary>
    public const string Top = "Top";

    /// <summary>
    /// The connection is bound to the right of the shape.
    /// </summary>
    public const string Right = "Right";

    /// <summary>
    /// The connection glides along the edge of the shape.
    /// </summary>
    /// <remarks>This connector does not have a relative position on the shape or absolute position with respect to the surface.</remarks>
    public const string Gliding = "Gliding";

    /// <summary>
    /// The connection is bound to the bottom of the shape.
    /// </summary>
    public const string Bottom = "Bottom";

    private static readonly Dictionary<string, Point> knownConnectors;

    static ConnectorPosition()
    {
        knownConnectors = new Dictionary<string, Point>
        {
            {
                Auto, new Point(.5f, .5f)
            },
            {
                Left, new Point(0, .5f)
            },
            {
                Right, new Point(1f, .5f)
            },
            {
                Top, new Point(.5f, 0)
            },
            {
                Bottom, new Point(.5f, 1)
            },
            {
                Gliding, new Point(.5f, .5f)
            }
        };
    }

    /// <summary>
    /// Determines whether the specified connector is custom.
    /// </summary>
    /// <param name="connector">The connector.</param>
    /// <returns>
    ///   <c>true</c> if the specified connector is custom; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsCustom(this IConnector connector)
    {
        return knownConnectors.All(c => connector.Name != c.Key);
    }

    /// <summary>
    /// Gets the known offset.
    /// </summary>
    /// <param name="name">The name of a connector.</param>
    /// <remarks>Note that the gliding connector return <c>double.NaN</c> since it's not located anywhere on the shape but rather spread among the edge of it.</remarks>
    /// <returns>The offset with respect to the shape. If the connector is not a known connector an exception will be thrown.</returns>
    public static Point GetKnownOffset(string name)
    {
        if (IsKnown(name))
            return knownConnectors.First(c => c.Key == name).Value;
        throw new ArgumentException("UnknownConnector", nameof(name));
    }

    /// <summary>
    /// Returns whether the given connector name is one of the predefined connector positions.
    /// </summary>
    /// <param name="name">The name of a connector.</param>
    /// <returns></returns>
    public static bool IsKnown(string name)
    {
        return knownConnectors.Any(c => c.Key == name);
    }
}