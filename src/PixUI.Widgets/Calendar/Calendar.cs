using System;

namespace PixUI;

public sealed class Calendar : Widget
{
    public Calendar(State<int> year, State<int> month)
    {
        _monthView = new MonthView(year, month);
        _monthView.Parent = this;
        NaviBarTitle = Compute(_monthView.Year, _monthView.Month, (y, m) => $"{y}年{m}月");

        _naviBar = new CalendarNaviBar(this);
        _naviBar.Parent = this;
    }

    public Calendar(State<DateTime?> selectedDate)
    {
        _monthView = new MonthView(selectedDate);
        _monthView.Parent = this;
        NaviBarTitle = Compute(_monthView.Year, _monthView.Month, (y, m) => $"{y}年{m}月");

        _naviBar = new CalendarNaviBar(this);
        _naviBar.Parent = this;
    }

    private readonly CalendarNaviBar _naviBar;
    private readonly MonthView _monthView;
    internal readonly State<string> NaviBarTitle;

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (action(_naviBar)) return;
        action(_monthView);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);
        SetSize(width, height);

        _naviBar.Layout(width, height);
        _naviBar.SetPosition(0, 0);

        _monthView.Layout(width, H - _naviBar.H);
        _monthView.SetPosition(0, _naviBar.H);
    }

    #region ====Navigation====

    internal void OnPrevYear()
    {
        if (_monthView.Year.Value > 1) _monthView.Year.Value -= 1;
    }

    internal void OnPrevMonth()
    {
        if (_monthView.Month.Value == 1)
        {
            _monthView.Year.Value -= 1;
            _monthView.Month.Value = 12;
        }
        else
        {
            _monthView.Month.Value -= 1;
        }
    }

    internal void OnNextYear()
    {
        if (_monthView.Year.Value < 9999) _monthView.Year.Value += 1;
    }

    internal void OnNextMonth()
    {
        if (_monthView.Month.Value == 12)
        {
            _monthView.Year.Value += 1;
            _monthView.Month.Value = 1;
        }
        else
        {
            _monthView.Month.Value += 1;
        }
    }

    #endregion
}

internal sealed class CalendarNaviBar : Widget
{
    public CalendarNaviBar(Calendar owner)
    {
        // _owner = owner;
        _btPrevYear = BuildNaviButton(MaterialIcons.KeyboardDoubleArrowLeft, owner.OnPrevYear);
        _btNextYear = BuildNaviButton(MaterialIcons.KeyboardDoubleArrowRight, owner.OnNextYear);
        _btPrevMonth = BuildNaviButton(MaterialIcons.KeyboardArrowLeft, owner.OnPrevMonth);
        _btNextMonth = BuildNaviButton(MaterialIcons.KeyboardArrowRight, owner.OnNextMonth);

        _title = new Text(owner.NaviBarTitle);
        _title.Parent = this;
    }

    // private readonly Calendar _owner;
    private readonly Button _btPrevYear;
    private readonly Button _btPrevMonth;
    private readonly Button _btNextYear;
    private readonly Button _btNextMonth;
    private readonly Text _title;

    private Button BuildNaviButton(IconData icon, Action action)
    {
        var button = new Button(icon: icon)
        {
            Style = ButtonStyle.Transparent,
            Shape = ButtonShape.Pills,
            FontSize = 20,
            OnTap = _ => action(),
        };

        button.Parent = this;
        button.Layout(float.MaxValue, float.MaxValue);

        return button;
    }

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (action(_btPrevYear)) return;
        if (action(_btPrevMonth)) return;
        if (action(_btNextMonth)) return;
        if (action(_btNextYear)) return;
        action(_title);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);
        SetSize(width, Math.Min(height, _btPrevYear.H));

        var offsetX = 5f;
        const float offsetY = 5f;
        const float space = 5f;
        _btPrevYear.SetPosition(offsetX, offsetY);
        offsetX += _btPrevYear.W + space;
        _btPrevMonth.SetPosition(offsetX, offsetY);
        offsetX += _btPrevMonth.W + space;

        var centerWidth = W - _btPrevYear.W * 4 - space * 6;
        _title.Layout(centerWidth, height);
        _title.SetPosition(offsetX + (centerWidth - _title.W) / 2, offsetY + 4 /*基线偏移*/);
        offsetX += centerWidth;

        _btNextMonth.SetPosition(offsetX, offsetY);
        offsetX += _btNextMonth.W + space;
        _btNextYear.SetPosition(offsetX, offsetY);
    }
}