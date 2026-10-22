public class DemoRow : View
{
    public DemoRow()
    {
        Child = new Center()
        {
            Child = new Column
            {
                Spacing = 5,
                Alignment = HorizontalAlignment.Left,
                Children = 
                [
                    new Text("1. 简单示例:"),
                    BuildRow(0, VerticalAlignment.Top),
                    new Text("2. 行间隙:"),
                    BuildRow(10, VerticalAlignment.Top),
                    new Text("3. 垂直居中对齐:"),
                    BuildRow(0, VerticalAlignment.Middle),
                    new Text("4. 充满可用宽度"),
                    new Container { Width = 300, Height = 50, Child = BuildRowWithExpanded() }
                ]
            }
        };
    }
    
    private Row BuildRow(float gap, VerticalAlignment align) => new Row
    {
          Spacing = gap,
          Alignment = align,
          Children = 
          [
              BuildRect(100, 30, Colors.Red),  
              BuildRect(100, 40, Colors.Blue), 
              BuildRect(100, 50, Colors.Green)
          ]  
    };
    
    private Row BuildRowWithExpanded() => new Row
    {
          Children = 
          [
              BuildRect(30, 30, Colors.Red), 
              //Expanded包裹的组件充满可用空间
              new Expanded(new Container { Height = 40, FillColor = Colors.Blue} ),
              BuildRect(30, 30, Colors.Green),
          ]  
    };
    
    private Widget BuildRect(float width, float height, Color color) => new Container()
    {
        Width = width,
        Height = height,
        FillColor = color,
    };
    
}