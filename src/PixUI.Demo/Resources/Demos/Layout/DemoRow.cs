public class DemoRow : View
{
    public DemoRow()
    {
        Child = new Center()
        {
            Child = new Text("Hello Row!")
        };
    }
}