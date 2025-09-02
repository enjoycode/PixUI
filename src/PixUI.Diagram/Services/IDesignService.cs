namespace PixUI.Diagram;

/// <summary>
/// 设计时服务
/// </summary>
public interface IDesignService
{
    /// <summary>
    /// 初始化设计服务的画布
    /// </summary>
    void InitSurface(DiagramSurface surface);

    /// <summary>
    /// 移动选择的元素
    /// </summary>
    void MoveSelection(int deltaX, int deltaY);

    /// <summary>
    /// 从画布删除选择的元素
    /// </summary>
    void DeleteSelection();
}