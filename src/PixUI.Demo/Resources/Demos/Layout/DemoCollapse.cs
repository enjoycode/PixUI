public class DemoCollapse : View
{
    public DemoCollapse()
    {
        Child = new Column()
        {
            Children =
            [
                new Collapse()
                    { Title = new Text("Collapse1"), Body = new Container() { Height = 180, FillColor = Colors.Red } },
                new Collapse()
                    { Title = new Text("Collapse2"), Body = new Container() { Height = 280, FillColor = Colors.Green } }
            ]
        };
    }
}