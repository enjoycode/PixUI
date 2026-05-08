using System.Diagnostics;

namespace PixUI;

public sealed class Center : SingleChildWidget
{
    public Center()
    {
        IsLayoutTight = false;
    }

    /// <summary>
    /// 布局充满可用空间
    /// </summary>
    protected override void OnLayout(Size maxSize)
    {
        if (Child != null)
        {
            Child.PerformLayout(maxSize);
            Child.SetLayoutLocation((maxSize.Width - Child.W) / 2, (maxSize.Height - Child.H) / 2);
        }

        SetLayoutSize(maxSize);
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        Debug.Assert(ReferenceEquals(child, Child));
        Child!.SetLayoutLocation((W - Child.W) / 2, (H - Child.H) / 2);
    }
}