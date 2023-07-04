using System;

namespace PixUI;

public sealed class MonthView : Widget, IMouseRegion
{
    public MonthView()
    {
        var today = DateTime.Now;
        _year = (ushort)today.Year;
        _month = (ushort)today.Month;
        InitMouseRegion();
    }

    public MonthView(int year, int month)
    {
        if (year < 1 || year > 9999 || month < 1 || month > 12) throw new ArgumentOutOfRangeException();

        _year = (ushort)year;
        _month = (ushort)month;
        InitMouseRegion();
    }

    private ushort _year;
    private ushort _month;
    private float _headerHeight = 26;
    private Size _cellSize;
    private Paragraph[]? _numberCache;
    private Paragraph[]? _weekCache;
    private byte _hitDay; //当前命中的日期，没有命中等于0
    private byte _selectedDay; //当前选择的日期，没有等于0

    public MouseRegion MouseRegion { get; private set; } = null!;

    private void InitMouseRegion()
    {
        MouseRegion = new MouseRegion();
        MouseRegion.PointerMove += OnPointerMove;
        MouseRegion.HoverChanged += OnHoverChanged;
        MouseRegion.PointerTap += OnPointerTap;
    }

    private byte HitTestForDay(float x, float y)
    {
        if (y > _headerHeight)
        {
            var hitRow = (int)Math.Truncate((y - _headerHeight) / _cellSize.Height);
            var hitCol = (int)Math.Truncate(x / _cellSize.Width);
            var firstDay = new DateTime(_year, _month, 1);
            var firstDayOffset = ((int)firstDay.DayOfWeek);
            var hitDay = (byte)(hitRow * 7 + hitCol - firstDayOffset + 1);
            if (hitDay < 1 || hitDay > DateTime.DaysInMonth(_year, _month)) hitDay = 0;
            //Log.Debug($"Hit row={hitRow} col={hitCol} hitDay={hitDay}");
            return hitDay;
        }

        return 0;
    }

    private void OnPointerMove(PointerEvent e)
    {
        if (e.Buttons != PointerButtons.None) return;

        var oldHitDay = _hitDay;
        _hitDay = HitTestForDay(e.X, e.Y);

        Cursor.Current = _hitDay != 0 ? Cursors.Hand : Cursors.Arrow;

        if (_hitDay != oldHitDay)
            Invalidate(InvalidAction.Repaint);
    }

    private void OnHoverChanged(bool isHover)
    {
        if (!isHover && _hitDay != 0)
        {
            _hitDay = 0;
            Cursor.Current = Cursors.Arrow;
            Invalidate(InvalidAction.Repaint);
        }
    }

    private void OnPointerTap(PointerEvent e)
    {
        var hitDay = HitTestForDay(e.X, e.Y);
        if (hitDay == _hitDay && _selectedDay != _hitDay)
        {
            _selectedDay = _hitDay;
            Invalidate(InvalidAction.Repaint);
        }
    }

    private Paragraph[] GenerateNumberCache()
    {
        var cache = new Paragraph[31];
        using var ts = new TextStyle() { Color = Colors.Black };
        for (var i = 1; i <= cache.Length; i++)
        {
            using var ps = new ParagraphStyle() { MaxLines = 1 };
            using var pb = new ParagraphBuilder(ps);
            pb.PushStyle(ts);
            pb.AddText(i.ToString());
            var ph = pb.Build();
            ph.Layout(float.MaxValue);
            cache[i - 1] = ph;
        }

        return cache;
    }

    private Paragraph[] GenerateWeekCache()
    {
        var cache = new Paragraph[7];
        using var ts = new TextStyle() { Color = Colors.Black };
        for (var i = 0; i < cache.Length; i++)
        {
            using var ps = new ParagraphStyle() { MaxLines = 1 };
            using var pb = new ParagraphBuilder(ps);
            pb.PushStyle(ts);
            var name = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetShortestDayName((DayOfWeek)i);
            pb.AddText(name);
            var ph = pb.Build();
            ph.Layout(float.MaxValue);
            cache[i] = ph;
        }

        return cache;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        SetSize(width, height);

        _cellSize = new Size(width / 7, (height - _headerHeight) / 6);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        PaintWeeks(canvas);

        _numberCache ??= GenerateNumberCache();

        var daysInMonth = DateTime.DaysInMonth(_year, _month);
        var firstDay = new DateTime(_year, _month, 1);

        var xIndex = ((int)firstDay.DayOfWeek);
        var yIndex = 0;
        var radius = _cellSize.Height / 2 - 2;
        for (var i = 0; i < daysInMonth; i++)
        {
            var para = _numberCache[i];
            var paraHeight = para.Height;
            var paraWidth = para.MaxIntrinsicWidth;
            var cx = (_cellSize.Width - paraWidth) / 2;
            var cy = (_cellSize.Height - paraHeight) / 2;

            if (_hitDay == i + 1 && _selectedDay != i + 1)
            {
                var paint = PaintUtils.Shared(new Color(0xFFAAAAAA) /*TODO: use Theme.HoverColor*/);
                paint.AntiAlias = true;
                canvas.DrawCircle(xIndex * _cellSize.Width + _cellSize.Width / 2,
                    _headerHeight + yIndex * _cellSize.Height + _cellSize.Height / 2,
                    radius, paint);
            }

            if (_selectedDay == i + 1)
            {
                var paint = PaintUtils.Shared(Theme.AccentColor);
                paint.AntiAlias = true;
                canvas.DrawCircle(xIndex * _cellSize.Width + _cellSize.Width / 2,
                    _headerHeight + yIndex * _cellSize.Height + _cellSize.Height / 2,
                    radius, paint);
            }

            if (i + 1 == DateTime.Today.Day)
            {
                var paint = PaintUtils.Shared(Colors.Red, PaintStyle.Stroke, 1.5f);
                paint.AntiAlias = true;
                canvas.DrawCircle(xIndex * _cellSize.Width + _cellSize.Width / 2,
                    _headerHeight + yIndex * _cellSize.Height + _cellSize.Height / 2,
                    radius, paint);
            }

            canvas.DrawParagraph(para, xIndex * _cellSize.Width + cx, _headerHeight + yIndex * _cellSize.Height + cy);
            xIndex++;
            if (xIndex >= 7)
            {
                yIndex++;
                xIndex = 0;
            }
        }
    }

    private void PaintWeeks(Canvas canvas)
    {
        _weekCache ??= GenerateWeekCache();

        for (var i = 0; i < 7; i++)
        {
            var para = _weekCache[i];
            var paraHeight = para.Height;
            var paraWidth = para.MaxIntrinsicWidth;
            var cx = (_cellSize.Width - paraWidth) / 2;
            var cy = (_headerHeight - paraHeight) / 2;
            canvas.DrawParagraph(para, i * _cellSize.Width + cx, cy);
        }
    }
}