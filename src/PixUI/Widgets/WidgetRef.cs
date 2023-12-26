using System;

namespace PixUI;

/// <summary>
/// Widget实例的引用
/// </summary>
[Obsolete("Use Widget.RefBy()")]
public sealed class WidgetRef<T> : IWidgetRef where T : Widget
{
    public T? Widget { get; private set; }

    public void SetWidget(Widget widget)
    {
        Widget = (T)widget;
    }
}

/// <summary>
/// Type erase for WidgetRef<T> 
/// </summary>
[Obsolete("Use Widget.RefBy()")]
public interface IWidgetRef
{
    void SetWidget(Widget widget);
}