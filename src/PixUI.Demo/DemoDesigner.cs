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
                            new Button("Add") { OnTap = OnAdd },
                            new Button("Remove") {OnTap = OnRemove},
                        }
                    }
                },
                // Designer
                new Row
                {
                    Children =
                    {
                        new Container
                            { Padding = EdgeInsets.All(8), Width = 200, Child = new Toolbox(_designController) },
                        new Expanded { Child = new DesignCanvas(_designController) },
                        new Container { Width = 220, Child = new PropertyPanel(_designController) }
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

    private void OnAdd(PointerEvent e)
    {
        if (_designController.FirstSelected == null) return;

        var meta = _designController.CurrentToolboxItem;
        if (meta == null) return;

        var active = _designController.FirstSelected!;
        active.OnDrop(meta);

        //active.OnDrop(DynamicWidgetManager.GetByName("Center"));
        //active.AddChild(new DesignElement(_designController, DynamicWidgetManager.GetByName("Button")));
    }

    private void OnRemove(PointerEvent e)
    {
        _designController.DeleteElements();
    }
}