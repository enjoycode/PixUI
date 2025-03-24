using System;
using System.Linq;

namespace PixUI.Dynamic.Design;

internal sealed class StateGroup : View
{
    public StateGroup(DesignController designController)
    {
        _designController = designController;

        Child = new Column(HorizontalAlignment.Left)
        {
            Children =
            {
                new Container
                {
                    Padding = EdgeInsets.Only(5, 0, 0, 0),
                    IsLayoutTight = true,
                    Child = new Row(spacing: 5)
                    {
                        Children =
                        {
                            new Text("State") { FontWeight = FontWeight.Bold },
                            new ButtonGroup
                            {
                                Children =
                                {
                                    new Button(icon: MaterialIcons.Add) { OnTap = OnAddState },
                                    new Button(icon: MaterialIcons.Remove) { OnTap = OnRemoveState },
                                }
                            }
                        }
                    }
                },
                new Card
                {
                    Child = new DataGrid<DynamicState>(_designController.StatesController) { Height = 119 }
                        .AddTextColumn("Name", s => s.Name)
                        .AddTextColumn("Type", s => s.Type.ToString())
                        .AddButtonColumn("Value", (s, i) => new Button(icon: MaterialIcons.Edit)
                        {
                            Style = ButtonStyle.Transparent,
                            Shape = ButtonShape.Pills,
                            OnTap = _ => OnEditState(s),
                        }, 50)
                }
            }
        };
    }

    private readonly DesignController _designController;

    private async void OnAddState(PointerEvent e)
    {
        var dlg = new NewStateDialog(_designController);
        var dlgResult = await dlg.ShowAsync();
        if (dlgResult != DialogResult.OK) return;

        var item = new DynamicState() { Name = dlg.Name, Type = dlg.Type };
        if (item.Type != DynamicStateType.DataTable)
            item.Value = DesignSettings.MakeValueState!(); //暂在这里直接新建，防止未设置状态值时绑定至组件
        _designController.StatesController.Add(item);
    }

    private void OnRemoveState(PointerEvent e)
    {
        if (_designController.StatesController.CurrentRowIndex < 0) return;

        //TODO: check usages
        _designController.StatesController.RemoveAt(_designController.StatesController.CurrentRowIndex);
    }

    private async void OnEditState(DynamicState state)
    {
        if (state.Type == DynamicStateType.DataTable)
        {
            var dlg = DesignSettings.GetTableStateEditor?.Invoke(_designController, state);
            dlg?.Show();
        }
        else
        {
            var dlg = DesignSettings.GetValueStateEditor?.Invoke(state);
            if (dlg != null)
            {
                var res = await dlg.ShowAsync();
                if (res == DialogResult.OK)
                    _designController.NotifyStateValueChanged?.Invoke(state);
            }
        }
    }
}