using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace PixUI;

public interface IDirtyArea
{
    /// <summary>
    /// 已存在的无效区域合并新的无效区域
    /// </summary>
    void Merge(IDirtyArea? newArea);

    /// <summary>
    /// 获取脏区域的外围
    /// </summary>
    Rect GetRect();

    /// <summary>
    /// 是否与子级相交
    /// </summary>
    bool IntersectsWith(Widget child);

    /// <summary>
    /// 转换为子级对应的脏区域
    /// </summary>
    IDirtyArea? ToChild(Widget child);
}

/// <summary>
/// 重绘指定Rect的区域
/// </summary>
public sealed class RepaintArea : IDirtyArea
{
    private readonly Rect _rect;

    public RepaintArea(Rect rect)
    {
        _rect = rect;
    }

    public Rect GetRect() => _rect;

    public void Merge(IDirtyArea? newArea)
    {
        //TODO:
    }

    public bool IntersectsWith(Widget child)
    {
        return _rect.IntersectsWith(child.X, child.Y, child.W, child.H);
    }

    public IDirtyArea? ToChild(Widget child)
    {
        if (child.X == 0 && child.Y == 0) return this;

        var childRect = Rect.FromLTWH(_rect.Left - child.X, _rect.Top - child.Y,
            _rect.Width, _rect.Height);
        return new RepaintArea(childRect);
    }

    public override string ToString() => $"RepaintArea[{_rect}]";
}

/// <summary>
/// 用于重绘指定的子组件
/// </summary>
internal sealed class RepaintChild : IDirtyArea
{
    private readonly IDirtyArea? _lastDirtyArea;
    private readonly List<Widget> _path;
    private int _current;

    public RepaintChild(Widget from, Widget to, IDirtyArea? lastDirtyArea)
    {
        _lastDirtyArea = lastDirtyArea;
        _path = new List<Widget>();
        Widget temp = to;
        while (!ReferenceEquals(temp, from))
        {
            _path.Add(temp);
            temp = temp.Parent!;
        }

        _current = _path.Count - 1;
    }

    public Widget Child
    {
        get
        {
            Debug.Assert(_current >= 0 && _current < _path.Count);
            return _path[_current];
        }
    }

    public void Merge(IDirtyArea? newArea) => throw new NotSupportedException();

    public bool IntersectsWith(Widget child)
    {
        if (_current < 0) return false;
        var cur = _path[_current];
        return ReferenceEquals(cur, child);
    }

    public Rect GetRect()
    {
        var cur = _path[_current];
        return Rect.FromLTWH(cur.X, cur.Y, cur.W, cur.H);
    }

    public IDirtyArea? ToChild(Widget child)
    {
        //防止重写了具备多个子组件的Paint方法，但忘了处理RepaintChild的情况
        if (!ReferenceEquals(_path[_current], child))
        {
            Log.Debug($"[{child.Parent!.GetType().Name}]重写了Paint，但未处理RepaintChild");
            return null;
        }

        _current--;
        //Console.WriteLine($"ToChild CUR={_current} Child={child.GetType().Name} Parent={child.Parent!.GetType().Name}");
        return _current < 0 ? _lastDirtyArea : this;
    }

    public override string ToString()
    {
        if (_current < 0)
            return _lastDirtyArea == null ? string.Empty : _lastDirtyArea.ToString();

        var cur = _path[_current];
        return $"RepaintChild[{cur}]";
    }
}