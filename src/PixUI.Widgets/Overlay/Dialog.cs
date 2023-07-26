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
    protected readonly State<string> Title = "";
    private TaskCompletionSource<bool>? _closeDone;

    protected sealed override bool IsDialog => true;

    #region ====Build====

    private void TryBuildChild()
    {
        if (_child != null) return;

        _child = new Card()
        {
            Elevation = 20,
            Child = new Column()
            {
                Children =
                {
                    BuildTitle(),
                    new Expanded(BuildBody()),
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
                new Container() { Width = 35 }, //TODO: SizeBox
                new Expanded()
                {
                    Child = new Center() { Child = new Text(Title) }
                },
                new Button(null, MaterialIcons.Close)
                {
                    Style = ButtonStyle.Transparent,
                    OnTap = _ => Close(true),
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
        return new Container()
        {
            Height = Button.DefaultHeight + 20 + 20,
            Padding = EdgeInsets.All(20),
            Child = new Row(VerticalAlignment.Middle, 20)
            {
                Children =
                {
                    new Expanded(),
                    new Button("Cancel") { Width = 80, OnTap = _ => Close(true) },
                    new Button("OK") { Width = 80, OnTap = _ => Close(false) }
                }
            }
        };
    }

    #endregion

    #region ====Show & Close====

    public void Show()
        => base.Show(null, null, DialogTransitionBuilder);

    /// <summary>
    /// 显示并等待关闭
    /// </summary>
    /// <returns>true=canceled</returns>
    public Task<bool> ShowAndWaitClose()
    {
        Show();
        _closeDone = new TaskCompletionSource<bool>();
        // @ts-ignore
        return _closeDone.Task;
    }

    /// <summary>
    /// 关闭前处理
    /// </summary>
    /// <returns>true=abort close</returns>
    protected virtual bool OnClosing(bool canceled) => false;

    protected void Close(bool canceled)
    {
        if (OnClosing(canceled)) return; //aborted
            
        Hide();
        _closeDone?.SetResult(canceled);
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

    public override void VisitChildren(Func<Widget, bool> action) => action(_child!);

    public override bool ContainsPoint(float x, float y) => true;

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        //always hit dialog
        result.Add(this);
        HitTestChild(_child!, x, y, result);
        return true;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        TryBuildChild();
        _child!.Layout(Width?.Value ?? availableWidth, Height?.Value ?? availableHeight);
        //不用设置_child位置,显示时设置自身位置，另外不能设置自身大小为无限，因为弹出动画需要
        SetSize(_child.W, _child.H);
    }

    #endregion
}