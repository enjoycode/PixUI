using System;

namespace PixUI;

/// <summary>
/// 用于监听状态变更后做相应的操作
/// </summary>
public sealed class StateListener<T> : IStateBindable
{
    public StateListener(State<T> state, Action<T> changeAction)
    {
        _state = state;
        _changeHandler = changeAction;
        state.AddBinding(this, BindingOptions.None);
    }

    private readonly State<T> _state;
    private readonly Action<T> _changeHandler;

    public void OnStateChanged(State state, BindingOptions options) => _changeHandler.Invoke(_state.Value);
}