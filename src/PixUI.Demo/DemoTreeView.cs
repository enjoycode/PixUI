using System.Collections.Generic;

namespace PixUI.Demo.Mac;

public sealed class DemoTreeView : View
{
    private readonly List<TreeData> _treeDataSource = new()
    {
        new TreeData
        {
            Icon = MaterialIcons.Cloud, Text = "Cloud", Children = new List<TreeData>
            {
                new() { Icon = MaterialIcons.Train, Text = "Train" },
                new() { Icon = MaterialIcons.AirplanemodeOn, Text = "AirPlane" },
            }
        },
        new TreeData
        {
            Icon = MaterialIcons.BeachAccess, Text = "Beach",
            Children = new List<TreeData>
            {
                new()
                {
                    Icon = MaterialIcons.Cake, Text = "Cake", Children = new List<TreeData>
                    {
                        new() { Icon = MaterialIcons.Apple, Text = "Apple Long Long" },
                        new() { Icon = MaterialIcons.Adobe, Text = "Adobe" },
                    }
                },
                new() { Icon = MaterialIcons.Camera, Text = "Camera Long" },
            }
        },
        new TreeData { Icon = MaterialIcons.Sunny, Text = "Sunny" }
    };

    private readonly TreeController<TreeData> _treeController1;
    private bool _loading;
    private readonly TreeController<TreeData> _treeController2;

    public DemoTreeView()
    {
        _treeController1 = new TreeController<TreeData> { DataSource = _treeDataSource };
        _treeController2 = new TreeController<TreeData> { DataSource = _treeDataSource };

        Child = new Container()
        {
            Padding = EdgeInsets.All(20),
            Child = new Column()
            {
                Spacing = 10,
                Children =
                {
                    new Row(VerticalAlignment.Middle, 20)
                    {
                        Children =
                        {
                            new Button("Insert") { OnTap = OnInsert },
                            new Button("Remove") { OnTap = OnRemove },
                            new Button("Loading") { OnTap = OnSwitchLoading },
                            new Button("Check") { OnTap = OnCheck }
                        }
                    },
                    new Expanded()
                    {
                        Child = new Splitter
                        {
                            Panel1 = new Container()
                            {
                                // Padding = EdgeInsets.All(10),
                                Child = new TreeView<TreeData>(_treeController1, BuildTreeNode,
                                    d => d.Children!)
                                {
                                    AllowDrag = true,
                                    AllowDrop = true,
                                    OnAllowDrag = OnAllowDrag,
                                    OnAllowDrop = OnAllowDrop,
                                    OnDrop = OnDrop,
                                    FillColor = new Color(0xFFDCDCDC),
                                }
                            },
                            Panel2 = new Center() { Child = new Text("Placeholder") }
                            // Panel2 = new TreeView<TreeData>(_treeController2, BuildTreeNode,
                            //     d => d.Children!, true)
                            // {
                            //     FillColor = new Color(0xFFDCDCDC),
                            // }
                        },
                    }
                }
            }
        };
    }

    private void BuildTreeNode(TreeNode<TreeData> node)
    {
        var data = node.Data;
        node.Icon = new Icon(data.Icon);
        node.Label = new Text(data.Text);
        node.IsLeaf = data.Children == null;
        node.IsExpanded = data.Text == "Cloud";
    }

    private static bool OnAllowDrag(TreeNode<TreeData> source)
    {
        if (source.Data.Text == "Sunny") return false;
        return true;
    }

    private static bool OnAllowDrop(TreeNode<TreeData> target, DragEvent e)
    {
        var source = e.TransferItem as TreeNode<TreeData>;
        if (source == null) return false;
        if (source.TreeView != target.TreeView) return false;

        if (target.ParentNode != null && target.ParentNode.Data.Text == "Cake")
        {
            if (e.DropPosition == DropPosition.In)
                return false;
        }

        return true;
    }

    private void OnDrop(TreeNode<TreeData> target, DragEvent e)
    {
        var source = (TreeNode<TreeData>)e.TransferItem;
        switch (e.DropPosition)
        {
            case DropPosition.In:
                _treeController1.RemoveNode(source);
                ToNoneLeafNode(target);
                _treeController1.InsertNode(source.Data, target);
                break;
            case DropPosition.Upper:
                _treeController1.RemoveNode(source);
                var parent = target.ParentNode;
                var index = target.Index;
                _treeController1.InsertNode(source.Data, parent, index);
                break;
            default: break;
        }
    }

    private static void ToNoneLeafNode(TreeNode<TreeData> node)
    {
        if (!node.IsLeaf) return;
        node.IsLeaf = false;
        node.Data.Children = new List<TreeData>();
        node.EnsureBuildChildren();
    }

    private void OnInsert(PointerEvent e)
    {
        var parentNode = _treeController1.FindNode(t => t.Text == "Cake");
        var childNode = _treeController1.InsertNode(
            new TreeData() { Icon = MaterialIcons.Start, Text = "AppBox" }, parentNode, 1);
        _treeController1.ExpandTo(childNode);
        _treeController1.SelectNode(childNode);
    }

    private void OnRemove(PointerEvent e)
    {
        var node = _treeController1.FindNode(t => t.Text == "AppBox");
        if (node != null)
            _treeController1.RemoveNode(node);
    }

    private void OnSwitchLoading(PointerEvent e)
    {
        _loading = !_loading;
        _treeController1.IsLoading = _loading;
    }

    private void OnCheck(PointerEvent e)
    {
        var node = _treeController2.FindNode(t => t.Text == "Cake");
        if (node != null)
            _treeController2.SetChecked(node.Data, true);
    }
}

internal sealed class TreeData
{
    public IconData Icon;
    public string Text;
    public List<TreeData>? Children;
}