using System;

namespace PixUI.Dynamic.Design;

internal sealed class BindPropertyStateDialog : Dialog
{
    public BindPropertyStateDialog(DesignElement element, DynamicPropertyMeta propertyMeta)
    {
        Title.Value = "Bind Property To State";
        Width = 250;
        Height = 300;

        _element = element;
        _propertyMeta = propertyMeta;
    }

    private readonly DesignElement _element;
    private readonly DynamicPropertyMeta _propertyMeta;
    private readonly DataGridController<DynamicState> _dgController = new();

    internal DynamicValue BindingValue { get; private set; }

    protected override Widget BuildBody()
    {
        //查询相同类型的状态列表
        var stateType = DynamicState.GetStateTypeByValueType(_propertyMeta, out var allowNull);
        var list = _element.Controller.FindStatesByValueType(stateType, allowNull);
        _dgController.DataSource = list;
        _dgController.TrySelectFirstRow();

        return new Container
        {
            Padding = EdgeInsets.All(20),
            Child = new DataGrid<DynamicState>(_dgController)
            {
                Columns =
                {
                    new DataGridTextColumn<DynamicState>("Name", s => s.Name),
                    new DataGridTextColumn<DynamicState>("Type", s => s.Type.ToString()),
                }
            }
        };
    }

    protected override bool OnClosing(string result)
    {
        if (result == DialogResult.OK)
        {
            var selected = _dgController.CurrentRow;
            if (selected == null)
            {
                Notification.Error("请先选择状态进行绑定");
                return true;
            }

            BindingValue = new DynamicValue() { From = ValueSource.State, Value = selected.Name };
        }

        return false;
    }
}