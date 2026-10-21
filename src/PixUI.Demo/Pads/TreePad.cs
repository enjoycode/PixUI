using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PixUI.Demo;

internal sealed class TreePad : View
{
    public TreePad(PlaygroundContext context)
    {
        _context = context;

        Child = new Column
        {
            Children =
            {
                new TextInput(_searchKey) { Prefix = new Icon(MaterialIcons.Search) },
                new TreeView<DemoNode>(_context.TreeController, BuildTreeNode, n => n.Children),
            }
        };
    }

    private readonly PlaygroundContext _context;
    private readonly State<string> _searchKey = "";

    private static void BuildTreeNode(TreeNode<DemoNode> node)
    {
        var data = node.Data;
        node.Icon = new Icon(node.Data.IsLeaf ? MaterialIcons.Code : MaterialIcons.Folder);
        node.Label = new Text(data.Name);
        node.IsLeaf = node.Data.IsLeaf;
        node.IsExpanded = !node.Data.IsLeaf;
    }

    protected override void OnMounted()
    {
        base.OnMounted();
        BuildTree();
    }

    private void BuildTree()
    {
        var prefix = "PixUI.Demo.Resources.Demos.";
        var folders = new List<DemoNode>();
        var spans = new Range[3];
        var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        foreach (var resource in resources)
        {
            if (!resource.StartsWith(prefix) || !resource.EndsWith(".cs")) continue;

            var path = resource.AsSpan(prefix.Length);
            var count = path.Split(spans, '.');
            if (count != 3) continue;

            var folder = path[spans[0]].ToString();
            var nameSpan = path[spans[1]];
            if (!nameSpan.StartsWith("Demo")) continue;
            var name = nameSpan[4..].ToString();

            var folderNode = folders.FirstOrDefault(f => f.Name == folder);
            if (folderNode == null)
            {
                folderNode = new DemoNode() { Name = folder, Children = [] };
                folders.Add(folderNode);
            }

            var demoNode = new DemoNode() { Name = name, Children = [] };
            folderNode.Children.Add(demoNode);
        }

        _context.TreeController.DataSource = folders;
    }
}