namespace PixUI;

/// <summary>
/// 需要绑定至数据集的动态表格、图形等组件的接口
/// </summary>
public interface IDataSetBinder
{
    /// <summary>
    /// DataSet属性的名称
    /// </summary>
    string DataSetPropertyName { get; }
}