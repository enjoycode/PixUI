using System;

namespace PixUI.Demo
{
    public sealed class DemoPage : View
    {
        private readonly State<string> _firstName = "Rick";
        private readonly State<string> _lastName = "Lg";
        private readonly RxComputed<string> _fullName;
        private readonly State<ImageSource> _imgSrc;

        private readonly State<string?> _selectedValue = "";

        private readonly WidgetRef<Button> _buttonRef = new();
        private ListPopup<Person>? _listPopup;

        public DemoPage()
        {
            _fullName = Compute(_firstName, _lastName, (first, last) => "Full:" + first + "-" + last);

            var imgData = ResourceLoad.LoadBytes("Resources.Cat.webp");
            Rx<ImageSource> defaultImgSrc = ImageSource.FromEncodedData(imgData);
            _imgSrc = Bind(defaultImgSrc, BindingOptions.AffectsVisual);

            Child = BuildChild();
        }

        private Widget BuildChild()
        {
            return new Column(debugLabel: "DemoMain")
            {
                Children =
                {
                    //Body
                    new Expanded { Child = BuildBody() },
                    //Footer
                    new Container { Height = 20, BgColor = new Color(0xFFCA673B), DebugLabel = "DemoFooter" }
                }
            };
        }

        private Widget BuildBody()
        {
            return new Row(spacing: 8)
            {
                Children =
                {
                    new Column(spacing: 8, debugLabel: "DemoBodyColumn1")
                    {
                        Children =
                        {
                            new Text(_firstName) { FontSize = 20, TextColor = Colors.Red },
                            new Text(_lastName) { FontSize = 20, TextColor = Colors.Red },
                            new Text(_fullName) { FontSize = 50, TextColor = Colors.Red },
                            new Button("Click Me", MaterialIcons.Search) { OnTap = OnButtonTap },
                            new ButtonGroup()
                            {
                                Children = new[]
                                {
                                    new Button("Button1") { OnTap = OnButton1Tap, Ref = _buttonRef },
                                    new Button("Button2") { OnTap = OnButton2Tap },
                                    new Button("Button3") { OnTap = OnButton3Tap },
                                },
                            },
                            new Row(VerticalAlignment.Middle, 10)
                            {
                                Children = new Widget[]
                                {
                                    new Switch(false),
                                    new Checkbox(false),
                                    Checkbox.Tristate(false),
                                    new Radio(false),
                                }
                            },
                            new Select<string>(_selectedValue)
                            {
                                Width = 200, Options = new[]
                                {
                                    "无锡", "上海", "苏州"
                                }
                            },
                            new Input("Hello World!")
                            {
                                Width = 200,
                                Prefix = new Icon(MaterialIcons.Person),
                                Suffix = new Icon(MaterialIcons.Search)
                            },
                            new Input("")
                            {
                                Width = 200, IsObscure = true,
                                Prefix = new Icon(MaterialIcons.Lock),
                                HintText = "Password",
                            },
                            new Card
                            {
                                Elevation = 2, Width = 200, Height = 266,
                                Child = new ImageBox(_imgSrc)
                                {
                                    Width = 200,
                                    Height = 266
                                }
                            }
                        }
                    },
                    new Column(spacing: 8, debugLabel: "DemoBodyColumn2")
                    {
                        Children =
                        {
                            new Card { Width = 250, Height = 220, Child = new Calendar(1977, 3) },
                            new DatePicker(new DateTime(1977, 3, 16)) {Width = 200},
                        }
                    }
                }
            };
        }

        private void OnButtonTap(PointerEvent e)
        {
            _firstName.Value = "Eric " + DateTime.Now.Second;

            Notification.Error("Click Done!");
        }

        private void OnButton1Tap(PointerEvent e)
        {
            _listPopup ??= new ListPopup<Person>(Overlay!, BuidPopupItem, 200, 25)
                { OnSelectionChanged = OnListPopupSelectionChanged };
            _listPopup.DataSource ??= Person.GeneratePersons(10);
            if (!_listPopup.IsMounted)
                _listPopup.Show(_buttonRef.Widget, new Offset(-4, -2), Popup.DefaultTransitionBuilder);
            else
                _listPopup?.Hide();
        }

        private async void OnButton2Tap(PointerEvent e)
        {
            var dlg = new DemoDialog();
            var canceled = await dlg.ShowAndWaitClose();
            Console.WriteLine($"Dialog closed: {canceled}");
        }

        private void OnButton3Tap(PointerEvent e)
        {
            var menuItems = new MenuItem[]
            {
                MenuItem.Item("Copy"),
                MenuItem.Item("Paste"),
                MenuItem.Divider(),
                MenuItem.SubMenu("Actions", null, new MenuItem[]
                {
                    MenuItem.Item("Action1"),
                    MenuItem.SubMenu("Actions2", null, new MenuItem[]
                    {
                        MenuItem.Item("Action3"),
                        MenuItem.Item("Action4"),
                    })
                })
            };
            ContextMenu.Show(menuItems);
        }

        private void OnListPopupSelectionChanged(Person? person)
        {
            _lastName.Value = person == null ? "" : person.Name;
        }

        private Widget BuidPopupItem(Person person, int index, State<bool> isHover,
            State<bool> isSelected)
        {
            var color = RxComputed<Color>.Make(isSelected,
                v => v ? Colors.White : Colors.Black);
            return new Text(person.Name) { TextColor = color };
        }

        protected override void OnMounted()
        {
            Console.WriteLine("DemoPage Mounted +++");
            base.OnMounted();
        }

        protected override void OnUnmounted()
        {
            Console.WriteLine("DemoPage Unmounted ---");
            base.OnUnmounted();
        }
    }

    // class DemoShadow : Widget
    // {
    //     protected override void Layout(float availableWidth, float availableHeight)
    //     {
    //         W = 192;
    //         H = 92;
    //     }
    //
    //     protected override void Paint(Canvas canvas, IDirtyArea? area = null)
    //     {
    //         var rect = Rect.FromLTWH(0, 0, W, H);
    //         using var path = new Path();
    //         path.AddRect(rect);
    //         // Console.WriteLine($"Path Bounds = {path.Bounds}");
    //         canvas.DrawShadow(path, Colors.Green, 5, false, Root!.Window.ScaleFactor);
    //
    //         canvas.DrawRect(rect, PixUI.Paint.Default(color: Colors.White));
    //     }
    // }
}