using System;
using System.Collections.Generic;

namespace PixUI;

public enum RouteChangeAction
{
    Init,
    Push,
    GotoBack,
    GotoForward,
}

/// <summary>
/// 路由导航器，与RouteView一对一绑定控制其导航
/// </summary>
public sealed class Navigator
{
    public Navigator(IEnumerable<Route> routes)
    {
        _routes.AddRange(routes);
    }

    private readonly List<Route> _routes = new List<Route>();
    internal RouteHistoryManager? HistoryManager;

    /// <summary>
    /// 路由改变时的通知绑定的RouteView重新Build
    /// </summary>
    internal Action<RouteChangeAction>? OnRouteChanged;

    public Navigator? Parent { get; private set; }
    internal List<Navigator>? Children { get; private set; }

    /// <summary>
    /// 命名路由视图的名称
    /// </summary>
    internal string? NameOfRouteView { get; private set; }

    internal Route ActiveRoute { get; private set; }
    internal string? ActiveArgument { get; private set; }

    #region ====命名路由视图相关====

    internal bool IsNamed => NameOfRouteView != null;

    internal bool IsInNamed => GetNamedParent() != null;

    internal Navigator? GetNamedParent()
    {
        var parent = Parent;
        while (parent != null)
        {
            if (parent.IsNamed) return parent;
            parent = parent.Parent;
        }

        return null;
    }

    #endregion

    #region ====路径相关属性====

    private string NameAndArgument
    {
        get
        {
            if (!ActiveRoute.Dynamic)
                return ActiveRoute.Name;
            return ActiveRoute.Name + "/" + ActiveArgument; //TODO:未指定参数转换为空
        }
    }

    internal string ParentPath
    {
        get
        {
            if (Parent == null)
                return ""; //Only for RootNavigate
            if (Parent.IsNamed)
                return Parent.NameAndArgument + "/";
            if (Parent.Parent == null)
                return "/";
            return Parent.ParentPath + Parent.NameAndArgument + "/";
        }
    }

    internal string Path => ParentPath + NameAndArgument;

    #endregion

    #region ====路由树(嵌套)结构====

    internal void AttachChild(Navigator child, string? nameOfRouteView)
    {
        //TODO: 检查有效性，比如只允许一个默认路由，不能重名的命名路由
        child.NameOfRouteView = nameOfRouteView;
        child.Parent = this;
        Children ??= new List<Navigator>();
        Children.Add(child);
    }

    internal void DetachChild(Navigator child)
    {
        child.Parent = null;
        Children?.Remove(child);
    }

    /// <summary>
    /// 获取默认路由(第一个路由)
    /// </summary>
    internal Route GetDefaultRoute() => _routes[0];

    /// <summary>
    /// 指定名称的路由是否动态路由
    /// </summary>
    internal bool IsDynamic(string name)
    {
        var matchRoute = _routes.Find(r => r.Name == name);
        if (matchRoute == null) return false;
        return matchRoute.Dynamic;
    }

    #endregion

    internal void InitRouteWidget()
    {
        if (_routes.Count == 0) return; //TODO: 404

        //默认指向第一个
        ActiveRoute = _routes[0];
        //处理指定的路由路径
        var asnPath = HistoryManager!.AssignedPath;
        if (!string.IsNullOrEmpty(asnPath))
        {
            if (IsNamed)
            {
                //TODO: 当前是命名的, eg: /一级路由?命名的路由=当前路由
            }
            else if (IsInNamed)
            {
                // TODO: 当前是命名的下级, eg: /一级路由?命名的路由=路由1/当前路由
            }
            else
            {
                var parentPath = ParentPath;
                //排除 eg: asnPath=/一级路由 但当前路径=/一级路由/当前路由
                if (asnPath.Length > parentPath.Length)
                {
                    var thisPath = asnPath.Substring(parentPath.Length);
                    var thisName = thisPath;
                    string? thisArg = null;
                    if (thisPath.IndexOf('/') >= 0)
                    {
                        var pss = thisPath.Split('/');
                        thisName = pss[0];
                        thisArg = pss[1];
                    }
                    
                    var matchRoute = _routes.Find(r => r.Name == thisName);
                    if (matchRoute == null) //TODO: 404
                        throw new Exception("Can't find route: " + thisName);
                    ActiveRoute = matchRoute;
                    if (ActiveRoute.Dynamic)
                        ActiveArgument = thisArg;
                }
            }
        }


        OnRouteChanged?.Invoke(RouteChangeAction.Init);
    }

    public void Push(string name, string? arg = null)
    {
        name = name.ToLower();
        if (arg != null) arg = arg.ToLower();
        //判断当前路由一致（包括动态参数）,是则忽略
        if (name == ActiveRoute.Name && arg == ActiveArgument)
            return;

        //查找静态路由表
        var matchRoute = _routes.Find(r => r.Name == name);
        if (matchRoute == null) //TODO: 404 not found route
            throw new ArgumentException($"Can't find route: {name}");
        ActiveRoute = matchRoute;
        ActiveArgument = arg;

        //添加至历史记录
        var fullPath = HistoryManager!.GetFullPath();
        HistoryManager.AssignedPath = fullPath;
        var entry = new RouteHistoryEntry(fullPath);
        HistoryManager!.PushEntry(entry);
#if __WEB__
            //同步浏览器历史记录
            PushWebHistory(fullPath, HistoryManager.Count - 1);
#endif

        //通知变更
        OnRouteChanged?.Invoke(RouteChangeAction.Push);
    }

    public void Pop() => HistoryManager!.Pop();

    internal void Goto(string name, string? arg, RouteChangeAction action)
    {
        var matchRoute = _routes.Find(r => r.Name == name);
        if (matchRoute == null)
            throw new Exception("Can't find route: " + name);
        ActiveRoute = matchRoute;
        ActiveArgument = arg;

        OnRouteChanged?.Invoke(action);
    }

#if __WEB__
        [TSRawScript(@"
        private static PushWebHistory(path: string, index: number) {
            let url = document.location.origin + '/#' + path;
            history.pushState(index, '', url);
        }
")]
        private static void PushWebHistory(string path, int index) {}
        
        [TSRawScript(@"
        public static ReplaceWebHistory(path: string, index: number) {
            let url = document.location.origin;
            if (path != '/')
                url += '/#' + path;
            history.replaceState(index, '', url);
        }
")]
        public static void ReplaceWebHistory(string path, int index) {}
#endif
}