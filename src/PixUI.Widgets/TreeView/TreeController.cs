using System;
using System.Collections.Generic;

namespace PixUI;

public delegate void TreeNodeBuilder<T>(TreeNode<T> node);

public delegate IList<T> TreeChildrenGetter<T>(T data);

public sealed class TreeController<T>
{
    internal TreeView<T>? TreeView { get; private set; }
    internal TreeNodeBuilder<T> NodeBuilder { get; set; } = null!;
    internal TreeChildrenGetter<T> ChildrenGetter { get; set; } = null!;
    internal readonly List<TreeNode<T>> Nodes = new();
    internal readonly ScrollController ScrollController = new(ScrollDirection.Both);

    internal Color HoverColor = 0xFFAAAAAA; //TODO: use Theme.HoverColor
    internal float NodeIndent = 20;
    internal float NodeHeight;
    internal float TotalWidth = 0;
    internal float TotalHeight = 0;

    /// <summary>
    /// 获取根节点只读列表
    /// </summary>
    public TreeNode<T>[] RootNodes => Nodes.ToArray();

    internal void AttachTreeView(TreeView<T> treeView)
    {
        if (TreeView != null) throw new Exception("Can't attach twice");
        TreeView = treeView;
    }

    #region ----Selection----

    private readonly List<TreeNode<T>> _selectedNodes = new();

    /// <summary>
    /// 第一个选中的节点
    /// </summary>
    public TreeNode<T>? FirstSelectedNode => _selectedNodes.Count > 0 ? _selectedNodes[0] : null;

    public TreeNode<T>[] SelectedNodes => _selectedNodes.ToArray();

    public event Action? SelectionChanged;

    #endregion

    #region ----Checkbox----

    internal bool ShowCheckbox;

    internal bool SuspendAutoCheck;

    public event Action<TreeNode<T>>? CheckChanged;

    internal void RaiseCheckChanged(TreeNode<T> node) => CheckChanged?.Invoke(node);

    #endregion

    #region ----Loading----

    private bool _isLoading;
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
                LoadingPainter.Start(() => TreeView?.Repaint());
            }
            else
            {
                LoadingPainter?.Stop();
                LoadingPainter?.Dispose();
                LoadingPainter = null;
            }

            TreeView?.Repaint();
        }
    }

    #endregion

    #region ----DataSource----

    private IList<T>? _dataSource;

    public IList<T>? DataSource
    {
        get => _dataSource;
        set
        {
            _dataSource = value;
            // if (DataSource is RxList<T> rxList)
            //     rxList.AddBinding(this, BindingOptions.None);

            _selectedNodes.Clear();
            Nodes.Clear(); //清除旧的节点
            TreeView?.Relayout();
        }
    }

    internal void TryBuildNodes()
    {
        if (_dataSource == null || _dataSource.Count == 0) return;
        if (Nodes.Count != 0) return; //has build
        
        foreach (var item in _dataSource)
        {
            var node = new TreeNode<T>(item, this);
            NodeBuilder(node);
            node.TryBuildCheckbox();
            node.Parent = TreeView;
            Nodes.Add(node);

            if (node.IsSelected.Value)
                SelectNode(node); //TODO:临时方案用于默认选中(第一个)节点
        }
    }

    #endregion

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
        NodeBuilder(node);
        if (parentNode == null)
        {
            node.Parent = TreeView;
            var index = insertIndex < 0 ? Nodes.Count : insertIndex;
            Nodes.Insert(index, node);
            DataSource!.Insert(index, child);
            //强制重新布局
            TreeView!.Relayout();
        }
        else
        {
            node.Parent = parentNode;
            parentNode.InsertChild(insertIndex, node);
            //强制重新布局
            if (parentNode.IsExpanded)
                parentNode.Relayout();
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
            TreeView!.Relayout();
        }
        else
        {
            var parentNode = (TreeNode<T>)node.Parent!;
            parentNode.RemoveChild(node);
            node.Parent = null;
            //强制重新布局
            if (parentNode.IsExpanded)
                parentNode.Relayout();
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