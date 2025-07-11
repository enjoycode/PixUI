namespace PixUI.Diagram;

public sealed class ToolboxService
{

    private readonly DiagramSurface _surface;
    private DiagramItem? _currentContainer;

    public IDesignToolbox? Toolbox { get; set; }

    internal IDesignToolboxItem? SelectedItem => Toolbox?.SelectedItem;

    public ToolboxService(DiagramSurface surface)
    {
        _surface = surface;
    }

    internal void BeginCreation(int x, int y)
    {
        //先获取mouse下的Container
        _currentContainer = _surface.GetContainerUnderMouse(x, y);
        //todo:可在这里处理canvas只允许一个根级Container的情况，如果Container==null则退出新建模式
        //清空已选择项
        _surface.SelectionService.ClearSelection();
        //通知Adorners开始画新建框
        _surface.Adorners.BeginCreation(x, y, SelectedItem.IsConnection);

        Cursor.Current = Cursors.Arrow; //TODO: cross cursor
    }

    internal void OnMouseMove(int x, int y)
    {
        _surface.Adorners.UpdateCreationEndPoint(x, y);
    }

    internal void EndCreation(int x, int y)
    {
        //通知Adorners停止画新建框
        _surface.Adorners.EndCreation();

        //开始创建DiagramItem，并加入画布
        var newItem = SelectedItem.Create();
        if (_currentContainer == null)
        {
            IConnection connection = newItem as IConnection;
            if (connection != null)
            {
                connection.StartPoint = _surface.Adorners.CreationStartPoint;
                connection.EndPoint = _surface.Adorners.CreationEndPoint;
            }
            else
            {
                newItem.Bounds = _surface.Adorners.CreationRectangle;
            }
            _surface.AddItem(newItem);
        }
        else
        {
            var ptCanvas = _currentContainer.PointToSurface(Point.Empty);
            IConnection connection = newItem as IConnection;
            if (connection != null)
            {
                var startPt = _surface.Adorners.CreationStartPoint;
                var endPt = _surface.Adorners.CreationEndPoint;
                connection.StartPoint = new Point(startPt.X - ptCanvas.X, startPt.Y - ptCanvas.Y);
                connection.EndPoint = new Point(endPt.X - ptCanvas.X, endPt.Y - ptCanvas.Y);
            }
            else
            {
                var newRect = _surface.Adorners.CreationRectangle;
                newRect.X = (int)(newRect.X - ptCanvas.X);
                newRect.Y = (int)(newRect.Y - ptCanvas.Y);
                newItem.Bounds = newRect;
            }

            _currentContainer.AddItem(newItem);
        }

        //选中新建的
        _surface.SelectionService.SelectItem(newItem);

        Cursor.Current = Cursors.Arrow;
        Toolbox?.ClearSelectedItem();
    }

}