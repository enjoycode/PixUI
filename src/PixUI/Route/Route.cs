using System;
using System.Threading.Tasks;

namespace PixUI;
#if __WEB__
    public delegate Task<Widget> RouteWidgetAsyncBuilder(string? arg);
#endif
    
[TSType("PixUI.RouteWidgetAsyncBuilder"), TSRename("RouteWidgetAsyncBuilder")]
public delegate Widget RouteWidgetBuilder(string? arg);

/// <summary>
/// 路由配置项
/// </summary>
public sealed class Route
{
    internal readonly string Name;

    /// <summary>
    /// 是否有动态参数 eg: /user/:id
    /// </summary>
    internal readonly bool Dynamic;

    internal readonly RouteWidgetBuilder Builder;

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

    public Route(string name, RouteWidgetBuilder builder, bool isDynamic = false,
        TransitionBuilder? enteringBuilder = null, TransitionBuilder? existingBuilder = null,
        int duration = 200, int reverseDuration = 200)
    {
        //TODO:检查名称有效性
        Name = name.ToLower();
        Dynamic = isDynamic;
        Builder = builder;
        Duration = duration;
        ReverseDuration = reverseDuration;
        EnteringBuilder = enteringBuilder;
    }

#if __WEB__
        [TSRawScript(
            "public BuildWidgetAsync(arg: string | null): Promise<PixUI.Widget> { return this.Builder(arg); }")]
        internal Task<Widget> BuildWidgetAsync(string? arg) => throw new NotSupportedException();
#endif
}