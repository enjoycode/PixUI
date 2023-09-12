using System;

namespace PixUI;

/// <summary>
/// 用于通知对象实例的变更
/// </summary>
public sealed class ObjectNotifier<T> where T : class //TODO: remove it
{
    private Action<T>? _changeHandler;

    public Action<T> OnChange
    {
        set => _changeHandler = value;
    }

    public void Notify(T obj) => _changeHandler?.Invoke(obj);
}