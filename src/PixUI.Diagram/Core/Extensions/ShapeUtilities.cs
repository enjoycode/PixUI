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

        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;
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

        foreach (var connector in shapes.SelectMany(shape => shape.Connectors.Where(c => c.Name != ConnectorPosition.Auto)))
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
}