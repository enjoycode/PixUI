using System;

namespace PixUI.Dynamic.Design;

public static class DesignSettings
{
    public static Func<DesignElement, DynamicEventMeta, Dialog>? GetEventEditor;
    
    /// <summary>
    /// 获取数据表状态的编辑器
    /// </summary>
    public static Func<DesignController, DynamicState, Dialog>? GetTableStateEditor;
    
    public static Func<IDynamicTableState>? MakeTableState;

    /// <summary>
    /// 获取单个值状态的编辑器
    /// </summary>
    public static Func<DynamicState, Dialog>? GetValueStateEditor;

    public static Func<IDynamicValueState>? MakeValueState;
}