namespace PixUI.Demo.Mac
{
    public sealed class DemoDataGrid : View
    {
        private readonly DataGridController<Person> _controller;

        public DemoDataGrid()
        {
            _controller = new DataGridController<Person>();
            _controller.DataSource = Person.GeneratePersons(1000);

            Child = new Container
            {
                Padding = EdgeInsets.All(20),
                Child = new DataGrid<Person>(_controller)
                {
                    Columns =
                    {
                        new DataGridTextColumn<Person>("Name", p => p.Name),
                        new DataGridGroupColumn<Person>("Gender")
                        {
                            new DataGridIconColumn<Person>("Icon", p =>
                                p.Female ? MaterialIcons.Woman : MaterialIcons.Man, ColumnWidth.Fixed(60)),
                            new DataGridCheckboxColumn<Person>("IsMan", p => !p.Female),
                        },
                        new DataGridTextColumn<Person>("Score", p => p.Score.ToString()),
                        new DataGridButtonColumn<Person>("Action",
                            (person, index) => new Button(icon: MaterialIcons.ShoppingCart)
                                { OnTap = _ => Notification.Info(person.Name) },
                            ColumnWidth.Fixed(80)),
                    }
                }
            };
        }
    }
}