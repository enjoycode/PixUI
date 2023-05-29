using System;

namespace PixUI;

public abstract class WidgetController<T> where T : Widget
{
    private T? _widget;

    public T Widget => _widget!;

    public void AttachWidget(T widget)
    {
        if (_widget != null) throw new InvalidOperationException();
        _widget = widget;
    }
}