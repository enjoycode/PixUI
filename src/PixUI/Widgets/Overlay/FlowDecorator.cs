using System.Collections.Generic;

namespace PixUI;

/// <summary>
/// 跟随目标Widget的装饰器
/// </summary>
public abstract class FlowDecorator<T> : Widget where T : Widget
{
    protected FlowDecorator(T target, bool onlyTransform)
    {
        Target = target;
        _onlyTransform = onlyTransform;
    }

    private readonly bool _onlyTransform;
    public T Target { get; }

    protected internal sealed override bool HitTest(float x, float y, HitTestResult result)
    {
        return false; // Can't hit
    }

    public sealed override void Layout(float availableWidth, float availableHeight)
    {
        //do nothing
    }

    public sealed override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var widgetToRoot = new List<Widget>();
        Widget temp = Target;
        while (true)
        {
            widgetToRoot.Add(temp);
            if (temp.Parent == null)
                break;
            temp = temp.Parent;
        }

        var saveCount = canvas.Save();
        for (var i = widgetToRoot.Count - 1; i >= 0; i--)
        {
            widgetToRoot[i].BeforePaint(canvas, _onlyTransform);
        }

        PaintCore(canvas);
        canvas.RestoreToCount(saveCount);
    }

    protected abstract void PaintCore(Canvas canvas);
}