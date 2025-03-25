using System;
using System.Collections.Generic;

namespace PixUI;

internal sealed class WhenBuilder<T>
{
    internal readonly Predicate<T> Match;
    private readonly Func<Widget> _builder;
    private Widget? _cachedWidget;

    public WhenBuilder(Predicate<T> match, Func<Widget> builder)
    {
        Match = match;
        _builder = builder;
    }

    internal Widget? GetWidget()
    {
        return _cachedWidget ??= _builder();
    }
}

/// <summary>
/// 根据状态条件构建不同的子组件
/// </summary>
public class Conditional<T> : DynamicView //where T: IEquatable<T>
{
    public Conditional(State<T> state)
    {
        IsLayoutTight = true;
        Bind(ref _state!, state, OnStateChanged);
    }

    private readonly State<T> _state;
    private readonly List<WhenBuilder<T>> _whens = new();

    //TODO: add AutoDispose property to dispose not used widget

    private Widget? MakeChildByCondition()
    {
        foreach (var item in _whens)
        {
            //EqualityComparer<T>.Default.Equals(item.StateValue, _state.Value)
            if (item.Match(_state.Value))
            {
                return item.GetWidget();
            }
        }

        return null;
    }

    public Conditional<T> When(Predicate<T> predicate, Func<Widget> builder)
    {
        _whens.Add(new WhenBuilder<T>(predicate, builder));
        Child ??= MakeChildByCondition();
        return this;
    }

    private void OnStateChanged(State state)
    {
        var newChild = MakeChildByCondition();
        ReplaceTo(newChild);
    }
}