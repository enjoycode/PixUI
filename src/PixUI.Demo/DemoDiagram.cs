using PixUI.Diagram;

namespace PixUI.Demo;

public sealed class DemoDiagram : View
{
    public DemoDiagram()
    {
        var view = new DiagramView(new DesignService());

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

internal sealed class DesignService : IDesignService
{
    private DiagramSurface _surface = null!;

    public void InitSurface(DiagramSurface surface) => _surface = surface;

    public void MoveSelection(int deltaX, int deltaY)
    {
        var selectedItems = _surface.SelectionService.SelectedItems;
        //TODO: 先判断有没有不能Move的对象，有则全部不允许移动
        // foreach (var item in selectedItems)
        // {
        //     if ((item.DesignBehavior & DesignBehavior.CanMove) != DesignBehavior.CanMove)
        //         return;
        // }

        //再处理移动所有选择的对象
        foreach (var item in selectedItems)
            item.Move(deltaX, deltaY);
    }

    public void DeleteSelection()
    {
        var selectedItems = _surface.SelectionService.SelectedItems;
        foreach (var item in selectedItems)
            item.Remove(); //TODO:判断是否允许删除，如RootView不允许删除

        _surface.SelectionService.ClearSelection();
    }
}