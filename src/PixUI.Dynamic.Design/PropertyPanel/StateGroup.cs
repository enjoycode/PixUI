using System;

namespace PixUI.Dynamic.Design;

internal sealed class StateGroup : View
{
    public StateGroup(DataGridController<DynamicState> statesController)
    {
        _statesController = statesController;

        Child = new Collapse
        {
            Title = new Text("State") { FontWeight = FontWeight.Bold },
            Body = new Card()
            {
                Child = new Column(HorizontalAlignment.Left)
                {
                    Children =
                    {
                        new ButtonGroup
                        {
                            Children =
                            {
                                new Button(icon: MaterialIcons.Add) { OnTap = OnAddState },
                                new Button(icon: MaterialIcons.Remove) { OnTap = OnRemoveState },
                            }
                        },
                        new DataGrid<DynamicState>(statesController)
                        {
                            Height = 118,
                            Columns =
                            {
                                new DataGridTextColumn<DynamicState>("Name", s => s.Name),
                                new DataGridTextColumn<DynamicState>("Type", s => s.Type.ToString()),
                                new DataGridButtonColumn<DynamicState>("Value",
                                    (s, i) => new Button(icon: MaterialIcons.Edit)
                                    {
                                        Style = ButtonStyle.Transparent,
                                        Shape = ButtonShape.Pills,
                                    }) { Width = ColumnWidth.Fixed(50) }
                            }
                        }
                    }
                }
            }
        };
    }

    private readonly DataGridController<DynamicState> _statesController;

    private async void OnAddState(PointerEvent e)
    {
        var dlg = new NewStateDialog();
        var canceled = await dlg.ShowAndWaitClose();
        if (canceled) return;

        var item = new DynamicState() { Name = dlg.Name, Type = dlg.Type };
        _statesController.Add(item);
    }

    private void OnRemoveState(PointerEvent e)
    {
        if (_statesController.CurrentRowIndex < 0) return;

        //TODO: check usages
        _statesController.RemoveAt(_statesController.CurrentRowIndex);
    }
}