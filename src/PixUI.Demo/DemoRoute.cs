using System.Collections.Generic;
using PixUI.Demo.Mac;

namespace PixUI.Demo
{
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
            };
            _navigator = new Navigator(routes);

            Child = new Column(debugLabel: "DemoRouteColumn")
            {
                Children = new[]
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
                Child = new Container { Child = child, BgColor = Colors.White }
            };
        }

        private Widget BuildMainMenu()
        {
            return new Container //TODO: remove Container
            {
                Child = new MainMenu(BuildMenuItems()),
                Height = 36,
                BgColor = new Color(200, 200, 200),
            };
        }

        private MenuItem[] BuildMenuItems()
        {
            return new MenuItem[]
            {
                MenuItem.SubMenu("Route", Icons.Filled.Map, new MenuItem[]
                {
                    MenuItem.Item("Back", Icons.Filled.ArrowBack, action: _navigator.Pop),
                    MenuItem.Item("Forward", Icons.Filled.ArrowForward, null),
                }),
                MenuItem.SubMenu("Debug", Icons.Filled.BugReport, new MenuItem[]
                {
                    MenuItem.Item("Outline", null, PaintDebugger.Switch),
                    MenuItem.SubMenu("Help", Icons.Filled.HelpOutline, new MenuItem[]
                    {
                        MenuItem.Item("About"),
                        MenuItem.Item("Window")
                    }),
                }),
                // MenuItem.Item("Form", null, () => _navigator.Push("form")),
                MenuItem.Item("Charts", null, () => _navigator.Push("charts")),
                MenuItem.Item("Animation", null, () => _navigator.Push("animation")),
                MenuItem.Item("ListView", null, () => _navigator.Push("list")),
                MenuItem.Item("Transform", null, () => _navigator.Push("transform")),
                MenuItem.Item("TabView", null, () => _navigator.Push("tabview")),
                MenuItem.Item("TreeView",
                    Icons.Filled.AccountTree, () => _navigator.Push("treeView")),
                MenuItem.Item("DataGrid",
                    Icons.Filled.TableView, () => _navigator.Push("datagrid")),
                MenuItem.Item("CodeEditor",
                    Icons.Filled.Edit, () => _navigator.Push("codeEditor")),
            };
        }
    }
}