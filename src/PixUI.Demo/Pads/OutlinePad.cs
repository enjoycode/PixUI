using System.Collections.Generic;

namespace PixUI.Demo;

internal sealed class OutlinePad : View
{
    public OutlinePad(PlaygroundContext context)
    {
        _context = context;
        _treeController.SelectionChanged += OnSelectedWidget;
        BuildWidgetTree();

        Child = new Column
        {
            Children =
            [
                new TextInput(_searchKey) { Prefix = new Icon(MaterialIcons.Search) },
                new TreeView<WidgetTreeNode>(_treeController, BuildTreeNode, n => n.Children)
            ]
        };
    }

    private readonly PlaygroundContext _context;
    private readonly TreeController<WidgetTreeNode> _treeController = new();
    private readonly State<string> _searchKey = "";
    private Inspector? _inspector;

    protected override void OnMounted()
    {
        _context.PreviewController.RefreshOutlineAction = BuildWidgetTree;
    }

    protected override void OnUnmounted()
    {
        ClearInspector();
        _context.PreviewController.RefreshOutlineAction = null;
    }

    private void BuildWidgetTree()
    {
        ClearInspector();
        _treeController.DataSource = new List<WidgetTreeNode>() { _context.PreviewController.GetWidgetTree()! };
    }

    private static void BuildTreeNode(TreeNode<WidgetTreeNode> node)
    {
        var data = node.Data;
        node.Icon = new Icon(MaterialIcons.Folder);
        node.Label = new Text(data.Widget.ToString());
        node.IsLeaf = data.Children.Count == 0;
        node.IsExpanded = !node.IsLeaf;
    }

    private void ClearInspector()
    {
        _inspector?.Remove();
        _inspector = null;
    }

    private void OnSelectedWidget()
    {
        if (_treeController.FirstSelectedNode == null)
        {
            ClearInspector();
            return;
        }

        _inspector = Inspector.Show(_treeController.FirstSelectedNode.Data.Widget);
    }
}