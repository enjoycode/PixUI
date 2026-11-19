using System.Diagnostics;

namespace PixUI;

public abstract class Style
{
    internal bool IsDefault { get; set; }
    
    protected static void Repaint(Widget widget) => widget.Repaint();
    protected static void Relayout(Widget widget) => widget.Relayout();
}

public abstract class Style<TWidget> : Style, IDisposable where TWidget : Widget
{
    private List<TWidget>? _widgets;
    private List<StateChangeListener>? _states;

    internal void AddWidget(TWidget widget)
    {
        if (IsDefault) throw new InvalidOperationException("Default style can't binding to widget");

        _widgets ??= new List<TWidget>();
        Debug.Assert(!_widgets.Exists(e => ReferenceEquals(e, widget)));
        _widgets.Add(widget);
    }

    internal void RemoveWidget(TWidget widget)
    {
        _widgets?.Remove(widget);
    }

    // protected void SetProperty<T>(string propName, ref BindableProperty<T> oldValue, BindableProperty<T> newValue, 
    //     Action<TWidget> action) where T : struct, IEquatable<T>
    // {
    //     var changed = !EqualityComparer<T>.Default.Equals(oldValue.Value, newValue.Value);
    //     RemoveBinding(oldValue.State, propName);
    //     oldValue = newValue;
    //     AddBinding(newValue.State, propName, action);
    //     if (changed)
    //         OnStyleValueChanged(action);
    // }

    protected void AddBinding<T>(State<T>? state, string propertyName, Action<TWidget> action)
        where T : IEquatable<T>
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

    protected void RemoveBinding<T>(State<T>? state, string propertyName)
        where T : IEquatable<T>
    {
        if (state == null) return;
        var existed = FindState(state);
        if (existed >= 0)
            _states![existed].RemoveProperty(propertyName);
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

    protected void OnStyleValueChanged(Action<TWidget> action)
    {
        if (_widgets == null || _widgets.Count == 0) return;

        foreach (var widget in _widgets)
        {
            action(widget);
        }
    }

    private void OnStateChanged(State state)
    {
        if (_states == null || _states.Count == 0) return;
        foreach (var stateChangeListener in _states)
        {
            if (ReferenceEquals(state, stateChangeListener.State))
            {
                foreach (var (propertyName, action) in stateChangeListener.Properties)
                {
                    OnStyleValueChanged(action);
                }
            }
        }
    }

    public void Dispose()
    {
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

        public void RemoveProperty(string propertyName)
        {
            for (var i = Properties.Count - 1; i >= 0; i--)
            {
                if (Properties[i].PropertyName == propertyName)
                    Properties.RemoveAt(i);
            }
        }
    }
}