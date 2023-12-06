using System;

namespace PixUI.Dynamic.Design;

internal sealed class BindPropertyStateDialog : Dialog
{
    public BindPropertyStateDialog(DesignElement element, DynamicPropertyMeta propertyMeta, State<Color> bindColor)
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

    protected override Widget BuildBody()
    {
        //查询相同类型的状态列表
        var valueType = _propertyMeta.ValueType;
        var allowNull = false;
        if (_propertyMeta.IsNullableValueType)
        {
            valueType = valueType.GenericTypeArguments[0];
            allowNull = true;
        }

        DynamicStateType stateType;
        if (valueType == typeof(string))
            stateType = DynamicStateType.String;
        else if (valueType == typeof(int))
            stateType = DynamicStateType.Int;
        else if (valueType == typeof(DateTime))
            stateType = DynamicStateType.DateTime;
        else
            throw new NotImplementedException();

        var list = _element.Controller.FindStatesByValueType(stateType, allowNull);
        _dgController.DataSource = list;

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
}