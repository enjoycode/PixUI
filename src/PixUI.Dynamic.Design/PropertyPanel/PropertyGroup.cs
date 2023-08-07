using System.Collections.Generic;

namespace PixUI.Dynamic.Design;

public sealed class PropertyGroup : SingleChildWidget
{
    public PropertyGroup(string title)
    {
        Child = new Collapse
        {
            Title = new Text(title),
            Body = new Form { Ref = _formRef, LabelWidth = 80 }
        };
    }

    private readonly WidgetRef<Form> _formRef = new();

    internal void SetItems(IList<FormItem> items)
    {
        _formRef.Widget!.Children = items;
    }
}