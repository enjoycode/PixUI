using System.Threading.Tasks;

namespace PixUI.Dynamic;

/// <summary>
/// 设计时的DesignCanvas及运行时的DynamicWidget实现此接口，用于获取状态数据
/// </summary>
public interface IDynamicView
{
    /// <summary>
    /// 获取运行时数据集
    /// </summary>
    /// <param name="name">数据集状态的名称</param>
    ValueTask<object?> GetDataSet(string name);

    /// <summary>
    /// 获取运行时状态实例
    /// </summary>
    /// <param name="name">定义的状态名称</param>
    State GetState(string name);
}