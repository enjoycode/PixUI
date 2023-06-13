using System;

namespace PixUI;

/// <summary>
/// 包装TabBar及TabBody
/// </summary>
public sealed class TabView<T> : Widget
{
    public TabView(TabController<T> controller,
        Func<T, State<bool>, Widget> tabBuilder,
        Func<T, Widget> bodyBuilder,
        bool closable = false, float tabBarIndent = 35)
    {
        _tabBarIndent = tabBarIndent;
        _tabBody = new TabBody<T>(controller, bodyBuilder);
        _tabBar = new TabBar<T>(controller, (data, tab) =>
        {
            tab.Child = new Container()
            {
                IsLayoutTight = true,
                Padding = EdgeInsets.Only(10, 2, closable ? 0 : 10, 2),
                Child = closable
                    ? new Row()
                    {
                        Children = new Widget[]
                        {
                            tabBuilder(data, tab.IsSelected),
                            new Button(null, MaterialIcons.Close)
                            {
                                Style = ButtonStyle.Transparent,
                                Shape = ButtonShape.Pills,
                                OnTap = _ => controller.Remove(data)
                            }
                        }
                    }
                    : tabBuilder(data, tab.IsSelected)
            };
        }, true);

        _tabBody.Parent = this;
        _tabBar.Parent = this;
    }

    private readonly TabBar<T> _tabBar;
    private readonly TabBody<T> _tabBody;
    private readonly float _tabBarIndent;

    public Color? TabBarBgColor
    {
        get => _tabBar.BgColor;
        set => _tabBar.BgColor = value;
    }

    public Color? SelectedTabColor
    {
        get => _tabBar.SelectedColor;
        set => _tabBar.SelectedColor = value;
    }

    public Color? HoverTabColor
    {
        get => _tabBar.HoverColor;
        set => _tabBar.HoverColor = value;
    }

    #region ====Widget Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (action(_tabBar)) return;
        action(_tabBody);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        //TODO:支持上、下、左、右布局
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        _tabBar.Layout(width, _tabBarIndent);
        _tabBar.SetPosition(0, 0);
        _tabBody.Layout(width, height - _tabBar.H);
        _tabBody.SetPosition(0, _tabBar.H);

        SetSize(width, height);
    }

    #endregion
}