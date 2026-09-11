namespace PixUI;

partial class Widget
{
    private const int MountedMask = 1;
    private const int HasLayoutMask = 2; //TODO:待实现自动判断是否需要重新布局后移除
    private const int LayoutTightMask = 1 << 3;
    private const int InvisibleMask = 1 << 4; //不可见状态
    private const int SuspendingMountMask = 1 << 20;

    /// <summary>
    /// 用于移动组件上下级关系时，临时禁止激发OnMount/OnUnmount
    /// </summary>
    public bool SuspendingMount
    {
        get => (_flag & SuspendingMountMask) == SuspendingMountMask;
        set => SetFlagValue(value, SuspendingMountMask);
    }

    /// <summary>
    /// 用于一些只需要布局一次的组件判断是否已经布局过，以减少布局计算
    /// </summary>
    protected bool HasLayout
    {
        get => (_flag & HasLayoutMask) == HasLayoutMask;
        set => SetFlagValue(value, HasLayoutMask);
    }

    /// <summary>
    /// 容器类布局时计算本身大小，如true则尽量收缩为子级的大小
    /// </summary>
    public bool IsLayoutTight
    {
        get => (_flag & LayoutTightMask) == LayoutTightMask;
        set
        {
            if (value == IsLayoutTight) return;
            SetFlagValue(value, LayoutTightMask);
            Relayout();
        }
    }

    /// <summary>
    /// 是否挂载至WidgetTree
    /// </summary>
    public bool IsMounted
    {
        get => (_flag & MountedMask) == MountedMask;
        protected set
        {
            if (IsMounted == value) return;

            if (value)
                _flag |= MountedMask;
            else
                _flag &= ~(MountedMask);
        }
    }

    /// <summary>
    /// 是否可见状态，不可见状态的组件不会重新绘制
    /// </summary>
    public bool IsVisible
    {
        get => (_flag & InvisibleMask) != InvisibleMask;
        set => SetFlagValue(!value, InvisibleMask);
    }

    private void SetFlagValue(bool value, int mask)
    {
        if (value)
            _flag |= mask;
        else
            _flag &= ~(mask);
    }
}