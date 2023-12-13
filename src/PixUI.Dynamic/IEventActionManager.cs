using System;
using System.Collections.Generic;

namespace PixUI.Dynamic;

/// <summary>
/// 用于包装IEventAction的分组信息
/// </summary>
public sealed class EventActionInfo
{
    public EventActionInfo(string groupName, string actionName, Func<IEventAction> creator)
    {
        GroupName = groupName;
        ActionName = actionName;
        Creator = creator;
    }

    public readonly string GroupName;
    public readonly string ActionName;
    public readonly Func<IEventAction> Creator;
}

public interface IEventActionManager
{
    /// <summary>
    /// 设计时获取所有
    /// </summary>
    IList<EventActionInfo> GetAll();

    /// <summary>
    /// 根据名称创建相应的实例
    /// </summary>
    IEventAction Create(string actionName);
}