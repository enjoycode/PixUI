using System;
using System.Threading.Tasks;

namespace PixUI.Dynamic;

/// <summary>
/// 设计时的DesignCanvas及运行时的DynamicWidget实现此接口，用于获取状态数据
/// </summary>
public interface IDynamicContext
{
    DynamicState? FindState(string name);
}

public static class DynamicContextExtensions
{
    /// <summary>
    /// 获取运行时数据集
    /// </summary>
    /// <param name="context"></param>
    /// <param name="name">数据集状态的名称</param>
    public static ValueTask<object?> GetDataSet(this IDynamicContext context, string name)
    {
        var state = context.FindState(name);
        if (state == null || state.Type != DynamicStateType.DataSet || state.Value == null)
            return new ValueTask<object?>();

        return ((IDynamicDataSetState)state.Value).GetRuntimeDataSet(context);
    }

    /// <summary>
    /// 获取运行时状态实例
    /// </summary>
    /// <param name="context"></param>
    /// <param name="name">定义的状态名称</param>
    public static State GetState(this IDynamicContext context, string name)
    {
        var state = context.FindState(name);
        if (state == null)
#if DEBUG
            throw new Exception($"Can't find state: {name}");
#else
            return State.Empty;
#endif
        if (state.Type == DynamicStateType.DataSet)
            throw new Exception($"State is DataSet: {name}");
        return ((IDynamicValueState)state.Value!).GetRuntimeValue(state);
    }

    /// <summary>
    /// 组件监听数据集变更
    /// </summary>
    public static void BindToDataSet(this IDynamicContext context, IDataSetBinder widget, string? dataset)
    {
        if (string.IsNullOrEmpty(dataset)) return;
        var state = context.FindState(dataset);
        if (state is not { Type: DynamicStateType.DataSet } || state.Value == null) return;

        ((IDynamicDataSetState)state.Value).DataSetValueChanged += widget.OnDataSetValueChanged;
    }

    /// <summary>
    /// 组件取消监听数据集变更
    /// </summary>
    public static void UnbindToDataSet(this IDynamicContext context, IDataSetBinder widget, string? dataset)
    {
        if (string.IsNullOrEmpty(dataset)) return;

        var state = context.FindState(dataset);
        if (state is not { Type: DynamicStateType.DataSet } || state.Value == null) return;

        ((IDynamicDataSetState)state.Value).DataSetValueChanged -= widget.OnDataSetValueChanged;
    }
}