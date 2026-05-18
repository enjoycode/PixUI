using System;
using System.Collections.Generic;

namespace PixUI;

/// <summary>
/// 管理输入焦点，UIWindow及每个显示的Popup各自拥有一个实例管理各自的焦点
/// </summary>
public sealed class FocusManager
{
    internal Widget? FocusedWidget { get; private set; }

    public void Focus(Widget? widget, UIWindow window)
    {
        if (ReferenceEquals(FocusedWidget, widget))
            return; //Already focused

        var oldFocused = FocusedWidget;

        if (FocusedWidget != null)
        {
            var eventArg = new FocusChangedEvent(false, oldFocused, widget, window);
            ((IFocusable)FocusedWidget).FocusNode.RaiseFocusChanged(eventArg);
            FocusedWidget = null;
        }

        if (widget is IFocusable focusable)
        {
            FocusedWidget = widget;
            var eventArg = new FocusChangedEvent(true, oldFocused, widget, window);
            focusable.FocusNode.RaiseFocusChanged(eventArg);
        }
    }

    internal void OnKeyDown(KeyEvent e, UIWindow window)
    {
        //TODO:考虑FocusedWidget==null时且为Tab从根节点开始查找Focusable
        if (FocusedWidget == null) return;
        PropagateEvent(FocusedWidget, e,
            (w, ke) => ((IFocusable)w).FocusNode.RaiseKeyDown(ke));
        //如果是Tab键跳转至下一个Focused
        if (!e.IsHandled && e.KeyCode == Keys.Tab)
        {
            var forward = !e.Shift;
            var found = forward
                ? FindFocusableForward(FocusedWidget.Parent!, FocusedWidget)
                : FindFocusableBackward(FocusedWidget.Parent!, FocusedWidget);
            if (found != null)
                Focus(found, window);
        }
    }

    internal void OnKeyUp(KeyEvent e)
    {
        if (FocusedWidget == null) return;
        PropagateEvent(FocusedWidget, e,
            (w, ke) => ((IFocusable)w).FocusNode.RaiseKeyUp(ke));
    }

    internal void OnTextInput(string text)
    {
        var focusable = FocusedWidget as IFocusable;
        focusable?.FocusNode.RaiseTextInput(text);
    }

    private static void PropagateEvent<T>(Widget? widget, T theEvent, Action<Widget, T> handler)
        where T : PropagateEvent
    {
        while (widget != null)
        {
            if (widget is IFocusable)
            {
                handler(widget, theEvent);
                if (theEvent.IsHandled) return;
            }

            widget = widget.Parent;
        }
    }

    private struct FindFocusableForwardVisitor : IChildrenVisitor
    {
        public FindFocusableForwardVisitor(Widget? start)
        {
            _start = start;
            _hasStart = start == null;
        }

        private readonly Widget? _start;
        private bool _hasStart;
        public Widget? Found { get; private set; }

        public bool Visit(Widget c)
        {
            if (!_hasStart)
            {
                if (ReferenceEquals(c, _start))
                    _hasStart = true;
            }
            else
            {
                if (c is IFocusable)
                {
                    Found = c;
                    return true;
                }

                var childFocused = FindFocusableForward(c, null);
                if (childFocused != null)
                {
                    Found = childFocused;
                    return true;
                }
            }

            return false;
        }
    }

    internal static Widget? FindFocusableForward(Widget container, Widget? start)
    {
        //start == null 表示向下
        while (true)
        {
            var visitor = new FindFocusableForwardVisitor(start);
            container.VisitChildren(ref visitor);

            if (visitor.Found != null || start == null) return visitor.Found;
            //继续向上
            if (container.Parent != null && container.Parent is not IRootWidget)
            {
                start = container;
                container = container.Parent!;
                continue;
            }

            return null;
        }
    }

    private struct FindFocusableBackwardVisitor : IChildrenVisitor
    {
        public FindFocusableBackwardVisitor(Widget? start)
        {
            _start = start;
        }

        private readonly Widget? _start;
        public Widget? Found { get; private set; }

        public bool Visit(Widget c)
        {
            if (_start != null && ReferenceEquals(c, _start))
                return true;

            if (c is IFocusable)
            {
                Found = c; //Do not break, continue
            }
            else
            {
                var childFocused = FindFocusableForward(c, null);
                if (childFocused != null)
                {
                    Found = childFocused; //Do not break, continue
                }
            }

            return false;
        }
    }

    private static Widget? FindFocusableBackward(Widget container, Widget? start)
    {
        //start == null 表示向下
        while (true)
        {
            var visitor = new FindFocusableBackwardVisitor(start);
            container.VisitChildren(ref visitor);

            if (visitor.Found != null || start == null) return visitor.Found;
            //继续向上
            if (container.Parent != null && container.Parent is not IRootWidget)
            {
                start = container;
                container = container.Parent!;
                continue;
            }

            return null;
        }
    }
}

/// <summary>
/// 每个UIWindow对应一个实例，管理当前窗体的FocusManger
/// </summary>
internal sealed class FocusManagerStack
{
    internal FocusManagerStack()
    {
        _stack.Add(new FocusManager()); // for UIWindow
    }

    private readonly List<FocusManager> _stack = new();

    internal void Push(FocusManager manager) => _stack.Add(manager);

    internal void Remove(FocusManager manager)
    {
        if (manager == _stack[0]) throw new NotSupportedException();
        _stack.Remove(manager);
    }

    internal void Focus(Widget? widget, UIWindow window)
    {
        if (widget == null)
            return; //TODO:考虑取消最后一层的FocusedWidget

        var manager = GetFocusManagerByWidget(widget);
        manager.Focus(widget, window);
    }

    internal void OnKeyDown(KeyEvent e, UIWindow window) => GetFocusManagerWithFocused().OnKeyDown(e, window);

    internal void OnKeyUp(KeyEvent e) => GetFocusManagerWithFocused().OnKeyUp(e);

    internal void OnTextInput(string text) => GetFocusManagerWithFocused().OnTextInput(text);

    internal FocusManager GetFocusManagerByWidget(Widget widget)
    {
        var temp = widget;
        while (temp.Parent != null)
        {
            if (temp.Parent is Popup popup)
                return popup.FocusManager;
            temp = temp.Parent;
        }

        return _stack[0];
    }

    private FocusManager GetFocusManagerWithFocused()
    {
        for (var i = _stack.Count - 1; i > 0; i--)
        {
            if (_stack[i].FocusedWidget != null)
                return _stack[i];
        }

        return _stack[0];
    }
}