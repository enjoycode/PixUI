using System;
using System.Diagnostics;
using System.IO;

namespace PixUI;

public abstract class UIWindow
{
    protected UIWindow(Widget child, string? initRoutePath = null)
    {
        //在构建WidgetTree前设置初始路由路径
        if (!string.IsNullOrEmpty(initRoutePath))
            RouteHistoryManager.AssignedPath = initRoutePath;

        Overlay = new Overlay(this);
        RootWidget = new Root(this, child);

        PaintDebugger.EnableChanged += () => RootWidget.Repaint();

        Current = this; //TODO:暂单窗体
    }

    #region ====Fields & Properties====

    /// <summary>
    /// 当前激活的窗体
    /// </summary>
    public static UIWindow Current { get; private set; } = null!; //TODO: 暂单窗体

    public readonly Root RootWidget;
    public readonly Overlay Overlay;

    internal readonly FocusManagerStack FocusManagerStack = new();
    public readonly EventHookManager EventHookManager = new();

    public Color BackgroundColor { get; set; } = Colors.White; //TODO: move to Root

    public abstract float Width { get; }
    public abstract float Height { get; }
    public virtual float ScaleFactor => 1.0f;

    internal readonly InvalidQueue WidgetsInvalidQueue = new();
    internal readonly InvalidQueue OverlayInvalidQueue = new();
    protected internal bool HasPostInvalidateEvent = false;

    #region ----Mouse----

    protected internal float LastMouseX { get; private set; } = -1;

    protected internal float LastMouseY { get; private set; } = -1;

    protected internal Point LastPointerDown { get; private set; }

    // Pointer.Move时检测命中的结果
    private HitTestResult _oldHitResult = new();

    private HitTestResult _newHitResult = new();

    internal HitTestResult NewHitResult => _newHitResult;

    // Pointer.Down时捕获的结果
    private HitTestEntry? _hitResultOnPointDown;

    // 临时方案用于解决退出A进入B之前无法知道新的命中的节点是否B
    public Widget? LastHitWidget { get; private set; }

    #endregion

    #endregion

    #region ====Route====

    private RouteHistoryManager? _routeHistoryManager;

    protected internal RouteHistoryManager RouteHistoryManager
    {
        get
        {
            _routeHistoryManager ??= new RouteHistoryManager();
            return _routeHistoryManager;
        }
    }

    /// <summary>
    /// 导航至指定路由
    /// </summary>
    /// <param name="fullPath">eg: /一级路由/二级路由</param>
    public void NavigateTo(string fullPath)
    {
        var index = RouteHistoryManager.Push(fullPath);
        //同步浏览器历史
        UIApplication.Current.PushWebHistory(fullPath, index);
    }

    #endregion

    #region ====Canvas & Show====

    /// <summary>
    /// 获取Onscreen画布, 用于绘制Overlay并显示
    /// </summary>
    protected internal abstract Canvas GetOnscreenCanvas();

    /// <summary>
    /// 获取Offscreen画布，用于绘布不常变化的Widgets
    /// </summary>
    protected internal abstract Canvas GetOffscreenCanvas();

#if __WEB__
        protected internal abstract void FlushOffscreenSurface();
        
        protected internal abstract void DrawOffscreenSurface();
#endif


    /// <summary>
    /// 窗体首次呈现
    /// </summary>
    protected
#if __WEB__
    internal
#endif
        void OnFirstShow()
    {
        RootWidget.Layout(Width, Height);
        Overlay.Layout(Width, Height);

        var widgetsCanvas = GetOffscreenCanvas();
        RootWidget.Paint(widgetsCanvas);

        var overlayCanvas = GetOnscreenCanvas();
#if __WEB__
            FlushOffscreenSurface();
            DrawOffscreenSurface();
#else
        widgetsCanvas.Surface?.Draw(overlayCanvas, 0, 0, null);
#endif

        //TODO: maybe paint Overlay

        Present();
    }

    /// <summary>
    /// 呈现已渲染好的当前帧
    /// </summary>
    protected internal abstract void Present();

    #endregion

    #region ====Input Events====

    public void OnPointerMove(PointerEvent e)
    {
        LastMouseX = e.X;
        LastMouseY = e.Y;

        if (e.Buttons == PointerButtons.None) //无按键开始HitTest
        {
            if (_oldHitResult.StillInLastRegion(e.X, e.Y))
                OldHitTest(e.X, e.Y); //仍在旧的命中范围内
            else
                NewHitTest(e.X, e.Y); //重新开始检测

            //开始比较新旧命中结果，激发相应的HoverChanged事件
            CompareAndSwapHitTestResult();
        }
        else if (e.Buttons == PointerButtons.Left)
        {
            //判断是否可以启动Drag或已在Drag过程中
            if (DragDropManager.MaybeStart(this, ref _hitResultOnPointDown, e))
                return;
        }

        //如果命中MouseRegion，则向上传播事件(TODO: 考虑不传播)
        if (_oldHitResult.IsHitAnyMouseRegion)
            _oldHitResult.PropagatePointerEvent(e, (w, pe) => w.RaisePointerMove(pe));
    }

    public void OnPointerMoveOutWindow()
    {
        LastMouseX = LastMouseY = -1;
        CompareAndSwapHitTestResult();
        EventHookManager.HookEvent(EventType.MoveOutWindow, null);
    }

    public void OnPointerDown(PointerEvent pointerEvent)
    {
        LastPointerDown = new Point(pointerEvent.X, pointerEvent.Y);

        if (EventHookManager.HookEvent(EventType.PointerDown, pointerEvent))
            return;

        //TODO:移动端强制HitTest
        if (!_oldHitResult.IsHitAnyWidget)
        {
            //TODO: overlay first,另考虑向上逐级判断是否在旧命中区域内以避免从根节点重新开始
            RootWidget.HitTest(pointerEvent.X, pointerEvent.Y, _oldHitResult);
        }

        if (!_oldHitResult.IsHitAnyMouseRegion) return;

        _hitResultOnPointDown = _oldHitResult.LastEntry;
        _oldHitResult.PropagatePointerEvent(pointerEvent, (w, e) => w.RaisePointerDown(e));

        //Set focus widget after propagate event
        FocusManagerStack.Focus(_oldHitResult.LastHitWidget, this);
    }

    public void OnPointerUp(PointerEvent pointerEvent)
    {
        if (!_oldHitResult.IsHitAnyMouseRegion)
            return;

        //先尝试激发PointerTap事件
        if (pointerEvent.Buttons == PointerButtons.Left && _hitResultOnPointDown != null)
        {
            if (!DragDropManager.MaybeStop(pointerEvent))
            {
                var winX = pointerEvent.X;
                var winY = pointerEvent.Y;
                var local = _hitResultOnPointDown.Value.ToLocalPoint(winX, winY);
                if (((Widget)_hitResultOnPointDown.Value.Widget).ContainsPoint(local.Dx, local.Dy))
                {
                    pointerEvent.SetPoint(local.Dx, local.Dy);
                    _hitResultOnPointDown.Value.Widget.MouseRegion.RaisePointerTap(pointerEvent);
                    pointerEvent.SetPoint(winX, winY);
                }
            }

            _hitResultOnPointDown = null;
        }

        _oldHitResult.PropagatePointerEvent(pointerEvent, (w, e) => w.RaisePointerUp(e));
    }

    public void OnScroll(ScrollEvent scrollEvent)
    {
        if (!_oldHitResult.IsHitAnyWidget) return;

        var scrollable = _oldHitResult.LastHitWidget!.FindParent(w => w is IScrollable);
        if (scrollable == null) return;

        //TODO:如果返回的偏移量为0，继续循环向上查找IScrollable处理
        var offset = ((IScrollable)scrollable).OnScroll(scrollEvent.Dx, scrollEvent.Dy);
        if (!offset.IsEmpty)
            AfterScrollDoneInternal(scrollable, offset.Dx, offset.Dy);
    }

    public void OnDropFile(int x, int y, string name, int size, string type, Stream stream)
    {
        //HitTest for drop area, TODO: maybe use temp HitTestResult
        NewHitTest(x, y);
        var lastHitEntry = _newHitResult.LastEntry;
        _newHitResult.Reset();
        if (lastHitEntry == null || lastHitEntry.Value.Widget is not IDroppable droppable)
            return;

        //暂在这里判断一下是否允许Drop
        var dropItem = new FileDataTransferItem(name, size, type, stream);
        var dragEvent = new DragEvent() { TransferItem = dropItem };
        //var localPt = lastHitEntry.Value.ToLocalPoint(x, y);
        if (!droppable.AllowDrop(dragEvent)) return;

        droppable.OnDrop(dragEvent, Point.Empty);
    }

    public void OnKeyDown(KeyEvent keyEvent)
    {
        if (EventHookManager.HookEvent(EventType.KeyDown, keyEvent))
            return;

        FocusManagerStack.OnKeyDown(keyEvent, this);
    }

    public void OnKeyUp(KeyEvent keyEvent) => FocusManagerStack.OnKeyUp(keyEvent);

    public void OnTextInput(string text) => FocusManagerStack.OnTextInput(text);

    #endregion

    #region ====HitTest====

    private void OldHitTest(float winX, float winY)
    {
        // Console.WriteLine($"========OldHitTest:({winX},{winY}) ========");
        var hitTestInOldRegion = true;
        if (_oldHitResult.LastHitWidget!.Root is Root && Overlay.HasEntry)
        {
            //特殊情况，例如Popup与原Hit存在相交区域，还是得先尝试HitTest overlay
            Overlay.HitTest(winX, winY, _newHitResult);
            if (_newHitResult.IsHitAnyMouseRegion)
                hitTestInOldRegion = false;
        }

        if (hitTestInOldRegion)
        {
            _newHitResult.CopyFrom(_oldHitResult);
            _newHitResult.HitTestInLastRegion(winX, winY);
        }
    }

    internal void NewHitTest(float winX, float winY)
    {
        Log.Debug($"========NewHitTest:({winX},{winY})========");
        //先检测Overlay，没有命中再从RootWidget开始
        if (Overlay.HasEntry)
            Overlay.HitTest(winX, winY, _newHitResult);
        if (!_newHitResult.IsHitAnyWidget /*IsHitAnyMouseRegion*/)
            RootWidget.HitTest(winX, winY, _newHitResult);
    }

    private void CompareAndSwapHitTestResult()
    {
        LastHitWidget = _newHitResult.LastHitWidget;
        _oldHitResult.ExitOldRegion(_newHitResult);
        _newHitResult.EnterNewRegion(_oldHitResult);

#if DEBUG
        if (_oldHitResult.LastHitWidget != _newHitResult.LastHitWidget)
            Log.Debug($"Hit: {_newHitResult.LastHitWidget} {_newHitResult.LastWidgetWithMouseRegion}");
#endif

        //重置并交换
        _oldHitResult.Reset();
        (_oldHitResult, _newHitResult) = (_newHitResult, _oldHitResult);
    }

    /// <summary>
    /// 仅用于程序滚动后重设之前缓存的HitTest结果
    /// </summary>
    public void AfterScrollDone(Widget scrollable, Offset offset)
    {
        //判断旧HitResult是否隶属于当前IScrollable的子级
        if (_oldHitResult.IsHitAnyWidget && scrollable.IsAnyParentOf(_oldHitResult.LastHitWidget))
        {
            AfterScrollDoneInternal(scrollable, offset.Dx, offset.Dy);
        }
    }

    private void AfterScrollDoneInternal(Widget scrollable, float dx, float dy)
    {
        Debug.Assert(dx != 0 || dy != 0);
        //Translate HitTestResult and Rerun hit test.
        var stillInLastRegion = _oldHitResult.TranslateOnScroll(scrollable, dx, dy, LastMouseX, LastMouseY);
        if (stillInLastRegion)
            OldHitTest(LastMouseX, LastMouseY);
        else
            NewHitTest(LastMouseX, LastMouseY);
        CompareAndSwapHitTestResult();
    }

    /// <summary>
    /// 目前仅用于DynamicView改变前取消FocusedWidget
    /// </summary>
    internal void BeforeDynamicViewChange(DynamicView dynamicView)
    {
        //判断当前Focused是否DynamicView的子级，是则取消Focus状态
        var focusManger = FocusManagerStack.GetFocusManagerByWidget(dynamicView);
        if (focusManger.FocusedWidget == null) return;

        if (dynamicView.IsAnyParentOf(focusManger.FocusedWidget))
            focusManger.Focus(null, this);
    }

    /// <summary>
    /// 目前仅用于DynamicView改变内容后，重设之前缓存的HitTest结果
    /// </summary>
    internal void AfterDynamicViewChange(DynamicView dynamicView)
    {
        if (!_oldHitResult.IsHitAnyWidget ||
            !ReferenceEquals(_oldHitResult.LastHitWidget, dynamicView)) return;

        //切换过程结束后仍旧在DynamicView内，继续HitTest子级
        OldHitTest(LastMouseX, LastMouseY);
        CompareAndSwapHitTestResult();
    }

    /// <summary>
    /// Popup弹出关闭后重新进行
    /// </summary>
    internal void NewHitTestForPopup()
    {
        NewHitTest(LastMouseX, LastMouseY);
        CompareAndSwapHitTestResult();
    }

    /// <summary>
    /// 重新布局后尝试重新进行
    /// </summary>
    internal void NewHitTestForLayoutChanged()
    {
        if (_hitResultOnPointDown == null) //暂在这里排除按下左键的情况
        {
            NewHitTest(LastMouseX, LastMouseY);
            CompareAndSwapHitTestResult();
        }
        else
        {
            _oldHitResult.IsInvalidated = true;
        }
    }

    #endregion

    #region ====TextInput Methods====

    public virtual void StartTextInput() { }

    public virtual void SetTextInputRect(Rect rect) { }

    public virtual void StopTextInput() { }

    #endregion
}