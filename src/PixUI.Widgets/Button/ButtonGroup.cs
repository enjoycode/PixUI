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

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        if (W == 0 || H == 0 || canvas.IsClipEmpty)
            return;

        //clip to round rectangle
        var rrect = RRect.FromRectAndRadius(Rect.FromLTWH(0, 0, W, H),
            Button.StandardRadius, Button.StandardRadius);
        using var path = Path.Create();
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
            var visitor = new PaintChildrenVisitor(canvas, area);
            VisitChildren(ref visitor);
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