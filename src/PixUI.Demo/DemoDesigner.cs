using System.Text;
using PixUI.Dynamic;
using PixUI.Dynamic.Design;

namespace PixUI.Demo;

public sealed class DemoDesigner : View
{
    public DemoDesigner()
    {
        Child = new Column
        {
            Children =
            {
                // CommandBar
                new Container
                {
                    Height = 40,
                    Padding = EdgeInsets.All(5),
                    BgColor = Colors.Gray,
                    Child = new Row(VerticalAlignment.Middle, 5f)
                    {
                        Children =
                        {
                            new Button("Load") { OnTap = OnLoad },
                            new Button("Save"),
                            new Button("Test") { OnTap = OnTest },
                        }
                    }
                },
                // Designer
                new Row
                {
                    Children =
                    {
                        new Container { Padding = EdgeInsets.All(8), Width = 200, Child = new Toolbox() },
                        new Expanded { Child = new DesignCanvas(_designController) },
                        new Container { Width = 200 }
                    }
                },
            }
        };
    }

    private readonly DesignController _designController = new();

    private void OnLoad(PointerEvent e)
    {
        const string json = """
                            {
                              "View": {
                                "Type": "Center",
                                "Child": {
                                  "Type": "Button",
                                  "CtorArgs": [ { "Const": "Button1" }, { "Const": null } ],
                                  "Properties": { "TextColor": { "Const": "FFFF0000" } }
                                }
                              }
                            }
                            """;

        _designController.Load(Encoding.UTF8.GetBytes(json));
    }

    private void OnTest(PointerEvent e)
    {
        if (_designController.FirstSelected == null) return;

        var active = _designController.FirstSelected!;
        active.OnDrop(DynamicWidgetManager.GetByName("Center"));
        active.AddChild(new DesignElement(_designController, DynamicWidgetManager.GetByName("Button")));
    }
}