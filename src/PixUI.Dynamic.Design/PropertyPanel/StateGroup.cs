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
            Body = new Column(HorizontalAlignment.Left)
            {
                Children =
                {
                    new Container
                    {
                        Padding = EdgeInsets.Only(4, 0, 0, 0),
                        IsLayoutTight = true,
                        Child = new ButtonGroup
                        {
                            Children =
                            {
                                new Button(icon: MaterialIcons.Add) { OnTap = OnAddState },
                                new Button(icon: MaterialIcons.Remove) { OnTap = OnRemoveState },
                            }
                        }
                    },
                    new Card
                    {
                        Child = new DataGrid<DynamicState>(statesController)
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
                                        OnTap = _ => OnEditState(s),
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
        var dlgResult = await dlg.ShowAsync();
        if (dlgResult != DialogResult.OK) return;

        var item = new DynamicState() { Name = dlg.Name, Type = dlg.Type };
        _statesController.Add(item);
    }

    private void OnRemoveState(PointerEvent e)
    {
        if (_statesController.CurrentRowIndex < 0) return;

        //TODO: check usages
        _statesController.RemoveAt(_statesController.CurrentRowIndex);
    }

    private static void OnEditState(DynamicState state)
    {
        if (state.Type == DynamicStateType.DataSet)
        {
            var dlg = DesignSettings.GetDataSetStateEditor?.Invoke(state);
            dlg?.Show();
        }
        else
        {
            var dlg = DesignSettings.GetValueStateEditor?.Invoke(state);
            dlg?.Show();
        }
    }
}