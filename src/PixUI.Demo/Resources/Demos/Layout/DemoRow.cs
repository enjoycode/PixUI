public class DemoRow : View
{
    public DemoRow()
    {
        Child = new Center()
        {
            Child = new Column
            {
                Alignment = HorizontalAlignment.Left,
                Children = 
                [
                    new Text("简单示例:"),
                    BuildRow(0, VerticalAlignment.Top),
                    new Text("行间隙:"),
                    BuildRow(10, VerticalAlignment.Top),
                    new Text("居中对齐:"),
                    BuildRow(0, VerticalAlignment.Middle),
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
    
    private Widget BuildRect(float width, float height, Color color) => new Container()
    {
        Width = width,
        Height = height,
        FillColor = color,
    };
    
}