using System;

namespace PixUI;

/// <summary>
/// 需要绑定至数据源的动态表格、图形等组件的接口
/// </summary>
public interface IDataSourceBinder
{

    /// <summary>
    /// 当前数据行改变事件
    /// </summary>
    event Action<IDataSourceBinder, object?> CurrentRowChanged;
    
    /// <summary>
    /// 数据源发生变更后刷新数据或重置相关配置
    /// </summary>
    /// <param name="isReset">是否数据源重置，仅用于设计时改变数据源配置</param>
    void OnDataChanged(bool isReset);
}