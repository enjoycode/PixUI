using System;
using System.Collections.Generic;

namespace PixUI.Demo;

internal sealed class WidgetTreeNode
{
    public WidgetTreeNode(Widget widget)
    {
        Widget = widget;
        Children = new List<WidgetTreeNode>();
        var visitor = new WidgetTreeChildrenVisitor(Children);
        Widget.VisitChildren(ref visitor);
    }

    public readonly Widget Widget;
    public readonly List<WidgetTreeNode> Children;

    private readonly struct WidgetTreeChildrenVisitor : IChildrenVisitor
    {
        public WidgetTreeChildrenVisitor(List<WidgetTreeNode> children)
        {
            _children = children;
        }

        private readonly List<WidgetTreeNode> _children;

        public bool Visit(Widget child)
        {
            _children.Add(new WidgetTreeNode(child));
            return false;
        }
    }
}

internal sealed class PreviewController
{
    private Action<bool>? _invalidateAction;
    internal Widget? CurrentWidget; //当前加载的预览的Widget实例

    /// <summary>
    /// 用于重新加载预览
    /// </summary>
    internal Action<bool> InvalidateAction
    {
        set => _invalidateAction = value;
    }

    /// <summary>
    /// 用于重新加载大纲视图
    /// </summary>
    internal Action? RefreshOutlineAction { get; set; }

    public void Invalidate(bool show) => _invalidateAction?.Invoke(show);

    public WidgetTreeNode? GetWidgetTree() => CurrentWidget == null ? null : new WidgetTreeNode(CurrentWidget);
}