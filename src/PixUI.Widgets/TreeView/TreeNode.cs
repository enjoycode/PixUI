using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PixUI;

public sealed class TreeNode<T> : Widget
{
    internal TreeNode(T data, TreeController<T> controller)
    {
        Data = data;
        _controller = controller;
        _row = new TreeNodeRow<T>();
        _row.Parent = this;

        _color = IsSelected.ToComputed(s => s ? Theme.FocusedColor : Colors.Black); //TODO: fix color

        IsSelected.AddListener(RepaintOnStateChanged);
    }

    #region ====Fields & Properties====

    public readonly T Data;
    private readonly TreeController<T> _controller;
    internal TreeController<T> Controller => _controller;

    private readonly TreeNodeRow<T> _row;
    private List<TreeNode<T>>? _children;
    public TreeNode<T>[]? Children => _children?.ToArray();

    public readonly State<bool> IsSelected = false;
    private readonly State<Color> _color; //for icon and label

    private State<bool?>? _checkState;

    private AnimationController? _expandController; //TODO:考虑提升至TreeController共用实例
    private Animation<double>? _expandCurve;
    private Animation<float>? _expandArrowAnimation;

    public Icon Icon
    {
        set
        {
            _row.Icon = value;
            _row.Icon.Color ??= _color;
        }
    }

    public Text Label
    {
        set
        {
            _row.Label = value;
            _row.Label.TextColor ??= _color;
        }
    }

    public bool IsLeaf { get; set; }
    public bool IsLazyLoad { get; set; }
    public bool IsExpanded { get; set; }


    private int _animationFlag; //0=none,1=expand,-1=collapse
    private double _animationValue;
    private bool IsExpanding => _animationFlag == 1;
    private bool IsCollapsing => _animationFlag == -1;

    internal int Depth
    {
        get
        {
            Widget temp = this;
            var depth = 0;
            while (true)
            {
                if (temp.Parent is TreeView<T>)
                    break;
                depth++;
                temp = temp.Parent!;
            }

            return depth;
        }
    }

    public TreeNode<T>? ParentNode => Parent as TreeNode<T>;

    #endregion

    #region ====Expand & Collapse=====

    private void TryBuildExpandIcon()
    {
        if (_expandController != null) return;

        _expandController = new AnimationController(200, IsExpanded ? 1 : 0);
        _expandController.ValueChanged += OnAnimationValueChanged;
        _expandCurve = new CurvedAnimation(_expandController, Curves.EaseInOutCubic);
        _expandArrowAnimation = new FloatTween(0.75f, 1.0f).Animate(_expandCurve);

        var expander = new ExpandIcon(_expandArrowAnimation);
        expander.OnPointerDown = OnTapExpander;
        _row.ExpandIcon = expander;
    }

    private void OnAnimationValueChanged()
    {
        _animationValue = _expandController!.Value;
        Relayout(); //自身改变高度并通知上级
    }

    /// <summary>
    /// 确保构建子节点
    /// </summary>
    public void EnsureBuildChildren()
    {
        if (IsLeaf || _children != null) return;

        var childrenList = _controller.ChildrenGetter(Data);
        _children = new List<TreeNode<T>>(childrenList.Count);
        foreach (var child in childrenList)
        {
            var node = new TreeNode<T>(child, _controller);
            _controller.NodeBuilder(node);
            node.TryBuildCheckbox();
            node.Parent = this;
            _children.Add(node);
        }
    }

    /// <summary>
    /// 尝试构建并布局子级节点
    /// </summary>
    /// <returns>返回最宽的子级节点的宽度</returns>
    private float TryBuildAndLayoutChildren()
    {
        if (_children != null && HasLayout && _children.All(t => t.HasLayout))
        {
            return TreeView<T>.CalcMaxChildWidth(_children);
        }

        EnsureBuildChildren();

        var maxWidth = 0f;
        var yPos = _controller.NodeHeight;
        for (var i = 0; i < _children!.Count; i++)
        {
            var node = _children[i];
            node.Layout(float.PositiveInfinity, float.PositiveInfinity);
            node.SetPosition(0, yPos);
            yPos += node.H;
            maxWidth = Math.Max(maxWidth, node.W);
        }

        return maxWidth;
    }

    private void OnTapExpander(PointerEvent e)
    {
        //TODO:先判断是否LazyLoad，是则异步加载后再处理

        if (IsExpanded)
        {
            IsExpanded = false;
            _animationFlag = -1;
            _expandController?.Reverse();
        }
        else
        {
            var maxChildWidth = TryBuildAndLayoutChildren(); //先尝试布局子节点
            SetSize(Math.Max(W, maxChildWidth), H); //仅预设宽度

            IsExpanded = true;
            _animationFlag = 1;
            _expandController?.Forward();
        }
    }

    #endregion

    #region ====Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (action(_row)) return;

        if (!IsLeaf && IsExpanded && _children != null)
        {
            foreach (var child in _children)
            {
                if (action(child)) break;
            }
        }
    }

    public override bool ContainsPoint(float x, float y) => y >= 0 && y < H;

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        //只需要判断坐标Y来确定是否命中行
        if (y < 0 || y > H) return false;

        result.Add(this);

        //先判断是否命中TreeNodeRow
        if (y <= _controller.NodeHeight)
        {
            HitTestChild(_row, x, y, result);
        }
        else
        {
            if (!IsLeaf && _children != null)
            {
                foreach (var child in _children)
                {
                    if (HitTestChild(child, x, y, result))
                        break;
                }
            }
        }

        return true;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        //先处理是否由动画引起的高度改变
        if (IsExpanding || IsCollapsing)
        {
            //根据动画值计算需要展开的高度
            var totalChildrenHeight = _children!.Sum(t => t.H);
            var expandedHeight = (float)(totalChildrenHeight * _animationValue);
            if (IsCollapsing && _animationValue == 0) //已收缩需要恢复本身的宽度
            {
                _animationFlag = 0;
                SetSize(_row.W, _controller.NodeHeight);
            }
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            else if (IsExpanding && _animationValue == 1)
            {
                _animationFlag = 0;
                SetSize(W, _controller.NodeHeight + totalChildrenHeight); //宽度之前已预设
            }
            else
            {
                SetSize(W, _controller.NodeHeight + expandedHeight); //宽度之前已预设
            }

            return;
        }

        if (HasLayout) return;

        // try build expand icon
        if (!IsLeaf)
            TryBuildExpandIcon();

        _row.Layout(float.PositiveInfinity, Controller.NodeHeight);

        if (IsLeaf || !IsExpanded)
        {
            SetSize(_row.W, _controller.NodeHeight);
            HasLayout = true;
            return;
        }

        // expanded, continue build and layout children
        if (!IsLeaf && IsExpanded)
        {
            var maxChildWidth = TryBuildAndLayoutChildren();
            SetSize(Math.Max(_row.W, maxChildWidth),
                _controller.NodeHeight + _children!.Sum(t => t.H));
            HasLayout = true;
        }
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        var oldWidth = W;
        var oldHeight = H;
        affects.Widget = this;
        affects.OldX += X;
        affects.OldY += Y;

        //更新后续子节点的Y坐标
        if (dy != 0 && _children != null)
            TreeView<T>.UpdatePositionAfter(child, _children, dy);

        //更新自身的宽高
        var newWidth = oldWidth;
        var newHeight = oldHeight + dy;
        if (dx > 0)
        {
            //宽度增加，总宽取现值及当前的大者
            newWidth = Math.Max(child.W, W);
        }
        else if (dx < 0 && child.W - dx == _controller.TotalWidth)
        {
            //宽度减小，且原本是最宽的那个, 重新计算最宽的子节点
            if (_children == null)
                newWidth = child.W;
            else
                newWidth = Math.Max(TreeView<T>.CalcMaxChildWidth(_children), W);
        }

        SetSize(newWidth, newHeight);

        //继续通知上级节点尺寸变更
        Parent!.OnChildSizeChanged(this, newWidth - oldWidth, newHeight - oldHeight, affects);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (IsExpanding || IsCollapsing) //need clip expanding area
        {
            canvas.Save();
            canvas.ClipRect(Rect.FromLTWH(0, 0, _controller.TreeView!.W, H), ClipOp.Intersect, false);
        }

        _row.Paint(canvas, area);

        if (IsExpanding || IsCollapsing)
        {
            for (var i = 0; i < _children!.Count; i++)
            {
                PaintChildNode(_children[i], canvas, area);
                if ((i + 1) * _controller.NodeHeight >= H)
                    break;
            }

            canvas.Restore();
        }
        else if (!IsLeaf && IsExpanded)
        {
            foreach (var child in _children!)
            {
                PaintChildNode(child, canvas, area);
            }
        }
    }

    private static void PaintChildNode(Widget child, Canvas canvas, IDirtyArea? area)
    {
        //TODO:判断是否可见
        canvas.Translate(child.X, child.Y);
        child.Paint(canvas, area?.ToChild(child));
        canvas.Translate(-child.X, -child.Y);

        PaintDebugger.PaintWidgetBorder(child, canvas);
    }

    public override string ToString()
    {
        var labelText = _row.Label == null ? "" : _row.Label.Text.Value;
        return $"TreeNode[\"{labelText}\"]";
    }

    #endregion

    #region ====Checkbox====

    internal void TryBuildCheckbox()
    {
        if (!Controller.ShowCheckbox) return;

        _checkState = new RxValue<bool?>(false);
        var checkbox = Checkbox.Tristate(_checkState);
        checkbox.ValueChanged += OnCheckChanged;
        _row.Checkbox = checkbox;
    }

    private void OnCheckChanged(bool? value)
    {
        //Auto check children and parent
        if (!Controller.SuspendAutoCheck)
        {
            Controller.SuspendAutoCheck = true;
            //auto check parent first
            AutoCheckParent(ParentNode);
            //auto check children
            AutoCheckChildren(this, value);
            Controller.SuspendAutoCheck = false;
        }

        //Raise CheckChanged event
        Controller.RaiseCheckChanged(this);
    }

    private static void AutoCheckParent<TNode>(TreeNode<TNode>? parent)
    {
        if (parent == null) return;

        var allChecked = true;
        var allUnchecked = true;

        foreach (var child in parent._children!)
        {
            if (child._checkState!.Value == null)
            {
                allChecked = false;
                allUnchecked = false;
                break;
            }
            else if (child._checkState.Value == true)
            {
                allUnchecked = false;
            }
            else
            {
                allChecked = false;
            }
        }

        bool? newValue = null;
        if (allChecked)
            newValue = true;
        else if (allUnchecked)
            newValue = false;

        parent._checkState!.Value = newValue;

        AutoCheckParent(parent.ParentNode);
    }

    private static void AutoCheckChildren<TNode>(TreeNode<TNode> node, bool? newValue)
    {
        Debug.Assert(newValue.HasValue);

        if (node.IsLeaf) return;

        node.EnsureBuildChildren(); //maybe not build

        if (node._children != null && node._children.Count > 0)
        {
            foreach (var child in node._children)
            {
                child._checkState!.Value = newValue;
                AutoCheckChildren(child, newValue);
            }
        }
    }

    internal void SetChecked(bool value)
    {
        if (!Controller.ShowCheckbox)
            throw new InvalidOperationException("Not supported");
        _checkState!.Value = value;
    }

    public bool? CheckState => _checkState?.Value;

    #endregion

    #region ====Operations====

    public int IndexOf(TreeNode<T> child)
    {
        if (_children != null)
            return _children.IndexOf(child);
        return -1;
    }
    
    internal TreeNode<T>? FindNode(Predicate<T> predicate)
    {
        if (predicate(Data)) return this;

        if (!IsLeaf)
        {
            EnsureBuildChildren(); //可能收缩中还没有构建子节点

            foreach (var child in _children!)
            {
                var found = child.FindNode(predicate);
                if (found != null) return found;
            }
        }

        return null;
    }

    internal void Expand()
    {
        if (IsLeaf || IsExpanded) return;

        IsExpanded = true;
        HasLayout = false;
        TryBuildExpandIcon();
        _expandController!.Forward(1f);
        if (ReferenceEquals(Parent, _controller.TreeView))
            _controller.TreeView!.Relayout();
        else
            Relayout();
    }

    /// <summary>
    /// 插入子节点，并且同步数据源
    /// </summary>
    internal void InsertChild(int index, TreeNode<T> child)
    {
        if (IsLeaf) return;

        EnsureBuildChildren();

        var insertIndex = index < 0 ? _children!.Count : index;
        _children!.Insert(insertIndex, child);
        //同步数据
        var dataChildren = _controller.ChildrenGetter(Data); //TODO: maybe null
        dataChildren.Insert(insertIndex, child.Data);
        //Reset HasLayout
        HasLayout = false;
    }

    /// <summary>
    /// 移除子节点，并且同步数据源
    /// </summary>
    internal void RemoveChild(TreeNode<T> child)
    {
        _children!.Remove(child);
        //同步数据
        var dataChildren = _controller.ChildrenGetter(Data);
        dataChildren.Remove(child.Data);
        //Reset HasLayout
        HasLayout = false;
    }

    #endregion
}