using System;
using System.Collections.Generic;
using System.Linq;

namespace PixUI.Dynamic.Design;

public sealed partial class DesignController
{
    public DesignController()
    {
        StatesController.DataSource = new List<DynamicState>();
    }

    /// <summary>
    /// 设计画布缩放百分比
    /// </summary>
    public readonly State<int> Zoom = 100;

    public DesignElement RootElement { get; internal set; } = null!;

    /// <summary>
    /// 当前工具箱选择的项
    /// </summary>
    public DynamicWidgetMeta? CurrentToolboxItem { get; internal set; }

    /// <summary>
    /// 通知属性面板附加的布局属性发生了变更
    /// </summary>
    internal Action<string>? NotifyLayoutPropertyChanged;

    /// <summary>
    /// 状态列表控制器
    /// </summary>
    internal readonly DataGridController<DynamicState> StatesController = new();

    public DynamicState? FindState(string name) =>
        StatesController.DataSource?.FirstOrDefault(s => s.Name == name);

    public IEnumerable<DynamicState> GetAllDataSet()
    {
        if (StatesController.DataSource == null) yield break;
        foreach (var state in StatesController.DataSource)
        {
            if (state.Type == DynamicStateType.DataSet) yield return state;
        }
    }

    #region ====DesignElement Selection====

    private readonly List<DesignElement> _selection = new();

    public event Action? SelectionChanged;

    public DesignElement? FirstSelected => _selection.Count > 0 ? _selection[0] : null;

    internal void Select(DesignElement element)
    {
        if (_selection.Count == 1 && ReferenceEquals(_selection[0], element)) return;

        _selection.ForEach(o => o.IsSelected = false);
        _selection.Clear();

        _selection.Add(element);
        element.IsSelected = true;

        OnSelectionChanged();
    }

    internal void OnSelectionChanged()
    {
        SelectionChanged?.Invoke();
        RootElement.Invalidate(InvalidAction.Repaint); //暂在这里全部刷新，待实现选择装饰器后移除
    }

    #endregion

    #region ====DesignElement Actions====

    /// <summary>
    /// 移动选择的位置，目前仅适用于Positioned组件
    /// </summary>
    internal void MoveElements(float dx, float dy)
    {
        //判断选择的是否可以移动，目前仅针对Stack下的Positioned组件,
        //另需要注意如果位置属性绑定了状态不可手工移动

        var canMove = true;
        foreach (var element in _selection)
        {
            if (element.Target is Positioned ||
                element.Parent is DesignElement { Target: Positioned })
                continue;
            canMove = false;
            break;
        }

        if (!canMove) return;


        foreach (var element in _selection)
        {
            var moveable = element.Target is Positioned ? element : (DesignElement)element.Parent!;
            var target = moveable.Child as DesignElement;
            var positioned = (Positioned)moveable.Target!;
            var oldX = positioned.Left?.Value ?? 0f;
            var oldY = positioned.Top?.Value ?? 0f;

            // Log.Debug($"old={oldX}, {oldY} delta={e.DeltaX}, {e.DeltaY}");
            moveable.SetPropertyValue(moveable.Data.SetPropertyValue("Left", oldX + dx));
            NotifyLayoutPropertyChanged?.Invoke("Left");
            moveable.SetPropertyValue(moveable.Data.SetPropertyValue("Top", oldY + dy));
            NotifyLayoutPropertyChanged?.Invoke("Top");
            //clear Right & Bottom value
            if (moveable.Data.TryGetPropertyValue("Right", out var _))
            {
                target?.SetPropertyValue(target.Data.SetPropertyValue("Width", moveable.W));
                moveable.Data.RemovePropertyValue("Right");
                moveable.RemovePropertyValue("Right");
                NotifyLayoutPropertyChanged?.Invoke("Width");
                NotifyLayoutPropertyChanged?.Invoke("Right");
            }

            if (moveable.Data.TryGetPropertyValue("Bottom", out var _))
            {
                target?.SetPropertyValue(target.Data.SetPropertyValue("Height", moveable.H));
                moveable.Data.RemovePropertyValue("Bottom");
                moveable.RemovePropertyValue("Bottom");
                NotifyLayoutPropertyChanged?.Invoke("Height");
                NotifyLayoutPropertyChanged?.Invoke("Bottom");
            }
        }
    }

    /// <summary>
    /// 删除选择的元素
    /// </summary>
    public void DeleteElements()
    {
        //TODO: maybe check can be deleted
        DesignElement? lastParentElement = null;

        foreach (var element in _selection)
        {
            if (element.IsRoot)
            {
                element.ChangeMeta(null, false);
                element.Invalidate(InvalidAction.Relayout);
                OnSelectionChanged();
                break; //ignore others
            }

            DesignElement parentElement;
            DesignElement childElement;
            Widget childWidget;
            if (element.Parent is DesignElement reversed) //是反向包装的
            {
                childElement = reversed;
                childWidget = reversed.Parent!;
                parentElement = (DesignElement)childWidget.Parent!.Parent!;
            }
            else
            {
                childWidget = childElement = element;
                parentElement = (DesignElement)element.Parent!.Parent!;
            }

            var slot = parentElement.Meta!.GetSlot(childElement.SlotName);
            if (slot.ContainerType == ContainerType.MultiChild)
            {
                if (slot.TryRemoveChild(parentElement.Target!, childWidget))
                {
                    parentElement.Invalidate(InvalidAction.Relayout);
                    lastParentElement = parentElement;
                }
            }
            else
            {
                if (slot.TrySetChild(parentElement.Target!, null))
                {
                    parentElement.Invalidate(InvalidAction.Relayout);
                    lastParentElement = parentElement;
                }
            }
        }

        if (lastParentElement != null)
            Select(lastParentElement);
    }

    #endregion
}