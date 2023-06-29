namespace PixUI.Demo.Mac
{
    public sealed class DemoListView : View
    {
        private readonly State<string> _scrollTo = "0";

        public DemoListView()
        {
            var listViewController = new ListViewController<Widget>();

            Child = new Column()
            {
                Children =
                {
                    new Row(VerticalAlignment.Middle, 20)
                    {
                        Children = new Widget[]
                        {
                            new Input(_scrollTo) { Width = 100 },
                            new Button("ScrollTo")
                            {
                                OnTap = e => listViewController.ScrollTo(
                                    int.Parse(_scrollTo.Value))
                            }
                        }
                    },
                    new Expanded { Child = ListView<Widget>.From(BuildList(), listViewController) }
                }
            };
        }

        private static Widget[] BuildList()
        {
            var list = new Widget[50];
            for (var i = 0; i < list.Length; i++)
            {
                list[i] = new Card()
                {
                    DebugLabel = $"Card{i.ToString()}",
                    Child = new Container()
                    {
                        BgColor = Colors.Random(), Height = 192,
                        Child = new Center()
                        {
                            DebugLabel = i.ToString(),
                            Child = new Text(i.ToString())
                        }
                    }
                };
            }

            return list;
        }
    }
}