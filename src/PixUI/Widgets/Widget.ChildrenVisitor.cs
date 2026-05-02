using System;

namespace PixUI;

public interface IChildrenVisitor
{
    /// <summary>
    /// 访问子组件
    /// </summary>
    /// <param name="child"></param>
    /// <returns>true=停止继续访问子组件</returns>
    bool Visit(Widget child);
}

partial class Widget
{
    private struct IndexOfChildrenVisitor : IChildrenVisitor
    {
        public IndexOfChildrenVisitor(Widget child)
        {
            _target = child;
        }

        private readonly Widget _target;
        public int Index { get; private set; } = -1;
        private int _at = -1;

        public bool Visit(Widget child)
        {
            _at++;
            if (ReferenceEquals(child, _target))
            {
                Index = _at;
                return true;
            }

            return false;
        }
    }

    private readonly struct SetVisibleChildrenVisitor : IChildrenVisitor
    {
        public SetVisibleChildrenVisitor(bool visible)
        {
            _visible = visible;
        }

        private readonly bool _visible;

        public bool Visit(Widget child)
        {
            child.SetVisibleWithChildren(_visible);
            return false;
        }
    }

    private readonly struct MountChildrenVisitor : IChildrenVisitor
    {
        public bool Visit(Widget child)
        {
            child.Mount();
            return false;
        }
    }

    private readonly struct UnmountChildrenVisitor : IChildrenVisitor
    {
        public bool Visit(Widget child)
        {
            child.Unmount();
            return false;
        }
    }

    internal readonly struct HitTestChildrenVisitor : IChildrenVisitor
    {
        public HitTestChildrenVisitor(Widget parent, float x, float y, HitTestResult result)
        {
            _parent = parent;
            _x = x;
            _y = y;
            _result = result;
        }

        private readonly Widget _parent;
        private readonly float _x;
        private readonly float _y;
        private readonly HitTestResult _result;

        public bool Visit(Widget child) => _parent.HitTestChild(child, _x, _y, _result);
    }

    private struct LayoutChildrenVisitor : IChildrenVisitor
    {
        public LayoutChildrenVisitor(Widget parent, Size maxSize)
        {
            _parent = parent;
            _maxSize = maxSize;
        }

        private readonly Widget _parent;
        private readonly Size _maxSize;
        public bool HasChildren { get; private set; }

        public bool Visit(Widget child)
        {
            HasChildren = true;
            child.Layout(_maxSize.Width, _maxSize.Height);
            _parent.SetSize(Math.Max(_parent.W, child.W), Math.Max(_parent.H, child.H));
            return false;
        }
    }

    internal readonly struct PaintChildrenVisitor : IChildrenVisitor
    {
        public PaintChildrenVisitor(Canvas canvas, IDirtyArea? dirtyArea)
        {
            _canvas = canvas;
            _dirtyArea = dirtyArea;
        }

        private readonly Canvas _canvas;
        private readonly IDirtyArea? _dirtyArea;

        public bool Visit(Widget child)
        {
            PaintChild(child, _canvas, _dirtyArea);
            return false;
        }
    }
}