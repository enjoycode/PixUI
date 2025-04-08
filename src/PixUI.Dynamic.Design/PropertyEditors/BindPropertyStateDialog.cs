using System;
using System.Threading.Tasks;

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
        try
        {
            var stateType = DynamicState.GetStateTypeByValueType(_propertyMeta, out var allowNull);
            var list = _element.Controller.FindPrimitiveStates(stateType, allowNull);
            _dgController.DataSource = list;

            //选择
            if (_element.Data.HasBindToState(_propertyMeta.Name, out var oldStateName))
            {
                var index = list.FindIndex(s => s.Name == oldStateName);
                _dgController.SelectAt(index);
            }
            else
            {
                _dgController.TrySelectFirstRow();
            }
        }
        catch (Exception e)
        {
            Notification.Error($"获取状态列表异常: {e.Message}");
        }

        return new Container
        {
            Padding = EdgeInsets.All(20),
            Child = new DataGrid<DynamicState>(_dgController)
                .AddTextColumn("Name", s => s.Name)
                .AddTextColumn("Type", s => s.Type.ToString())
        };
    }

    protected override ValueTask<bool> OnClosing(string result)
    {
        if (result == DialogResult.OK)
        {
            var selected = _dgController.CurrentRow;
            if (selected == null)
            {
                Notification.Error("请先选择状态进行绑定");
                return new ValueTask<bool>(true);
            }

            BindingValue = new DynamicValue() { From = ValueSource.State, Value = selected.Name };
        }

        return new ValueTask<bool>(false);
    }
}