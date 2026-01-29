using System;

namespace PixUI;

public interface IScrollable
{
    ScrollDirection ScrollDirection { get; }

    ScrollBarVisibility ShowScrollBar { get; }

    float ScrollOffsetX { get; }

    float ScrollOffsetY { get; }

    /// <summary>
    /// 处理滚动事件的偏移量
    /// </summary>
    /// <returns>实际滚动的偏移量</returns>
    Offset OnScroll(float dx, float dy);
}

public enum ScrollDirection
{
    Horizontal = 1,
    Vertical = 2,
    Both = Horizontal | Vertical,
}

public enum ScrollBarVisibility
{
    /// <summary>
    /// 不显示
    /// </summary>
    Never,
    /// <summary>
    /// Hover时显示
    /// </summary>
    Hover,
    /// <summary>
    /// 始终显示
    /// </summary>
    Always,
}

public sealed class ScrollController
{
    public readonly ScrollDirection Direction;

    //TODO: ScrollPhysics & ScrollBehavior

    public float OffsetX { get; set; }
    public float OffsetY { get; set; }

    public ScrollController(ScrollDirection direction)
    {
        Direction = direction;
    }

    /// <summary>
    /// 默认的滚动行为
    /// </summary>
    /// <returns>实际滚动的偏移量</returns>
    public Offset OnScroll(float dx, float dy, float maxOffsetX, float maxOffsetY)
    {
        var oldX = OffsetX;
        var oldY = OffsetY;

        //暂滚动不允许超出范围
        if (Direction is ScrollDirection.Both or ScrollDirection.Horizontal)
            OffsetX = Math.Clamp(OffsetX + dx, 0, maxOffsetX);

        if (Direction is ScrollDirection.Both or ScrollDirection.Vertical)
            OffsetY = Math.Clamp(OffsetY + dy, 0, maxOffsetY);

        return new Offset(OffsetX - oldX, OffsetY - oldY);
    }

    /// <summary>
    /// 在IScrollable组件改变Size后确保滚动值在范围内
    /// </summary>
    public bool Clamp(float maxOffsetX, float maxOffsetY)
    {
        if (OffsetX <= maxOffsetX && OffsetY <= maxOffsetY)
            return false;
        
        if (Direction is ScrollDirection.Both or ScrollDirection.Horizontal)
            OffsetX = Math.Clamp(OffsetX, 0, maxOffsetX);

        if (Direction is ScrollDirection.Both or ScrollDirection.Vertical)
            OffsetY = Math.Clamp(OffsetY, 0, maxOffsetY);

        return true;
    }
}