using System;
using System.Collections.Generic;

namespace PixUI.Dynamic.Design;

public sealed class PropertyPanel : SingleChildWidget
{
    public PropertyPanel(DesignController controller)
    {
        _controller = controller;
        _controller.SelectionChanged += OnSelectionChanged;
        _controller.NotifyLayoutPropertyChanged = OnNotifyLayoutPropertyChanged;
        _controller.NotifyStateValueChanged = OnNotifyStateValueChanged;

        _layoutGroup = new PropertyGroup(_layoutGroupTitle);
        _listView = ListView<Widget>.From(new Widget[]
        {
            new StateGroup(controller),
            _widgetGroup,
            new IfConditional(_layoutGroupVisible, () => _layoutGroup),
            new IfConditional(_propGroupVisible, () => _propGroup),
            new IfConditional(_eventGroupVisible, () => _eventGroup)
        });

        Child = new Container
        {
            FillColor = new Color(0xFFF3F3F3),
            Child = _listView
        };
    }

    private readonly DesignController _controller;
    private readonly ListView<Widget> _listView;
    private readonly PropertyGroup _layoutGroup;
    private readonly PropertyGroup _widgetGroup = new("Widget");
    private readonly PropertyGroup _propGroup = new("Properties");
    private readonly PropertyGroup _eventGroup = new("Events");

    private readonly State<string> _layoutGroupTitle = string.Empty;
    private readonly State<bool> _layoutGroupVisible = false;
    private readonly State<bool> _propGroupVisible = false;
    private readonly State<bool> _eventGroupVisible = false;

    /// <summary>
    /// 属性组的所有编辑器，用于状态编辑面板的状态值变更通知刷新
    /// </summary>
    private readonly List<PropertyEditor> _propertyEditors = new();

    /// <summary>
    /// 附加的布局属性字典表
    /// </summary>
    private readonly Dictionary<string, State> _layoutProperties = new();

    private void OnNotifyLayoutPropertyChanged(string layoutPropertyName)
    {
        if (_layoutProperties.TryGetValue(layoutPropertyName, out var editingValue))
        {
            editingValue.NotifyValueChanged();
        }
    }

    private void OnNotifyStateValueChanged(DynamicState state)
    {
        foreach (var propertyEditor in _propertyEditors)
        {
            var propertyMeta = propertyEditor.PropertyMeta;
            if (!propertyMeta.IsState) continue;

            if (!propertyEditor.Element.Data.TryGetPropertyValue(propertyMeta.Name, out var propValue))
                continue;

            if (propValue!.Value.From == ValueSource.State && propValue.Value.StateName == state.Name)
                propertyEditor.EditingValue?.NotifyValueChanged();
        }
    }

    private void OnSelectionChanged()
    {
        if (_controller.FirstSelected == null || _controller.FirstSelected.Meta == null)
        {
            _widgetGroup.SetItems(Array.Empty<FormItem>());
            _layoutProperties.Clear();
            _layoutGroupVisible.Value = false;
            _propGroupVisible.Value = false;
            _eventGroupVisible.Value = false;
            _listView.Relayout();
            return;
        }

        var element = _controller.FirstSelected!;
        var meta = element.Meta;

        BuildWidgetGroup(element, meta);
        BuildLayoutGroup(element, meta);
        BuildPropertyGroup(element, meta);
        BuildEventGroup(element, meta);

        _listView.Controller.ResetScroll();
        _listView.Relayout();
    }

    private void BuildWidgetGroup(DesignElement element, DynamicWidgetMeta meta)
    {
        var widgetGroupItems = new List<FormItem> { new("Type:", new Text(meta.Name)) };
        //TODO:考虑在这里添加宽高属性
        _widgetGroup.SetItems(widgetGroupItems);
    }

    private void BuildLayoutGroup(DesignElement element, DynamicWidgetMeta meta)
    {
        _layoutGroupVisible.Value = element.Parent is DesignElement;
        _layoutProperties.Clear();
        if (!_layoutGroupVisible.Value) return;

        var parentElement = (DesignElement)element.Parent!;
        var parentMeta = parentElement.Meta!;
        _layoutGroupTitle.Value = parentMeta.Name;

        var propItems = new FormItem[parentMeta.Properties!.Length];
        for (var i = 0; i < propItems.Length; i++)
        {
            var propName = parentMeta.Properties[i].Name;
            var propEditor = new PropertyEditor(parentElement, parentMeta.Properties[i]);
            if (propEditor.EditingValue != null)
                _layoutProperties.Add(propName, propEditor.EditingValue);
            propItems[i] = new($"{propName}:", propEditor);
        }

        _layoutGroup.SetItems(propItems);
    }

    private void BuildPropertyGroup(DesignElement element, DynamicWidgetMeta meta)
    {
        _propGroupVisible.Value = meta.Properties is { Length: > 0 };
        _propertyEditors.Clear();
        if (!_propGroupVisible.Value) return;

        var propItems = new FormItem[meta.Properties!.Length];
        for (var i = 0; i < propItems.Length; i++)
        {
            var propName = meta.Properties[i].Name;
            var propEditor = new PropertyEditor(element, meta.Properties[i]);
            _propertyEditors.Add(propEditor);
            propItems[i] = new($"{propName}:", propEditor);
            if ((propName == "Width" || propName == "Height") && propEditor.EditingValue != null)
                _layoutProperties.Add(propName, propEditor.EditingValue);
        }

        _propGroup.SetItems(propItems);
    }

    private void BuildEventGroup(DesignElement element, DynamicWidgetMeta meta)
    {
        _eventGroupVisible.Value = meta.Events is { Length: > 0 };
        if (!_eventGroupVisible.Value) return;

        var eventItems = new FormItem[meta.Events!.Length];
        for (var i = 0; i < eventItems.Length; i++)
        {
            var eventName = meta.Events[i].Name;
            var eventEditor = new EventEditor(element, meta.Events[i]);
            eventItems[i] = new($"{eventName}:", eventEditor);
        }

        _eventGroup.SetItems(eventItems);
    }
}