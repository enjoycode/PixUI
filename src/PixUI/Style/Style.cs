using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PixUI;

public interface IStyle<TSelf> where TSelf : Style, IStyle<TSelf>
{
    /// <summary>
    /// 当前默认样式的静态实例
    /// </summary>
    static abstract TSelf? Default { get; set; }
}

public abstract class Style
{
    public bool IsDefault { get; internal init; }

    protected static void Repaint(Widget widget) => widget.Repaint();
    protected static void Relayout(Widget widget) => widget.Relayout();
}

public abstract class Style<TWidget, TStyle> : Style, IDisposable 
    where TWidget : Widget where TStyle : Style<TWidget, TStyle>
{
    private List<TWidget>? _widgets;
    private List<StateChangeListener>? _states;

    /// <summary>
    /// 添加当前样式关联的Widget
    /// </summary>
    internal void AddWidget(TWidget widget)
    {
        if (IsDefault) throw new InvalidOperationException("Default style can't binding to widget");

        _widgets ??= new List<TWidget>();
        Debug.Assert(!_widgets.Exists(e => ReferenceEquals(e, widget)));
        _widgets.Add(widget);
    }

    /// <summary>
    /// 移除当前样式关联的Widget
    /// </summary>
    internal void RemoveWidget(TWidget widget)
    {
        _widgets?.Remove(widget);
    }

    protected void SetStyleProperty<T>(ref BindableProperty<T>? local, in T oldValue,
        in BindableProperty<T> newValue, Action<TWidget> action,
        [CallerMemberName] string? propName = null)
    {
        var changed = !EqualityComparer<T>.Default.Equals(oldValue, newValue.Value);
        if (local is { State: not null } && newValue.State == null)
        {
            //已经绑定至状态，直接修改状态值
            local.Value.State.Value = newValue.Value;
        }
        else
        {
            RemoveBinding(local?.State, propName!);
            local = newValue.State is UnbindState<float> ? newValue.Value : newValue;
            AddBinding(local.Value.State, propName!, action);
            if (changed) OnStyleValueChanged(action);
        }
    }

    private void AddBinding<T>(State<T>? state, string propertyName, Action<TWidget> action)
    {
        if (state == null) return;

        _states ??= new List<StateChangeListener>();

        var existed = FindState(state);
        if (existed < 0)
        {
            var listener = new StateChangeListener() { State = state };
            listener.AddProperty(propertyName, action);
            _states.Add(listener);
        }
        else
            _states[existed].AddProperty(propertyName, action);

        state.AddListener(OnStateChanged);
    }

    private void RemoveBinding<T>(State<T>? state, string propertyName)
    {
        if (state == null) return;
        var existed = FindState(state);
        if (existed >= 0)
        {
            if (_states![existed].RemoveProperty(propertyName))
                _states.RemoveAt(existed);
        }
    }

    private int FindState(State state)
    {
        if (_states == null || _states.Count == 0) return -1;

        var existed = -1;
        for (int i = 0; i < _states.Count; i++)
        {
            if (_states[i].State == state)
            {
                existed = i;
                break;
            }
        }

        return existed;
    }

    /// <summary>
    /// 在widget.Style属性变更时比较当前与oldStyle的各个样式值
    /// </summary>
    public virtual void OnStyleChanged(TWidget widget, TStyle oldStyle) { }

    /// <summary>
    /// 样式属性值变更时通知相关Widget进行相应的操作
    /// </summary>
    private void OnStyleValueChanged(Action<TWidget> action)
    {
        if (_widgets == null || _widgets.Count == 0) return;

        foreach (var widget in _widgets)
        {
            action(widget);
        }
    }

    /// <summary>
    /// 侦听样式属性绑定的状态值变更
    /// </summary>
    /// <param name="state"></param>
    private void OnStateChanged(State state)
    {
        if (_states == null || _states.Count == 0) return;
        foreach (var stateChangeListener in _states)
        {
            if (ReferenceEquals(state, stateChangeListener.State))
            {
                foreach (var (_, action) in stateChangeListener.Properties)
                {
                    OnStyleValueChanged(action);
                }
            }
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _widgets = null;

        if (_states == null || _states.Count == 0) return;
        foreach (var stateChangeListener in _states)
        {
            stateChangeListener.State.RemoveListener(OnStateChanged);
        }
    }

    private readonly struct StateChangeListener
    {
        public StateChangeListener() { }
        public required State State { get; init; }
        public readonly List<(string PropertyName, Action<TWidget>)> Properties = [];

        public void AddProperty(string propertyName, Action<TWidget> listener)
        {
            Properties.Add((propertyName, listener));
        }

        public bool RemoveProperty(string propertyName)
        {
            for (var i = Properties.Count - 1; i >= 0; i--)
            {
                if (Properties[i].PropertyName == propertyName)
                    Properties.RemoveAt(i);
            }

            return Properties.Count == 0;
        }
    }
}