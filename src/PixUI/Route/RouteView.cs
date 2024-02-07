namespace PixUI;

public class RouteView : DynamicView //Don't sealed this class
{
    public RouteView(Navigator navigator, string? name = null)
    {
        Name = name;
        Navigator = navigator;
        Navigator.OnRouteChanged = OnRouteChanged;
        //Child = Navigator.GetCurrentRoute();
    }

    internal readonly Navigator Navigator;
    internal readonly string? Name;

    //OnNavigateIn, OnNavigateOut

    protected sealed override void OnMounted()
    {
        base.OnMounted();

        var historyManager = Root!.Window.RouteHistoryManager;
        //尝试向HistoryManager添加第一条记录
        if (historyManager.IsEmpty)
        {
            var path = historyManager.AssignedPath ?? "/";
            var entry = new RouteHistoryEntry(path);
            historyManager.PushEntry(entry);
            UIApplication.Current.ReplaceWebHistory(path, 0);
#if __WEB__
            Navigator.ReplaceWebHistory(path, 0);
#endif
        }

        // set Navigator's tree & HistoryManager
        Navigator.HistoryManager = historyManager;
        var parentNavigator = Parent?.CurrentNavigator ?? historyManager.RootNavigator;
        parentNavigator.AttachChild(Navigator, Name);
        // init child widget to match route
        Navigator.InitRouteWidget();
    }

    protected sealed override void OnUnmounted()
    {
        base.OnUnmounted();

        //detach from parent navigator
        var parentNavigator = Navigator.Parent!;
        parentNavigator.DetachChild(Navigator);
        Navigator.HistoryManager = null;
    }

    private async void OnRouteChanged(RouteChangeAction action)
    {
        //TODO: stop running transition.
        //TODO: if action is Goto, and route is keepalive, try get widget instance from cache

        // check not found first
        var route = Navigator.ActiveRoute;
        if (route is NotFoundRoute)
        {
            var notFound = Navigator.NotFoundBuilder?.Invoke() ?? await route.BuildWidgetAsync(null);
            ReplaceTo(notFound);
            return;
        }

        // check allow access
        if (route.AllowAccess != null && !(await route.AllowAccess()))
        {
            var forbidden = Navigator.DenyAccessBuilder?.Invoke() ??
                            new Center
                            {
                                Child = new Card
                                {
                                    Padding = EdgeInsets.Only(10, 5, 10, 5),
                                    Child = new Text("403 Forbidden.") { FontSize = 20 }
                                }
                            };
            ReplaceTo(forbidden);
            return;
        }

        var widget = await route.BuildWidgetAsync(Navigator.ActiveArgument);
        if (action == RouteChangeAction.Init || route.EnteringBuilder == null)
        {
            ReplaceTo(widget);
        }
        else
        {
            var from = Child!;
            from.SuspendingMount = true; //动画开始前挂起

            Widget to;
            var reverse = action == RouteChangeAction.GotoBack;
            if (reverse)
            {
                to = from;
                from = widget;
            }
            else
            {
                to = widget;
            }

            AnimateTo(from, to, route.Duration, reverse, route.EnteringBuilder, route.ExistingBuilder);
        }
    }
}