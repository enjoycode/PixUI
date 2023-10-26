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
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        if (Child != null)
        {
            Child.Layout(width, height);
            Child.SetPosition((width - Child.W) / 2, (height - Child.H) / 2);
        }

        SetSize(width, height);
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        Debug.Assert(ReferenceEquals(child, Child));
        Child!.SetPosition((W - Child.W) / 2, (H - Child.H) / 2);
    }
}