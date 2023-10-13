using System;
using System.Collections.Generic;

namespace PixUI;

public sealed class TabController<T>
{
    public TabController(IList<T> dataSource)
    {
        DataSource = dataSource;
        // if (DataSource is RxList<T> rxList)
        // {
        //     rxList.AddBinding(this, BindingOptions.None);
        // }
    }

    private TabBar<T>? _tabBar;
    private TabBody<T>? _tabBody;

    internal readonly IList<T> DataSource;

    public int Count => DataSource.Count;
    public int SelectedIndex { get; private set; } = -1;

    internal void BindTabBar(TabBar<T> tabBar) => _tabBar = tabBar;
    internal void BindTabBody(TabBody<T> tabBody) => _tabBody = tabBody;

    #region ====Events====

    public event Action<int>? TabSelectChanged;
    public event Action<T>? TabAdded;
    public event Action<T>? TabClosed;

    #endregion

    #region ====Operations====

    public T GetAt(int index) => DataSource[index];
    // public T this[int index] => DataSource[index];

    public int IndexOf(T dataItem) => DataSource.IndexOf(dataItem);

    /// <summary>
    /// 选择指定的Tab
    /// </summary>
    public void SelectAt(int index, bool byTapTab = false)
    {
        if (index < 0 || index == SelectedIndex) return;

        //TODO: check need scroll to target tab
        if (_tabBar != null && SelectedIndex >= 0)
            _tabBar.Tabs[SelectedIndex].IsSelected.Value = false;

        var oldIndex = SelectedIndex;
        SelectedIndex = index;
        _tabBody?.SwitchFrom(oldIndex);

        if (_tabBar != null)
            _tabBar.Tabs[SelectedIndex].IsSelected.Value = true;

        TabSelectChanged?.Invoke(index);
    }

    public void Add(T dataItem)
    {
        DataSource.Add(dataItem);
        _tabBar?.OnAdd(dataItem);
        _tabBody?.OnAdd(dataItem);
        TabAdded?.Invoke(dataItem);

        SelectAt(DataSource.Count - 1); //选中添加的
    }

    public void Remove(T dataItem)
    {
        var index = DataSource.IndexOf(dataItem);
        if (index < 0) return;

        var isSelected = index == SelectedIndex; //是否正在移除选中的
        if (index < SelectedIndex)
            SelectedIndex -= 1;

        DataSource.RemoveAt(index);
        _tabBar?.OnRemoveAt(index);
        _tabBody?.OnRemoveAt(index);

        //原本是选中的那个，移除后选择新的
        if (isSelected)
        {
            SelectedIndex = -1; //reset first
            if (DataSource.Count > 0)
            {
                var newSelectedIndex = Math.Max(0, index - 1);
                SelectAt(newSelectedIndex);
            }
            else
            {
                _tabBody?.ClearBody();
                TabSelectChanged?.Invoke(-1);
            }
        }

        //最后激发TabClosed事件
        TabClosed?.Invoke(dataItem);
    }

    #endregion
}