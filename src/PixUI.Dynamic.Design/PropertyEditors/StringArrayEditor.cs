using System.Collections.Generic;
using System.Linq;

namespace PixUI.Dynamic.Design;

public sealed class StringArrayEditor : ValueEditorBase
{
    public StringArrayEditor(State<string[]?> state, DesignElement element) : base(element)
    {
        _state = state;

        Child = new Button("...") { Width = float.MaxValue, OnTap = OnTap };
    }

    private readonly State<string[]?> _state;

    private async void OnTap(PointerEvent _)
    {
        //编辑副本
        var list = new List<string>();
        if (_state.Value is { Length: > 0 })
            list.AddRange(_state.Value.ToList());

        var dlg = new StringArrayDialog(list);
        var dlgResult = await dlg.ShowAsync();
        if (dlgResult != DialogResult.OK) return;

        _state.Value = list.ToArray();
    }
}

public sealed class StringArrayDialog : Dialog
{
    public StringArrayDialog(List<string> list)
    {
        Title.Value = "String Array";
        Width = 250;
        Height = 300;

        _list = list;
    }

    private readonly List<string> _list;
    private static readonly State<float> _buttonSize = 18f;
    private readonly ListViewController<string> _listController = new();

    protected override Widget BuildBody() => new Container
    {
        Padding = EdgeInsets.All(20),
        Child = new Column(alignment: HorizontalAlignment.Left, spacing: 5)
        {
            Children =
            {
                new Button("Add", MaterialIcons.Add) { OnTap = _ => OnAdd() },
                new Expanded(BuildListView())
            }
        }
    };

    private Widget BuildListView() => new ListView<string>(
        (_, i) =>
        {
            var text = new RxProxy<string>(() => _list[i], v => _list[i] = v);
            var notFirst = new RxProxy<bool>(() => i != 0);
            var notLast = new RxProxy<bool>(() => i != _list.Count - 1);
            return new Card
            {
                Child = new Row
                {
                    Children =
                    {
                        new Expanded(new TextInput(text)),
                        new Button(icon: MaterialIcons.Clear)
                        {
                            Style = ButtonStyle.Transparent,
                            Width = _buttonSize,
                            Height = _buttonSize,
                            OnTap = _ => OnRemove(i),
                        },
                        new IfConditional(notFirst, () => new Button(icon: MaterialIcons.ArrowUpward)
                        {
                            Style = ButtonStyle.Transparent,
                            Width = _buttonSize,
                            Height = _buttonSize,
                            OnTap = _ => OnMoveUp(i),
                        }, () => new Container() { Width = _buttonSize, Height = _buttonSize }),
                        new IfConditional(notLast, () => new Button(icon: MaterialIcons.ArrowDownward)
                        {
                            Style = ButtonStyle.Transparent,
                            Width = _buttonSize,
                            Height = _buttonSize,
                            OnTap = _ => OnMoveDonw(i),
                        }, () => new Container() { Width = _buttonSize, Height = _buttonSize })
                    }
                }
            };
        },
        dataSource: _list,
        controller: _listController
    );

    private void OnAdd()
    {
        _list.Add(string.Empty);
        _listController.DataSource = _list;
    }

    private void OnRemove(int index)
    {
        _list.RemoveAt(index);
        _listController.DataSource = _list;
    }

    private void OnMoveUp(int index)
    {
        (_list[index - 1], _list[index]) = (_list[index], _list[index - 1]);
        _listController.DataSource = _list;
    }

    private void OnMoveDonw(int index)
    {
        (_list[index + 1], _list[index]) = (_list[index], _list[index + 1]);
        _listController.DataSource = _list;
    }
}