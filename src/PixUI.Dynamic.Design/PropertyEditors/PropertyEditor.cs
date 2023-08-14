using System;

namespace PixUI.Dynamic.Design;

/// <summary>
/// 包装具体的属性值编辑器，提供绑定状态及重设(删除)按钮
/// </summary>
public sealed class PropertyEditor : Widget
{
    /// <summary>
    /// 新建属性编辑器
    /// </summary>
    public PropertyEditor(DesignElement element, DynamicPropertyMeta propertyMeta)
    {
        var valueType = GetValueType(propertyMeta.Value);
        _targetEditor = GetPropertyValueEditor(valueType, element, propertyMeta, out var editingValue);
        _targetEditor.Parent = this;

        if (propertyMeta.Value.IsState)
            _bindButton = BuildBindButton();
        if (propertyMeta.AllowNull)
        {
            _deleteButton = BuildDeleteButton();
            _deleteButton.OnTap = _ => DeletePropertyValue(element, propertyMeta, editingValue);
        }
    }

    /// <summary>
    /// 新建构造参数编辑器
    /// </summary>
    public PropertyEditor(DesignElement element, DynamicCtorArgMeta ctorArgMeta) { }

    private readonly Widget _targetEditor;
    private readonly Button? _deleteButton;
    private readonly Button? _bindButton;

    private static readonly State<float> _buttonSize = 20f;

    private Button BuildBindButton()
    {
        var button = new Button(icon: MaterialIcons.Link)
        {
            Style = ButtonStyle.Transparent,
            Width = _buttonSize,
            Height = _buttonSize
        };
        button.Parent = this;
        return button;
    }

    private Button BuildDeleteButton()
    {
        var button = new Button(icon: MaterialIcons.Clear)
        {
            Style = ButtonStyle.Transparent,
            Width = _buttonSize,
            Height = _buttonSize,
        };
        button.Parent = this;
        return button;
    }

    private static Type GetValueType(DynamicValueMeta valueMeta)
    {
        var valueType = valueMeta.ValueType;
        if (valueType is { IsValueType: true, IsGenericType: true } &&
            valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
            valueType = valueType.GenericTypeArguments[0];
        return valueType;
    }

    #region ====PropertyValue====

    private static Widget GetPropertyValueEditor(Type valueType, DesignElement element,
        DynamicPropertyMeta propertyMeta, out State? editingValue)
    {
        //TODO: test only now
        if (valueType == typeof(Color))
        {
            var rxProp = new RxProperty<Color?>(() => (Color?)GetPropertyValue(element, propertyMeta),
                (newValue) => SetPropertyValue(element, propertyMeta, newValue));
            editingValue = rxProp;
            return new ColorEditor(rxProp);
        }

        editingValue = null;
        return new Input("12345");
    }

    private static object? GetPropertyValue(DesignElement element, DynamicPropertyMeta propertyMeta)
    {
        var exists = element.Data.TryGetPropertyValue(propertyMeta.Name, out var currentValue);
        if (exists)
        {
            if (currentValue!.Value.From != ValueSource.Const) throw new NotImplementedException();
            return currentValue.Value.Value;
        }

        if (propertyMeta.Value.DefaultValue != null)
            return propertyMeta.Value.DefaultValue.Value.Value;

        return null;
    }

    private static void SetPropertyValue(DesignElement element, DynamicPropertyMeta propertyMeta, object? newValue)
    {
        //属性编辑器设置的值不可能为null
        var dynamicValue = new DynamicValue { From = ValueSource.Const, Value = newValue };
        var propertyValue = element.Data.SetPropertyValue(propertyMeta.Name, dynamicValue);
        element.SetPropertyValue(propertyValue);
    }

    private static void DeletePropertyValue(DesignElement element, DynamicPropertyMeta propertyMeta,
        State? editingValue)
    {
        element.Data.RemovePropertyValue(propertyMeta.Name);
        element.RemovePropertyValue(propertyMeta.Name);

        editingValue?.NotifyValueChanged();
    }

    #endregion

    #region ====Widget Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (action(_targetEditor)) return;
        if (_deleteButton != null && action(_deleteButton)) return;
        if (_bindButton != null) action(_bindButton);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        var editorWidth = width - (_deleteButton == null ? 0 : _buttonSize.Value) -
                          (_bindButton == null ? 0 : _buttonSize.Value);
        _targetEditor.Layout(editorWidth, height);
        _targetEditor.SetPosition(0, 0);
        SetSize(width, _targetEditor.H);


        var right = width;
        if (_deleteButton != null)
        {
            _deleteButton.Layout(_buttonSize.Value, _buttonSize.Value);
            right -= _deleteButton.W;
            _deleteButton.SetPosition(right, (H - _deleteButton.H) / 2f);
        }

        if (_bindButton != null)
        {
            _bindButton.Layout(_buttonSize.Value, _buttonSize.Value);
            right -= _bindButton.W;
            _bindButton.SetPosition(right, (H - _bindButton.H) / 2f);
        }
    }

    #endregion
}