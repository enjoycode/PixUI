using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PixUI;

internal sealed class NotificationEntry : SingleChildWidget
{
    private readonly AnimationController _controller = new AnimationController(100);

    public NotificationEntry(Icon icon, Text text)
    {
        _controller.ValueChanged += OnAnimationValueChanged;
        _controller.StatusChanged += OnAnimationStateChanged;

        Child = new Card()
        {
            Width = 280,
            Child = new Row(VerticalAlignment.Middle, 5)
            {
                Children = new Widget[]
                {
                    icon,
                    new Expanded() { Child = text },
                    new Button(null, MaterialIcons.Close)
                    {
                        Style = ButtonStyle.Transparent,
                        Shape = ButtonShape.Pills,
                        OnTap = _ => _controller.Reverse()
                    }
                }
            }
        };
    }

    private void OnAnimationValueChanged()
    {
        var offsetX = Overlay!.Window.Width - W * _controller.Value;
        SetPosition((float)offsetX, Y);
        Invalidate(InvalidAction.Repaint);
    }

    private void OnAnimationStateChanged(AnimationStatus status)
    {
        if (status == AnimationStatus.Completed)
            StartHide();
        else if (status == AnimationStatus.Dismissed)
            ((Notification)Parent!).RemoveEntry(this);
    }

    private async void StartHide()
    {
        await Task.Delay(3000);
        _controller.Reverse();
    }

    internal void StartShow()
    {
        _controller.Forward();
    }

    public override void Dispose()
    {
        _controller.Dispose();
        base.Dispose();
    }
}

public sealed class Notification : Popup
{
    private const float _firstOffset = 10f;
    private const float _sepSpace = 2f;

    private Notification(Overlay overlay) : base(overlay) { }

    private readonly IList<NotificationEntry> _children = new List<NotificationEntry>();

    internal void RemoveEntry(NotificationEntry entry)
    {
        var index = _children.IndexOf(entry);
        var entryHeight = entry.H;
        for (var i = index + 1; i < _children.Count; i++)
        {
            _children[i].SetPosition(_children[i].X, _children[i].Y - entryHeight);
        }

        _children.RemoveAt(index);
        Invalidate(InvalidAction.Repaint);
    }

    #region ====Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        foreach (var child in _children)
        {
            if (action(child)) break;
        }
    }

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        foreach (var child in _children)
        {
            if (HitTestChild(child, x, y, result)) return true;
        }

        return false;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        //do nothing,加入前已经手动布局过
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        foreach (var child in _children)
        {
            canvas.Translate(child.X, child.Y);
            child.Paint(canvas, area);
            canvas.Translate(-child.X, -child.Y);
        }
    }

    #endregion

    #region ====Static Show Methods====

    private static void Show(Icon icon, Text text)
    {
        var win = UIWindow.Current;
        var exists = win.Overlay.FindEntry(e => e is Notification);
        var notification = exists == null
            ? new Notification(win.Overlay)
            : (Notification)exists;
        if (exists == null)
            notification.Show();

        var entry = new NotificationEntry(icon, text);
        entry.Parent = notification;
        //布局并设置位置
        entry.Layout(float.PositiveInfinity, float.PositiveInfinity);
        var childrenCount = notification._children.Count;
        var yPos = childrenCount == 0
            ? _firstOffset
            : notification._children[childrenCount - 1].Y +
              notification._children[childrenCount - 1].H + _sepSpace;
        entry.SetPosition(win.Width, yPos);
        notification._children.Add(entry);

        entry.StartShow();
    }

    public static void Info(string message)
    {
        State<Color> color = Colors.Gray;
        Show(new Icon(MaterialIcons.Info) { Size = 18, Color = color },
            new Text(message) { TextColor = color, MaxLines = 5});
    }

    public static void Success(string message)
    {
        State<Color> color = Colors.Green;
        Show(new Icon(MaterialIcons.Error) { Size = 18, Color = color },
            new Text(message) { TextColor = color, MaxLines = 5});
    }

    public static void Error(string message)
    {
        State<Color> color = Colors.Red;
        Show(new Icon(MaterialIcons.Error) { Size = 18, Color = color },
            new Text(message) { TextColor = color, MaxLines = 5 });
    }

    #endregion
}