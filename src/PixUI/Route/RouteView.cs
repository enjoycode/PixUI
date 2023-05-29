using System.Threading.Tasks;

namespace PixUI;

public sealed class RouteView : DynamicView
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

    protected override void OnMounted()
    {
        base.OnMounted();

        var historyManager = Root!.Window.RouteHistoryManager;
        //尝试向HistoryManager添加第一条记录
        if (historyManager.Count == 0)
        {
            var path = historyManager.AssignedPath ?? "/";
            var entry = new RouteHistoryEntry(path);
            historyManager.PushEntry(entry);
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

    protected override void OnUnmounted()
    {
        base.OnUnmounted();

        //detach from parent navigator
        var parentNavigator = Navigator.Parent!;
        parentNavigator.DetachChild(Navigator);
        Navigator.HistoryManager = null;
    }

#if __WEB__
        private async void OnRouteChanged(RouteChangeAction action)
#else
    private void OnRouteChanged(RouteChangeAction action)
#endif
    {
        //TODO: stop running transition and check is 404.
        //TODO: if action is Goto, and route is keepalive, try get widget instance from cache
        var route = Navigator.ActiveRoute;
#if __WEB__
            var widget = await route.BuildWidgetAsync(Navigator.ActiveArgument);
#else
        var widget = route.Builder(Navigator.ActiveArgument);
#endif
            
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