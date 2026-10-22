using System.IO;
using LiveChartsCore.Drawing;
using LiveChartsCore.Geo;
using PixUI.LiveCharts;
using PixUI.LiveCharts.Painting;

public class DemoGeoMap : View
{
    public DemoGeoMap()
    {
        var demoAssembly = System.Reflection.Assembly.Load("PixUI.Demo");
        var geoJson = demoAssembly.GetManifestResourceStream("PixUI.Demo.Resources.China.json");
        //var gcp1 = new LvcPointD(116.3683244, 39.915085);
        //var gcp2 = new LvcPointD(104.113164, 37.570667);
        //max 135.09567000000001, 53.563268999999998
        //min 73.502354999999994, 3.3971618700000001
        var maxBounds = new LvcPointD(135.09567000000001, 53.563268999999998);
        var minBounds = new LvcPointD(73.502354999999994, 3.3971618700000001);

        var scale = 5f;
        var mapWidth = 400f;
        var mapHeight = 300f;
        var projection = MapProjection.Mercator;
        var projector = Maps.BuildProjector(projection, new[] { mapWidth, mapHeight });

        //var center = projector.ToMap(gcp2);
        // var ox = mapWidth / 2f - center.X;
        // var oy = mapHeight / 2f - center.Y;
        projector.ToMap(minBounds.X, minBounds.Y, out var minX, out var minY);
        projector.ToMap(maxBounds.X, maxBounds.Y, out var maxX, out var maxY);
        var cx = (maxX - minX) / 2f + minX;
        var cy = (maxY - minY) / 2f + minY;
        var ox = mapWidth / 2f - cx;
        var oy = mapHeight / 2f - cy;

        var matrix = Matrix4.CreateTranslation(mapWidth / 2f, mapHeight / 2f);
        matrix.Scale(scale, scale);
        matrix.Translate(-(mapWidth / 2f), -(mapHeight / 2f));
        matrix.Translate(ox, oy);
        
        Child = new Center
        {
              Child = new Card
              {
                  Child = new GeoMap()
                  {
                      Width = mapWidth,
                      Height = mapHeight,
                      MapProjection = projection,
                      MinLongitude = minBounds.X,
                      MinLatitude = minBounds.Y,
                      MaxLongitude = maxBounds.X,
                      MaxLatitude = maxBounds.Y,
                      Stroke = new SolidColorPaint { Color = Colors.Green },
                      Fill = new SolidColorPaint
                      {
                          Color = Colors.Red,
                          // ImageFilter = new DropShadow(
                          //     2 / scale, 2 / scale, 6 / scale, 6 / scale,
                          //     Colors.Black /*new Color(50, 0, 0, 100)*/
                          // )
                       },
                       ActiveMap = Maps.GetMapFromStreamReader(new StreamReader(geoJson))
                  }
              }
        };
    }
}