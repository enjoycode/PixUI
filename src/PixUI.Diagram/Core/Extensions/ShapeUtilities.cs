namespace PixUI.Diagram;

internal static class ShapeUtilities
{
    /// <summary>
    /// Gets the connectors' enclosing bounds.
    /// </summary>
    /// <param name="shape">The shape.</param>
    /// <returns>The enclosing bounds.</returns>
    internal static Rect GetConnectorsBounds(IShape? shape)
    {
        if (shape == null || shape.Connectors.Count == 0) return Rect.Empty;

        var minX = float.MaxValue;
        var minY = float.MaxValue;
        var maxX = float.MinValue;
        var maxY = float.MinValue;
        foreach (var connector in shape.Connectors)
        {
            var position = connector.AbsolutePosition;
            minX = Math.Min(minX, position.X);
            minY = Math.Min(minY, position.Y);

            maxX = Math.Max(maxX, position.X);
            maxY = Math.Max(maxY, position.Y);
        }

        return Rect.FromLTWH(minX, minY, maxX, maxY);
    }

    /// <summary>
    /// Gets the closest connector position.
    /// </summary>
    /// <param name="shape">The shape.</param>
    /// <param name="point">The point.</param>
    /// <returns></returns>
    internal static IConnector? GetNearestConnector(IShape? shape, Point point)
    {
        return shape != null ? GetNearestConnector([shape], point, int.MaxValue) : null;
    }

    /// <summary>
    /// Gets the nearest connector.
    /// </summary>
    /// <param name="shapes">The shapes.</param>
    /// <param name="point">The point.</param>
    /// <param name="delta">The delta.</param>
    /// <returns></returns>
    internal static IConnector? GetNearestConnector(IEnumerable<IShape> shapes, Point point, double delta)
    {
        IConnector? resolvedConnector = null;
        var minDistance = double.MaxValue;

        foreach (var connector in shapes.SelectMany(shape =>
                     shape.Connectors.Where(c => c.Name != ConnectorPosition.Auto)))
        {
            var currentDistance = connector.AbsolutePosition.Distance(point);
            if (currentDistance < minDistance && currentDistance < delta)
            {
                minDistance = currentDistance;
                resolvedConnector = connector;
            }
        }

        return resolvedConnector;
    }

    /// <summary>
    /// Gets the nearest connectors for a connection.
    /// </summary>
    public static void GetNearestConnectors(IConnection connection, out IConnector? startConnector,
        out IConnector? endConnector)
    {
        startConnector = null;
        endConnector = null;
        if ("Auto".Equals(connection.SourceConnectorPosition) && "Auto".Equals(connection.TargetConnectorPosition) &&
            connection.Source != null && connection.Target != null)
        {
            var minDistance = double.MaxValue;

            foreach (var sourceConnector in connection.Source.Connectors)
            {
                if ("Auto".Equals(sourceConnector.Name)) continue;

                foreach (var targetConnector in connection.Target.Connectors)
                {
                    if ("Auto".Equals(targetConnector.Name)) continue;

                    var currentDistance = sourceConnector.AbsolutePosition.Distance(targetConnector.AbsolutePosition);
                    if (currentDistance < minDistance)
                    {
                        minDistance = currentDistance;
                        startConnector = sourceConnector;
                        endConnector = targetConnector;
                    }
                    else if (Math.Abs(currentDistance - minDistance) < FloatUtils.NearlyZero)
                    {
                        //// This is a corner case, and we should consider the possible approaches because in this scenario
                        //// the order in which you go through the connectors have impact on the end result and this is not a good practice.

                        //// This is not the best approach, but this way we give priority for the Bottom connectors,
                        //// and we fix the case in which connections with the reversed Source and Target are attached to different connectors.
                        if ("Bottom".Equals(sourceConnector.Name) || "Bottom".Equals(targetConnector.Name))
                        {
                            minDistance = currentDistance;
                            startConnector = sourceConnector;
                            endConnector = targetConnector;
                        }
                    }
                }
            }
        }
        else
        {
            if ("Auto".Equals(connection.SourceConnectorPosition) && connection.Source != null)
                startConnector = GetNearestConnector(connection.Source, connection.EndPoint);

            if ("Auto".Equals(connection.TargetConnectorPosition) && connection.Target != null)
                endConnector = GetNearestConnector(connection.Target, connection.StartPoint);
        }
    }
}