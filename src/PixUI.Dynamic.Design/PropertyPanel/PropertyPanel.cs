using System;

namespace PixUI.Dynamic.Design;

public sealed class PropertyPanel : SingleChildWidget
{
    public PropertyPanel(DesignController controller)
    {
        _controller = controller;
        _controller.SelectionChanged += OnSelectionChanged;

        _listView = ListView<Widget>.From(new Widget[]
        {
            _widgetGroup,
            new IfConditional(_propGroupVisible, () => _propGroup),
            new IfConditional(_eventGroupVisible, () => _eventGroup)
        });
        Child = _listView;
    }

    private readonly DesignController _controller;
    private readonly ListView<Widget> _listView;
    private readonly PropertyGroup _widgetGroup = new("Widget");
    private readonly PropertyGroup _propGroup = new("Properties");
    private readonly PropertyGroup _eventGroup = new("Events");

    private readonly State<bool> _propGroupVisible = false;
    private readonly State<bool> _eventGroupVisible = false;

    private void OnSelectionChanged()
    {
        if (_controller.FirstSelected == null)
        {
            _widgetGroup.SetItems(Array.Empty<FormItem>());
            _propGroupVisible.Value = false;
            _eventGroupVisible.Value = false;
            return;
        }

        var meta = _controller.FirstSelected!.Meta!;
        var data = _controller.FirstSelected!.Data;

        //Widget Group
        _widgetGroup.SetItems(new FormItem[]
        {
            new("Type:", new Input(meta.Name))
        });

        //Properties Group
        _propGroupVisible.Value = meta.Properties != null && meta.Properties.Length > 0;
        if (_propGroupVisible.Value)
        {
            var propItems = new FormItem[meta.Properties!.Length];
            for (var i = 0; i < propItems.Length; i++)
            {
                propItems[i] = new($"{meta.Properties[i].Name}:", new Input("12345"));
            }

            _propGroup.SetItems(propItems);
        }

        _listView.Invalidate(InvalidAction.Relayout);
    }
}