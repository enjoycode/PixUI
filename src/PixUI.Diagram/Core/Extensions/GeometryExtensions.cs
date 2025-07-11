namespace PixUI.Diagram;

internal static class GeometryExtensions
{
    public class PathSegmentCollection : List<PathSegment> { }

    private const float Tolerance = 2f;

    internal static PathFigure CreateConnectionCapFigure(Point startPoint, Point endPoint, CapType capType
        , float capWidth, float capHeight, ref Point basePoint)
    {
        PathFigure? connectionCapFigure = null;
        var isFilled = capType.ToString().Contains("Filled");
        switch (capType)
        {
            case CapType.None:
                break;
            case CapType.Arrow1:
            case CapType.Arrow2:
            case CapType.Arrow3:
            case CapType.Arrow1Filled:
            case CapType.Arrow2Filled:
                //// arrows
                var arrowPoints = GetArrowPoints(capType, endPoint, startPoint, capWidth, capHeight);
                basePoint = arrowPoints.Item1;
                connectionCapFigure = CreateArrow(startPoint, arrowPoints.Item2, arrowPoints.Item3, basePoint,
                    capType != CapType.Arrow3, isFilled);
                break;
            case CapType.Arrow4:
            case CapType.Arrow4Filled:
                //var doubleArrowPoints = GetDoubleArrowPoints(endPoint, startPoint, capWidth, capHeight);
                //basePoint = doubleArrowPoints.Item4;
                //connectionCapFigure = CreateDoubleArrow(startPoint, doubleArrowPoints, true, isFilled);
                //break;
                throw new NotImplementedException();
            case CapType.Arrow5:
            case CapType.Arrow5Filled:
                //// diamonds
                //var diamondPoints = GetDiamondPoints(endPoint, startPoint, capWidth, capHeight);
                //basePoint = diamondPoints.Item1;
                //connectionCapFigure = CreateDiamond(startPoint, diamondPoints.Item2, diamondPoints.Item1, diamondPoints.Item3);
                //break;
                throw new NotImplementedException();
            case CapType.Arrow6:
            case CapType.Arrow6Filled:
                //var circlePoints = GetEllipsePoints(startPoint, endPoint, capWidth / 2d, capHeight / 2d);
                //basePoint = circlePoints.Item2;
                //connectionCapFigure = CreateCircle(circlePoints.Item1, circlePoints.Item2, capWidth / 2d, capHeight / 2d);
                //break;
                throw new NotImplementedException();
        }

        if (connectionCapFigure != null)
        {
            connectionCapFigure.IsFilled = isFilled;
        }

        return connectionCapFigure;
    }

    /// <summary>
    /// Creates an arrow based from the specified points.
    /// </summary>
    /// <param name="startPoint">The start point - the tip of the arrow.</param>
    /// <param name="arrowPoint1">The arrow point1 - the left end point from the start-base line.</param>
    /// <param name="arrowPoint2">The arrow point2 - the right end points from the start-base line.</param>
    /// <param name="basePoint">The base point of the arrow - the end of the arrow's head.</param>
    /// <param name="isClosed">If set to <c>true</c> the arrow will be closed.</param>
    /// <param name="isFilled">If set to <c>true</c> the arrow's head will be filled.</param>
    /// <returns>Returns a figure of an arrow.</returns>
    private static PathFigure CreateArrow(Point startPoint, Point arrowPoint1, Point arrowPoint2, Point basePoint,
        bool isClosed, bool isFilled)
    {
        if (isClosed)
        {
            return new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = isClosed,
                IsFilled = isFilled,
                Segments =
                    (new PathSegmentCollection
                    {
                        new LineSegment { Point = arrowPoint1 },
                        new LineSegment { Point = basePoint },
                        new LineSegment { Point = arrowPoint2 },
                    })
            };
        }

        return new PathFigure
        {
            StartPoint = arrowPoint1,
            IsClosed = false,
            IsFilled = false,
            Segments = new PathSegmentCollection
            {
                new LineSegment { Point = startPoint },
                new LineSegment { Point = arrowPoint2 },
            }
        };
    }

    /// <summary>
    /// Creates a line geometry based on the given specifications.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>This method will create the baseline of a connection. This line will include possible bridges but no end caps.</item> 
    /// <item>All  coordinates handled in this method are local positions. </item>
    /// </list>
    /// </remarks>
    /// <param name="specs">The specs.</param>
    /// <returns></returns>
    public static PathGeometry CreateBaseLineGeometry(LineSpecification specs)
    {
        if (specs == null) throw new ArgumentNullException(nameof(specs));
        if (specs.StartPoint == specs.EndPoint && specs.Points == null) return null;

        var geometry = new PathGeometry();

        // if there are caps then the endpoints will be shifted to make space for these caps
        var startBaseLinePoint = specs.StartPoint;
        var endBaseLinePoint = specs.EndPoint;

        // TODO: Check
        if (specs.BridgeType != BridgeType.None && specs.ConnectionType == ConnectionType.Polyline)
        {
            if (specs.Crossings.SegmentCrossings[0].Any())
                specs.Crossings.SegmentCrossings[0].RemoveAt(0);
            specs.Crossings.SegmentCrossings[0].Insert(0, startBaseLinePoint);
            var l = specs.Crossings.SegmentCrossings[specs.Crossings.SegmentCrossings.Count - 1];
            if (l.Any())
                l.Remove(l.Last());
            l.Add(endBaseLinePoint);
        }

        switch (specs.ConnectionType)
        {
            case ConnectionType.Bezier:
            {
                var bezierStartPoint = specs.Points.Count == 2 ? specs.Points[0] : new Point();
                var bezierEndPoint = specs.Points.Count == 2 ? specs.Points[1] : new Point();
                var baseFigure = CreateBezierLineFigure(startBaseLinePoint, endBaseLinePoint, bezierStartPoint,
                    bezierEndPoint, specs.BezierTension);
                baseFigure.IsFilled = false;
                baseFigure.IsClosed = false;
                geometry.Figures.Add(baseFigure);
            }
                break;
            case ConnectionType.Polyline:
            {
                var figs = GetPolylineLineFigures(new PolylineSpecification
                {
                    StartPoint = startBaseLinePoint,
                    EndPoint = endBaseLinePoint,
                    RoundedCorners = specs.RoundedCorners,
                    Points = specs.Points,
                    BridgeType = specs.BridgeType,
                    Crossings = specs.Crossings,
                    Bounds = specs.Bounds
                });
                geometry.Figures.AddRange(figs);
            }
                break;
            case ConnectionType.Spline:
            {
                var points = new List<Point> { startBaseLinePoint };
                points.AddRange(specs.Points);
                points.Add(endBaseLinePoint);
                var figs = CreateSplineFigures(points);
                geometry.Figures.AddRange(figs);
            }
                break;
        }

        return geometry;
    }

    /// <summary>
    /// Creates the bezier line figure.
    /// </summary>
    /// <param name="startPoint">The start point.</param>
    /// <param name="endPoint">The end point.</param>
    /// <param name="startBezierPoint">The start bezier point.</param>
    /// <param name="endBezierPoint">The end bezier point.</param>
    /// <param name="tension">The tension is normally a value between zero and one. A value of one is a normal Bezier segment, a lower value will relax the Bezier and zero leads to a straight line.</param>
    /// <returns></returns>
    private static PathFigure CreateBezierLineFigure(Point startPoint, Point endPoint, Point startBezierPoint,
        Point endBezierPoint, double tension = 1d)
    {
        var bezierFigure = new PathFigure { IsFilled = false, IsClosed = false, StartPoint = startPoint };

        var segment = new BezierSegment
        {
            Point1 = Utils.Lerp(startPoint, startBezierPoint, tension),
            Point2 = Utils.Lerp(endPoint, endBezierPoint, tension),
            Point3 = endPoint
        };

        bezierFigure.Segments.Add(segment);
        bezierFigure.IsFilled = false;
        return bezierFigure;
    }

    /// <summary>
    /// Creates a canonical spline figure.
    /// </summary>
    /// <param name="points">The points defining the spline. These should be in local coordinates and include the endpoints.</param>
    /// <param name="tension">The tension of the spline. A tension or zero will result in a polyline, a tension between 0.5 and 0.8 is pleasing, other values lead to wild curves.</param>
    /// <param name="isClosed">If set to <c>true</c> the curve will be closed.</param>
    /// <param name="isFilled">If set to <c>true</c> the shape will be filled .</param>
    /// <remarks>Based on the code in <c>http://www.charlespetzold.com/blog/2009/01/canonical-splines-in-wpf-and-silverlight.html</c>.</remarks>
    public static IList<PathFigure>? CreateSplineFigures(IList<Point>? points, double tension = 0.5,
        bool isClosed = false, bool isFilled = false)
    {
        if (points == null || points.Count < 1) return null;
        var polyLineSegment = new PolyLineSegment();
        var pathFigure = new PathFigure { IsClosed = isClosed, IsFilled = isFilled, StartPoint = points[0] };
        pathFigure.Segments.Add(polyLineSegment);
        var figures = new List<PathFigure> { pathFigure };
        if (points.Count < 2) return figures;
        if (points.Count == 2)
        {
            if (!isClosed)
                Segment(polyLineSegment.Points, points[0], points[0], points[1], points[1], tension, tension,
                    Tolerance);
            else
            {
                Segment(polyLineSegment.Points, points[1], points[0], points[1], points[0], tension, tension,
                    Tolerance);
                Segment(polyLineSegment.Points, points[0], points[1], points[0], points[1], tension, tension,
                    Tolerance);
            }
        }
        else
        {
            for (var i = 0; i < points.Count; i++)
            {
                if (i == 0)
                    Segment(polyLineSegment.Points, isClosed ? points[points.Count - 1] : points[0], points[0],
                        points[1], points[2], tension, tension, Tolerance);
                else if (i == points.Count - 2)
                    Segment(polyLineSegment.Points, points[i - 1], points[i], points[i + 1],
                        isClosed ? points[0] : points[i + 1], tension, tension, Tolerance);
                else if (i == points.Count - 1)
                {
                    if (isClosed)
                        Segment(polyLineSegment.Points, points[i - 1], points[i], points[0], points[1], tension,
                            tension, Tolerance);
                }
                else
                    Segment(polyLineSegment.Points, points[i - 1], points[i], points[i + 1], points[i + 2], tension,
                        tension, Tolerance);
            }
        }

        return figures;
    }

    /// <summary>
    /// Canonical spline as a collection of points.
    /// </summary>
    private static void Segment(ICollection<Point> points, Point pt0, Point pt1, Point pt2, Point pt3, double t1,
        double t2, double tolerance)
    {
        var sx1 = t1 * (pt2.X - pt0.X);
        var sy1 = t1 * (pt2.Y - pt0.Y);
        var sx2 = t2 * (pt3.X - pt1.X);
        var sy2 = t2 * (pt3.Y - pt1.Y);

        var ax = sx1 + sx2 + (2 * pt1.X) - (2 * pt2.X);
        var ay = sy1 + sy2 + (2 * pt1.Y) - (2 * pt2.Y);
        var bx = -(2 * sx1) - sx2 - (3 * pt1.X) + (3 * pt2.X);
        var by = -(2 * sy1) - sy2 - (3 * pt1.Y) + (3 * pt2.Y);

        var cx = sx1;
        var cy = sy1;
        var dx = pt1.X;
        var dy = pt1.Y;

        var num = (int)((Math.Abs(pt1.X - pt2.X) + Math.Abs(pt1.Y - pt2.Y)) / tolerance);

        // Notice begins at 1 so excludes the first point (which is just pt1)
        for (var i = 1; i < num; i++)
        {
            var t = (double)i / (num - 1);
            var pt = new Point((float)((ax * t * t * t) + (bx * t * t) + (cx * t) + dx),
                (float)((ay * t * t * t) + (by * t * t) + (cy * t) + dy));
            points.Add(pt);
        }
    }

    private static List<PathFigure> GetPolylineLineFigures(PolylineSpecification specs)
    {
        var startPointResult = specs.StartPoint;
        var endPointResult = specs.EndPoint;
        var figures = new List<PathFigure>();
        var figure = new PathFigure { IsClosed = false, IsFilled = false, StartPoint = startPointResult };
        figures.Add(figure);
        if (specs.BridgeType == BridgeType.None)
        {
            if (specs.RoundedCorners)
            {
                var ps = specs.Points!.ToList();
                ps.Insert(0, startPointResult);
                ps.Add(endPointResult);

                /*the last parameter should be true since the method draws the whole line,
                 in the case of crossings below this is not the case since the construction
                 is pieceswise and not solely done by the CreateRoundedPolyline method */
                CreateRoundedPolyline(figure, ps, true);
            }
            else
            {
                foreach (var point in specs.Points!)
                    figure.Segments.Add(new LineSegment { Point = point });
                figure.Segments.Add(new LineSegment { Point = specs.EndPoint });
            }
        }
        else
        {
            if (specs.Crossings == null)
                throw new ArgumentException("Bridges need to be used but no crossings data was supplied.",
                    nameof(specs));
            var crad = DiagramConstants.CrossingRadius;
            var crossings = specs.Crossings;
            var localConnectionPoints = specs.Points.ToList();
            localConnectionPoints.Insert(0, startPointResult);
            localConnectionPoints.Add(endPointResult);
            var segmentCount = specs.Points.Count() + 1;
            for (var segmentIndex = 0; segmentIndex < segmentCount; ++segmentIndex)
            {
                // by design, the number of segment crossing points will always contain the begin and end of the segment
                var segmentCrossings = crossings.SegmentCrossings[segmentIndex];
                for (var segmentPositionIndex = 0; segmentPositionIndex < segmentCrossings.Count - 1; ++segmentPositionIndex)
                {
                    var segmentStartPoint = segmentCrossings[segmentPositionIndex];
                    var segmentEndPoint = segmentCrossings[segmentPositionIndex + 1];

                    // at the even positions we have rect1 normal line joining the points
                    if (segmentPositionIndex % 2 == 0)
                    {
                        // if this is the last one, we need to check whether the end is being rounded,
                        // except for the very last one segment where the rounding is obviously not being applied
                        if (segmentPositionIndex == segmentCrossings.Count - 2 && segmentIndex != segmentCount - 1)
                        {
                            if (specs.RoundedCorners)
                            {
                                // The third point in the poly is
                                // the second point of the next segment
                                // if it does not have crossings, or
                                // the second point of the crossings array
                                var ni = segmentIndex + 2;
                                var next = localConnectionPoints[ni];
                                while (Math.Abs(next.X - segmentEndPoint.X) + Math.Abs(next.Y - segmentEndPoint.Y) <
                                       Utils.Epsilon)
                                {
                                    ni++;
                                    if (ni == localConnectionPoints.Count) break;

                                    next = localConnectionPoints[ni];
                                }

                                if (ni == localConnectionPoints.Count)
                                {
                                    figure.AddLine(segmentStartPoint, segmentEndPoint);
                                }
                                else
                                {
                                    var nextPc = crossings.SegmentCrossings[ni - 1];
                                    if (nextPc.Count > 2) next = nextPc[1];

                                    var triPoints = new[] { segmentStartPoint, segmentEndPoint, next };

                                    // the last parameter should be set to false since this loop will take care of adding the line after the rounding
                                    CreateRoundedPolyline(figure, triPoints, false);
                                }
                            }
                            else
                                figure.Segments.Add(new LineSegment { Point = segmentEndPoint });
                        }
                        else
                        {
                            figure.Segments.Add(new LineSegment { Point = segmentEndPoint });
                        }
                    }
                    else
                    {
                        // the segment points now define an intersection
                        if (specs.BridgeType == BridgeType.Bow)
                        {
                            var radv = segmentEndPoint.Delta(segmentStartPoint) /
                                segmentStartPoint.Distance(segmentEndPoint) * crad * 4d / 3d;
                            var radvPerpLeft = radv.Perpendicular();
                            var radvPerpUpper = radvPerpLeft.Y > 0 ? -radvPerpLeft : radvPerpLeft;

                            figure.Segments.Add(
                                new BezierSegment
                                {
                                    Point1 = segmentStartPoint + (Point)radvPerpUpper,
                                    Point2 = segmentEndPoint + (Point)radvPerpUpper,
                                    Point3 = segmentEndPoint,
                                });
                        }
                        else
                        {
                            // ConnectionBridges.Gap, Start new figure in the graph,
                            figure = new PathFigure
                                { IsClosed = false, IsFilled = false, StartPoint = segmentEndPoint };
                            figures.Add(figure);
                        }
                    }
                }
            }
        }

        return figures;
    }

    /// <summary>
    /// Adds rounded corners to the existing polyline figure.
    /// </summary>
    /// <param name="figure">The figure which will be incremented with corner arcs.</param>
    /// <param name="points">The points of the polyline.</param>
    /// <param name="addLastLine">If set to <c>true</c> the last line segment will be added.</param>
    /// <remarks>
    /// The corner radius is set in the <see cref="DiagramConstants.ConnectionCornerRadius" /> and has default <c>5.0</c> pixels.
    /// </remarks>
    private static void CreateRoundedPolyline(PathFigure figure, IList<Point> points, bool addLastLine)
    {
        if (Math.Abs(DiagramConstants.ConnectionCornerRadius) < Utils.Epsilon) return;
        if (DiagramConstants.ConnectionCornerRadius < 0)
            throw new InvalidOperationException("NegativeCornerRadius");
        var distanceToStartPoint = 0f;
        var distanceToEndPoint = 0f;
        var angleToStartPoint = 0f;
        var angleToEndPoint = 0f;
        var startArcPointAngle = 0f;
        var startArcPointRadius = 0f;
        var endArcPointAngle = 0f;
        var endArcPointRadius = 0f;

        if (points.Count == 2)
        {
            figure.Segments.Add(new LineSegment { Point = points[1] });
            return;
        }

        // loop over the points except the first and last one,
        // considering only the corners
        var radius = DiagramConstants.ConnectionCornerRadius;
        for (var k = 0; k < points.Count - 2; k++)
        {
            var startPoint = points[k];
            var cornerPoint = points[k + 1];
            var endPoint = points[k + 2];
            Utils.CartesianToPolar(cornerPoint, startPoint, ref angleToStartPoint, ref distanceToStartPoint);
            Utils.CartesianToPolar(cornerPoint, endPoint, ref angleToEndPoint, ref distanceToEndPoint);
            Utils.NormalizeAngle(angleToStartPoint);
            Utils.NormalizeAngle(angleToEndPoint);

            // if too close it doesn't make sense to add an arc
            if (distanceToEndPoint < Utils.Epsilon) continue;

            // order the two angles such that we can decide upon the angle between the start and end with the (eventual) corner in between
            if (angleToEndPoint < angleToStartPoint) angleToEndPoint += 360;

            var cornerAngle = angleToEndPoint - angleToStartPoint;

            // if the corner is zero or 180 degress it doesn't make sense to add an arc
            if (Math.Abs(cornerAngle - 0) < Utils.Epsilon || Math.Abs(cornerAngle - 180) < Utils.Epsilon) continue;

            var cornerAngleIsBiggerThan180Degrees = false;
            if (cornerAngle > 180)
            {
                cornerAngleIsBiggerThan180Degrees = true;
                Utils.Swap(ref angleToStartPoint, ref angleToEndPoint);
                Utils.Swap(ref distanceToStartPoint, ref distanceToEndPoint);
                cornerAngle = 360 - cornerAngle;
            }

            // half the angle; the bisec
            var bisec = (cornerAngle / 2f).ToRadians();
            var minOfEndPoints = Math.Min(distanceToStartPoint, distanceToEndPoint);
            var sinBisec = Math.Sin(bisec);
            var cosBisec = Math.Cos(bisec);
            var computedRadius = radius / Math.Tan(bisec);
            computedRadius = Math.Min(computedRadius, minOfEndPoints / 2f);
            radius = (float)((sinBisec * computedRadius) / cosBisec);

            var startArcPoint = Utils.PolarToCartesian(cornerPoint, angleToStartPoint, (float)computedRadius);
            var endArcPoint = Utils.PolarToCartesian(cornerPoint, angleToEndPoint, (float)computedRadius);
            var radiiDistance = Utils.Distance((float)computedRadius, radius);
            var center = Utils.PolarToCartesian(cornerPoint, angleToStartPoint + (cornerAngle / 2), radiiDistance);

            // time to fetch the actual coordinates of the arc points
            Utils.CartesianToPolar(center, startArcPoint, ref startArcPointAngle, ref startArcPointRadius);
            Utils.CartesianToPolar(center, endArcPoint, ref endArcPointAngle, ref endArcPointRadius);

            // ensure angles are in the first quadrant
            while (Math.Abs(endArcPointAngle - startArcPointAngle) > 180)
            {
                if (endArcPointAngle < startArcPointAngle) endArcPointAngle += 360;
                else startArcPointAngle += 360;
            }

            if (endArcPointAngle < startArcPointAngle) Utils.Swap(ref endArcPointAngle, ref startArcPointAngle);

            // if the points were swapped, swap them back to make sure the figure sequence is correct
            if (cornerAngleIsBiggerThan180Degrees) Utils.Swap(ref startArcPoint, ref endArcPoint);

            figure.Segments.Add(new LineSegment { Point = startArcPoint });
            var startArcAngle = 360 - startArcPointAngle;
            var sweepAngle = -(endArcPointAngle - startArcPointAngle);

            // ...and due to the Arc logic we need...
            if (!cornerAngleIsBiggerThan180Degrees)
            {
                startArcAngle = startArcAngle + sweepAngle;
                sweepAngle = -sweepAngle;
            }

            // ...to finally add an arc to the figure and move on to the next corner, if any
            //figure.AddArc(Rect.FromLTWH(center.X - radius, center.Y - radius, radius * 2, radius * 2), startArcAngle, sweepAngle, false);
        }

        if (addLastLine)
            figure.Segments.Add(new LineSegment { Point = points[points.Count - 1] });
    }

    /// <summary>
    /// Gets the points of an arrow.
    /// </summary>
    /// <param name="type">The type of arrow.</param>
    /// <param name="startPoint">The start point - the tip of the arrow.</param>
    /// <param name="endPoint">The end of the line of the arrow.</param>
    /// <param name="arrowWidth">The width of the arrow.</param>
    /// <param name="arrowHeight">The height of the arrow.</param>
    /// <returns>
    /// Returns the points of an arrow in the order:
    /// base arrow point, left arrow head point, right arrow head point.
    /// </returns>
    private static Tuple<Point, Point, Point> GetArrowPoints(CapType type, Point startPoint, Point endPoint
        , float arrowWidth, float arrowHeight)
    {
        arrowHeight = arrowHeight / 2f;
        var theta = Math.Atan2(startPoint.Y - endPoint.Y, startPoint.X - endPoint.X);
        var sint = Math.Round(Math.Sin(theta), 2);
        var cost = Math.Round(Math.Cos(theta), 2);

        // ge the side points of the arrow's head:
        var leftPoint = new Point((float)(endPoint.X + ((arrowWidth * cost) - (arrowHeight * sint)))
            , (float)(endPoint.Y + ((arrowWidth * sint) + (arrowHeight * cost))));
        var rightPoint = new Point((float)(endPoint.X + ((arrowWidth * cost) + (arrowHeight * sint)))
            , (float)(endPoint.Y - ((arrowHeight * cost) - (arrowWidth * sint))));
        var arrowBase = Utils.MiddlePoint(leftPoint, rightPoint);

        switch (type)
        {
            case CapType.Arrow2:
            case CapType.Arrow2Filled:
                var deltaX = endPoint.X - arrowBase.X;
                var deltaY = endPoint.Y - arrowBase.Y;
                //// the arrow base point is opposite of the tip of the arrow and the incoming line stops in it:
                arrowBase = new Point(arrowBase.X + (deltaX * 0.2f), arrowBase.Y + (deltaY * 0.2f));
                break;
            case CapType.Arrow3:
                //// arrow3 is not closed, so the base of the arrow is the tip of the arrow:
                arrowBase = endPoint;
                break;
        }

        return new Tuple<Point, Point, Point>(arrowBase, leftPoint, rightPoint);
    }


    /// <summary>
    /// Gets the tangents of the spline figure.
    /// </summary>
    /// <param name="points">The points of the connection.</param>
    /// <param name="startTangentPoint">The start tangent point.</param>
    /// <param name="endTangentPoint">The end tangent point.</param>
    /// <param name="tension">The tension.</param>
    /// <param name="isClosed">If set to <c>true</c> [is closed].</param>
    public static void GetSplineFigureTangents(IList<Point> points,
        out Point startTangentPoint, out Point endTangentPoint, float tension = 0.5f, bool isClosed = false)
    {
        startTangentPoint = new Point();
        endTangentPoint = new Point();
        if (points.Count < 2) return;
        if (points.Count == 2)
        {
            startTangentPoint = points[1];
            endTangentPoint = points[0];
        }
        else
        {
            var i1 = points.Count - 1;
            var i2 = points.Count - 2;
            startTangentPoint = GetSplineFigureTangentPoint(ManipulationPointType.First
                , isClosed ? points[i1] : points[0]
                , points[0], points[1], points[2], tension);
            endTangentPoint = GetSplineFigureTangentPoint(ManipulationPointType.Last
                , points[i2 - 1], points[i2], points[i2 + 1]
                , isClosed ? points[0] : points[i2 + 1], tension);
        }
    }

    private static Point GetSplineFigureTangentPoint(ManipulationPointType type
        , Point pt0, Point pt1, Point pt2, Point pt3, float tension = 0.5f)
    {
        var sx1 = tension * (pt2.X - pt0.X);
        var sy1 = tension * (pt2.Y - pt0.Y);
        var sx2 = tension * (pt3.X - pt1.X);
        var sy2 = tension * (pt3.Y - pt1.Y);

        var ax = sx1 + sx2 + (2 * pt1.X) - (2 * pt2.X);
        var ay = sy1 + sy2 + (2 * pt1.Y) - (2 * pt2.Y);
        var bx = -(2 * sx1) - sx2 - (3 * pt1.X) + (3 * pt2.X);
        var by = -(2 * sy1) - sy2 - (3 * pt1.Y) + (3 * pt2.Y);

        var cx = sx1;
        var cy = sy1;
        var dx = pt1.X;
        var dy = pt1.Y;
        var num = (int)((Math.Abs(pt1.X - pt2.X) + Math.Abs(pt1.Y - pt2.Y)) / Tolerance);
        var i = type == ManipulationPointType.First ? 12 : num - 12;
        var t = (double)i / (num - 1);
        return new Point((float)((ax * t * t * t) + (bx * t * t) + (cx * t) + dx)
            , (float)((ay * t * t * t) + (by * t * t) + (cy * t) + dy));
    }

    public static PathFigure AddLine(this PathFigure pathFigure, Point point1, Point point2)
    {
        var seg = new LineSegment { Point = point2 };
        return Add(pathFigure, seg, point1);
    }

    /// <summary>
    /// Adds the given <see cref="PathSegment"/> to the path figure and sets the starting point at the same time.
    /// </summary>
    public static PathFigure Add(this PathFigure pathFigure, PathSegment segment, Point startPoint)
    {
        pathFigure.StartPoint = startPoint;
        return pathFigure.Add(segment);
    }

    /// <summary>
    /// Adds the given <see cref="PathSegment"/> to the path figure.
    /// </summary>
    public static PathFigure Add(this PathFigure pathFigure, PathSegment segment)
    {
        pathFigure.Segments.Add(segment);
        return pathFigure;
    }
}