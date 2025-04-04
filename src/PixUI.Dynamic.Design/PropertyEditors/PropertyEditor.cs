using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PixUI.Dynamic.Design;

/// <summary>
/// 包装具体的属性值编辑器，提供绑定状态及重设(删除)按钮
/// </summary>
public sealed class PropertyEditor : Widget
{
    static PropertyEditor()
    {
        RegisterClassValueEditor<string, TextEditor>(true);
        RegisterStructValueEditor<int, NumberEditor<int>>(true);
        RegisterStructValueEditor<float, NumberEditor<float>>(true);
        RegisterStructValueEditor<Color, ColorEditor>(true);
        RegisterStructValueEditor<IconData, IconEditor>(true);
        RegisterStructValueEditor<EdgeInsets, EdgeInsetsEditor>(true);
        RegisterStructValueEditor<DateTime, DateEditor>(false);
        RegisterClassValueEditor<InputBorder, InputBorderEditor>(true);
        RegisterClassValueEditor<string[], StringArrayEditor>(true);
    }

    public PropertyEditor(DesignElement element, DynamicPropertyMeta propertyMeta)
    {
#if DEBUG
        DebugLabel = propertyMeta.Name;
#endif

        Element = element;
        PropertyMeta = propertyMeta;

        var valueType = propertyMeta.IsNullableValueType
            ? propertyMeta.ValueType.GenericTypeArguments[0]
            : propertyMeta.ValueType; //可空值类型转换为不可空值类型作为字典Key
        _targetEditor = GetPropertyValueEditor(valueType, this, out var editingValue);
        _targetEditor.Parent = this;
        EditingValue = editingValue;

        if (propertyMeta.IsState)
        {
            _bindingColorState = new RxProxy<Color>(() =>
                Element.Data.HasBindToState(propertyMeta.Name, out _) ? 0xFF57A64A : Colors.Black);
            _bindButton = BuildBindButton();
            _bindButton.TextColor = _bindingColorState;
            _bindButton.OnTap = _ => BindPropertyToState(element, propertyMeta);
        }

        if (propertyMeta.AllowNull)
        {
            _deleteButton = BuildDeleteButton();
            _deleteButton.OnTap = _ => DeletePropertyValue();
        }
    }

    internal readonly DesignElement Element;
    internal readonly DynamicPropertyMeta PropertyMeta;
    internal readonly State? EditingValue;

    private readonly Widget _targetEditor;
    private readonly Button? _deleteButton;
    private readonly Button? _bindButton;
    private readonly State<Color>? _bindingColorState; //用于表示状态属性是否绑定
    private static readonly State<float> ButtonSize = 20f;

    private Button BuildBindButton()
    {
        var button = new Button(icon: MaterialIcons.Link)
        {
            Style = ButtonStyle.Transparent,
            Width = ButtonSize,
            Height = ButtonSize
        };
        button.Parent = this;
        return button;
    }

    private Button BuildDeleteButton()
    {
        var button = new Button(icon: MaterialIcons.Clear)
        {
            Style = ButtonStyle.Transparent,
            Width = ButtonSize,
            Height = ButtonSize,
        };
        button.Parent = this;
        return button;
    }

    #region ====ValueEditors====

    private static readonly List<ValueEditorInfo> ValueEditors = new();

    private static Func<State, DesignElement, Widget> CreateEditorMaker(Type valueType, Type editorType)
    {
        var stateType = typeof(State<>).MakeGenericType(valueType);
        var stateArg = Expression.Parameter(typeof(State));
        var elementArg = Expression.Parameter(typeof(DesignElement));

        var ctorInfo = editorType.GetConstructor(new[] { stateType, typeof(DesignElement) });
        if (ctorInfo == null)
            throw new Exception("编辑器无指定状态的构造");
        return Expression.Lambda<Func<State, DesignElement, Widget>>(
            Expression.New(ctorInfo, Expression.Convert(stateArg, stateType), elementArg), stateArg, elementArg
        ).Compile();
    }

    public static void RegisterStructValueEditor<TValue, TEditor>(bool isDefault, string? name = null)
        where TValue : struct
        where TEditor : Widget
    {
        name ??= typeof(TEditor).Name;
        var valueType = typeof(TValue);
        valueType = typeof(Nullable<>).MakeGenericType(valueType); //转换为可空值类型

        var editor = new ValueEditorInfo(
            name, isDefault, typeof(TValue),
            CreateEditorMaker(valueType, typeof(TEditor)),
            propertyEditor => new RxProxy<TValue?>(
                () => (TValue?)GetPropertyValue(propertyEditor),
                newValue => SetPropertyValue(propertyEditor, newValue)
            )
        );

        ValueEditors.Add(editor);
    }

    public static void RegisterClassValueEditor<TValue, TEditor>(bool isDefault, string? name = null)
        where TValue : class
        where TEditor : ValueEditorBase
    {
        name ??= typeof(TEditor).Name;
        var editor = new ValueEditorInfo(
            name, isDefault, typeof(TValue),
            CreateEditorMaker(typeof(TValue), typeof(TEditor)),
            propertyEditor => new RxProxy<TValue?>(
                () => (TValue?)GetPropertyValue(propertyEditor),
                newValue => SetPropertyValue(propertyEditor, newValue)
            )
        );

        ValueEditors.Add(editor);
    }

    private static ValueEditorInfo? TryGetValueEditorByName(string name) =>
        ValueEditors.FirstOrDefault(e => e.Name == name);

    /// <summary>
    /// 根据值类型获取默认的编辑器
    /// </summary>
    private static ValueEditorInfo? TryGetValueEditorByValueType(Type valueType) =>
        ValueEditors.FirstOrDefault(e => e.IsDefault && e.ValueType == valueType);

    #endregion

    #region ====PropertyValue====

    private static Widget GetPropertyValueEditor(Type valueType, PropertyEditor propertyEditor, out State? editingValue)
    {
        var element = propertyEditor.Element;
        var propertyMeta = propertyEditor.PropertyMeta;

        //先判断是否指定编辑器
        var isAssignedEditor = !string.IsNullOrEmpty(propertyMeta.EditorName);

        //判断是否枚举类型
        if (valueType.IsEnum && !isAssignedEditor)
        {
            var propState = new RxProxy<string?>(
                () =>
                {
                    var enumValue = GetPropertyValue(propertyEditor);
                    return enumValue?.ToString(); //TODO: 考虑无值转换为枚举的默认值
                },
                v =>
                {
                    var enumValue = Enum.Parse(valueType, v!);
                    SetPropertyValue(propertyEditor, enumValue);
                });
            editingValue = propState;
            return new Select<string>(propState) { Options = Enum.GetNames(valueType) };
        }

        //获取注册的编辑器信息
        var editorInfo = isAssignedEditor
            ? TryGetValueEditorByName(propertyMeta.EditorName!)
            : TryGetValueEditorByValueType(valueType);
        if (editorInfo == null)
        {
            //TODO:
            editingValue = new RxProxy<string>(() =>
            {
                var propValue = GetPropertyValue(propertyEditor);
                return propValue?.ToString() ?? string.Empty;
            });
            return new Text((State<string>)editingValue);
        }

        editingValue = editorInfo.PropertyStateMaker(propertyEditor);
        return editorInfo.EditorMaker(editingValue, element);
    }

    /// <summary>
    /// 获取设计器对应的属性设计值
    /// </summary>
    private static object? GetPropertyValue(PropertyEditor propertyEditor)
    {
        var propName = propertyEditor.PropertyMeta.Name;
        var exists = propertyEditor.Element.Data.TryGetPropertyValue(propName, out var currentValue);
        if (exists)
        {
            var valueSource = currentValue!.Value.From;
            if (valueSource == ValueSource.Const)
                return currentValue.Value.Value;
            if (valueSource == ValueSource.State)
            {
                var state = propertyEditor.Element.Controller.FindState(currentValue.Value.StateName);
                var stateValue = state?.Value as IDynamicPrimitive;
                return stateValue?.GetDesignValue(propertyEditor.Element.Controller.DesignCanvas); //注意非运行时值
            }

            throw new NotSupportedException("Unknown property value source");
        }

        //TODO: none nullable get default value

        return null;
    }

    /// <summary>
    /// 设置设计器对应的属性设计值
    /// </summary>
    private static void SetPropertyValue(PropertyEditor propertyEditor, object? newValue)
    {
        //属性编辑器设置的值不可能为null
        var dynamicValue = new DynamicValue { From = ValueSource.Const, Value = newValue };
        var propName = propertyEditor.PropertyMeta.Name;
        var propertyValue = propertyEditor.Element.Data.SetPropertyValue(propName, dynamicValue);

        if (propertyEditor.PropertyMeta.IsInitSetter)
            propertyEditor.Element.OnInitPropertyValueChanged();
        else
            propertyEditor.Element.SetPropertyValue(propertyValue);

        propertyEditor._bindingColorState?.NotifyValueChanged(); //暂简单强制刷新绑定状态
    }

    private void DeletePropertyValue()
    {
        Element.Data.RemovePropertyValue(PropertyMeta.Name);
        if (PropertyMeta.IsInitSetter)
            Element.OnInitPropertyValueChanged();
        else
            Element.RemovePropertyValue(PropertyMeta.Name);

        EditingValue?.NotifyValueChanged();
        _bindingColorState?.NotifyValueChanged(); //暂简单强制刷新绑定状态
    }

    /// <summary>
    /// 绑定状态属性至状态
    /// </summary>
    private async void BindPropertyToState(DesignElement element, DynamicPropertyMeta propertyMeta)
    {
        var dlg = new BindPropertyStateDialog(element, propertyMeta);
        var res = await dlg.ShowAsync();
        if (res != DialogResult.OK) return;

        var dynamicValue = dlg.BindingValue;
        var propertyValue = element.Data.SetPropertyValue(propertyMeta.Name, dynamicValue);
        if (propertyMeta.IsInitSetter)
            element.OnInitPropertyValueChanged();
        else
            element.SetPropertyValue(propertyValue);

        _bindingColorState?.NotifyValueChanged();
        EditingValue?.NotifyValueChanged();
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
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        _targetEditor.Layout(maxSize.Width - ButtonSize.Value * 2, maxSize.Height);
        _targetEditor.SetPosition(0, 0);
        SetSize(maxSize.Width, _targetEditor.H);

        // 按钮固定位置
        if (_deleteButton != null)
        {
            _deleteButton.Layout(ButtonSize.Value, ButtonSize.Value);
            _deleteButton.SetPosition(maxSize.Width - ButtonSize.Value, (H - _deleteButton.H) / 2f);
        }

        if (_bindButton != null)
        {
            _bindButton.Layout(ButtonSize.Value, ButtonSize.Value);
            _bindButton.SetPosition(maxSize.Width - ButtonSize.Value * 2, (H - _bindButton.H) / 2f);
        }
    }

    #endregion
}