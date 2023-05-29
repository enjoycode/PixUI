using System;
using System.Collections.Generic;

namespace PixUI;

public sealed class TabBody<T> : DynamicView
{
    private readonly TabController<T> _controller;
    private readonly Func<T, Widget> _bodyBuilder;
    private readonly List<Widget?> _bodies;

    public TabBody(TabController<T> controller, Func<T, Widget> bodyBuilder)
    {
        _controller = controller;
        _controller.BindTabBody(this);
        _bodyBuilder = bodyBuilder;

        _bodies = new List<Widget?>(new Widget[_controller.DataSource.Count]);
    }

    private Widget TryBuildBody()
    {
        var selectedIndex = _controller.SelectedIndex;
        if (_bodies[selectedIndex] == null)
        {
            var selectedData = _controller.DataSource[selectedIndex];
            _bodies[selectedIndex] = _bodyBuilder(selectedData);
        }

        return _bodies[selectedIndex]!;
    }

    internal void OnAdd(T dataItem) => _bodies.Add(null);

    internal void OnRemoveAt(int index)
    {
        if (_bodies[index] != null)
            _bodies[index]!.Parent = null;
        _bodies.RemoveAt(index);
    }

    /// <summary>
    /// 从旧TabBody切换至新的
    /// </summary>
    internal void SwitchFrom(int oldIndex)
    {
        var newIndex = _controller.SelectedIndex;
        var to = TryBuildBody();

        if (oldIndex < 0)
        {
            ReplaceTo(to);
        }
        else
        {
            var from = Child;
            from!.SuspendingMount = true; //动画开始前挂起
            AnimateTo(from, to, 200, false,
                (a, w) =>
                    BuildDefaultTransition(a, w, new Offset(newIndex > oldIndex ? 1 : -1, 0),
                        Offset.Empty),
                (a, w) =>
                    BuildDefaultTransition(a, w, Offset.Empty,
                        new Offset(newIndex > oldIndex ? -1 : 1, 0)));
        }
    }

    /// <summary>
    /// 用于移除最后一个后清空Body内容
    /// </summary>
    internal void ClearBody() => ReplaceTo(null);

    private static Widget BuildDefaultTransition(Animation<double> animation, Widget child,
        Offset fromOffset, Offset toOffset)
    {
        var offsetAnimation = new OffsetTween(fromOffset, toOffset).Animate(animation);
        return new SlideTransition(offsetAnimation) { Child = child };
    }
}