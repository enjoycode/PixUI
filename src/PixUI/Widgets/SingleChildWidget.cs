using System;
using System.Diagnostics;

namespace PixUI;

/// <summary>
/// 具有单个子级的Widget
/// </summary>
public abstract class SingleChildWidget : Widget
{
    protected SingleChildWidget()
    {
        IsLayoutTight = true;
    }

    private Widget? _child;
    private State<EdgeInsets>? _padding;

    public State<EdgeInsets>? Padding
    {
        get => _padding;
        set => Bind(ref _padding, value, RelayoutOnStateChanged);
    }

    public Widget? Child
    {
        get => _child;
        set
        {
            if (_child != null)
                _child.Parent = null;

            _child = value;

            if (_child != null)
                _child.Parent = this;
        }
    }

    public sealed override void VisitChildren(Func<Widget, bool> action)
    {
        if (_child != null)
            action(_child);
    }

    /// <summary>
    /// 无子组件如果IsLayoutTight==true则设为空,否则充满可用空间;
    /// 有子组件如果IsLayoutTight==true则设为子组件大小,否则充满可用空间
    /// </summary>
    public override void Layout(float availableWidth, float availableHeight)
    {
        var max = CacheAndGetMaxSize(availableWidth, availableHeight);

        var padding = _padding?.Value ?? EdgeInsets.All(0);

        if (Child == null)
        {
            if (IsLayoutTight)
                SetSize(0, 0);
            else
                SetSize(max.Width, max.Height);
            return;
        }

        Child.Layout(max.Width - padding.Left - padding.Right, max.Height - padding.Top - padding.Bottom);
        Child.SetPosition(padding.Left, padding.Top);

        if (IsLayoutTight)
            SetSize(Child.W + padding.Left + padding.Right, Child.H + padding.Top + padding.Bottom);
        else
            SetSize(max.Width, max.Height);
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        Debug.Assert(AutoSize);

        if (!IsLayoutTight) return; //do nothing when not IsLayoutTight

        var oldWidth = W;
        var oldHeight = H;
        SetSize(oldWidth + dx, oldHeight + dy); //直接更新自己的大小

        TryNotifyParentIfSizeChanged(oldWidth, oldHeight, affects);
    }
}