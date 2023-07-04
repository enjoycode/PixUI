using System.Collections.ObjectModel;

namespace PixUI;

public sealed class DataGridColumns<T> : Collection<DataGridColumn<T>>
{
    internal DataGridColumns(DataGridController<T> controller)
    {
        _controller = controller;
    }

    private readonly DataGridController<T> _controller;

    protected override void ClearItems()
    {
        base.ClearItems();
        
        _controller.ClearLeafColumns();
        _controller.CheckHasFrozen();
        _controller.RelayoutIfMounted();
    }

    protected override void InsertItem(int index, DataGridColumn<T> item)
    {
        base.InsertItem(index, item);
        
        _controller.GetLeafColumns(item, null);
        _controller.CheckHasFrozen();
        _controller.RelayoutIfMounted();
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        base.RemoveItem(index);
        
        _controller.RemoveLeafColumns(item);
        _controller.CheckHasFrozen();
        _controller.RelayoutIfMounted();
    }

    protected override void SetItem(int index, DataGridColumn<T> item)
    {
        var oldItem = this[index];
        base.SetItem(index, item);
        
        _controller.RemoveLeafColumns(oldItem);
        _controller.GetLeafColumns(item, null);
        _controller.CheckHasFrozen();
        _controller.RelayoutIfMounted();
    }
}