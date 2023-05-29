namespace PixUI;

/// <summary>
/// Widget实例的引用
/// </summary>
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
public interface IWidgetRef
{
    void SetWidget(Widget widget);
}