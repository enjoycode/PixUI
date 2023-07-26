using PixUI.Dynamic.Design;

namespace PixUI.Demo;

public sealed class DemoDesigner : View
{
    public DemoDesigner()
    {
        Child = new Row
        {
            Children =
            {
                new Container { Padding = EdgeInsets.All(8), Width = 200, Child = new Toolbox() },
                new Expanded { Child = new DesignCanvas() },
                new Container { Width = 200 }
            }
        };
    }
}