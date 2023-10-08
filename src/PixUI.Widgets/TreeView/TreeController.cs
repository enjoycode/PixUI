using System;
using System.Collections.Generic;

namespace PixUI;

public sealed class TreeController<T> //: IStateBindable
{
    public TreeController(Action<T, TreeNode<T>> nodeBuilder,
        Func<T, IList<T>> childrenGetter)
    {
        NodeBuilder = nodeBuilder;
        ChildrenGetter = childrenGetter;
    }

    internal TreeView<T>? TreeView { get; private set; }
    internal readonly Action<T, TreeNode<T>> NodeBuilder;
    internal readonly Func<T, IList<T>> ChildrenGetter;
    internal readonly List<TreeNode<T>> Nodes = new List<TreeNode<T>>();
    internal readonly ScrollController ScrollController = new(ScrollDirection.Both);

    /// <summary>
    /// 获取根节点只读列表
    /// </summary>
    public TreeNode<T>[] RootNodes => Nodes.ToArray();

    #region ----Selection----

    private readonly List<TreeNode<T>> _selectedNodes = new List<TreeNode<T>>();

    /// <summary>
    /// 第一个选中的节点
    /// </summary>
    public TreeNode<T>? FirstSelectedNode =>
        _selectedNodes.Count > 0 ? _selectedNodes[0] : null;

    public TreeNode<T>[] SelectedNodes => _selectedNodes.ToArray();

    public event Action? SelectionChanged;

    #endregion

    internal Color HoverColor = new Color(0xFFAAAAAA); //TODO: use Theme.HoverColor
    internal float NodeIndent = 20;
    internal float NodeHeight;
    internal float TotalWidth = 0;
    internal float TotalHeight = 0;

    #region ----Checkbox----

    internal bool ShowCheckbox;

    internal bool SuspendAutoCheck;

    public event Action<TreeNode<T>>? CheckChanged;

    internal void RaiseCheckChanged(TreeNode<T> node) => CheckChanged?.Invoke(node);

    #endregion

    #region ----Loading----

    private bool _isLoading = false;
    internal CircularProgressPainter? LoadingPainter { get; private set; }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading == value) return;
            _isLoading = value;
            if (_isLoading)
            {
                LoadingPainter = new CircularProgressPainter();
                LoadingPainter.Start(() => TreeView?.Invalidate(InvalidAction.Repaint));
            }
            else
            {
                LoadingPainter?.Stop();
                LoadingPainter?.Dispose();
                LoadingPainter = null;
            }

            TreeView?.Invalidate(InvalidAction.Repaint);
        }
    }

    #endregion

    private IList<T>? _dataSource;

    public IList<T>? DataSource
    {
        get => _dataSource;
        set
        {
            _dataSource = value;
            // if (DataSource is RxList<T> rxList)
            //     rxList.AddBinding(this, BindingOptions.None);
            if (TreeView != null)
            {
                Nodes.Clear();
                InitNodes(TreeView);
                if (TreeView.IsMounted)
                    TreeView.Invalidate(InvalidAction.Relayout);
            }
        }
    }

    internal void InitNodes(TreeView<T> treeView)
    {
        TreeView = treeView;
        if (_dataSource == null) return;

        foreach (var item in _dataSource)
        {
            var node = new TreeNode<T>(item, this);
            NodeBuilder(item, node);
            node.TryBuildCheckbox();
            node.Parent = treeView;
            Nodes.Add(node);
        }
    }

    //public void OnStateChanged(StateBase state, BindingOptions options)
    //{
    //    throw new NotImplementedException();
    //}

    #region ====Operations====

    public TreeNode<T>? FindNode(Predicate<T> predicate)
    {
        foreach (var child in Nodes)
        {
            var found = child.FindNode(predicate);
            if (found != null) return found;
        }

        return null;
    }

    /// <summary>
    /// 选中单个节点
    /// </summary>
    public void SelectNode(TreeNode<T> node)
    {
        //是否已经选择
        if (_selectedNodes.Count == 1 && ReferenceEquals(_selectedNodes[0], node))
            return;

        //取消旧的选择并选择新的
        foreach (var oldSelectedNode in _selectedNodes)
        {
            oldSelectedNode.IsSelected.Value = false;
        }

        _selectedNodes.Clear();

        _selectedNodes.Add(node);
        node.IsSelected.Value = true;

        SelectionChanged?.Invoke();
    }

    public void ExpandTo(TreeNode<T> node)
    {
        var temp = node.Parent;
        while (temp != null && !ReferenceEquals(temp, TreeView))
        {
            var tempNode = (TreeNode<T>)temp;
            tempNode.Expand();
            temp = tempNode.Parent;
        }

        //TODO: scroll to
    }

    public TreeNode<T> InsertNode(T child, TreeNode<T>? parentNode = null, int insertIndex = -1)
    {
        var node = new TreeNode<T>(child, this);
        NodeBuilder(child, node);
        if (parentNode == null)
        {
            node.Parent = TreeView;
            var index = insertIndex < 0 ? Nodes.Count : insertIndex;
            Nodes.Insert(index, node);
            DataSource!.Insert(index, child);
            //强制重新布局
            TreeView!.Invalidate(InvalidAction.Relayout);
        }
        else
        {
            node.Parent = parentNode;
            parentNode.InsertChild(insertIndex, node);
            //强制重新布局
            if (parentNode.IsExpanded)
                parentNode.Invalidate(InvalidAction.Relayout);
        }

        return node;
    }

    public void RemoveNode(TreeNode<T> node)
    {
        if (ReferenceEquals(node.Parent, TreeView))
        {
            Nodes.Remove(node);
            DataSource!.Remove(node.Data);
            node.Parent = null;
            //强制重新布局
            TreeView!.Invalidate(InvalidAction.Relayout);
        }
        else
        {
            var parentNode = (TreeNode<T>)node.Parent!;
            parentNode.RemoveChild(node);
            node.Parent = null;
            //强制重新布局
            if (parentNode.IsExpanded)
                parentNode.Invalidate(InvalidAction.Relayout);
        }

        //如果是选择的，则清除
        var selectedAt = _selectedNodes.IndexOf(node);
        if (selectedAt >= 0)
        {
            _selectedNodes.RemoveAt(selectedAt);
            SelectionChanged?.Invoke();
        }
    }

    public void SetChecked(T data, bool value)
    {
        var node = FindNode(t => EqualityComparer<T>.Default.Equals(data, t));
        if (node == null)
            throw new Exception("Can't find node");

        node.SetChecked(value);
    }

    #endregion
}