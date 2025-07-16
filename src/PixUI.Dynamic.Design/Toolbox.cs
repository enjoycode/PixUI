using System.Collections.Generic;
using System.Linq;

namespace PixUI.Dynamic.Design;

public sealed class Toolbox : View
{
    public Toolbox(DesignController designController)
    {
        _designController = designController;
        _treeController.SelectionChanged += OnSelectionChanged;

        BuildTreeDataSource();

        Child = new Column
        {
            Children =
            {
                new TextInput(_searchKey) { Suffix = new Icon(MaterialIcons.Search) },
                new TreeView<ToolboxNode>(_treeController, BuildTreeNode, data => data.Children!)
                {
                    AllowDrag = true,
                    OnAllowDrag = OnAllowDrag,
                }
            }
        };
    }

    private readonly DesignController _designController;
    private readonly State<string> _searchKey = string.Empty;
    private readonly TreeController<ToolboxNode> _treeController = new();

    private static void BuildTreeNode(TreeNode<ToolboxNode> node)
    {
        var data = node.Data;
        node.Label = new Text(data.IsCatalog ? data.CatalogName! : data.DynamicWidgetMeta!.Name);
        node.Icon = data.IsCatalog ? new(MaterialIcons.Folder) : new(data.DynamicWidgetMeta!.Icon);
        node.IsLeaf = !data.IsCatalog;
        node.IsExpanded = true;
    }

    private void BuildTreeDataSource()
    {
        var all = DynamicWidgetManager.GetAll()
            .Where(t => t.ShowOnToolbox)
            .GroupBy(w => w.Catalog);

        var treeList = new List<ToolboxNode>();
        foreach (var group in all)
        {
            var groupIndex = treeList.FindIndex(n => n.CatalogName == group.Key);
            if (groupIndex < 0)
            {
                treeList.Add(new ToolboxNode(group.Key));
                groupIndex = treeList.Count - 1;
            }

            foreach (var meta in group)
            {
                treeList[groupIndex].Children!.Add(new ToolboxNode(meta));
            }
        }

        _treeController.DataSource = treeList;
    }

    public void Rebuild() => BuildTreeDataSource();

    private static bool OnAllowDrag(TreeNode<ToolboxNode> node) => !node.Data.IsCatalog;

    private void OnSelectionChanged()
    {
        _designController.CurrentToolboxItem = _treeController.FirstSelectedNode?.Data.DynamicWidgetMeta;
    }
}

public sealed class ToolboxNode
{
    public ToolboxNode(string catalogName)
    {
        CatalogName = catalogName;
        Children = new List<ToolboxNode>();
    }

    public ToolboxNode(DynamicWidgetMeta widgetMeta)
    {
        DynamicWidgetMeta = widgetMeta;
    }

    public string? CatalogName { get; }
    public IList<ToolboxNode>? Children { get; }

    public DynamicWidgetMeta? DynamicWidgetMeta { get; }

    public bool IsCatalog => CatalogName != null;
}