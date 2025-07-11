using PixUI.Diagram;

namespace PixUI.Demo;

public sealed class DemoDiagram : View
{
    public DemoDiagram()
    {
        var view = new DiagramView();

        var shape1 = new DiagramShape();
        shape1.Bounds = Rect.FromLTWH(100, 100, 100, 100);
        view.Surface.AddItem(shape1);

        var shape2 = new DiagramShape();
        shape2.Bounds = Rect.FromLTWH(300, 300, 100, 100);
        view.Surface.AddItem(shape2);

        var conn = new DiagramConnection();
        conn.ConnectionType = ConnectionType.Bezier;
        conn.BezierTension = 1;
        conn.Attach(shape1.Connectors["Right"], shape2.Connectors["Top"]);
        view.Surface.AddItem(conn);

        Child = view;
    }
}