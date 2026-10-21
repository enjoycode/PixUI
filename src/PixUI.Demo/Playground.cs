namespace PixUI.Demo;

public sealed class Playground : View
{
    public Playground()
    {
        var context = new PlaygroundContext();

        Child = new Column()
        {
            Children =
            [
                new HeaderPad(),
                new Expanded
                {
                    Child = new Row
                    {
                        Children =
                        {
                            new NaviBar(context),
                            new Expanded()
                            {
                                Child = new Splitter
                                {
                                    Fixed = Splitter.FixedPanel.Panel1,
                                    Distance = 200,
                                    Panel1 = new SidePad(context),
                                    Panel2 = new Splitter
                                    {
                                        Orientation = Axis.Vertical,
                                        Fixed = Splitter.FixedPanel.Panel2,
                                        Distance = 100,
                                        Panel1 = new DesignerPad(context),
                                        Panel2 = new BottomPad(context)
                                    }
                                }
                            }
                        }
                    }
                },
                new FooterPad()
            ]
        };
    }
}