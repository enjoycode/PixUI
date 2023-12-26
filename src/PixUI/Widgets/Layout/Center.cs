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
    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        if (Child != null)
        {
            Child.Layout(maxSize.Width, maxSize.Height);
            Child.SetPosition((maxSize.Width - Child.W) / 2, (maxSize.Height - Child.H) / 2);
        }

        SetSize(maxSize.Width, maxSize.Height);
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        Debug.Assert(ReferenceEquals(child, Child));
        Child!.SetPosition((W - Child.W) / 2, (H - Child.H) / 2);
    }
}