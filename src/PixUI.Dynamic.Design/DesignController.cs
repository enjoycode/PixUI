using System;
using System.Collections.Generic;
using System.Linq;

namespace PixUI.Dynamic.Design;

public sealed class DesignController
{
    public DesignController()
    {
        StatesController.DataSource = new List<DynamicState>();
    }

    /// <summary>
    /// 设计画布缩放百分比
    /// </summary>
    public readonly State<int> Zoom = 100;

    public DesignCanvas DesignCanvas { get; set; } = null!;

    public DesignElement RootElement { get; set; } = null!;

    private IImage? _cachedBgImage;

    public DynamicBackground? Background
    {
        get;
        set
        {
            field = value;
            _cachedBgImage?.Dispose();
            _cachedBgImage = null;
            if (field is { ImageData: not null })
            {
                _cachedBgImage = Image.FromEncodedData(field.ImageData);
            }

            RootElement.Repaint();
        }
    }

    internal IImage? BackgroundImage => _cachedBgImage;

    /// <summary>
    /// 当前工具箱选择的项
    /// </summary>
    public DynamicWidgetMeta? CurrentToolboxItem { get; internal set; }

    /// <summary>
    /// 外部(非属性编辑器)改变布局属性值时通知属性面板附加的布局属性发生了变更
    /// </summary>
    internal Action<string>? NotifyLayoutPropertyChanged;

    /// <summary>
    /// 状态编辑面板改变了状态值时通知属性面板状态值发生了变更
    /// </summary>
    internal Action<DynamicState>? NotifyStateValueChanged;

    /// <summary>
    /// 需要刷新大纲视图的事件
    /// </summary>
    public event Action? OutlineChanged;

    internal void RaiseOutlineChanged() => OutlineChanged?.Invoke();

    /// <summary>
    /// 状态列表控制器
    /// </summary>
    public readonly DataGridController<DynamicState> StatesController = new();

    public DynamicState? FindState(string name)
    {
        var allStates = StatesController.DataSource;
        if (allStates == null || allStates.Count == 0)
            return null;

        if (name.Contains('.'))
        {
            return allStates.Where(s => s.Value is IWithChildStates)
                .SelectMany(s => ((IWithChildStates)s.Value!).GetChildStates(DesignCanvas, s))
                .FirstOrDefault(s => s.Name == name);
        }

        return allStates.FirstOrDefault(s => s.Name == name);
    }

    /// <summary>
    /// 获取所有基元类型的状态列表
    /// </summary>
    public List<DynamicState> FindPrimitiveStates(DynamicStateType type, bool allowNull)
    {
        if (type is DynamicStateType.DataTable or DynamicStateType.DataRow)
            throw new NotSupportedException();

        var allStates = StatesController.DataSource!;
        var list = allStates
            .Where(s => s.Type == type && s.AllowNull == allowNull)
            .ToList();

        //继续查询DataTable及DataRow对应的子级状态
        var withChildren = allStates
            .Where(s => s.Value is IWithChildStates)
            .SelectMany(s => ((IWithChildStates)s.Value!).GetChildStates(DesignCanvas, s))
            .Where(s => s.Type == type && s.AllowNull == allowNull);
        list.AddRange(withChildren);

        return list;
    }

    /// <summary>
    /// 获取所有数据表状态列表
    /// </summary>
    public IEnumerable<DynamicState> FindDataTableStates()
    {
        if (StatesController.DataSource == null) yield break;
        foreach (var state in StatesController.DataSource)
        {
            if (state.Type == DynamicStateType.DataTable) yield return state;
        }
    }

    public IEnumerable<DynamicState> FindDataTableAndDataRowStates()
    {
        if (StatesController.DataSource == null) yield break;
        foreach (var state in StatesController.DataSource)
        {
            if (state.Type is DynamicStateType.DataTable or DynamicStateType.DataRow)
                yield return state;
        }
    }

    #region ====GetChildren====

    public static IEnumerable<DesignElement> GetAllChildrenElements(DesignElement parentElement)
    {
        if (parentElement.Meta == null)
            return [];

        var list = new List<DesignElement>();
        var start = parentElement.Meta.IsReversedWrapElement ? parentElement : parentElement.Child;

        var visitor = new GetAllChildrenVisitor(list);
        start?.VisitChildren(ref visitor);

        return list;
    }

    private static DesignElement? GetChildElement(Widget child)
    {
        if (child is DesignElement designElement)
            return designElement;

        var visitor = new GetChildElementVisitor();
        child.VisitChildren(ref visitor);
        return visitor.Element;
    }

    private readonly struct GetAllChildrenVisitor : IChildrenVisitor
    {
        public GetAllChildrenVisitor(List<DesignElement> list)
        {
            _list = list;
        }

        private readonly List<DesignElement> _list;

        public bool Visit(Widget child)
        {
            var childElement = GetChildElement(child);
            if (childElement != null)
                _list.Add(childElement);

            return false;
        }
    }

    private struct GetChildElementVisitor : IChildrenVisitor
    {
        public DesignElement? Element { get; private set; }

        public bool Visit(Widget child)
        {
            Element = child as DesignElement;
            return true;
        }
    }

    #endregion

    #region ====ContextMenu====

    internal void ShowContextMenu()
    {
        var list = new List<MenuItem>();
        list.Add(MenuItem.Item("Select Parent", MaterialIcons.SwipeUp, () => new SelectParentCommand().Run(this)));
        list.Add(MenuItem.Divider());
        list.Add(MenuItem.Item("Move Forward", MaterialIcons.MoveUp,
            () => new MoveChildCommand(MoveChildAction.Forward).Run(this)));
        list.Add(MenuItem.Item("Move Backward", MaterialIcons.MoveDown,
            () => new MoveChildCommand(MoveChildAction.Backward).Run(this)));
        ContextMenu.Show(list.ToArray());
    }

    #endregion

    #region ====DesignElement Selection====

    internal readonly List<DesignElement> Selection = new();

    public event Action? SelectionChanged;

    public DesignElement? FirstSelected => Selection.Count > 0 ? Selection[0] : null;

    public void Select(DesignElement element)
    {
        if (Selection.Count == 1 && ReferenceEquals(Selection[0], element)) return;

        Selection.ForEach(o => o.IsSelected = false);
        Selection.Clear();

        Selection.Add(element);
        element.IsSelected = true;

        OnSelectionChanged();
    }

    internal void OnSelectionChanged() => SelectionChanged?.Invoke();

    #endregion
}