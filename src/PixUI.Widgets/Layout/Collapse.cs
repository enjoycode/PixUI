using System;

namespace PixUI;

public sealed class Collapse : Widget
{
    public Collapse()
    {
        _expandIcon = BuildExpandIcon();
    }

    private const float _titleHeight = 25; //暂固定标题高度

    private Widget? _title;
    private Widget? _body;
    private readonly ExpandIcon _expandIcon;

    public Widget Title
    {
        set
        {
            _title = value;
            _title.Parent = this;
        }
    }

    public Widget Body
    {
        set
        {
            _body = value;
            _body.Parent = this;
        }
    }

    private AnimationController? _expandController;
    private Animation<double>? _expandCurve;
    private Animation<float>? _expandArrowAnimation;

    public bool IsExpanded { get; set; } = true;

    private int _animationFlag = 0; //0=none,1=expand,-1=collapse
    private double _animationValue = 0;
    private bool IsExpanding => _animationFlag == 1;
    private bool IsCollapsing => _animationFlag == -1;

    #region ====Expand & Collapse=====

    private ExpandIcon BuildExpandIcon()
    {
        _expandController = new AnimationController(200, IsExpanded ? 1 : 0);
        _expandController.ValueChanged += OnAnimationValueChanged;
        _expandCurve = new CurvedAnimation(_expandController, Curves.EaseInOutCubic);
        _expandArrowAnimation = new FloatTween(0.75f, 1.0f).Animate(_expandCurve);

        var expander = new ExpandIcon(_expandArrowAnimation);
        expander.OnPointerDown = OnTapExpander;
        expander.Parent = this;
        return expander;
    }

    private void OnAnimationValueChanged()
    {
        _animationValue = _expandController!.Value;
        Invalidate(InvalidAction.Relayout); //自身改变高度并通知上级
    }

    private void OnTapExpander(PointerEvent e)
    {
        if (IsExpanded)
        {
            IsExpanded = false;
            _animationFlag = -1;
            _expandController?.Reverse();
        }
        else
        {
            IsExpanded = true;
            _animationFlag = 1;
            _expandController?.Forward();
        }
    }

    #endregion

    #region ====Widget Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (action(_expandIcon)) return;
        if (_title != null && action(_title)) return;
        if (_body != null && !(_animationFlag == 0 && !IsExpanded)) action(_body);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        //先处理是否由动画引起的高度改变
        if (IsExpanding || IsCollapsing)
        {
            //根据动画值计算需要展开的高度
            var bodyHeight = _body?.H ?? 0;
            var expandedHeight = (float)(bodyHeight * _animationValue);
            if (IsCollapsing && _animationValue == 0) //已收缩需要恢复本身的宽度
            {
                _animationFlag = 0;
                SetSize(W, _titleHeight);
            }
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            else if (IsExpanding && _animationValue == 1)
            {
                _animationFlag = 0;
                SetSize(W, _titleHeight + bodyHeight); //宽度之前已预设
            }
            else
            {
                SetSize(W, _titleHeight + expandedHeight); //宽度之前已预设
            }

            return;
        }

        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        const float padding = 5f;
        _expandIcon.Layout(_titleHeight, _titleHeight);
        _expandIcon.SetPosition(width - padding - _expandIcon.W, (_titleHeight - _expandIcon.H) / 2f);

        if (_title != null)
        {
            _title.Layout(width - _expandIcon.W - padding, _titleHeight);
            _title.SetPosition(padding, (_titleHeight - _title.H) / 2f);
        }

        if (_body != null)
        {
            _body.Layout(width, height /*maybe infinity*/);
            _body.SetPosition(0, _titleHeight);
        }

        SetSize(width, IsExpanded ? _titleHeight + _body?.H ?? 0 : _titleHeight);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var needClip = IsExpanding || IsCollapsing;
        if (needClip)
        {
            canvas.Save();
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
        }

        base.Paint(canvas, area);

        if (needClip)
            canvas.Restore();
    }

    #endregion
}