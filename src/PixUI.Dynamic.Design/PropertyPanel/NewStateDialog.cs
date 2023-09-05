using System;

namespace PixUI.Dynamic.Design;

internal sealed class NewStateDialog : Dialog
{
    public NewStateDialog()
    {
        Width = 300;
        Height = 210;
        Title.Value = "Add State";
    }

    private readonly State<string> _name = "";
    private readonly State<string?> _type = "DataSet";

    public string Name => _name.Value;
    public DynamicStateType Type => Enum.Parse<DynamicStateType>(_type.Value!);

    protected override Widget BuildBody()
    {
        return new Container()
        {
            Padding = EdgeInsets.All(20),
            Child = new Form()
            {
                LabelWidth = 80,
                Children =
                {
                    new FormItem("Name:", new TextInput(_name)),
                    new FormItem("Type:",
                        new Select<string>(_type) { Options = Enum.GetNames(typeof(DynamicStateType)) })
                }
            }
        };
    }
}