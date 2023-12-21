using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PixUI.Dynamic.Design;

public sealed class IconDialog : Dialog
{
    public IconDialog(State<IconData?> state)
    {
        Title.Value = "Icons Picker";
        Width = 450;
        Height = 350;

        _state = state;
    }

    private readonly State<string> _keyword = string.Empty;
    private readonly State<IconData?> _state;

    protected override Widget BuildBody() => new Container
    {
        Padding = EdgeInsets.All(20),
        Child = new Column
        {
            Spacing = 10,
            Children =
            {
                new TextInput(_keyword) { Prefix = new Icon(MaterialIcons.Search) },
                new IconList(_keyword) { OnSelected = OnSelect },
            }
        }
    };

    protected override Widget BuildFooter() => new Container { Height = 0 };

    private void OnSelect(IconData data)
    {
        _state.Value = data;
        Close(DialogResult.OK);
    }
}

internal sealed class IconList : Widget, IScrollable
{
    public IconList(State<string> keyword)
    {
        _keyword = keyword;
        _filtered = _allIcons = GetAllIcons();
        _keyword.AddListener(RelayoutOnStateChanged);
    }

    private readonly List<ValueTuple<string, IconData>> _allIcons;
    private List<ValueTuple<string, IconData>> _filtered;
    private readonly State<string> _keyword;
    private readonly List<IconItem> _items = new();
    private Action<IconData>? _onSelected;

    public Action<IconData> OnSelected
    {
        set => _onSelected = value;
    }

    private List<ValueTuple<string, IconData>> GetAllIcons()
    {
        var list = new List<ValueTuple<string, IconData>>();
        var type = typeof(MaterialIcons);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
        foreach (var prop in properties)
        {
            if (prop.PropertyType != typeof(IconData))
                continue;
            list.Add((prop.Name, (IconData)prop.GetValue(null)!));
        }

        return list;
    }

    private void OnSelect(IconData data) => _onSelected?.Invoke(data);

    #region ====IScrollable====

    private readonly ScrollController _scrollController = new(ScrollDirection.Vertical);

    public float ScrollOffsetX => _scrollController.OffsetX;
    public float ScrollOffsetY => _scrollController.OffsetY;

    public Offset OnScroll(float dx, float dy)
    {
        if (_items.Count == 0) return Offset.Empty;

        var lastChild = _items[_items.Count - 1];
        if (lastChild.Y + lastChild.H <= H) return Offset.Empty;

        var maxOffsetX = 0f;
        var maxOffsetY = lastChild.Y + lastChild.H - H;

        var offset = _scrollController.OnScroll(dx, dy, maxOffsetX, maxOffsetY);
        if (!offset.IsEmpty)
            Repaint();
        return offset;
    }

    #endregion

    public override void VisitChildren(Func<Widget, bool> action)
    {
        foreach (var item in _items)
        {
            if (action(item)) break;
        }
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        SetSize(maxSize.Width, maxSize.Height);

        _filtered = string.IsNullOrEmpty(_keyword.Value)
            ? _allIcons
            : _allIcons.Where(t => t.Item1.Contains(_keyword.Value, StringComparison.OrdinalIgnoreCase)).ToList();

        _items.Clear();

        var ox = 0f;
        var oy = 0f;
        foreach (var tuple in _filtered)
        {
            var item = new IconItem( /*tuple.Item1, */tuple.Item2);
            item.OnTap = _ => OnSelect(tuple.Item2);
            item.Parent = this;
            item.Layout(float.MaxValue, float.MaxValue);
            item.SetPosition(ox, oy);
            ox += item.W;
            if (ox >= W)
            {
                ox = 0;
                oy += item.H;
            }

            _items.Add(item);
        }

        _scrollController.OffsetY = 0; //reset scroll offset
    }

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false,
        IDirtyArea? dirtyArea = null)
    {
        canvas.Save();
        canvas.Translate(X, Y);
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
        canvas.Translate(0, -ScrollOffsetY);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        foreach (var child in _items)
        {
            if (child.Y + child.H <= ScrollOffsetY) continue; //小于上边界
            if (child.Y >= ScrollOffsetY + H) break; //大于下边界

            child.BeforePaint(canvas);
            child.Paint(canvas);
            child.AfterPaint(canvas);
        }
    }

    protected internal override void AfterPaint(Canvas canvas) => canvas.Restore();
}

internal sealed class IconItem : Widget, IMouseRegion
{
    public IconItem( /*string name, */ IconData data)
    {
        // _name = TextPainter.BuildParagraph(name, float.MaxValue, 8, Colors.Black);
        _data = data;
        _iconPainter = new IconPainter(() => Repaint());

        MouseRegion = new MouseRegion(() => Cursors.Hand);
        MouseRegion.HoverChanged += OnHoverChanged;
    }

    // private readonly Paragraph _name;
    private readonly IconData _data;
    private readonly IconPainter _iconPainter;
    private bool _isHover;

    public MouseRegion MouseRegion { get; }

    public Action<PointerEvent> OnTap
    {
        set => MouseRegion.PointerTap += value;
    }

    private void OnHoverChanged(bool isHover)
    {
        _isHover = isHover;
        Repaint();
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        SetSize(50, 50);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_isHover)
        {
            var rrect = RRect.FromRectAndRadius(Rect.FromLTWH(0, 0, W, H), 5, 5);
            var paint = PaintUtils.Shared(Colors.Gray);
            canvas.DrawRRect(rrect, paint);

            _iconPainter.Paint(canvas, 30, 0xFFF7821B, _data, 10, 10);
        }
        else
        {
            _iconPainter.Paint(canvas, 30, Colors.Black, _data, 10, 10);
        }

        //canvas.DrawParagraph(_name, (W - _name.MaxIntrinsicWidth) / 2f, 40);
    }
}