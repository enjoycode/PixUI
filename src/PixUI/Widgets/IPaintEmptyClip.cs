namespace PixUI;

/// <summary>
/// 在重绘组件时如果裁剪区域为空，实现了此接口的组件会调用以清除或中断动画绘制过程
/// </summary>
public interface IPaintEmptyClip
{
    /// <summary>
    /// 目前主要用于解决ChartView不可见时，RunDrawingLoop时不停止的问题
    /// </summary>
    /// <param name="canvas">裁剪区域为空的画布</param>
    void ClearOrStopPaint(Canvas canvas);
}