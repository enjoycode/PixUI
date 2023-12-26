using System.Collections.Generic;

namespace PixUI.Dynamic.Design;

public sealed class PropertyGroup : SingleChildWidget
{
    public PropertyGroup(State<string> title)
    {
        Child = new Collapse
        {
            Title = new Text(title) { FontWeight = FontWeight.Bold },
            Body = new Form { LabelWidth = 108 }.RefBy(ref _formRef)
        };
    }

    private readonly Form _formRef = null!;

    internal void SetItems(IList<FormItem> items)
    {
        _formRef.Children = items;
    }
}