using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PixUI;

internal sealed class NotificationEntry : SingleChildWidget
{
    private readonly AnimationController _controller = new(100);

    public NotificationEntry(Icon icon, Text text, State<Color> bgColor)
    {
        _controller.ValueChanged += OnAnimationValueChanged;
        _controller.StatusChanged += OnAnimationStateChanged;

        Child = new Card
        {
            Width = 280,
            Color = bgColor,
            Child = new Row(VerticalAlignment.Middle, 5)
            {
                Children =
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

    private static readonly State<Color> _textColor = Colors.White;
    private static readonly State<Color> _infoBgColor = new Color(0xFF0288D1);
    private static readonly State<Color> _warnBgColor = new Color(0xFFED6C02);
    private static readonly State<Color> _errorBgColor = new Color(0xFFD32F2F);
    private static readonly State<Color> _successBgColor = new Color(0xFF2E7D32);

    private static void Show(IconData icon, string text, State<Color> textColor, State<Color> bgColor)
    {
        var iconWidget = new Icon(icon) { Size = 18, Color = textColor };
        var textWidget = new Text(text) { TextColor = textColor, MaxLines = 5 };

        var win = UIWindow.Current;
        var exists = win.Overlay.FindEntry(e => e is Notification);
        var notification = exists == null
            ? new Notification(win.Overlay)
            : (Notification)exists;
        if (exists == null)
            notification.Show();

        var entry = new NotificationEntry(iconWidget, textWidget, bgColor);
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

    public static void Info(string message) =>
        Show(MaterialIcons.Info, message, _textColor, _infoBgColor);

    public static void Success(string message) =>
        Show(MaterialIcons.CheckCircle, message, _textColor, _successBgColor);

    public static void Warn(string message) =>
        Show(MaterialIcons.Warning, message, _textColor, _warnBgColor);

    public static void Error(string message) =>
        Show(MaterialIcons.Error, message, _textColor, _errorBgColor);

    #endregion
}