using System.Collections.Generic;

namespace PixUI.Demo.Mac
{
    public class DemoTabView : View
    {
        private readonly List<TabItem> _icons = new()
        {
            new TabItem { Icon = MaterialIcons.Cloud, Text = "Cloud" },
            new TabItem { Icon = MaterialIcons.BeachAccess, Text = "Long Beach" },
            new TabItem { Icon = MaterialIcons.Sunny, Text = "Sunny" }
        };

        private readonly TabController<TabItem> _tabController;

        public DemoTabView()
        {
            _tabController = new TabController<TabItem>(_icons);

            Child = new Container()
            {
                Padding = EdgeInsets.All(20),
                Child = new TabView<TabItem>(_tabController, BuildTab, BuildBody, true)
                    { SelectedTabColor = Colors.Gray, HoverTabColor = Theme.AccentColor }
            };
        }

        private static Widget BuildTab(TabItem data, State<bool> isSelected)
        {
            var color = RxComputed<Color>.Make(
                isSelected,
                selected => selected ? Theme.AccentColor : new Color(200, 200, 200)
            );
            return new Text(data.Text) { TextColor = color };
            //tab.Child = new Icon(data.Icon) { Color = color, Size = 20 };
        }

        private static Widget BuildBody(TabItem data)
        {
            return new Container()
            {
                Padding = EdgeInsets.All(10),
                FillColor = new Color(0xFFDCDCDC),
                Child = new Text(data.Text),
            };
        }
    }

    internal struct TabItem
    {
        public IconData Icon;
        public string Text;
    }
}