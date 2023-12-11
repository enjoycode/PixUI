namespace PixUI;

public delegate Widget TransitionBuilder(Animation<double> animation, Widget child);

public abstract class DynamicView : SingleChildWidget
{
    protected DynamicView()
    {
        IsLayoutTight = false; //默认非紧凑布局
    }

    private AnimationController? _animationController;
    private Widget? _animationFrom;
    private Widget? _animationTo;
    private TransitionStack? _transitionStack;

    /// <summary>
    /// 直接替换
    /// </summary>
    protected void ReplaceTo(Widget? to)
    {
        if (!IsMounted)
        {
            Child = to;
            return;
        }

        Root!.Window.BeforeDynamicViewChange(this);
        Child = to;
        Root!.Window.AfterDynamicViewChange(this); //TODO: 检查是否需要，因重新布局会同样处理
        Relayout(); //这里始终重新布局
    }

    /// <summary>
    /// 动画替换
    /// </summary>
    protected void AnimateTo(Widget from, Widget to, int duration, bool reverse,
        TransitionBuilder enteringBuilder,
        TransitionBuilder? existingBuilder)
    {
        _animationFrom = from;
        _animationTo = to;

        Root!.Window.BeforeDynamicViewChange(this);

        CreateAnimationControl(duration, reverse);
        var exsiting = existingBuilder == null
            ? from
            : existingBuilder(_animationController!, from);
        var entering = enteringBuilder(_animationController!, to);

        _transitionStack = new TransitionStack(exsiting, entering);
        Child = _transitionStack;

        Layout(CachedAvailableWidth, CachedAvailableHeight); //开始前强制重新布局
        if (reverse)
            _animationController!.Reverse();
        else
            _animationController!.Forward();
    }

    private void CreateAnimationControl(int duration, bool reverse)
    {
        var initValue = reverse ? 1 : 0;
        _animationController = new AnimationController(duration, initValue);
        _animationController.ValueChanged += OnAnimationValueChanged;
        _animationController.StatusChanged += OnAnimationStatusChanged;
    }

    private void OnAnimationValueChanged()
    {
        Invalidate(InvalidAction.Repaint); //这里仅重绘，因开始动画前已强制重新布局
    }

    private void OnAnimationStatusChanged(AnimationStatus status)
    {
        if (status == AnimationStatus.Completed || status == AnimationStatus.Dismissed)
        {
            _animationController!.ValueChanged -= OnAnimationValueChanged;
            _animationController.StatusChanged -= OnAnimationStatusChanged;
            _animationController.Dispose();
            _animationController = null;

            if (_animationFrom!.SuspendingMount)
            {
                _animationFrom.SuspendingMount = false;
                _animationFrom.Parent = null;
                _animationTo!.SuspendingMount = true;
            }
            else
            {
                _animationTo!.SuspendingMount = false;
                _animationTo.Parent = null;
                _animationFrom.SuspendingMount = true;
            }

            Child = status == AnimationStatus.Dismissed ? _animationFrom : _animationTo;

            if (_animationTo!.SuspendingMount)
                _animationTo.SuspendingMount = false;
            else
                _animationFrom.SuspendingMount = false;

            _transitionStack!.Dispose();
            _transitionStack = null;

            Root!.Window.AfterDynamicViewChange(this);
        }
    }

    #region ====Widget Overrides====

    protected internal sealed override bool HitTest(float x, float y, HitTestResult result)
    {
        //如果在动画切换过程中，不继续尝试HitTest子级，因为缓存的HitTestResult.LastTransform如终是动画开始前的
        if (_animationController != null &&
            _animationController.Status != AnimationStatus.Dismissed &&
            _animationController.Status != AnimationStatus.Completed)
        {
            if (!ContainsPoint(x, y)) return false;
            result.Add(this);
            return true;
        }

        return base.HitTest(x, y, result);
    }

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false, Rect? dirtyRect = null)
    {
        base.BeforePaint(canvas, onlyTransform, dirtyRect);
        if (!onlyTransform)
        {
            canvas.Save();
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
        }
    }

    protected internal override void AfterPaint(Canvas canvas)
    {
        canvas.Restore();
        base.AfterPaint(canvas);
    }

    #endregion
}