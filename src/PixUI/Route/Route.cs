using System;
using System.Threading.Tasks;

namespace PixUI;

public delegate Task<Widget> RouteWidgetAsyncBuilder(string? arg);

[TSType("PixUI.RouteWidgetAsyncBuilder"), TSRename("RouteWidgetAsyncBuilder")]
public delegate Widget RouteWidgetBuilder(string? arg);

public abstract class RouteBase
{
    protected RouteBase(string name, bool isDynamic = false,
        TransitionBuilder? enteringBuilder = null, TransitionBuilder? existingBuilder = null,
        int duration = 200, int reverseDuration = 200, Func<ValueTask<bool>>? allowAccess = null)
    {
        //TODO:检查名称有效性
        Name = name.ToLower();
        Dynamic = isDynamic;
        Duration = duration;
        ReverseDuration = reverseDuration;
        EnteringBuilder = enteringBuilder;
        ExistingBuilder = existingBuilder;
        AllowAccess = allowAccess;
    }

    internal readonly string Name;

    /// <summary>
    /// 是否有动态参数 eg: /user/:id
    /// </summary>
    internal readonly bool Dynamic;

    internal readonly Func<ValueTask<bool>>? AllowAccess;

    internal readonly int Duration;

    internal readonly int ReverseDuration;

    /// <summary>
    /// 进入动画
    /// </summary>
    internal readonly TransitionBuilder? EnteringBuilder;

    /// <summary>
    /// 退出动画
    /// </summary>
    internal readonly TransitionBuilder? ExistingBuilder;

    public abstract ValueTask<Widget> BuildWidgetAsync(string? args);
}

/// <summary>
/// 路由配置项
/// </summary>
public class Route : RouteBase
{
    public Route(string name, RouteWidgetBuilder builder, bool isDynamic = false,
        TransitionBuilder? enteringBuilder = null, TransitionBuilder? existingBuilder = null,
        int duration = 200, int reverseDuration = 200, Func<ValueTask<bool>>? allowAccess = null)
        : base(name, isDynamic, enteringBuilder, existingBuilder, duration, reverseDuration, allowAccess)
    {
        _builder = builder;
    }

    private readonly RouteWidgetBuilder _builder;

    [TSRawScript("public BuildWidgetAsync(arg: string | null): Promise<PixUI.Widget> { return this._builder(arg); }")]
    public override ValueTask<Widget> BuildWidgetAsync(string? args) => new(_builder(args));
}

internal sealed class NotFoundRoute : RouteBase
{
    public NotFoundRoute(string name) : base(name, false, null, null, 0, 0) { }

    public override ValueTask<Widget> BuildWidgetAsync(string? args)
    {
        return new ValueTask<Widget>(new Center
        {
            Child = new Card
            {
                Padding = EdgeInsets.Only(10, 5, 10, 5),
                Child = new Text("404 Not Found.") { FontSize = 20 }
            }
        });
    }
}