namespace PixUI;

/// <summary>
/// 需要绑定至数据集的动态表格、图形等组件的接口
/// </summary>
public interface IDataSetBinder
{
    /// <summary>
    /// 数据集发生变更后刷新数据
    /// </summary>
    void OnDataSetValueChanged();
}