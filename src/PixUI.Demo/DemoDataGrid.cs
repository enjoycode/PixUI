using System.Globalization;
using System.Linq;

namespace PixUI.Demo.Mac;

public sealed class DemoDataGrid : View
{
    public DemoDataGrid()
    {
        var controller = new DataGridController<Person>();
        controller.DataSource = Person.GeneratePersons(100);

        var redCellStyle = new CellStyle { TextColor = Colors.Red };
        var greenCellStyle = new CellStyle { TextColor = Colors.Green };

        Child = new Container
        {
            Padding = EdgeInsets.All(20),
            Child = new DataGrid<Person>(controller)
                {
                    FooterCells = new DataGridFooterCell[]
                    {
                        new(1, 4) { Text = "平均:", CellStyle = CellStyle.AlignMiddleRight().WithFillColor(0xFFF5F7FA) },
                        new(5, () => controller.DataView!.Average(o => o.Score).ToString(CultureInfo.InvariantCulture)),
                    }
                }
                .AddRowNumColumn("行号", 60, frozen: true)
                .AddTextColumn("城市", p => p.City, 100, cellStyle: CellStyle.AlignCenter(), autoMergeCell: true)
                .AddTextColumn("姓名", p => p.Name, 400)
                .AddGroupColumn("性别", out var genderGroup)
                .AddIconColumnTo(genderGroup, "Icon", p => p.Female ? MaterialIcons.Woman : MaterialIcons.Man, 60,
                    cellStyleGetter: (p, _) => p.Female ? greenCellStyle : redCellStyle)
                .AddCheckboxColumnTo(genderGroup, "IsMan", p => !p.Female, width: 100)
                .AddTextColumn("成绩", p => p.Score.ToString(), 100)
                .AddButtonColumn("操作", (person, _) => new Button(icon: MaterialIcons.ShoppingCart)
                {
                    OnTap = _ => Notification.Info(person.Name)
                }, width: 80, frozen: true)
        };
    }
}