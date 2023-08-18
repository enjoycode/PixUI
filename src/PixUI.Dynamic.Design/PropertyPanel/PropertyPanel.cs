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

        _layoutGroup = new PropertyGroup(_layoutGroupTitle);
        _listView = ListView<Widget>.From(new Widget[]
        {
            _widgetGroup,
            new IfConditional(_layoutGroupVisible, () => _layoutGroup),
            new IfConditional(_propGroupVisible, () => _propGroup),
            new IfConditional(_eventGroupVisible, () => _eventGroup)
        });
        Child = _listView;
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

    private void OnSelectionChanged()
    {
        if (_controller.FirstSelected == null || _controller.FirstSelected.Meta == null)
        {
            _widgetGroup.SetItems(Array.Empty<FormItem>());
            _layoutProperties.Clear();
            _layoutGroupVisible.Value = false;
            _propGroupVisible.Value = false;
            _eventGroupVisible.Value = false;
            return;
        }

        var element = _controller.FirstSelected!;
        var meta = element.Meta;

        BuildWidgetGroup(element, meta);
        BuildLayoutGroup(element, meta);
        BuildPropertyGroup(element, meta);

        _listView.Invalidate(InvalidAction.Relayout);
    }

    private void BuildWidgetGroup(DesignElement element, DynamicWidgetMeta meta)
    {
        var widgetGroupItems = new List<FormItem> { new("Type:", new Text(meta.Name)) };
        if (meta.CtorArgs is { Length: > 0 })
        {
            foreach (var ctorArgMeta in meta.CtorArgs)
            {
                widgetGroupItems.Add(new FormItem($"{ctorArgMeta.Name}:", new PropertyEditor(element, ctorArgMeta)));
            }
        }

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
            var propEditor = new PropertyEditor(parentElement, parentMeta.Properties[i]);
            if (propEditor.EditingValue != null)
                _layoutProperties.Add(parentMeta.Properties[i].Name, propEditor.EditingValue);
            propItems[i] = new($"{parentMeta.Properties[i].Name}:", propEditor);
        }

        _layoutGroup.SetItems(propItems);
    }

    private void BuildPropertyGroup(DesignElement element, DynamicWidgetMeta meta)
    {
        _propGroupVisible.Value = meta.Properties is { Length: > 0 };
        if (!_propGroupVisible.Value) return;
        
        var propItems = new FormItem[meta.Properties!.Length];
        for (var i = 0; i < propItems.Length; i++)
        {
            propItems[i] = new($"{meta.Properties[i].Name}:", new PropertyEditor(element, meta.Properties[i]));
        }

        _propGroup.SetItems(propItems);
    }
}