namespace PixUI;

public sealed class Center : SingleChildWidget
{
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
}