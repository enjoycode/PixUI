namespace PixUI.Diagram;

internal static class ConnectionUtilities
{
    /// <summary>
    /// Gets the connection end points.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <param name="useConnectionCoordinates">If set to <c>true</c> the points will be in global coordinates. 
    /// If set to false, then the coordinates will be relative to the connection.</param>
    /// <returns></returns>
    internal static Tuple<Point, Point> GetConnectionEndPoints(this IConnection connection,
        bool useConnectionCoordinates = false)
    {
        var startPoint = connection.StartPoint;
        var endPoint = connection.EndPoint;

        if (useConnectionCoordinates)
        {
            startPoint = startPoint.Substract(connection.Bounds.TopLeft());
            endPoint = endPoint.Substract(connection.Bounds.TopLeft());
        }

        return new Tuple<Point, Point>(startPoint, endPoint);
    }

    /// <summary>
    /// Returns all the points of this connection, i.e., the start and end points together with the intermediate connection points.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <returns></returns>
    internal static IList<Point> AllPoints(this IConnection connection)
    {
        var points = connection.ConnectionPoints.Clone();
        points.Insert(0, connection.StartPoint);
        points.Add(connection.EndPoint);
        return points;
    }

    /// <summary>
    /// Calculates the middle point of line.
    /// </summary>
    /// <param name="connectionEnds">The connection end points.</param>
    /// <param name="connectionPoints">The points of interest (points where the line is curved/segmented).</param>
    /// <returns></returns>
    internal static Point CalculateMiddlePointOfLine(Tuple<Point, Point> connectionEnds,
        IList<Point> connectionPoints)
    {
        var points = new List<Point> { connectionEnds.Item1 };
        points.AddRange(connectionPoints);

        points.Add(connectionEnds.Item2);

        var connectionLength = 0d;
        for (var i = 0; i < points.Count; i++)
        {
            var currentPoint = points[i];
            var nextPoint = points[i + (i + 1 < points.Count ? 1 : 0)];
            connectionLength += currentPoint.Distance(nextPoint);
        }

        var currentLength = 0d;

        for (var i = 0; i < points.Count; i++)
        {
            var currentPoint = points[i];
            var nextPoint = points[i + (i + 1 < points.Count ? 1 : 0)];
            var distance = currentPoint.Distance(nextPoint);
            currentLength += distance;
            if (currentLength >= connectionLength / 2)
            {
                var l = currentLength - (connectionLength / 2);
                var a = l * Math.Abs(currentPoint.X - nextPoint.X) / distance;
                var b = l * Math.Abs(currentPoint.Y - nextPoint.Y) / distance;

                // adjustment
                a = nextPoint.X - currentPoint.X > 0 ? -a : a;
                b = nextPoint.Y - currentPoint.Y > 0 ? -b : b;

                var newEdittinPoint = new Point((float)(nextPoint.X + a), (float)(nextPoint.Y + b));
                if (!double.IsNaN(newEdittinPoint.X) && !double.IsNaN(newEdittinPoint.Y)) return newEdittinPoint;
            }
        }

        return new Point();
    }

    /// <summary>
    /// Shifts the <see cref="IConnection.ConnectionPoints"/> by adding or subtracting the <see cref="IDiagramItem.Position"/> vector.
    /// </summary>
    /// <remarks>The start/end points are not taken into account here.</remarks>
    /// <param name="connection">The connection to shift.</param>
    /// <param name="globalCoordinates">If set to <c>true</c> points will be transformed to global coordinates. If set to false, then the coordinates will be relative to the connection.</param>
    internal static IEnumerable<Point> TranslateConnectionPoints(this IConnection connection,
        bool globalCoordinates = true)
    {
        var transformedPoints = new List<Point>();

        for (var i = 0; i < connection.ConnectionPoints.Count; i++)
        {
            transformedPoints.Add(globalCoordinates
                ? connection.ConnectionPoints[i].Add(connection.Position)
                : connection.ConnectionPoints[i].Substract(connection.Position));
        }

        return transformedPoints;
    }

    //TODO:改为传入缓存的surface.Shapes，包括转换至画布坐标系的Bounds
    internal static void ActivateNearestConnector(DiagramSurface surface, IConnection connection,
        bool isStartPoint, Point point)
    {
        var shapes = surface.GetShapes();
        if (shapes.Count == 0)
            return;

        var nearestShapes = new List<IShape>();
        for (int i = 0; i < shapes.Count; i++)
        {
            var bounds = ShapeUtilities.GetConnectorsBounds(shapes[i]); //todo:转换成画布坐标系
            bounds.Inflate(DiagramConstants.ConnectorActivationRadius, DiagramConstants.ConnectorActivationRadius);
            if (bounds.Contains(point) && shapes[i].CanConnect(isStartPoint, connection))
                nearestShapes.Add(shapes[i]);
        }

        if (nearestShapes.Count > 0)
        {
            //todo:验证 IsTargetShapeValid(activeConnector, topShape, connectionType)

            //先查找最接近的
            for (int i = nearestShapes.Count - 1; i >= 0; i--)
            {
                var shape = nearestShapes[i];
                for (int j = 0; j < shape.Connectors.Count; j++)
                {
                    var distance = shape.Connectors[j].AbsolutePosition.Distance(point);
                    if (distance < DiagramConstants.ConnectorActivationRadius)
                    {
                        //shape.IsConnectorsAdornerVisible = true;
                        if (distance < DiagramConstants.ConnectorHitTestRadius)
                        {
                            surface.Adorners.ActiveConnector =
                                shape.Connectors[j]; //shape.Connectors[j].IsActive = true;
                            return;
                        }
                    }
                }
            }

            //再查找默认的
            var topShape = nearestShapes[nearestShapes.Count - 1];
            if (topShape.Connectors.Contains(ConnectorPosition.Gliding))
                surface.Adorners.ActiveConnector =
                    topShape.Connectors
                        [ConnectorPosition.Gliding]; //topShape.Connectors[ConnectorPosition.Gliding].IsActive = true;
            else if (topShape.Connectors.Contains(ConnectorPosition.Auto))
                surface.Adorners.ActiveConnector =
                    topShape.Connectors
                        [ConnectorPosition.Auto]; //topShape.Connectors[ConnectorPosition.Auto].IsActive = true;
        }

        //开始更新NearestShapes
        surface.Adorners.UpdateNearestShapes(nearestShapes);
    }
}