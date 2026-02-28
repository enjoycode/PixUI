using System;

namespace PixUI;

public enum Visibility
{
    /// <summary>
    /// 显示子组件
    /// </summary>
    Visible,

    /// <summary>
    /// 不显示子组件，但保留其布局
    /// </summary>
    Hidden,

    /// <summary>
    /// 不显示子组件，不保留其布局
    /// </summary>
    Collapsed
}

/// <summary>
/// 通过状态控制包含的子组件是否显示
/// </summary>
public sealed class Visible : SingleChildWidget
{
    public Visible(State<Visibility>? visibility = null)
    {
        _visibility = visibility ?? PixUI.Visibility.Visible;
    }

    private Visibility _oldVisibility = PixUI.Visibility.Visible;
    private readonly State<Visibility> _visibility;

    public required State<Visibility> Visibility
    {
        get => _visibility;
        init
        {
            _visibility = value;
            _oldVisibility = _visibility.Value;
            Bind(ref _visibility!, value, OnVisibilityChanged, true);
        }
    }

    private void OnVisibilityChanged(State state)
    {
        var currentVisibility = _visibility.Value;
        switch (currentVisibility)
        {
            case PixUI.Visibility.Collapsed:
                Relayout();
                break;
            case PixUI.Visibility.Visible:
            {
                if (_oldVisibility == PixUI.Visibility.Hidden)
                    Repaint();
                else
                    Relayout();
                break;
            }
            case PixUI.Visibility.Hidden:
            {
                if (_oldVisibility == PixUI.Visibility.Visible)
                    Repaint();
                else
                    Relayout();
                break;
            }
        }

        _oldVisibility = currentVisibility;
    }

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (Child != null && _visibility.Value == PixUI.Visibility.Visible)
            action(Child);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var max = CacheAndGetMaxSize(availableWidth, availableHeight);

        if (Child == null || _visibility.Value == PixUI.Visibility.Collapsed)
        {
            SetSize(0, 0);
            return;
        }

        //暂不考虑Padding
        var padding = EdgeInsets.All(0);
        Child.Layout(max.Width - padding.Left - padding.Right, max.Height - padding.Top - padding.Bottom);
        Child.SetPosition(padding.Left, padding.Top);

        SetSize(Child.W + padding.Left + padding.Right, Child.H + padding.Top + padding.Bottom);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_visibility.Value != PixUI.Visibility.Visible || W == 0 || H == 0 || canvas.IsClipEmpty)
            return;

        PaintChildren(canvas, area);
    }
}