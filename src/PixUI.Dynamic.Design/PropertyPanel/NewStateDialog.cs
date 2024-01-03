using System;
using System.Linq;
using System.Threading.Tasks;

namespace PixUI.Dynamic.Design;

internal sealed class NewStateDialog : Dialog
{
    public NewStateDialog(DesignController designController)
    {
        Width = 300;
        Height = 210;
        Title.Value = "Add State";

        _designController = designController;
    }

    private readonly DesignController _designController;
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

    protected override ValueTask<bool> OnClosing(string result)
    {
        if (result != DialogResult.OK) return new ValueTask<bool>(false);

        //检查名称是否已存在
        if (_designController.StatesController.DataSource!.Any(s => s.Name == Name))
        {
            Notification.Error("状态名称已存在");
            return new ValueTask<bool>(true);
        }

        return new ValueTask<bool>(false);
    }
}