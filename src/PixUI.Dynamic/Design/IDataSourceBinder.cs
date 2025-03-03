namespace PixUI;

/// <summary>
/// 需要绑定至数据源的动态表格、图形等组件的接口
/// </summary>
public interface IDataSourceBinder
{
    /// <summary>
    /// 数据集发生变更后刷新数据
    /// </summary>
    void OnDataSourceChanged();
}