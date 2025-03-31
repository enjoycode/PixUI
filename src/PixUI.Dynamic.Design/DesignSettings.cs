using System;

namespace PixUI.Dynamic.Design;

/// <summary>
/// 动态化设计时的委托
/// </summary>
public static class DesignSettings
{
    /// <summary>
    /// 根据类型创建DynamicState的Value属性值的委托
    /// </summary>
    public static Func<DynamicStateType, IDynamicStateValue> CreateDynamicStateValue = null!;

    /// <summary>
    /// 获取状态对应的设计时编辑器
    /// </summary>
    public static Func<DesignController, DynamicState, Dialog?> GetStateEditor = null!;

    /// <summary>
    /// 获取事件对应的设计时编辑器
    /// </summary>
    public static Func<DesignElement, DynamicEventMeta, Dialog?> GetEventEditor = null!;
}