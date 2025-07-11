using System.Collections.Generic;
using PixUI.Demo.Mac;

namespace PixUI.Demo;

public sealed class DemoRoute : View
{
    private readonly Navigator _navigator;

    public DemoRoute()
    {
        var routes = new List<Route>
        {
            new("page", s => new DemoPage(), false, BuildDefaultTransition),
            //new("form", s => new DemoForm(), false, BuildDefaultTransition),
            new("charts", s => new DemoCharts(), false),
            new("list", s => new DemoListView(), false, BuildDefaultTransition),
            new("animation", s => new DemoAnimation(), false, BuildDefaultTransition),
            new("transform", s => new DemoTransform(), false, BuildDefaultTransition),
            new("tabview", s => new DemoTabView(), false, BuildDefaultTransition),
            new("treeView", s => new DemoTreeView(), false, BuildDefaultTransition),
            new("datagrid", s => new DemoDataGrid(), false, BuildDefaultTransition),
            new("codeEditor", s => new DemoCodeEditor(), false, BuildDefaultTransition),
            new("stack", s => new DemoStack()),
            new("designer", s => new DemoDesigner()),
            new("splitter", s => new DemoSplitter()),
            new("pdf", s => new DemoPdfViewer()),
            new("diagram", s => new DemoDiagram()),
            new("colors", s => new DemoColorPicker()),
        };
        _navigator = new Navigator(routes);

        Child = new Column(debugLabel: "DemoRouteColumn")
        {
            Children =
            {
                BuildMainMenu(),
                new Expanded(new RouteView(_navigator)) { DebugLabel = "RouteView" }
            }
        };
    }

    private static Widget BuildDefaultTransition(Animation<double> animation, Widget child)
    {
        var offsetAnimation = new OffsetTween(new Offset(1, 0), new Offset(0, 0)).Animate(animation);

        return new SlideTransition(offsetAnimation)
        {
            Child = new Container { Child = child, FillColor = Colors.White }
        };
    }

    private Widget BuildMainMenu()
    {
        return new Container //TODO: remove Container
        {
            Child = new MainMenu(BuildMenuItems()),
            Height = 36,
            FillColor = new Color(200, 200, 200),
        };
    }

    private MenuItem[] BuildMenuItems()
    {
        return new MenuItem[]
        {
            MenuItem.SubMenu("Route", MaterialIcons.Map, new MenuItem[]
            {
                MenuItem.Item("Back", MaterialIcons.ArrowBack, action: _navigator.Pop),
                MenuItem.Item("Forward", MaterialIcons.ArrowForward, null),
            }),
            MenuItem.SubMenu("Debug", MaterialIcons.BugReport, new MenuItem[]
            {
                MenuItem.Item("Outline", null, PaintDebugger.Switch),
                MenuItem.SubMenu("Help", MaterialIcons.HelpOutline, new MenuItem[]
                {
                    MenuItem.Item("About"),
                    MenuItem.Item("Window")
                }),
                MenuItem.Item("Stack", null, () => _navigator.Push("stack")),
                MenuItem.Item("Designer", null, () => _navigator.Push("designer")),
                MenuItem.Item("Splitter", null, () => _navigator.Push("splitter")),
                MenuItem.Item("PdfViewer", null, () => _navigator.Push("pdf")),
                MenuItem.Item("Diagram", null, () => _navigator.Push("diagram")),
                MenuItem.Item("ColorPicker", null, () => _navigator.Push("colors")),
            }),
            // MenuItem.Item("Form", null, () => _navigator.Push("form")),
            MenuItem.Item("Charts", null, () => _navigator.Push("charts")),
            MenuItem.Item("Animation", null, () => _navigator.Push("animation")),
            MenuItem.Item("ListView", null, () => _navigator.Push("list")),
            MenuItem.Item("Transform", null, () => _navigator.Push("transform")),
            MenuItem.Item("TabView", null, () => _navigator.Push("tabview")),
            MenuItem.Item("TreeView", MaterialIcons.AccountTree, () => _navigator.Push("treeView")),
            MenuItem.Item("DataGrid", MaterialIcons.TableView, () => _navigator.Push("datagrid")),
            MenuItem.Item("CodeEditor", MaterialIcons.Edit, () => _navigator.Push("codeEditor")),
        };
    }
}