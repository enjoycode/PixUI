namespace PixUI.Demo;

public sealed class DemoSplitter : View
{
    public DemoSplitter()
    {
        Child = new Splitter
        {
            Distance = 200,
            Fixed = Splitter.FixedPanel.Panel1,
            Panel1 = new Center { Child = new Text("Panel1") },
            Panel2 = new Splitter
            {
                Orientation = Axis.Vertical,
                Distance = 200,
                Fixed = Splitter.FixedPanel.Panel2,
                Panel1 = new Center { Child = new Text("Panel2") },
                Panel2 = new Center { Child = new Text("Panel3") },
            }
        };
    }
}