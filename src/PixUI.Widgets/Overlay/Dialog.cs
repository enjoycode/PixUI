using System;
using System.Threading.Tasks;

namespace PixUI;

public abstract class Dialog : Popup
{
    protected Dialog(Overlay? overlay = null) : base(overlay ?? UIWindow.Current.Overlay)
    {
        //注意不在这里构建WidgetTree,参照以下OnMounted时的说明
    }

    private Card? _child;
    private Widget? _body;
    protected readonly State<string> Title = "";
    private TaskCompletionSource<DialogResult>? _closeDone;

    protected sealed override bool IsDialog => true;

    #region ====Build====

    private void TryBuildChild()
    {
        if (_child != null) return;

        _body = BuildBody();
        _child = new Card
        {
            Elevation = 20,
            Child = new Column
            {
                Children =
                {
                    BuildTitle(),
                    new Expanded(_body),
                    BuildFooter()
                }
            }
        };
        _child.Parent = this;
    }

    private Widget BuildTitle()
    {
        return new Row()
        {
            Height = 25,
            Children =
            {
                new Container { Width = 35 }, //TODO: SizeBox
                new Expanded
                {
                    Child = new Center { Child = new Text(Title) }
                },
                new Button(null, MaterialIcons.Close)
                {
                    Style = ButtonStyle.Transparent,
                    OnTap = _ => Close(DialogResult.None),
                },
            }
        };
    }

    protected abstract Widget BuildBody();

    /// <summary>
    /// 构建对话框的Footer, 必须指定高度
    /// </summary>
    protected virtual Widget BuildFooter()
    {
        return new Container
        {
            Height = Button.DefaultHeight + 20 + 20,
            Padding = EdgeInsets.All(20),
            Child = new Row(VerticalAlignment.Middle, 20)
            {
                Children =
                {
                    new Expanded(),
                    new Button(nameof(DialogResult.Cancel)) { Width = 80, OnTap = _ => Close(DialogResult.Cancel) },
                    new Button(nameof(DialogResult.OK)) { Width = 80, OnTap = _ => Close(DialogResult.OK) }
                }
            }
        };
    }

    #endregion

    #region ====Show & Close====

    public static Dialog Show(string title, Func<Dialog, Widget> bodyBuilder,
        Func<Dialog, Widget>? footerBuilder = null, Size? size = null)
    {
        var dlg = new WrapDialog(title, bodyBuilder, footerBuilder, size);
        dlg.Show();
        return dlg;
    }

    public static Task<DialogResult> ShowAsync(string title, Func<Dialog, Widget> bodyBuilder,
        Func<Dialog, Widget>? footerBuilder = null, Size? size = null)
    {
        var dlg = new WrapDialog(title, bodyBuilder, footerBuilder, size);
        return dlg.ShowAsync();
    }

    /// <summary>
    /// 显示确认操作的对话框
    /// </summary>
    public static Task<DialogResult> ShowConfirmAsync(string title, string message, Size? size = null)
    {
        return ShowAsync(title,
            _ => new Center()
            {
                Child = new Row()
                {
                    Children =
                    [
                        new Icon(MaterialIcons.Help) { Size = 25, Color = Colors.Red },
                        new Text(message) { TextColor = Colors.Red }
                    ]
                }
            },
            dlg => new Container
            {
                Height = Button.DefaultHeight + 20 + 20,
                Padding = EdgeInsets.All(20),
                Child = new Row(VerticalAlignment.Middle, 20)
                {
                    Children =
                    {
                        new Expanded(),
                        new Button(nameof(DialogResult.No)) { Width = 80, OnTap = _ => dlg.Close(DialogResult.No) },
                        new Button(nameof(DialogResult.Yes)) { Width = 80, OnTap = _ => dlg.Close(DialogResult.Yes) }
                    }
                }
            },
            size ?? new(280, 180)
        );
    }

    /// <summary>
    /// 显示简单文本输入框的对话框
    /// </summary>
    public static Task<DialogResult> ShowTextInputAsync(string title, string label, State<string> value,
        Size? size = null)
    {
        return ShowAsync(title,
            _ => new Container()
            {
                Padding = EdgeInsets.Only(20, 20, 20, 0),
                Child = new Row()
                {
                    Children =
                    [
                        new Text(label),
                        new TextInput(value),
                    ]
                }
            },
            dlg => new Container
            {
                Height = Button.DefaultHeight + 20 + 20,
                Padding = EdgeInsets.All(20),
                Child = new Row(VerticalAlignment.Middle, 20)
                {
                    Children =
                    {
                        new Expanded(),
                        new Button(nameof(DialogResult.Cancel))
                            { Width = 80, OnTap = _ => dlg.Close(DialogResult.Cancel) },
                        new Button(nameof(DialogResult.OK)) { Width = 80, OnTap = _ => dlg.Close(DialogResult.OK) }
                    }
                }
            },
            size ?? new(280, 180)
        );
    }

    /// <summary>
    /// 显示不等待关闭
    /// </summary>
    public void Show()
    {
        base.Show(null, null, DialogTransitionBuilder);

        if (_body != null)
        {
            var focusable = FocusManager.FindFocusableForward(_body, null);
            if (focusable != null && Overlay != null)
                FocusManager.Focus(focusable, Overlay.Window);
        }
    }

    /// <summary>
    /// 显示并等待关闭
    /// </summary>
    public Task<DialogResult> ShowAsync()
    {
        Show();
        _closeDone = new TaskCompletionSource<DialogResult>();
        return _closeDone.Task;
    }

    /// <summary>
    /// 关闭前处理
    /// </summary>
    /// <returns>true=abort close</returns>
    protected virtual ValueTask<bool> OnClosing(DialogResult result) => new(false);

    public async void Close(DialogResult result)
    {
        if (await OnClosing(result)) return; //aborted
        Hide();
        _closeDone?.SetResult(result);
    }

    #endregion

    #region ====Widget Overrides====

    protected override void OnMounted()
    {
        //由于转换为Web后，继承自Dialog构造的初始化顺序问题, 所以在这里构建WidgetTree
        // class SomeDialog extends Dialog<string> {
        //      private State<string> _someState = "Hello";
        //      constructor(overlay: Overlay) {
        //          super(overlay); //如果在这里构建WidgetTree,则_someState为undefined
        //      }
        // }
        TryBuildChild();
        base.OnMounted();
    }

    public override void VisitChildren<TVisitor>(ref TVisitor visitor) => visitor.Visit(_child!);

    public override bool ContainsPoint(float x, float y) => true;

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        //always hit dialog
        result.Add(this);
        HitTestChild(_child!, x, y, result);
        return true;
    }

    protected override void OnLayout(Size maxSize)
    {
        TryBuildChild();
        _child!.PerformLayout(Width?.Value ?? AvailableSize.Width, Height?.Value ?? AvailableSize.Height);
        //不用设置_child位置,显示时设置自身位置，另外不能设置自身大小为无限，因为弹出动画需要
        SetLayoutSize(_child.W, _child.H);
    }

    #endregion
}