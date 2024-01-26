using System;

namespace PixUI;

public sealed class ButtonGroup : MultiChildWidget<Button>
{
    private readonly State<float> _buttonHeight = Button.DefaultHeight;

    #region ====Widget Overrides====

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        var xPos = 0f;
        _buttonHeight.Value = Math.Min(maxSize.Height, Button.DefaultHeight); //暂强制同高
        for (var i = 0; i < _children.Count; i++)
        {
            _children[i].Height = _buttonHeight;
            _children[i].Shape = ButtonShape.Square;
            _children[i].Layout(Math.Max(0, maxSize.Width - xPos), _buttonHeight.Value);
            _children[i].SetPosition(xPos, 0);

            xPos += _children[i].W;
        }

        SetSize(xPos, _buttonHeight.Value);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (W == 0 || H == 0 || canvas.IsClipEmpty)
            return;

        //clip to round rectangle
        using var rrect = RRect.FromRectAndRadius(Rect.FromLTWH(0, 0, W, H),
            Button.StandardRadius, Button.StandardRadius);
        using var path = new Path();
        path.AddRRect(rrect);
        canvas.Save();
        canvas.ClipPath(path, ClipOp.Intersect, true);

        //注意不要调用PaintChildren，因RepaintChild转换了坐标但没有恢复，下面还要画分割条
        if (area is RepaintChild repaintChild)
        {
            var child = repaintChild.Repaint(canvas);
            child.AfterPaint(canvas); //这里转换回原来的坐标
        }
        else
        {
            VisitChildren(child =>
            {
                if (child.W <= 0 || child.H <= 0)
                    return false;
                if (area != null && !area.IntersectsWith(child))
                    return false; //脏区域与子组件没有相交部分，不用绘制

                child.BeforePaint(canvas);
                child.Paint(canvas, area?.ToChild(child));
                child.AfterPaint(canvas);

                PaintDebugger.PaintWidgetBorder(child, canvas);
                return false;
            });
        }

        //画分隔条
        var paint = PixUI.Paint.Shared(Colors.White, PaintStyle.Stroke);
        for (var i = 1; i < _children.Count; i++)
        {
            var x = _children[i].X - 0.5f;
            canvas.DrawLine(x, 0, x, H, paint);
        }

        canvas.Restore();
    }

    #endregion
}