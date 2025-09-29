namespace PixUI.Diagram;

public sealed class SelectionService
{
    public SelectionService(DiagramSurface surface)
    {
        _surface = surface;
    }

    private readonly DiagramSurface _surface;
    private readonly List<DiagramItem> _selectedItems = new();
    public event EventHandler? SelectionChanged;

    public IReadOnlyList<DiagramItem> SelectedItems => _selectedItems;

    public bool HasSelection => _selectedItems.Count > 0;

    public void ClearSelection()
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

    public void SelectItem(DiagramItem? item)
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

    public void SelectItems(DiagramItem[]? items)
    {
        if (IsSameSelection(items))
            return;

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

    private bool IsSameSelection(DiagramItem[]? newItems)
    {
        if (newItems == null)
            return _selectedItems.Count <= 0;

        if (newItems.Length != _selectedItems.Count)
            return false;

        for (var i = 0; i < newItems.Length; i++)
        {
            if (!ReferenceEquals(newItems[i], _selectedItems[i]))
                return false;
        }

        return true;
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
}