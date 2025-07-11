namespace PixUI.Diagram;

public sealed class SelectionService
{
    private readonly DiagramSurface _surface;
    private readonly List<DiagramItem> _selectedItems = new();

    public SelectionService(DiagramSurface surface)
    {
        _surface = surface;
    }

    public DiagramItem[] SelectedItems
    {
        get { return _selectedItems.ToArray(); }
    }

    public bool HasSelectedItem
    {
        get { return _selectedItems.Count > 0; }
    }

    internal void MoveSelection(int deltaX, int deltaY)
    {
        //TODO: 先判断有没有不能Move的对象，有则全部不允许移动
        //for (int i = 0; i < selectedItems.Count; i++)
        //{
        //    if ((selectedItems[i].DesignBehavior & DesignBehavior.CanMove) == 0)
        //    {
        //        return;
        //    }
        //}

        //再处理移动所有选择的对象
        for (var i = 0; i < _selectedItems.Count; i++)
        {
            _selectedItems[i].Move(deltaX, deltaY);
        }
    }

    internal void DeleteSelection()
    {
        for (int i = 0; i < _selectedItems.Count; i++)
        {
            //todo:判断是否允许删除，如RootView不允许删除
            _selectedItems[i].Remove();
        }

        ClearSelection();
    }

    internal void ClearSelection()
    {
        SelectItem(null);
        _surface.ResetHoverItem();
    }

    private void ClearInternal()
    {
        foreach (var item in _selectedItems)
            item.IsSelected = false;
        _selectedItems.Clear();
    }

    internal void SelectItem(DiagramItem? item)
    {
        if (item == null && _selectedItems.Count > 0)
        {
            ClearInternal();
            OnSelectionChanged();
        }
        else if (item != null)
        {
            //todo: 判断旧选择项是否与新选择项相同
            ClearInternal();
            _selectedItems.Add(item);
            item.IsSelected = true;
            OnSelectionChanged();
        }
    }

    internal void SelectItems(DiagramItem[]? items)
    {
        if ((items == null || items.Length == 0) && _selectedItems.Count > 0)
        {
            ClearInternal();
        }
        else
        {
            ClearInternal();
            _selectedItems.AddRange(items!);
            foreach (var item in _selectedItems)
            {
                item.IsSelected = true;
            }
        }

        OnSelectionChanged();
    }

    private void OnSelectionChanged()
    {
        _surface.Adorners.ClearSelected();
        for (var i = 0; i < _selectedItems.Count; i++)
        {
            //根据选择的设计对象的CreateSelectionAdorner()方法创建相应的选择装饰器
            var sad = (DesignAdorner)_selectedItems[i].GetSelectionAdorner(_surface.Adorners); //TODO:考虑允许返回null
            _surface.Adorners.Add(sad);
        }

        //No need to repaint adorners.

        SelectionChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? SelectionChanged;
}