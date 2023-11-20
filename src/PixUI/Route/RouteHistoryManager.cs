using System;
using System.Collections.Generic;

namespace PixUI;

/// <summary>
/// 路由历史项
/// </summary>
internal sealed class RouteHistoryEntry
{
    internal RouteHistoryEntry(string path)
    {
        Path = path;
    }

    internal readonly string Path;
    //TODO: cache of keepalive widget, think about use Map<PathString, Widget>
}

internal sealed class BuildPathContext
{
    internal Navigator LeafDefault = null!;
    internal readonly Dictionary<string, Navigator> LeafNamed = new();

    internal string GetFullPath()
    {
        var fullPath = LeafDefault.Path;
        if (LeafNamed.Count > 0)
        {
            fullPath += "?";
            var first = true;
            foreach (var key in LeafNamed.Keys)
            {
                if (first) first = false;
                else fullPath += "&";

                fullPath += key + "=" + LeafNamed[key]!.Path;
            }
        }

        return fullPath;
    }
}

/// <summary>
/// 路由历史管理，一个UIWindow对应一个实例
/// </summary>
public sealed class RouteHistoryManager
{
    private readonly List<RouteHistoryEntry> _history = new List<RouteHistoryEntry>();
    private int _historyIndex = -1;

    internal readonly Navigator RootNavigator = new Navigator(Array.Empty<Route>());

    internal string? AssignedPath { get; set; }

    internal bool IsEmpty => _history.Count == 0;

    public int NewIdForPush() => _historyIndex + 1;

    /// <summary>
    /// 获取当前路由的全路径
    /// </summary>
    internal string GetFullPath()
    {
        if (RootNavigator.Children == null || RootNavigator.Children.Count == 0)
            return "";

        var ctx = new BuildPathContext();
        BuildFullPath(ctx, RootNavigator);
        return ctx.GetFullPath();
    }

    private static void BuildFullPath(BuildPathContext ctx, Navigator navigator)
    {
        if (navigator.IsNamed)
        {
            ctx.LeafNamed.Add(navigator.NameOfRouteView!, navigator);
        }
        else if (navigator.IsInNamed)
        {
            var named = navigator.GetNamedParent()!;
            ctx.LeafNamed.Add(named.NameOfRouteView!, navigator);
        }
        else
        {
            ctx.LeafDefault = navigator;
        }

        if (navigator.Children != null)
        {
            foreach (var child in navigator.Children)
            {
                BuildFullPath(ctx, child);
            }
        }
    }

    internal int PushEntry(RouteHistoryEntry entry)
    {
        //先清空之后的记录
        if (_historyIndex != _history.Count - 1)
        {
            //TODO: dispose will removed widgets
            _history.RemoveRange(_historyIndex + 1, _history.Count - _historyIndex - 1);
        }

        _history.Add(entry);
        _historyIndex++;
        return _historyIndex;
    }

    internal RouteHistoryEntry? Pop()
    {
        if (_historyIndex <= 0) return null;

        var oldEntry = _history[_historyIndex];
        Goto(_historyIndex - 1);
        return oldEntry;
    }

    public void Goto(int index)
    {
        if (index < 0 || index >= _history.Count)
            throw new Exception("index out of range");

        var action = index < _historyIndex ? RouteChangeAction.GotoBack : RouteChangeAction.GotoForward;
        _historyIndex = index;
        var newEntry = _history[_historyIndex];
        var oldPath = AssignedPath;
        AssignedPath = newEntry.Path;

        NavigateTo(oldPath, newEntry.Path, action);
    }

    public int Push(string fullPath)
    {
        //TODO: 验证fullPath start with '/' and convert to lowercase
        var oldPath = AssignedPath;
        AssignedPath = fullPath;
        var newEntry = new RouteHistoryEntry(fullPath); //TODO:考虑已存在则改为Goto
        var index = PushEntry(newEntry);

        NavigateTo(oldPath, fullPath, RouteChangeAction.Push);
        return index;
    }

    private void NavigateTo(string? oldPath, string newPath, RouteChangeAction action)
    {
        //从根开始比较
        var psa = newPath.Split('?');
        var defaultPath = psa[0];
        var defaultPss = defaultPath.Split('/');

        //TODO:考虑在这里处理/biz/orders跳转至/biz下级默认路由，以下暂简单判断
        //获取当前的Leaf导航器路径的默认路径与oldPath比较是否一致
        var maybeChildToDefaut = oldPath != null && oldPath.Length > newPath.Length;

        //先比较处理默认路径
        var navigator = GetDefaultNavigator(RootNavigator);
        ComparePath(navigator, defaultPss, 1, maybeChildToDefaut, action);
        //TODO: 再处理各命名路由的路径
    }

    private static bool ComparePath(Navigator? navigator, string[] pss, int index, bool maybeChildToDefault,
        RouteChangeAction action)
    {
        while (true)
        {
            if (navigator == null) return false;

            var name = pss[index];
            if (name == "") name = navigator.GetDefaultRoute().Name;
            string? arg = null;
            if (navigator.IsDynamic(name))
            {
                arg = pss[index + 1];
                index++;
            }

            if (name != navigator.ActiveRoute.Name || arg != navigator.ActiveArgument)
            {
                navigator.Goto(name, arg, action);
                return true;
            }

            if (index == pss.Length - 1)
            {
                //可能从/biz/orders跳转至/biz下的默认下级路由
                if (maybeChildToDefault)
                {
                    //TODO:这里尚未考虑keepalive的情况，以下跳转会失效。另如果上面先处理该情况移除这里的实现
                    navigator.Goto(name, arg, action);
                    return true;
                }

                return false;
            }

            navigator = GetDefaultNavigator(navigator);
            index = index + 1;
        }
    }

    /// <summary>
    /// 获取默认路由（惟一的非命名的）
    /// </summary>
    private static Navigator? GetDefaultNavigator(Navigator navigator)
    {
        if (navigator.Children == null || navigator.Children.Count == 0)
            return null;

        for (var i = 0; i < navigator.Children.Count; i++)
        {
            if (!navigator.Children[i].IsNamed)
                return navigator.Children[i];
        }

        return null;
    }
}